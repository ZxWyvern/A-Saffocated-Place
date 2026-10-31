using System;

namespace LastShift.Matchstick
{
    /// <summary>
    /// Contract for matchstick actions and state.
    /// Owned by SSOT 13-data-contracts-and-resolution.md.
    /// </summary>
    public interface IMatchstickService
    {
        MatchstickState CurrentState { get; }
        bool TryIgnite();
        void Extinguish();

        event Action OnIgnited;
        event Action OnExtinguished;
        event Action<float> OnBurnProgress;
    }
}
