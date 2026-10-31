using System;
using System.Collections.Generic;

namespace LastShift.Domain
{
    /// <summary>
    /// Pure C# implementation of audio cue resolution.
    /// Implements RES-01 through RES-09 from SSOT 13-data-contracts-and-resolution.md.
    /// </summary>
    public sealed class CueResolver : ICueResolver
    {
        private readonly HashSet<string> _processedEventIds = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>
        /// Clears event deduplication cache (useful across full game resets / test runs).
        /// </summary>
        public void ClearEventHistory()
        {
            _processedEventIds.Clear();
        }

        public ResolutionResult Resolve(InputResolutionContext context)
        {
            if (context == null)
            {
                return ResolutionResult.InputRejected;
            }

            // RES-01: Reject input if encounter inactive, input locked, match unready, or cooldown active
            if (!context.IsEncounterActive ||
                context.IsInputLocked ||
                !context.IsMatchstickReady ||
                context.IsCooldownActive)
            {
                return ResolutionResult.InputRejected;
            }

            // RES-07: Event ID deduplication
            if (!string.IsNullOrEmpty(context.EventId))
            {
                if (_processedEventIds.Contains(context.EventId))
                {
                    return ResolutionResult.InputRejected;
                }
                _processedEventIds.Add(context.EventId);
            }

            // If the monster is already in Attacking phase, matchstick cannot prevent attack
            if (context.CurrentThreatPhase == ThreatPhase.Attacking)
            {
                return ResolutionResult.AttackTriggered;
            }

            // RES-02: Filter valid, unexpired cues
            List<AudioCueSnapshot> validCues = null;
            if (context.ActiveCues != null)
            {
                for (int i = 0; i < context.ActiveCues.Count; i++)
                {
                    var cue = context.ActiveCues[i];
                    if (cue.IsValid && cue.RemainingDuration > 0f)
                    {
                        validCues ??= new List<AudioCueSnapshot>();
                        validCues.Add(cue);
                    }
                }
            }

            // No active valid cues present during match strike -> Environment fallback
            if (validCues == null || validCues.Count == 0)
            {
                return ResolutionResult.EnvironmentConfirmed;
            }

            // RES-03: Deterministic priority selection among overlapping cues
            AudioCueSnapshot selectedCue = SelectPriorityCue(validCues);

            // RES-04, RES-05, RES-06: Resolve by cue category
            switch (selectedCue.Category)
            {
                case CueCategory.Threat:
                    // RES-04: Threat cue does not automatically mean attack; check threat phase
                    return context.CurrentThreatPhase == ThreatPhase.Attacking
                        ? ResolutionResult.AttackTriggered
                        : ResolutionResult.ThreatConfirmed;

                case CueCategory.Environment:
                    // RES-05: Environment cue MUST NOT cause strike or game over
                    return ResolutionResult.EnvironmentConfirmed;

                case CueCategory.Decoy:
                    // RES-06: Decoy cue revealed
                    return ResolutionResult.DecoyRevealed;

                default:
                    return ResolutionResult.EnvironmentConfirmed;
            }
        }

        private static AudioCueSnapshot SelectPriorityCue(List<AudioCueSnapshot> cues)
        {
            AudioCueSnapshot best = cues[0];

            for (int i = 1; i < cues.Count; i++)
            {
                var candidate = cues[i];

                if (candidate.Priority > best.Priority)
                {
                    best = candidate;
                }
                else if (candidate.Priority == best.Priority)
                {
                    // Deterministic tie-breaker: Threat > Decoy > Environment
                    if (GetCategoryWeight(candidate.Category) > GetCategoryWeight(best.Category))
                    {
                        best = candidate;
                    }
                }
            }

            return best;
        }

        private static int GetCategoryWeight(CueCategory category)
        {
            return category switch
            {
                CueCategory.Threat => 3,
                CueCategory.Decoy => 2,
                CueCategory.Environment => 1,
                _ => 0
            };
        }
    }
}
