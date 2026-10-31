using System;
using UnityEngine;
using LastShift.Threat;

namespace LastShift.Flow
{
    public enum GameStage
    {
        LVL01_Overtime = 1,
        LVL02_FireAlarm = 2,
        LVL03_TheLoop = 3,
        LVL04_ThePursuit = 4,
        LVL05_FalseExit = 5,
        LVL06_Reveal = 6
    }

    /// <summary>
    /// Master coordinator for the 6-stage level progression.
    /// Implements LVL-01..LVL-10 and prevents softlocks.
    /// </summary>
    public class GameFlowManager : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private GameStage _initialStage = GameStage.LVL01_Overtime;

        [Header("Subsystem References")]
        [SerializeField] private EncounterController _encounterController;
        [SerializeField] private EndingSequenceController _endingSequenceController;
        [SerializeField] private CheckpointController _checkpointController;

        [Header("Environment Controls")]
        [SerializeField] private GameObject _fireAlarmEffects;
        [SerializeField] private GameObject _officeBlockedDoors;
        [SerializeField] private GameObject _ventEntranceGrate;
        [SerializeField] private GameObject _ventExitGrate;

        [Header("Stage Checkpoints")]
        [SerializeField] private Transform _officeSpawn;
        [SerializeField] private Transform _loopSpawn;
        [SerializeField] private Transform _pursuitSpawn;

        public GameStage CurrentStage { get; private set; }

        public event Action<GameStage, GameStage> OnStageChanged;

        private void Start()
        {
            SetStage(_initialStage);
        }

        public void AdvanceStage()
        {
            if (CurrentStage < GameStage.LVL06_Reveal)
            {
                SetStage(CurrentStage + 1);
            }
        }

        public void SetStage(GameStage newStage)
        {
            GameStage prev = CurrentStage;
            CurrentStage = newStage;

            ApplyStageEnvironment(newStage);

            OnStageChanged?.Invoke(prev, newStage);
        }

        private void ApplyStageEnvironment(GameStage stage)
        {
            switch (stage)
            {
                case GameStage.LVL01_Overtime:
                    if (_fireAlarmEffects != null) _fireAlarmEffects.SetActive(false);
                    if (_officeBlockedDoors != null) _officeBlockedDoors.SetActive(false);
                    if (_ventEntranceGrate != null) _ventEntranceGrate.SetActive(true);
                    if (_checkpointController != null && _officeSpawn != null)
                    {
                        _checkpointController.SetCheckpoint(_officeSpawn);
                    }
                    break;

                case GameStage.LVL02_FireAlarm:
                    // Fire alarm starts, exit blocked, vent accessible
                    if (_fireAlarmEffects != null) _fireAlarmEffects.SetActive(true);
                    if (_officeBlockedDoors != null) _officeBlockedDoors.SetActive(true);
                    break;

                case GameStage.LVL03_TheLoop:
                    // Entering vent system
                    if (_fireAlarmEffects != null) _fireAlarmEffects.SetActive(false);
                    if (_checkpointController != null && _loopSpawn != null)
                    {
                        _checkpointController.SetCheckpoint(_loopSpawn);
                    }
                    if (_encounterController != null)
                    {
                        _encounterController.StartLoopSequence();
                    }
                    break;

                case GameStage.LVL04_ThePursuit:
                    // Chase segment
                    if (_checkpointController != null && _pursuitSpawn != null)
                    {
                        _checkpointController.SetCheckpoint(_pursuitSpawn);
                    }
                    if (_encounterController != null)
                    {
                        _encounterController.StartPursuitSequence();
                    }
                    break;

                case GameStage.LVL05_FalseExit:
                    // False exit reached
                    if (_encounterController != null)
                    {
                        _encounterController.StopAllEncounters();
                    }
                    // Transition to ending
                    SetStage(GameStage.LVL06_Reveal);
                    break;

                case GameStage.LVL06_Reveal:
                    // Ending sequence triggered
                    if (_endingSequenceController != null)
                    {
                        _endingSequenceController.PlayEndingSequence();
                    }
                    break;
            }
        }
    }
}
