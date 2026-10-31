# Project Context — A Saffocated Place (LAST SHIFT)

## 1. Provenance and coverage
Snapshot: 2026-10-31, git commit 6654a282e6536674792d9ce4d460e8bf66054ebd (working tree has uncommitted URP settings modifications).
GDD Authority: `last-shift-gdd/` (LAST SHIFT Single Source of Truth v1.1, status: pre-production, 23 design specifications).
Inspected paths: ProjectSettings/ProjectVersion.txt, Packages/manifest.json, ProjectSettings/EditorBuildSettings.asset, Assets/Scenes/SampleScene.unity, Assets/InputSystem_Actions.inputactions, Assets/Settings/PC_RPAsset.asset, Assets/TutorialInfo/Scripts/Readme.cs, Assembly-CSharp.csproj, and all 23 files in last-shift-gdd/.
Completeness limits: Pre-production milestone. Engine infrastructure, URP settings, and input asset exist. Core gameplay components (matchstick, audio cue controller, resolver, threat AI) defined in GDD are not yet authored in Assets/.

## 2. Pack profile and tier
Profile: compact (lean vertical slice).
Tier: small (observed counts: 1 Unity root, 2 template C# scripts in Assets/TutorialInfo/Scripts/, 1 scene Assets/Scenes/SampleScene.unity, 0 custom asmdefs).

## 3. Protected areas
- [P-001] .git/** | reason: version control repository metadata and internal git objects
- [P-002] Library/**, Temp/**, Logs/**, ProfilerCaptures/** | reason: engine-generated local caches, compile artifacts, and diagnostic logs
- [P-003] unity-workflow-kit/** | reason: generator kit source and tool templates; do not edit during gameplay workflow
- [P-004] last-shift-gdd/** | reason: authoritative Single Source of Truth (SSOT v1.1); design changes require explicit product decision entries
- [P-005] Assets/TutorialInfo/** | reason: template tutorial assets scheduled for cleanup or isolated reference; do not modify

## 4. Intended experience and design conflicts
LAST SHIFT is a 20-30 minute PC psychological horror game (first-person).
Intended mechanics: Player navigates office and looping vents while managing sensory trade-offs. Striking a matchstick provides temporary illumination (P-MATCH-BURN: 1.8s) but ducks audio by 12 dB (P-MATCH-DUCK-DB) and applies a 1500 Hz low-pass filter (P-MATCH-LPF-HZ), hampering threat detection.
Design conflicts: None recorded between documentation files. Open design decisions DEC-01 through DEC-09 in last-shift-gdd/19-open-decisions.md specify prototype defaults (e.g. unlimited matchsticks DEC-01, non-fatal jumpscares DEC-02, partial visual reveals DEC-03).

## 5. Stack and version evidence
- Unity Editor: 6000.3.8f1 (C-001).
- Universal Render Pipeline: 17.3.0 (C-002).
- Unity Input System: 1.18.0 (C-002).
- C# project compilation: Assembly-CSharp.csproj compiles cleanly with 0 errors (C-006, V-001).

## 6. Entry and startup path
Assets/Scenes/SampleScene.unity is the default scene registered in ProjectSettings/EditorBuildSettings.asset (C-003, C-004). Dedicated bootstrap scene and encounter triggers are proposed for implementation.

## 7. Subsystem map
- Matchstick & Audio Cue Resolution: Evaluates audio cues against player actions. Pure C# CueResolver (ARC-05, C-007, proposed C-009) processes valid cues and returns deterministic ResolutionResult (RES-01..RES-09). MatchstickController (proposed C-010) governs ignition state, timers (P-MATCH-BURN: 1.8s, P-MATCH-COOLDOWN: 1.0s), and AudioMixer ducking snapshot transitions (MAT-01..07).
- Threat State & Encounter Controller: ThreatStateController (proposed C-011) manages threat states (Dormant, Approaching, Verifying, Attacking, Disengaged) and tracks strikes up to P-STRIKE-MAX (2) with recovery timer P-STRIKE-RECOVERY (20s) (THR-01..18).

## 8. Representative paths
- Matchstick Ignition & Audio Ducking: Player input -> MatchstickController.TryIgnite() -> snapshot transition to Ducked (-12 dB, 1500 Hz LPF) -> CueResolver.Resolve() snapshot evaluation -> ThreatResponse triggers visual hint or strike -> timer expiration -> snapshot returns to Normal.
- Respawn & Retry: Failure/Jumpscare -> CheckpointController cleans up active audio sources, cancels timers, resets strikes to 0, and restores normal audio mixer snapshot within P-RETRY-MAX (< 5s) (RST-01..07, THR-10).

## 9. Scene, prefab, and config bindings
- Assets/Scenes/SampleScene.unity currently holds standard Main Camera, Directional Light, and Global Volume (C-004).
- Assets/InputSystem_Actions.inputactions defines default input action map.
- Proposed ScriptableObject configurations for AudioCue items and MatchstickParameters will bind tuning parameters from last-shift-gdd/14-parameters.md.

## 10. Feature state split
- Pure C# CueResolver: source missing, wiring missing, execution unverified (proposed C-009).
- Matchstick Controller & Audio Ducking: source missing, wiring missing, execution unverified (proposed C-010, proposed C-012).
- Threat State & Encounter System: source missing, wiring missing, execution unverified (proposed C-011).
- C# Build Verification: source verified (C-001, C-002, C-005), execution verified via verify_compile.py (C-006, V-001).

## 11. Claims
- [C-001 | source_verified | ProjectSettings/ProjectVersion.txt | snapshot] Declared Unity editor version is 6000.3.8f1. | deps: none | invalidates: package_or_unity_version_changed | limit: running editor session not connected
- [C-002 | source_verified | Packages/manifest.json | snapshot] Manifest declares Universal Render Pipeline 17.3.0 and Input System 1.18.0 packages. | deps: none | invalidates: package_or_unity_version_changed | limit: none
- [C-003 | serialized_verified | ProjectSettings/EditorBuildSettings.asset | snapshot] Editor build settings register Assets/Scenes/SampleScene.unity as the single active build scene at index 0. | deps: none | invalidates: entry_path_changed,serialized_dependency_changed | limit: placeholder template scene
- [C-004 | serialized_verified | Assets/Scenes/SampleScene.unity | snapshot] SampleScene serializes default camera, directional light, and global volume without gameplay components. | deps: C-003 | invalidates: serialized_dependency_changed,entry_path_changed | limit: not verified in Play Mode
- [C-005 | source_verified | Assets/TutorialInfo/Scripts/Readme.cs#Readme | snapshot] Template tutorial Readme script exists; no gameplay scripts currently exist under Assets/. | deps: none | invalidates: source_changed | limit: template code only
- [C-006 | execution_verified | Assembly-CSharp.csproj | snapshot] Solution and project compilation check passed with 0 compile errors via verify_compile.py, see V-001. | deps: C-001,C-002 | invalidates: source_changed,toolchain_changed | limit: static dotnet compile check only
- [C-007 | inferred | last-shift-gdd/12-technical-architecture.md | snapshot] GDD architectural requirements (ARC-01..05) mandate pure C# CueResolver without MonoBehaviour dependencies to enable EditMode testing. | deps: none | invalidates: design_authority_changed | limit: GDD documentation authority
- [C-008 | inferred | last-shift-gdd/04-matchstick.md | snapshot] Matchstick mechanics require audio ducking to 12 dB (P-MATCH-DUCK-DB) and 1500 Hz low-pass filter (P-MATCH-LPF-HZ) during burn duration (P-MATCH-BURN: 1.8s). | deps: none | invalidates: design_authority_changed | limit: GDD baseline tuning values
- [C-009 | proposed | Assets/Scripts/Game/CueResolver.cs | snapshot] Pure C# CueResolver implementing ICueResolver to evaluate audio cue snapshots and return deterministic ResolutionResult. | deps: C-007 | limit: implementation pending
- [C-010 | proposed | Assets/Scripts/Game/MatchstickController.cs | snapshot] MatchstickController MonoBehaviour managing ignition state, burn/cooldown timers, and AudioMixer ducking snapshot transitions. | deps: C-008 | limit: implementation pending
- [C-011 | proposed | Assets/Scripts/Game/ThreatStateController.cs | snapshot] ThreatStateController managing threat phases (Dormant, Approaching, Verifying, Attacking, Disengaged) and strike tracking. | deps: C-007 | limit: implementation pending
- [C-012 | unknown | Assets/Audio/AudioMixer.mixer | snapshot] AudioMixer asset and mixer snapshots for ducking and filter transitions are not yet created in the project. | deps: none | limit: requires audio authoring in Unity editor

## 12. Detailed findings
### F-001 No gameplay implementation in initial repository snapshot
Class: missing integration/content | Confidence: high | Basis: static
Evidence: C-003, C-004, C-005
Impact and trigger conditions: Project contains only default URP template assets; scripts, scenes, prefabs, and audio for LAST SHIFT remain to be authored.
Smallest follow-up and acceptance: V-002

### F-002 Pure C# CueResolver logic unverified by EditMode test suite
Class: verification gap | Confidence: high | Basis: static
Evidence: C-007, C-009
Impact and trigger conditions: Core resolution logic (RES-01..RES-09) must run without MonoBehaviour dependencies; test framework exists in manifest (C-002) but no test assembly or EditMode tests exist.
Smallest follow-up and acceptance: V-003

### F-003 AudioMixer ducking and snapshot infrastructure missing
Class: missing integration/content | Confidence: high | Basis: static
Evidence: C-008, C-012
Impact and trigger conditions: Matchstick audio ducking (P-MATCH-DUCK-DB: 12 dB, P-MATCH-LPF-HZ: 1500 Hz) cannot function until an AudioMixer with appropriate ducked snapshot is authored in Unity.
Smallest follow-up and acceptance: V-004

## 12b. Closed findings
(No closed findings in initial generation)

## 13. Overflow finding stubs
F-011 | design conflict | medium | C-008 | reason deferred: open decisions DEC-01..09 require validation in playtesting
F-012 | optional improvement | low | C-002 | reason deferred: VContainer DI framework referenced as optional in GDD ARC-03

## 14. Available tools and tests
- verify_compile.py: Filters dotnet build output for Assembly-CSharp and Assembly-CSharp-Editor.
- context.py: Excerpts C# skeletons and serialized Unity scene/prefab hierarchies.
- validate_pack.py: Verifies structural consistency, claim validity, and GUID references.
- Unity Test Framework: Package com.unity.test-framework 1.6.0 present in manifest.json.

## 15. Not inspected
- Package cache internal sources and compiled assemblies in Library/PackageCache/.
- Built-in shaders and URP render pipeline internal assets outside Assets/Settings/.
- Editor user preferences and layout files in UserSettings/.
- Binary texture data in Assets/TutorialInfo/Icons/URP.png.
