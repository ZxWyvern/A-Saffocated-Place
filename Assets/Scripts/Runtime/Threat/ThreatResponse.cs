using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using LastShift.Audio;
using LastShift.Config;
using LastShift.Domain;
using LastShift.Matchstick;

namespace LastShift.Threat
{
    /// <summary>
    /// Evaluates CueResolver results and applies strikes or disengagements to ThreatStateController.
    /// Implements THR-08, THR-09, CUE-04, and P-REVEAL-DURATION.
    /// </summary>
    public class ThreatResponse : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ThreatStateController _threatController;
        [SerializeField] private AudioCueController _audioCueController;
        [SerializeField] private MatchstickController _matchstickController;

        [Header("Monster Visual Presentation (THR-01)")]
        [SerializeField] private GameObject _monsterSilhouette;
        [SerializeField] private float _revealDuration = 1.0f; // P-REVEAL-DURATION (0.5..1.5s)

        public event Action OnThreatRepelled;
        public event Action<string> OnStrikeAwarded;
        public event Action<string> OnFirstMistakeWarningOnly;
        public event Action OnMonsterRevealed;

        private void OnEnable()
        {
            if (_matchstickController != null)
            {
                _matchstickController.OnCueResolved += HandleCueResolved;
            }
        }

        private void OnDisable()
        {
            if (_matchstickController != null)
            {
                _matchstickController.OnCueResolved -= HandleCueResolved;
            }
        }

        /// <summary>
        /// THR-08: Strike diberikan oleh ThreatResponse berdasarkan hasil CueResolver.
        /// </summary>
        public void ProcessResolution(ResolutionResult result, AudioCueSnapshot? activeCue)
        {
            if (_threatController == null) return;

            switch (result)
            {
                case ResolutionResult.ThreatConfirmed:
                    // Player successfully identified threat and burned matchstick
                    TriggerMonsterRevealAndRepel();
                    _threatController.Disengage();
                    OnThreatRepelled?.Invoke();
                    break;

                case ResolutionResult.DecoyRevealed:
                    HandleDecoyRevealed(activeCue);
                    break;

                case ResolutionResult.AttackTriggered:
                    _threatController.TriggerAttack(isScripted: false);
                    break;

                case ResolutionResult.EnvironmentConfirmed:
                case ResolutionResult.InputRejected:
                    // Harmless resolution, no strike
                    break;
            }
        }

        private void HandleCueResolved(ResolutionResult result)
        {
            // For general ignition resolution, find primary active cue snapshot if any
            AudioCueSnapshot? primarySnapshot = null;
            if (_audioCueController != null)
            {
                var snapshots = _audioCueController.GetActiveSnapshots();
                if (snapshots.Count > 0)
                {
                    primarySnapshot = snapshots[0];
                }
            }

            ProcessResolution(result, primarySnapshot);
        }

        private void HandleDecoyRevealed(AudioCueSnapshot? cueSnapshot)
        {
            if (cueSnapshot.HasValue)
            {
                var cue = cueSnapshot.Value;
                // THR-09 & CUE-04: Decoy hanya memberi strike jika CueId sudah pernah dipaparkan sebelumnya
                bool isExposed = _audioCueController != null &&
                                 _audioCueController.ExposedCueIds != null &&
                                 _audioCueController.ExposedCueIds.Contains(cue.CueId);

                if (!isExposed)
                {
                    // First mistake on this decoy - safe warning only, no strike!
                    OnFirstMistakeWarningOnly?.Invoke(cue.CueId);
                    return;
                }

                // Repeat decoy mistake -> award strike
                _threatController.AddStrike($"Mistake on decoy: {cue.CueId}");
                OnStrikeAwarded?.Invoke(cue.CueId);
            }
            else
            {
                _threatController.AddStrike("Decoy mistaken");
                OnStrikeAwarded?.Invoke("UnknownDecoy");
            }
        }

        private void TriggerMonsterRevealAndRepel()
        {
            if (_monsterSilhouette != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(RevealMonsterRoutine());
            }
        }

        private IEnumerator RevealMonsterRoutine()
        {
            _monsterSilhouette.SetActive(true);
            OnMonsterRevealed?.Invoke();
            yield return new WaitForSeconds(_revealDuration);
            _monsterSilhouette.SetActive(false);
        }
    }
}
