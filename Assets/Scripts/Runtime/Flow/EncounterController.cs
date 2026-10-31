using System;
using System.Collections;
using UnityEngine;
using LastShift.Audio;
using LastShift.Config;
using LastShift.Domain;
using LastShift.Threat;

namespace LastShift.Flow
{
    /// <summary>
    /// Coordinates curated audio encounters for LVL-03 (The Loop) and LVL-04 (The Pursuit).
    /// Implements THR-02, THR-13a, LVL-03, LVL-04, LVL-11..14.
    /// </summary>
    public class EncounterController : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private AudioCueController _audioCueController;
        [SerializeField] private ThreatStateController _threatController;

        [Header("Cue Configurations")]
        [SerializeField] private AudioCueData _envPipeDripCue;
        [SerializeField] private AudioCueData _threatClawScrapeCue;
        [SerializeField] private AudioCueData _decoyRattleCue;
        [SerializeField] private AudioCueData _pursuitThreatCue;

        [Header("Spatial Audio Positions")]
        [SerializeField] private Transform _loopCueOrigin;
        [SerializeField] private Transform _restRoomOrigin;
        [SerializeField] private Transform _pursuitCueOrigin;

        [Header("Tuning Settings")]
        [SerializeField] private float _restRoomDuration = 30f; // P-REST-DURATION (30..45s)

        public event Action<int> OnEncounterStepChanged;
        public event Action OnLoopEncountersCompleted;
        public event Action OnPursuitCompleted;

        private int _currentStep = 0;
        private Coroutine _loopRoutine;

        public void StartLoopSequence()
        {
            if (_loopRoutine != null)
            {
                StopCoroutine(_loopRoutine);
            }
            _loopRoutine = StartCoroutine(LoopSequenceRoutine());
        }

        public void StartPursuitSequence()
        {
            StartCoroutine(PursuitSequenceRoutine());
        }

        public void StopAllEncounters()
        {
            if (_loopRoutine != null)
            {
                StopCoroutine(_loopRoutine);
                _loopRoutine = null;
            }

            if (_audioCueController != null)
            {
                _audioCueController.StopAllCues();
            }

            if (_threatController != null)
            {
                _threatController.ResetThreatState(false);
            }
        }

        private IEnumerator LoopSequenceRoutine()
        {
            // Step 1: Introduce Environment cue (Pipes dripping / hum)
            _currentStep = 1;
            OnEncounterStepChanged?.Invoke(_currentStep);

            if (_audioCueController != null && _envPipeDripCue != null)
            {
                Vector3 pos = _loopCueOrigin != null ? _loopCueOrigin.position : transform.position;
                _audioCueController.PlayCue(_envPipeDripCue, pos);
            }
            yield return new WaitForSeconds(5.0f);

            // Step 2: Introduce Threat cue (Approaching claw scrape)
            _currentStep = 2;
            OnEncounterStepChanged?.Invoke(_currentStep);

            if (_threatController != null)
            {
                _threatController.TransitionTo(ThreatPhase.Approaching);
            }

            if (_audioCueController != null && _threatClawScrapeCue != null)
            {
                Vector3 pos = _loopCueOrigin != null ? _loopCueOrigin.position + Vector3.forward * 5f : transform.position;
                _audioCueController.PlayCue(_threatClawScrapeCue, pos);
            }

            // Wait for player reaction or approach window
            float waitTimer = 0f;
            while (waitTimer < 6.0f)
            {
                if (_threatController != null && _threatController.CurrentPhase == ThreatPhase.Disengaged)
                {
                    break;
                }
                waitTimer += Time.deltaTime;
                yield return null;
            }

            // Step 3: Scripted non-lethal jumpscare (THR-13a, THR-17 exception)
            _currentStep = 3;
            OnEncounterStepChanged?.Invoke(_currentStep);
            yield return new WaitForSeconds(3.0f);

            if (_threatController != null)
            {
                // Scripted jumpscare, not fatal, does not deduct strike or restart
                _threatController.TriggerAttack(isScripted: true);
            }
            yield return new WaitForSeconds(2.0f);

            if (_threatController != null)
            {
                _threatController.Disengage();
            }

            // Step 4: Ruang Jeda (LVL-11..13): Quiet drone, calm ambient, P-REST-DURATION
            _currentStep = 4;
            OnEncounterStepChanged?.Invoke(_currentStep);
            yield return new WaitForSeconds(Mathf.Min(_restRoomDuration, 10.0f)); // 10s for MVP pace

            // Step 5: First threat after rest room with heightened pressure (LVL-14)
            _currentStep = 5;
            OnEncounterStepChanged?.Invoke(_currentStep);

            if (_threatController != null)
            {
                _threatController.TransitionTo(ThreatPhase.Approaching);
            }

            if (_audioCueController != null && _threatClawScrapeCue != null)
            {
                Vector3 pos = _loopCueOrigin != null ? _loopCueOrigin.position + Vector3.back * 4f : transform.position;
                _audioCueController.PlayCue(_threatClawScrapeCue, pos);
            }

            yield return new WaitForSeconds(5.0f);
            OnLoopEncountersCompleted?.Invoke();
        }

        private IEnumerator PursuitSequenceRoutine()
        {
            // LVL-04: The Pursuit - Active chase under audio pressure
            if (_threatController != null)
            {
                _threatController.TransitionTo(ThreatPhase.Approaching);
            }

            if (_audioCueController != null && _pursuitThreatCue != null)
            {
                Vector3 pos = _pursuitCueOrigin != null ? _pursuitCueOrigin.position : transform.position;
                _audioCueController.PlayCue(_pursuitThreatCue, pos);
            }

            // Pursuit timer
            yield return new WaitForSeconds(10.0f);

            OnPursuitCompleted?.Invoke();
        }
    }
}
