namespace LastShift.Domain
{
    /// <summary>
    /// Contract for evaluating player matchstick inputs against cue snapshots.
    /// Owned by SSOT 13-data-contracts-and-resolution.md.
    /// </summary>
    public interface ICueResolver
    {
        /// <summary>
        /// Evaluates input context deterministically to return exactly one resolution result.
        /// </summary>
        ResolutionResult Resolve(InputResolutionContext context);
    }
}
