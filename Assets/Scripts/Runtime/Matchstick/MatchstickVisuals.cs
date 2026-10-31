using UnityEngine;

namespace LastShift.Matchstick
{
    /// <summary>
    /// Visual and optical feedback for the matchstick flame (MAT-06).
    /// Decoupled from ignition and resolution logic.
    /// </summary>
    public class MatchstickVisuals : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Light _flameLight;
        [SerializeField] private GameObject _flameObject;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _strikeSound;
        [SerializeField] private AudioClip _burnLoopSound;

        [Header("Flicker Animation")]
        [SerializeField] private float _baseIntensity = 2.0f;
        [SerializeField] private float _flickerRange = 0.4f;
        [SerializeField] private float _flickerSpeed = 16f;

        private bool _isActive = false;

        private void Awake()
        {
            if (_flameLight == null)
            {
                _flameLight = GetComponentInChildren<Light>();
            }

            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            SetVisualsActive(false);
        }

        private void Update()
        {
            if (!_isActive || _flameLight == null)
            {
                return;
            }

            // Procedural organic flame flicker
            float noise = Mathf.PerlinNoise(Time.time * _flickerSpeed, 0f);
            _flameLight.intensity = _baseIntensity + (noise - 0.5f) * _flickerRange;
        }

        public void SetVisualsActive(bool active)
        {
            _isActive = active;

            if (_flameLight != null)
            {
                _flameLight.enabled = active;
            }

            if (_flameObject != null)
            {
                _flameObject.SetActive(active);
            }

            if (_audioSource != null)
            {
                if (active)
                {
                    if (_strikeSound != null)
                    {
                        _audioSource.PlayOneShot(_strikeSound);
                    }

                    if (_burnLoopSound != null)
                    {
                        _audioSource.clip = _burnLoopSound;
                        _audioSource.loop = true;
                        _audioSource.Play();
                    }
                }
                else
                {
                    _audioSource.Stop();
                }
            }
        }
    }
}
