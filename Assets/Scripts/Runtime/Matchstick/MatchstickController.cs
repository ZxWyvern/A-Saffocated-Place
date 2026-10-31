using System;
using UnityEngine;
using LastShift.Config;
using LastShift.Domain;

namespace LastShift.Matchstick
{
    /// <summary>
    /// Governs matchstick state machine, ignition timers, and ducking requests.
    /// Implements MAT-01 through MAT-07 from SSOT 04-matchstick.md.
    /// </summary>
    public class MatchstickController : MonoBehaviour, IMatchstickService
    {
        [Header("Configuration (14-parameters.md)")]
        [SerializeField] private MatchstickConfig _config;

        [Header("Dependencies (ARC-01)")]
        [SerializeField] private MatchstickVisuals _visuals;
        [SerializeField] private AudioDucker _audioDucker;

        private MatchstickState _currentState = MatchstickState.Unlit;
        private float _burnTimer = 0f;
        private float _cooldownTimer = 0f;
        private bool _isLocked = false;
        private ICueResolver _cueResolver;
        private Func<InputResolutionContext> _contextProvider;

        public MatchstickState CurrentState => _currentState;
        public bool IsCooldownActive => _cooldownTimer > 0f;
        public float RemainingBurnRatio => _config != null && _config.BurnDuration > 0f ? Mathf.Clamp01(_burnTimer / _config.BurnDuration) : 0f;

        public event Action OnIgnited;
        public event Action OnExtinguished;
        public event Action<float> OnBurnProgress;
        public event Action<ResolutionResult> OnCueResolved;
        public event Action<MatchstickState, MatchstickState> OnStateChanged;

        private void SetState(MatchstickState newState)
        {
            if (_currentState == newState) return;
            MatchstickState old = _currentState;
            _currentState = newState;
            OnStateChanged?.Invoke(old, newState);
        }

        private void Awake()
        {
            if (_visuals == null)
            {
                _visuals = GetComponentInChildren<MatchstickVisuals>();
            }

            if (_audioDucker == null)
            {
                _audioDucker = GetComponentInChildren<AudioDucker>();
            }

            // Default internal resolver if none injected
            _cueResolver = new CueResolver();
        }

        private void Update()
        {
            if (Time.timeScale <= 0f)
            {
                // MAT-05: Timers stop during pause
                return;
            }

            // Handle Cooldown
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
                if (_cooldownTimer < 0f)
                {
                    _cooldownTimer = 0f;
                }
            }

            // Handle Burning
            if (_currentState == MatchstickState.Burning)
            {
                _burnTimer -= Time.deltaTime;
                OnBurnProgress?.Invoke(RemainingBurnRatio);

                if (_burnTimer <= 0f)
                {
                    Extinguish();
                }
            }
        }

        public void Initialize(ICueResolver resolver, Func<InputResolutionContext> contextProvider)
        {
            _cueResolver = resolver ?? new CueResolver();
            _contextProvider = contextProvider;
        }

        public void SetLocked(bool locked)
        {
            _isLocked = locked;
            if (locked && _currentState == MatchstickState.Burning)
            {
                Extinguish();
            }
        }

        public bool TryIgnite()
        {
            // MAT-01: Reject if not Unlit, locked, or cooldown active
            if (_currentState != MatchstickState.Unlit || _isLocked || _cooldownTimer > 0f)
            {
                return false;
            }

            float burnDuration = _config != null ? _config.BurnDuration : 1.8f;
            _burnTimer = burnDuration;
            SetState(MatchstickState.Burning);

            // MAT-02: Activate audio ducking
            if (_audioDucker != null)
            {
                _audioDucker.SetDuckingActive(true);
            }

            // MAT-06: Presentation handled separately
            if (_visuals != null)
            {
                _visuals.SetVisualsActive(true);
            }

            OnIgnited?.Invoke();

            // MAT-07: Resolve active cues via CueResolver
            if (_contextProvider != null && _cueResolver != null)
            {
                var context = _contextProvider.Invoke();
                var result = _cueResolver.Resolve(context);
                OnCueResolved?.Invoke(result);
            }

            return true;
        }

        public void ExtinguishMatch() => Extinguish();

        public void Extinguish()
        {
            if (_currentState == MatchstickState.Unlit)
            {
                return;
            }

            SetState(MatchstickState.Extinguishing);

            // MAT-03: Restore normal audio snapshot
            if (_audioDucker != null)
            {
                _audioDucker.SetDuckingActive(false);
            }

            if (_visuals != null)
            {
                _visuals.SetVisualsActive(false);
            }

            float cooldown = _config != null ? _config.CooldownDuration : 1.0f;
            _cooldownTimer = cooldown;
            _burnTimer = 0f;
            SetState(MatchstickState.Unlit);

            OnExtinguished?.Invoke();
        }

        public void ResetForRetry()
        {
            Extinguish();
            _cooldownTimer = 0f;
            _burnTimer = 0f;
            SetState(MatchstickState.Unlit);
        }
    }
}
