namespace LastShift.Domain
{
    /// <summary>
    /// Outcome of evaluating matchstick input against active audio cues.
    /// Owned by SSOT 13-data-contracts-and-resolution.md.
    /// </summary>
    public enum ResolutionResult
    {
        InputRejected,
        ThreatConfirmed,
        EnvironmentConfirmed,
        DecoyRevealed,
        AttackTriggered
    }
}
