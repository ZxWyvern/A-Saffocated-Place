using System.Collections.Generic;

namespace LastShift.Domain
{
    /// <summary>
    /// Context payload provided to CueResolver upon matchstick input.
    /// Pure C# domain model (ARC-05).
    /// </summary>
    public sealed class InputResolutionContext
    {
        public bool IsEncounterActive { get; set; } = true;
        public bool IsInputLocked { get; set; } = false;
        public bool IsMatchstickReady { get; set; } = true;
        public bool IsCooldownActive { get; set; } = false;
        public ThreatPhase CurrentThreatPhase { get; set; } = ThreatPhase.Dormant;
        public IReadOnlyList<AudioCueSnapshot> ActiveCues { get; set; } = new List<AudioCueSnapshot>();
        public IReadOnlyCollection<string> ExposedCueIds { get; set; } = new HashSet<string>();
        public string EventId { get; set; } = string.Empty;
    }
}
