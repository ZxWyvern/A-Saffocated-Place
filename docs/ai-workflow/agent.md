# Agent Profile — A Saffocated Place (LAST SHIFT)

Start at `Assets/Scenes/SampleScene.unity` (provisional, see C-003, C-004). Facts: `project-context.md`. Status: `validation-matrix.md`.
Authority: `last-shift-gdd/` SSOT v1.1. Architecture policy: conform (ARC-01..ARC-05).
Protected areas: P-001, P-002, P-003, P-004, P-005.

## Operating loop
1. Read applicable repository instructions, this file, and `sessions/<branch-slug>__<actor-slug>.md` when resuming. Never write a shared handoff.
2. List routes with `python docs/ai-workflow/tools/context.py . --pack docs/ai-workflow`, then select `--route R-###` for the current task.
3. Check claim freshness before using claims. Read relevant source and contracts; without Python, follow route IDs and claim dependencies in canonical files.
4. Implement the smallest complete change following ARC-01..05. Keep CueResolver free of MonoBehaviour dependencies to enable EditMode testing.
5. Verify changes using `python docs/ai-workflow/tools/verify_compile.py .` and `python docs/ai-workflow/tools/validate_pack.py .`. Update only your own session handoff.

## Task routes
### R-001 Cue resolution pure C# logic and EditMode test suite
Start: proposed Assets/Scripts/Game/CueResolver.cs (C-007, C-009)
State owner and scene/assets: ICueResolver, no scene dependencies
Invariants: Deterministic single resolution per input; Environment cues never strike; pure C# without UnityEngine scene dependencies (ARC-05, RES-01..RES-09)
Protected areas: P-004
Required integration step: Implement CueResolver with EditMode tests under Tests/EditMode/
Checks: V-003

### R-002 Matchstick controller and audio ducking implementation
Start: proposed Assets/Scripts/Game/MatchstickController.cs (C-008, C-010)
State owner and scene/assets: MatchstickController, proposed Assets/Audio/AudioMixer.mixer
Invariants: Burn duration 1.8s (P-MATCH-BURN), cooldown 1.0s (P-MATCH-COOLDOWN); ducking snapshot active only during burn (MAT-01..07)
Protected areas: P-004
Required integration step: Create AudioMixer with ducking snapshot; wire MatchstickController events
Checks: V-004

### R-003 Threat state machine and encounter manager
Start: proposed Assets/Scripts/Game/ThreatStateController.cs (C-011)
State owner and scene/assets: ThreatStateController, proposed EncounterController
Invariants: Max 2 strikes (P-STRIKE-MAX); approach time 8s (P-THREAT-APPROACH); telegraph minimum 1.5s (P-TELEGRAPH-MIN; THR-01..18)
Protected areas: P-004
Required integration step: Integrate with CueResolver results and strike tracking
Checks: V-005

## Increments
### I-001 Implement CueResolver and verify with EditMode tests
Hard depends on: none
Soft depends on: none
Verification depends on: V-001, V-003
Exit check: V-003
Status: not_started
