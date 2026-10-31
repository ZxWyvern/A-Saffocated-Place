namespace LastShift.Domain
{
    /// <summary>
    /// Current state of the threat entity according to SSOT 06-threat-and-chase.md.
    /// </summary>
    public enum ThreatPhase
    {
        Dormant,
        Approaching,
        Verifying,
        Attacking,
        Disengaged
    }
}
