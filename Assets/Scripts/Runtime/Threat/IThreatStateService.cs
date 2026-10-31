using System;
using LastShift.Domain;
using LastShift.Config;

namespace LastShift.Threat
{
    /// <summary>
    /// Service interface for Threat FSM and Strike System.
    /// Implements THR-04..THR-09, THR-12..THR-18.
    /// </summary>
    public interface IThreatStateService
    {
        ThreatPhase CurrentPhase { get; }
        int CurrentStrikes { get; }
        int MaxStrikes { get; }
        int JumpscaresTriggered { get; }
        bool IsTelegraphing { get; }

        event Action<ThreatPhase, ThreatPhase> OnPhaseChanged;
        event Action<int, int> OnStrikeReceived;
        event Action OnStrikeRecovered;
        event Action<float> OnTelegraphStarted;
        event Action<bool> OnAttackTriggered;
        event Action OnThreatDisengaged;
        event Action OnThreatReset;

        void StartEncounter(ThreatConfig config);
        void AddStrike(string reason);
        void TransitionTo(ThreatPhase newPhase);
        void TriggerAttack(bool isScripted);
        void Disengage();
        void ResetThreatState(bool fullPlaythroughReset = false);
    }
}
