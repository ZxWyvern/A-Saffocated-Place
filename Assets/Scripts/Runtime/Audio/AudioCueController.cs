using System;
using System.Collections.Generic;
using UnityEngine;
using LastShift.Config;
using LastShift.Domain;

namespace LastShift.Audio
{
    /// <summary>
    /// Manages playback, spatialization, and active snapshots of all audio cues.
    /// Implements CUE-01..CUE-10, RST-01, and RST-07.
    /// </summary>
    public class AudioCueController : MonoBehaviour
    {
        [Header("Pool Settings (PERF-01)")]
        [SerializeField] private int _initialPoolSize = 6;
        [SerializeField] private AudioCueInstance _instancePrefab;
        [SerializeField] private Transform _playerTransform;

        private readonly List<AudioCueInstance> _activeInstances = new List<AudioCueInstance>();
        private readonly Queue<AudioCueInstance> _availablePool = new Queue<AudioCueInstance>();
        private readonly HashSet<string> _exposedCueIds = new HashSet<string>(StringComparer.Ordinal);

        public IReadOnlyCollection<string> ExposedCueIds => _exposedCueIds;

        public event Action<AudioCueData> OnCueStarted;
        public event Action<AudioCueData> OnCueEnded;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                var instance = CreateNewInstance();
                instance.gameObject.SetActive(false);
                _availablePool.Enqueue(instance);
            }
        }

        private AudioCueInstance CreateNewInstance()
        {
            if (_instancePrefab != null)
            {
                var inst = Instantiate(_instancePrefab, transform);
                return inst;
            }

            // Fallback: create dynamic GameObject with AudioSource & AudioCueInstance
            var go = new GameObject("AudioCue_Instance");
            go.transform.SetParent(transform);
            var audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            var cueInstance = go.AddComponent<AudioCueInstance>();
            return cueInstance;
        }

        public AudioCueInstance PlayCue(AudioCueData data, Vector3 position)
        {
            if (data == null)
            {
                return null;
            }

            AudioCueInstance instance = _availablePool.Count > 0 ? _availablePool.Dequeue() : CreateNewInstance();
            instance.gameObject.SetActive(true);
            instance.Play(data, position, _playerTransform);
            _activeInstances.Add(instance);

            // RST-07: Track exposed cues across playthrough
            _exposedCueIds.Add(data.CueId);

            OnCueStarted?.Invoke(data);
            return instance;
        }

        private void Update()
        {
            // Clean up finished instances
            for (int i = _activeInstances.Count - 1; i >= 0; i--)
            {
                var instance = _activeInstances[i];
                if (!instance.IsPlaying)
                {
                    OnCueEnded?.Invoke(instance.Data);
                    instance.gameObject.SetActive(false);
                    _activeInstances.RemoveAt(i);
                    _availablePool.Enqueue(instance);
                }
            }
        }

        public List<AudioCueSnapshot> GetActiveSnapshots()
        {
            var snapshots = new List<AudioCueSnapshot>(_activeInstances.Count);
            for (int i = 0; i < _activeInstances.Count; i++)
            {
                var inst = _activeInstances[i];
                if (inst.IsPlaying)
                {
                    snapshots.Add(inst.GetSnapshot());
                }
            }
            return snapshots;
        }

        /// <summary>
        /// Stops all active audio cues immediately upon retry or respawn (RST-01).
        /// Note: ExposedCueIds are retained across retries per RST-07.
        /// </summary>
        public void StopAllCues()
        {
            for (int i = _activeInstances.Count - 1; i >= 0; i--)
            {
                var instance = _activeInstances[i];
                instance.Stop();
                instance.gameObject.SetActive(false);
                _availablePool.Enqueue(instance);
            }
            _activeInstances.Clear();
        }

        /// <summary>
        /// Clears all state including exposure history (only for full new game starts).
        /// </summary>
        public void ResetAll()
        {
            StopAllCues();
            _exposedCueIds.Clear();
        }
    }
}
