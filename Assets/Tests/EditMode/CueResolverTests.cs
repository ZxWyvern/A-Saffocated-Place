using System.Collections.Generic;
using NUnit.Framework;
using LastShift.Domain;

namespace LastShift.Tests.EditMode
{
    [TestFixture]
    public class CueResolverTests
    {
        private CueResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _resolver = new CueResolver();
        }

        [Test]
        public void RES01_InputRejected_WhenEncounterInactive()
        {
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = false,
                IsMatchstickReady = true,
                IsCooldownActive = false
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.InputRejected, result);
        }

        [Test]
        public void RES01_InputRejected_WhenControlsLocked()
        {
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsInputLocked = true,
                IsMatchstickReady = true
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.InputRejected, result);
        }

        [Test]
        public void RES01_InputRejected_WhenMatchstickNotReadyOrCooldownActive()
        {
            var ctxNotReady = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = false,
                IsCooldownActive = false
            };
            Assert.AreEqual(ResolutionResult.InputRejected, _resolver.Resolve(ctxNotReady));

            var ctxCooldown = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                IsCooldownActive = true
            };
            Assert.AreEqual(ResolutionResult.InputRejected, _resolver.Resolve(ctxCooldown));
        }

        [Test]
        public void RES02_IgnoresExpiredOrInvalidCues()
        {
            var expiredCue = new AudioCueSnapshot("CUE_ENV_EXPIRED", CueCategory.Threat, isValid: true, remainingDuration: 0f);
            var invalidCue = new AudioCueSnapshot("CUE_ENV_INVALID", CueCategory.Threat, isValid: false, remainingDuration: 1.5f);

            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                ActiveCues = new List<AudioCueSnapshot> { expiredCue, invalidCue }
            };

            var result = _resolver.Resolve(ctx);
            // With no valid cues, fallback to EnvironmentConfirmed
            Assert.AreEqual(ResolutionResult.EnvironmentConfirmed, result);
        }

        [Test]
        public void RES03_OverlappingCues_ResolvedDeterministicallyByPriority()
        {
            var lowPriorityThreat = new AudioCueSnapshot("CUE_THREAT_LOW", CueCategory.Threat, isValid: true, priority: 1, remainingDuration: 2f);
            var highPriorityDecoy = new AudioCueSnapshot("CUE_DECOY_HIGH", CueCategory.Decoy, isValid: true, priority: 5, remainingDuration: 2f);

            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                ActiveCues = new List<AudioCueSnapshot> { lowPriorityThreat, highPriorityDecoy }
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.DecoyRevealed, result);
        }

        [Test]
        public void RES04_ThreatCue_ReturnsThreatConfirmed_WhenNotAttacking()
        {
            var threatCue = new AudioCueSnapshot("CUE_THREAT_01", CueCategory.Threat, isValid: true, remainingDuration: 2f);
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                CurrentThreatPhase = ThreatPhase.Approaching,
                ActiveCues = new List<AudioCueSnapshot> { threatCue }
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.ThreatConfirmed, result);
        }

        [Test]
        public void RES04_ThreatCue_ReturnsAttackTriggered_WhenThreatAttacking()
        {
            var threatCue = new AudioCueSnapshot("CUE_THREAT_01", CueCategory.Threat, isValid: true, remainingDuration: 2f);
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                CurrentThreatPhase = ThreatPhase.Attacking,
                ActiveCues = new List<AudioCueSnapshot> { threatCue }
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.AttackTriggered, result);
        }

        [Test]
        public void RES05_EnvironmentCue_NeverTriggersAttackOrStrike()
        {
            var envCue = new AudioCueSnapshot("CUE_ENV_FAN", CueCategory.Environment, isValid: true, remainingDuration: 3f);
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                CurrentThreatPhase = ThreatPhase.Verifying,
                ActiveCues = new List<AudioCueSnapshot> { envCue }
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.EnvironmentConfirmed, result);
        }

        [Test]
        public void RES06_DecoyCue_ReturnsDecoyRevealed()
        {
            var decoyCue = new AudioCueSnapshot("CUE_DECOY_01", CueCategory.Decoy, isValid: true, remainingDuration: 2.5f);
            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                ActiveCues = new List<AudioCueSnapshot> { decoyCue }
            };

            var result = _resolver.Resolve(ctx);
            Assert.AreEqual(ResolutionResult.DecoyRevealed, result);
        }

        [Test]
        public void RES07_EventDeduplication_RejectsDuplicateEventId()
        {
            var cue = new AudioCueSnapshot("CUE_01", CueCategory.Environment, isValid: true, remainingDuration: 1f);
            var ctx1 = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                EventId = "EVT_STRIKE_12345",
                ActiveCues = new List<AudioCueSnapshot> { cue }
            };

            var first = _resolver.Resolve(ctx1);
            Assert.AreEqual(ResolutionResult.EnvironmentConfirmed, first);

            var ctx2 = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                EventId = "EVT_STRIKE_12345",
                ActiveCues = new List<AudioCueSnapshot> { cue }
            };

            var duplicate = _resolver.Resolve(ctx2);
            Assert.AreEqual(ResolutionResult.InputRejected, duplicate);
        }

        [Test]
        public void RES09_DeterministicEvaluation_YieldsIdenticalResults()
        {
            var threat = new AudioCueSnapshot("CUE_T", CueCategory.Threat, isValid: true, priority: 2, remainingDuration: 1f);
            var env = new AudioCueSnapshot("CUE_E", CueCategory.Environment, isValid: true, priority: 1, remainingDuration: 1f);

            var ctx = new InputResolutionContext
            {
                IsEncounterActive = true,
                IsMatchstickReady = true,
                ActiveCues = new List<AudioCueSnapshot> { threat, env }
            };

            var r1 = _resolver.Resolve(ctx);
            var r2 = _resolver.Resolve(ctx);

            Assert.AreEqual(r1, r2);
            Assert.AreEqual(ResolutionResult.ThreatConfirmed, r1);
        }
    }
}
