using UnityEngine;
using LastShift.Domain;

namespace LastShift.Config
{
    public enum SpatialMode
    {
        Spatial3D,
        Stereo2D
    }

    /// <summary>
    /// Configuration asset for an audio cue in LAST SHIFT.
    /// Schema defined in SSOT 13-data-contracts-and-resolution.md.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioCue_New", menuName = "LAST SHIFT/Audio Cue", order = 10)]
    public class AudioCueData : ScriptableObject
    {
        [Header("Identification")]
        [Tooltip("Stable unique ID for the cue (e.g. CUE_VENT_SCRAPE_01)")]
        [SerializeField] private string _cueId = "CUE_NEW";

        [Tooltip("Internal category determining gameplay consequence")]
        [SerializeField] private CueCategory _category = CueCategory.Environment;

        [Header("Audio Asset & Spatial Settings")]
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private SpatialMode _spatialMode = SpatialMode.Spatial3D;
        [SerializeField] private float _minDistance = 1.0f;
        [SerializeField] private float _maxDistance = 15.0f;
        [SerializeField] private float _cueDuration = 3.0f;

        [Header("Behavior & Progression")]
        [Tooltip("True for Threat cues; False for Decoys and Environment (CUE-03)")]
        [SerializeField] private bool _movesTowardPlayer = false;

        [Tooltip("Encounter ID where this cue is introduced without strike penalty (CUE-04)")]
        [SerializeField] private string _introductionEncounter = string.Empty;

        [Tooltip("Encounter tags authorized to use this cue")]
        [SerializeField] private string _encounterTag = "Encounter_All";

        [Tooltip("Priority for deterministic resolution if overlapping with other cues")]
        [SerializeField] private int _priority = 0;

        public string CueId => _cueId;
        public CueCategory Category => _category;
        public AudioClip AudioClip => _audioClip;
        public SpatialMode SpatialMode => _spatialMode;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public float CueDuration => _cueDuration;
        public bool MovesTowardPlayer => _movesTowardPlayer;
        public string IntroductionEncounter => _introductionEncounter;
        public string EncounterTag => _encounterTag;
        public int Priority => _priority;

        public void Initialize(string cueId, CueCategory category, float duration, bool movesTowardPlayer, int priority)
        {
            _cueId = cueId;
            _category = category;
            _cueDuration = duration;
            _movesTowardPlayer = movesTowardPlayer;
            _priority = priority;
        }

        public AudioCueSnapshot CreateSnapshot(float remainingDuration = -1f)
        {
            float duration = remainingDuration >= 0f ? remainingDuration : _cueDuration;
            return new AudioCueSnapshot(_cueId, _category, isValid: true, _priority, _movesTowardPlayer, duration);
        }
    }
}
