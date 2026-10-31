using System.Collections.Generic;
using UnityEngine;
using LastShift.Audio;
using LastShift.Config;
using LastShift.Domain;
using LastShift.Flow;
using LastShift.Matchstick;
using LastShift.Player;
using LastShift.Threat;
using LastShift.UI;

namespace LastShift.Level
{
    /// <summary>
    /// Composition root / bootstrapper for LAST SHIFT playable MVP.
    /// Wires runtime subsystems, ScriptableObject configs (14-parameters.md),
    /// level geometry, and input resolution loop without singletons (ARC-01..05).
    /// </summary>
    public class MVPBootstrap : MonoBehaviour
    {
        [Header("Level Generator")]
        [SerializeField] private GreyboxLevelBuilder _levelBuilder;

        [Header("Configurations (SSOT 14-parameters.md)")]
        [SerializeField] private MatchstickConfig _matchstickConfig;
        [SerializeField] private ThreatConfig _threatConfig;

        [Header("Audio Cue Data")]
        [SerializeField] private AudioCueData _envPipeCue;
        [SerializeField] private AudioCueData _threatClawCue;
        [SerializeField] private AudioCueData _decoyRattleCue;

        [Header("Subsystem References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private MatchstickController _matchstickController;
        [SerializeField] private AudioCueController _audioCueController;
        [SerializeField] private ThreatStateController _threatController;
        [SerializeField] private ThreatResponse _threatResponse;
        [SerializeField] private CheckpointController _checkpointController;
        [SerializeField] private EncounterController _encounterController;
        [SerializeField] private GameFlowManager _flowManager;
        [SerializeField] private EndingSequenceController _endingSequenceController;
        [SerializeField] private HUDController _hudController;

        private void Awake()
        {
            BootstrapConfigs();
            BootstrapLevel();
            BootstrapPlayerAndSubsystems();
            BootstrapFlowAndInteractions();
        }

        private void BootstrapConfigs()
        {
            if (_matchstickConfig == null)
            {
                _matchstickConfig = ScriptableObject.CreateInstance<MatchstickConfig>();
            }

            if (_threatConfig == null)
            {
                _threatConfig = ScriptableObject.CreateInstance<ThreatConfig>();
            }

            if (_envPipeCue == null)
            {
                _envPipeCue = ScriptableObject.CreateInstance<AudioCueData>();
                _envPipeCue.Initialize("CUE_ENV_PIPE", CueCategory.Environment, 10.0f, movesTowardPlayer: false, priority: 1);
            }

            if (_threatClawCue == null)
            {
                _threatClawCue = ScriptableObject.CreateInstance<AudioCueData>();
                _threatClawCue.Initialize("CUE_THR_CLAW", CueCategory.Threat, 8.0f, movesTowardPlayer: true, priority: 10);
            }

            if (_decoyRattleCue == null)
            {
                _decoyRattleCue = ScriptableObject.CreateInstance<AudioCueData>();
                _decoyRattleCue.Initialize("CUE_DEC_RATTLE", CueCategory.Decoy, 5.0f, movesTowardPlayer: false, priority: 5);
            }
        }

        private void BootstrapLevel()
        {
            if (_levelBuilder == null)
            {
                var builderGO = new GameObject("Level_Greybox");
                builderGO.transform.SetParent(transform);
                _levelBuilder = builderGO.AddComponent<GreyboxLevelBuilder>();
            }

            _levelBuilder.BuildLevelIfEmpty();
        }

        private void BootstrapPlayerAndSubsystems()
        {
            // Player
            if (_playerController == null)
            {
                var playerGO = new GameObject("Player");
                playerGO.tag = "Player";
                var cc = playerGO.AddComponent<CharacterController>();
                cc.height = 1.8f;
                cc.radius = 0.4f;

                var camGO = new GameObject("PlayerCamera");
                camGO.transform.SetParent(playerGO.transform);
                camGO.transform.localPosition = new Vector3(0f, 1.6f, 0f);
                var cam = camGO.AddComponent<Camera>();
                cam.nearClipPlane = 0.1f;
                camGO.AddComponent<AudioListener>();

                _playerController = playerGO.AddComponent<PlayerController>();

                // Matchstick
                var matchGO = new GameObject("Matchstick");
                matchGO.transform.SetParent(camGO.transform);
                matchGO.transform.localPosition = new Vector3(0.3f, -0.25f, 0.5f);
                _matchstickController = matchGO.AddComponent<MatchstickController>();
                matchGO.AddComponent<MatchstickVisuals>();
                matchGO.AddComponent<AudioDucker>();
            }

            // Audio Cue Controller
            if (_audioCueController == null)
            {
                var audioGO = new GameObject("AudioCueController");
                audioGO.transform.SetParent(transform);
                _audioCueController = audioGO.AddComponent<AudioCueController>();
            }

            // Threat Controller & Response
            if (_threatController == null)
            {
                var threatGO = new GameObject("ThreatSubsystem");
                threatGO.transform.SetParent(transform);
                _threatController = threatGO.AddComponent<ThreatStateController>();
                _threatResponse = threatGO.AddComponent<ThreatResponse>();
                _checkpointController = threatGO.AddComponent<CheckpointController>();
            }

            // Connect CueResolver to MatchstickController (ARC-01, RES-01..09)
            var resolver = new CueResolver();
            _matchstickController.Initialize(resolver, ProvideResolutionContext);

            // Connect Checkpoints
            if (_levelBuilder != null && _levelBuilder.OfficePlayerSpawn != null)
            {
                _checkpointController.SetCheckpoint(_levelBuilder.OfficePlayerSpawn);
            }
        }

        private InputResolutionContext ProvideResolutionContext()
        {
            var snapshots = _audioCueController != null ? _audioCueController.GetActiveSnapshots() : new List<AudioCueSnapshot>();
            var threatPhase = _threatController != null ? _threatController.CurrentPhase : ThreatPhase.Dormant;
            var matchState = _matchstickController != null ? _matchstickController.CurrentState : MatchstickState.Unlit;
            bool isCooldown = _matchstickController != null && _matchstickController.IsCooldownActive;

            return new InputResolutionContext
            {
                EventId = System.Guid.NewGuid().ToString("N"),
                IsMatchstickReady = matchState == MatchstickState.Unlit && !isCooldown,
                IsCooldownActive = isCooldown,
                IsInputLocked = false,
                IsEncounterActive = threatPhase != ThreatPhase.Dormant,
                CurrentThreatPhase = threatPhase,
                ActiveCues = snapshots,
                ExposedCueIds = _audioCueController != null ? _audioCueController.ExposedCueIds : new HashSet<string>()
            };
        }

        private void BootstrapFlowAndInteractions()
        {
            if (_flowManager == null)
            {
                var flowGO = new GameObject("GameFlowManager");
                flowGO.transform.SetParent(transform);
                _flowManager = flowGO.AddComponent<GameFlowManager>();
                _encounterController = flowGO.AddComponent<EncounterController>();
                _endingSequenceController = flowGO.AddComponent<EndingSequenceController>();
            }

            // Wire Interactables from Level
            if (_levelBuilder != null)
            {
                if (_levelBuilder.MotherNoteInteractable != null)
                {
                    _levelBuilder.MotherNoteInteractable.OnInteracted += (item) =>
                    {
                        if (_hudController != null)
                        {
                            _hudController.ShowDocumentModal("Note from Mom:\n\"Stay safe honey. Don't stay at work too late again. Remember to eat dinner.\"");
                        }
                    };
                }

                if (_levelBuilder.DeadlineNoteInteractable != null)
                {
                    _levelBuilder.DeadlineNoteInteractable.OnInteracted += (item) =>
                    {
                        if (_hudController != null)
                        {
                            _hudController.ShowDocumentModal("URGENT MEMORANDUM:\n\"CRITICAL DEADLINE: TONIGHT 23:59. Delivery is mandatory. No overtime claims accepted.\"");
                        }
                    };
                }

                if (_levelBuilder.ComputerInteractable != null)
                {
                    _levelBuilder.ComputerInteractable.OnInteracted += (item) =>
                    {
                        // Interacting with computer in LVL-01 advances to Fire Alarm (LVL-02)
                        if (_flowManager.CurrentStage == GameStage.LVL01_Overtime)
                        {
                            _flowManager.AdvanceStage();
                        }
                    };
                }

                if (_levelBuilder.VentEntranceInteractable != null)
                {
                    _levelBuilder.VentEntranceInteractable.OnInteracted += (item) =>
                    {
                        // Entering vent transitions from LVL-02 Fire Alarm to LVL-03 The Loop
                        if (_flowManager.CurrentStage == GameStage.LVL02_FireAlarm)
                        {
                            _flowManager.AdvanceStage();
                            if (_playerController != null && _levelBuilder.LoopPlayerSpawn != null)
                            {
                                _playerController.TeleportTo(_levelBuilder.LoopPlayerSpawn.position, _levelBuilder.LoopPlayerSpawn.rotation);
                            }
                        }
                    };
                }

                if (_levelBuilder.VentExitInteractable != null)
                {
                    _levelBuilder.VentExitInteractable.OnInteracted += (item) =>
                    {
                        // Exiting vent transitions to False Exit & Ending sequence
                        _flowManager.AdvanceStage();
                    };
                }
            }
        }
    }
}
