using System;
using System.Collections;
using UnityEngine;
using LastShift.Audio;
using LastShift.Matchstick;
using LastShift.Player;

namespace LastShift.Threat
{
    /// <summary>
    /// Manages checkpoint saving and fast reset transitions upon encounter failure.
    /// Implements THR-10, THR-11, RST-01..RST-07, and P-RETRY-MAX.
    /// </summary>
    public class CheckpointController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ThreatStateController _threatController;
        [SerializeField] private AudioCueController _audioCueController;
        [SerializeField] private MatchstickController _matchstickController;

        [Header("Spawns & Checkpoints")]
        [SerializeField] private Transform _activeCheckpoint;

        [Header("Retry Timing (P-RETRY-MAX < 5.0s)")]
        [SerializeField] private float _fadeDuration = 0.5f;

        public event Action<Transform> OnCheckpointUpdated;
        public event Action OnRespawnStarted;
        public event Action OnRespawnCompleted;

        private void OnEnable()
        {
            if (_threatController != null)
            {
                _threatController.OnAttackTriggered += HandlePlayerAttacked;
            }
        }

        private void OnDisable()
        {
            if (_threatController != null)
            {
                _threatController.OnAttackTriggered -= HandlePlayerAttacked;
            }
        }

        public void SetCheckpoint(Transform checkpoint)
        {
            if (checkpoint == null) return;
            _activeCheckpoint = checkpoint;
            OnCheckpointUpdated?.Invoke(checkpoint);
        }

        private void HandlePlayerAttacked(bool isFullJumpscare)
        {
            // Delay slightly for jump/attack hit impact before restarting
            float delay = isFullJumpscare ? 1.0f : 0.4f;
            StartCoroutine(RespawnRoutine(delay));
        }

        public void TriggerManualRespawn()
        {
            StartCoroutine(RespawnRoutine(0f));
        }

        private IEnumerator RespawnRoutine(float delayBeforeFade)
        {
            OnRespawnStarted?.Invoke();

            if (delayBeforeFade > 0f)
            {
                yield return new WaitForSeconds(delayBeforeFade);
            }

            // RST-01: Stop all playing audio cues immediately
            if (_audioCueController != null)
            {
                _audioCueController.StopAllCues();
            }

            // Lock player during brief fade
            if (_playerController != null)
            {
                _playerController.SetInputLocked(true);
            }

            yield return new WaitForSeconds(_fadeDuration);

            // RST-02: Teleport player to active checkpoint
            if (_activeCheckpoint != null && _playerController != null)
            {
                CharacterController cc = _playerController.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                _playerController.transform.position = _activeCheckpoint.position;
                _playerController.transform.rotation = _activeCheckpoint.rotation;

                if (cc != null) cc.enabled = true;
            }

            // RST-03: Reset matchstick state
            if (_matchstickController != null)
            {
                _matchstickController.ExtinguishMatch();
            }

            // RST-04 & RST-05: Reset threat state (strikes wiped to 0; jumpscare cap preserved)
            if (_threatController != null)
            {
                _threatController.ResetThreatState(fullPlaythroughReset: false);
            }

            yield return new WaitForSeconds(_fadeDuration);

            // Unlock player
            if (_playerController != null)
            {
                _playerController.SetInputLocked(false);
            }

            OnRespawnCompleted?.Invoke();
        }
    }
}
