using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using LastShift.Config;

namespace LastShift.Matchstick
{
    /// <summary>
    /// Handles ambient and audio cue attenuation (-12 dB, 1500 Hz LPF) during matchstick burn.
    /// Implements MAT-02, MAT-03, and AUD-05.
    /// </summary>
    public class AudioDucker : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private MatchstickConfig _config;

        [Header("Audio Mixer Routing (Optional)")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerSnapshot _normalSnapshot;
        [SerializeField] private AudioMixerSnapshot _duckedSnapshot;

        [Header("Direct Audio Filter Fallback")]
        [SerializeField] private AudioLowPassFilter _lowPassFilter;
        [SerializeField] private AudioSource _ambientSource;

        private Coroutine _transitionCoroutine;
        private bool _isDucking = false;
        private float _originalAmbientVolume = 1.0f;

        public bool IsDucking => _isDucking;

        private void Awake()
        {
            if (_lowPassFilter == null)
            {
                _lowPassFilter = GetComponent<AudioLowPassFilter>();
            }

            if (_ambientSource != null)
            {
                _originalAmbientVolume = _ambientSource.volume;
            }

            SetDuckingImmediate(false);
        }

        public void SetDuckingActive(bool active)
        {
            _isDucking = active;

            // Option A: Use AudioMixer Snapshots if wired
            if (_duckedSnapshot != null && _normalSnapshot != null)
            {
                float duration = active
                    ? (_config != null ? _config.DuckAttackTime : 0.1f)
                    : (_config != null ? _config.DuckReleaseTime : 0.4f);

                var targetSnapshot = active ? _duckedSnapshot : _normalSnapshot;
                targetSnapshot.TransitionTo(duration);
                return;
            }

            // Option B: Programmatic filter and volume interpolation
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
            }
            _transitionCoroutine = StartCoroutine(TransitionDuckingRoutine(active));
        }

        public void SetDuckingImmediate(bool active)
        {
            _isDucking = active;
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
                _transitionCoroutine = null;
            }

            if (_normalSnapshot != null && !active)
            {
                _normalSnapshot.TransitionTo(0.01f);
            }

            if (_lowPassFilter != null)
            {
                _lowPassFilter.enabled = active;
                _lowPassFilter.cutoffFrequency = active
                    ? (_config != null ? _config.LowPassCutoffHz : 1500f)
                    : 22000f;
            }

            if (_ambientSource != null)
            {
                // -12 dB is approximately 25% of linear amplitude
                _ambientSource.volume = active ? (_originalAmbientVolume * 0.25f) : _originalAmbientVolume;
            }
        }

        private IEnumerator TransitionDuckingRoutine(bool active)
        {
            float duration = active
                ? (_config != null ? _config.DuckAttackTime : 0.1f)
                : (_config != null ? _config.DuckReleaseTime : 0.4f);

            float targetCutoff = active ? (_config != null ? _config.LowPassCutoffHz : 1500f) : 22000f;
            float targetVolume = active ? (_originalAmbientVolume * 0.25f) : _originalAmbientVolume;

            if (_lowPassFilter != null)
            {
                _lowPassFilter.enabled = true;
            }

            float startCutoff = _lowPassFilter != null ? _lowPassFilter.cutoffFrequency : 22000f;
            float startVolume = _ambientSource != null ? _ambientSource.volume : 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                if (_lowPassFilter != null)
                {
                    _lowPassFilter.cutoffFrequency = Mathf.Lerp(startCutoff, targetCutoff, t);
                }

                if (_ambientSource != null)
                {
                    _ambientSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                }

                yield return null;
            }

            if (_lowPassFilter != null && !active)
            {
                _lowPassFilter.enabled = false;
            }

            _transitionCoroutine = null;
        }
    }
}
