namespace LastShift.Domain
{
    /// <summary>
    /// Pure C# snapshot of an active audio cue at the moment of evaluation.
    /// Free of UnityEngine components or scene references (ARC-05).
    /// </summary>
    public readonly struct AudioCueSnapshot
    {
        public string CueId { get; }
        public CueCategory Category { get; }
        public bool IsValid { get; }
        public int Priority { get; }
        public bool MovesTowardPlayer { get; }
        public float RemainingDuration { get; }

        public AudioCueSnapshot(
            string cueId,
            CueCategory category,
            bool isValid = true,
            int priority = 0,
            bool movesTowardPlayer = false,
            float remainingDuration = 1.0f)
        {
            CueId = cueId ?? string.Empty;
            Category = category;
            IsValid = isValid;
            Priority = priority;
            MovesTowardPlayer = movesTowardPlayer;
            RemainingDuration = remainingDuration;
        }
    }
}
