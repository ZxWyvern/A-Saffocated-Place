using UnityEngine;
using LastShift.Config;
using LastShift.Domain;

namespace LastShift.Audio
{
    /// <summary>
    /// Runtime instance of an active audio cue.
    /// Manages 3D spatial positioning and movement toward player (CUE-03).
    /// </summary>
    public class AudioCueInstance : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private AudioCueData _data;
        private Transform _playerTransform;
        private float _remainingDuration;
        private float _initialDistance;
        private Vector3 _startPosition;

        public AudioCueData Data => _data;
        public bool IsPlaying => _remainingDuration > 0f && (_audioSource == null || _audioSource.isPlaying);
        public float RemainingDuration => _remainingDuration;

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }

        private void Update()
        {
            if (_data == null || _remainingDuration <= 0f)
            {
                return;
            }

            _remainingDuration -= Time.deltaTime;

            // CUE-03: Threat cues move toward the player; Decoy and Environment stay static
            if (_data.MovesTowardPlayer && _playerTransform != null && _data.CueDuration > 0f)
            {
                float progress = 1.0f - Mathf.Clamp01(_remainingDuration / _data.CueDuration);
                // Moves smoothly from initial position closer toward the player
                Vector3 target = _playerTransform.position + Vector3.up * 0.5f;
                transform.position = Vector3.Lerp(_startPosition, target, progress * 0.85f);
            }

            if (_remainingDuration <= 0f)
            {
                Stop();
            }
        }

        public void Play(AudioCueData data, Vector3 position, Transform playerTransform = null)
        {
            _data = data;
            _playerTransform = playerTransform;
            _startPosition = position;
            transform.position = position;
            _remainingDuration = data.CueDuration;

            if (_audioSource != null)
            {
                _audioSource.clip = data.AudioClip;
                _audioSource.spatialBlend = data.SpatialMode == SpatialMode.Spatial3D ? 1.0f : 0.0f;
                _audioSource.minDistance = data.MinDistance;
                _audioSource.maxDistance = data.MaxDistance;
                _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                _audioSource.loop = false;
                if (data.AudioClip != null)
                {
                    _audioSource.Play();
                }
            }
        }

        public void Stop()
        {
            _remainingDuration = 0f;
            if (_audioSource != null && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }

        public AudioCueSnapshot GetSnapshot()
        {
            if (_data == null)
            {
                return new AudioCueSnapshot(string.Empty, CueCategory.Environment, isValid: false);
            }

            return new AudioCueSnapshot(
                _data.CueId,
                _data.Category,
                isValid: IsPlaying,
                priority: _data.Priority,
                movesTowardPlayer: _data.MovesTowardPlayer,
                remainingDuration: _remainingDuration);
        }
    }
}
