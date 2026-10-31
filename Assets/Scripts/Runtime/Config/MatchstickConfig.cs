using UnityEngine;

namespace LastShift.Config
{
    /// <summary>
    /// Configuration asset for Matchstick mechanics.
    /// Values strictly sourced from SSOT 14-parameters.md.
    /// </summary>
    [CreateAssetMenu(fileName = "MatchstickConfig", menuName = "LAST SHIFT/Matchstick Config", order = 20)]
    public class MatchstickConfig : ScriptableObject
    {
        [Header("Timers (14-parameters.md)")]
        [Tooltip("Burn duration of lit matchstick (P-MATCH-BURN)")]
        [SerializeField] private float _burnDuration = 1.8f;

        [Tooltip("Cooldown period before another match can be lit (P-MATCH-COOLDOWN)")]
        [SerializeField] private float _cooldownDuration = 1.0f;

        [Tooltip("Visual reveal duration when threat is illuminated (P-REVEAL-DURATION)")]
        [SerializeField] private float _revealDuration = 1.0f;

        [Header("Audio Ducking & Filter (14-parameters.md)")]
        [Tooltip("Attenuation in dB applied to cues & ambience during burn (P-MATCH-DUCK-DB)")]
        [SerializeField] private float _duckDb = 12.0f;

        [Tooltip("Low-pass cutoff frequency in Hz during burn (P-MATCH-LPF-HZ)")]
        [SerializeField] private float _lowPassCutoffHz = 1500.0f;

        [Tooltip("Transition attack time into ducked snapshot (P-MATCH-DUCK-ATTACK)")]
        [SerializeField] private float _duckAttackTime = 0.1f;

        [Tooltip("Transition release time returning to normal snapshot (P-MATCH-DUCK-RELEASE)")]
        [SerializeField] private float _duckReleaseTime = 0.4f;

        [Header("Visual Light Settings")]
        [SerializeField] private float _lightRange = 7.0f;
        [SerializeField] private Color _flameColor = new Color(1.0f, 0.65f, 0.25f, 1.0f);
        [SerializeField] private float _lightIntensity = 1.8f;

        public float BurnDuration => _burnDuration;
        public float CooldownDuration => _cooldownDuration;
        public float RevealDuration => _revealDuration;
        public float DuckDb => _duckDb;
        public float LowPassCutoffHz => _lowPassCutoffHz;
        public float DuckAttackTime => _duckAttackTime;
        public float DuckReleaseTime => _duckReleaseTime;
        public float LightRange => _lightRange;
        public Color FlameColor => _flameColor;
        public float LightIntensity => _lightIntensity;
    }
}
