using UnityEngine;

namespace LastShift.Config
{
    /// <summary>
    /// Configuration asset for Threat Entity & Chase mechanics.
    /// Values strictly sourced from SSOT 14-parameters.md.
    /// </summary>
    [CreateAssetMenu(fileName = "ThreatConfig", menuName = "LAST SHIFT/Threat Config", order = 30)]
    public class ThreatConfig : ScriptableObject
    {
        [Header("Threat Timing & Limits (14-parameters.md)")]
        [Tooltip("Time before an approaching threat reaches lethal attack state (P-THREAT-APPROACH)")]
        [SerializeField] private float _approachDuration = 8.0f;

        [Tooltip("Maximum strikes permitted before triggering an attack (P-STRIKE-MAX)")]
        [SerializeField] private int _maxStrikes = 2;

        [Tooltip("Time without detection required to recover one strike (P-STRIKE-RECOVERY)")]
        [SerializeField] private float _strikeRecoveryDuration = 20.0f;

        [Tooltip("Minimum warning telegraph required before a lethal attack (P-TELEGRAPH-MIN)")]
        [SerializeField] private float _telegraphMinDuration = 1.5f;

        [Tooltip("Maximum retry/respawn sequence duration (P-RETRY-MAX)")]
        [SerializeField] private float _maxRetryDuration = 5.0f;

        [Header("Jumpscare Controls (14-parameters.md)")]
        [Tooltip("Hard cap on full jumpscares per complete playthrough (P-JUMPSCARE-CAP)")]
        [SerializeField] private int _jumpscareCap = 2;

        [Tooltip("Duration of full jumpscare camera lock and animation (P-JUMPSCARE-DURATION)")]
        [SerializeField] private float _jumpscareDuration = 0.8f;

        [Header("Pacing (14-parameters.md)")]
        [Tooltip("Duration of safe breathing room in rest segment (P-REST-DURATION)")]
        [SerializeField] private float _restDuration = 35.0f;

        public float ApproachDuration => _approachDuration;
        public int MaxStrikes => _maxStrikes;
        public float StrikeRecoveryDuration => _strikeRecoveryDuration;
        public float TelegraphMinDuration => _telegraphMinDuration;
        public float MaxRetryDuration => _maxRetryDuration;
        public int JumpscareCap => _jumpscareCap;
        public float JumpscareDuration => _jumpscareDuration;
        public float RestDuration => _restDuration;
    }
}
