using System;
using UnityEngine;
using LastShift.Config;
using LastShift.Domain;

namespace LastShift.Threat
{
    /// <summary>
    /// Threat Finite State Machine and Strike Tracker.
    /// Implements THR-01..THR-18, RST-01..RST-07, and P-THREAT-*, P-STRIKE-*.
    /// </summary>
    public class ThreatStateController : MonoBehaviour, IThreatStateService
    {
        [Header("Configuration")]
        [SerializeField] private ThreatConfig _config;

        [Header("Audio Feedback")]
        [SerializeField] private AudioSource _warningAudioSource;
        [SerializeField] private AudioSource _attackAudioSource;

        [Header("Visual Feedback")]
        [SerializeField] private GameObject _monsterVisualRoot;

        public ThreatPhase CurrentPhase { get; private set; } = ThreatPhase.Dormant;
        public int CurrentStrikes { get; private set; } = 0;
        public int MaxStrikes => _config != null ? _config.MaxStrikes : 2;
        public int JumpscaresTriggered { get; private set; } = 0;
        public bool IsTelegraphing { get; private set; } = false;

        public event Action<ThreatPhase, ThreatPhase> OnPhaseChanged;
        public event Action<int, int> OnStrikeReceived;
        public event Action OnStrikeRecovered;
        public event Action<float> OnTelegraphStarted;
        public event Action<bool> OnAttackTriggered;
        public event Action OnThreatDisengaged;
        public event Action OnThreatReset;

        private float _approachTimer = 0f;
        private float _strikeRecoveryTimer = 0f;
        private float _telegraphTimer = 0f;
        private bool _isPausedOrCutscene = false;

        public void SetPausedOrCutscene(bool paused)
        {
            _isPausedOrCutscene = paused;
        }

        private void Start()
        {
            if (_monsterVisualRoot != null)
            {
                _monsterVisualRoot.SetActive(false);
            }
        }

        public void StartEncounter(ThreatConfig config)
        {
            if (config != null)
            {
                _config = config;
            }

            TransitionTo(ThreatPhase.Approaching);
            _approachTimer = 0f;
        }

        public void TransitionTo(ThreatPhase newPhase)
        {
            if (CurrentPhase == newPhase) return;

            ThreatPhase oldPhase = CurrentPhase;
            CurrentPhase = newPhase;

            if (newPhase == ThreatPhase.Approaching)
            {
                _approachTimer = 0f;
            }
            else if (newPhase == ThreatPhase.Disengaged)
            {
                _strikeRecoveryTimer = 0f;
                IsTelegraphing = false;
                if (_monsterVisualRoot != null)
                {
                    _monsterVisualRoot.SetActive(false);
                }
                OnThreatDisengaged?.Invoke();
            }
            else if (newPhase == ThreatPhase.Dormant)
            {
                IsTelegraphing = false;
                if (_monsterVisualRoot != null)
                {
                    _monsterVisualRoot.SetActive(false);
                }
            }

            OnPhaseChanged?.Invoke(oldPhase, newPhase);
        }

        public void AddStrike(string reason)
        {
            CurrentStrikes++;
            _strikeRecoveryTimer = 0f;

            OnStrikeReceived?.Invoke(CurrentStrikes, MaxStrikes);

            // THR-05: Strike pertama MUST menghasilkan peringatan audio dari jarak dekat
            if (CurrentStrikes == 1)
            {
                PlayCloseWarningAudio();
            }

            // THR-06: Strike yang mencapai batas maksimum MUST memicu fase Attacking
            if (CurrentStrikes >= MaxStrikes)
            {
                StartTelegraphAndAttack(false);
            }
        }

        public void TriggerAttack(bool isScripted)
        {
            StartTelegraphAndAttack(isScripted);
        }

        public void Disengage()
        {
            TransitionTo(ThreatPhase.Disengaged);
        }

        public void ResetThreatState(bool fullPlaythroughReset = false)
        {
            CurrentStrikes = 0;
            _approachTimer = 0f;
            _strikeRecoveryTimer = 0f;
            _telegraphTimer = 0f;
            IsTelegraphing = false;

            if (fullPlaythroughReset)
            {
                JumpscaresTriggered = 0;
            }

            if (_monsterVisualRoot != null)
            {
                _monsterVisualRoot.SetActive(false);
            }

            CurrentPhase = ThreatPhase.Dormant;
            OnThreatReset?.Invoke();
        }

        private void PlayCloseWarningAudio()
        {
            if (_warningAudioSource != null)
            {
                _warningAudioSource.Play();
            }
        }

        private void StartTelegraphAndAttack(bool isScripted)
        {
            if (IsTelegraphing || CurrentPhase == ThreatPhase.Attacking)
            {
                return;
            }

            float telegraphDuration = _config != null ? _config.TelegraphMinDuration : 1.5f;

            // THR-17 / Scripted jumpscare exception in The Loop
            if (isScripted)
            {
                telegraphDuration = 0.1f;
            }

            IsTelegraphing = true;
            _telegraphTimer = telegraphDuration;
            OnTelegraphStarted?.Invoke(telegraphDuration);
        }

        private void ExecuteAttack()
        {
            IsTelegraphing = false;
            TransitionTo(ThreatPhase.Attacking);

            int cap = _config != null ? _config.JumpscareCap : 2;
            bool canJumpscare = JumpscaresTriggered < cap;

            // THR-12, THR-14, THR-15: Jumpscare guard & cap
            if (canJumpscare)
            {
                JumpscaresTriggered++;
            }

            if (_attackAudioSource != null)
            {
                _attackAudioSource.Play();
            }

            if (_monsterVisualRoot != null)
            {
                _monsterVisualRoot.SetActive(true);
            }

            OnAttackTriggered?.Invoke(canJumpscare);
        }

        private void Update()
        {
            // THR-18: Timer ancaman MUST NOT berjalan saat pause atau cutscene
            if (_isPausedOrCutscene) return;

            // Telegraph update
            if (IsTelegraphing)
            {
                _telegraphTimer -= Time.deltaTime;
                if (_telegraphTimer <= 0f)
                {
                    ExecuteAttack();
                }
                return;
            }

            // Approaching state update
            if (CurrentPhase == ThreatPhase.Approaching)
            {
                float maxApproach = _config != null ? _config.ApproachDuration : 8.0f;
                _approachTimer += Time.deltaTime;

                if (_approachTimer >= maxApproach)
                {
                    // Approaching timed out -> Verifying -> Attack
                    TransitionTo(ThreatPhase.Verifying);
                    StartTelegraphAndAttack(false);
                }
            }
            // Disengaged strike recovery
            else if (CurrentPhase == ThreatPhase.Disengaged && CurrentStrikes > 0)
            {
                float recoveryDuration = _config != null ? _config.StrikeRecoveryDuration : 20.0f;
                _strikeRecoveryTimer += Time.deltaTime;

                if (_strikeRecoveryTimer >= recoveryDuration)
                {
                    CurrentStrikes = Mathf.Max(0, CurrentStrikes - 1);
                    _strikeRecoveryTimer = 0f;
                    OnStrikeRecovered?.Invoke();
                }
            }
        }
    }
}
