# Audio Cues and CueResolver Specification

Authority: `last-shift-gdd/05-audio-cues.md`, `last-shift-gdd/06-threat-and-chase.md`, `last-shift-gdd/13-data-contracts-and-resolution.md`.

## Cue Categories and Invariants

| Category | Max Variants | Spatial Behavior | Gameplay Consequence |
|---|---|---|---|
| **Threat** | 3 (`P-CUE-MAX-THREAT`) | `MovesTowardPlayer = true` | Confirms threat; if ignored, threat approaches within 8s (`P-THREAT-APPROACH`). |
| **Environment** | 4 (`P-CUE-MAX-ENV`) | `MovesTowardPlayer = false` (Fixed location) | Confirms ambient source; NEVER causes strike or death (CUE-08, RES-05). |
| **Decoy** | 3 (`P-CUE-MAX-DECOY`) | `MovesTowardPlayer = false` (Fixed location) | Replicas of threat audio. First exposure never causes a strike (CUE-04, RES-06); repeat errors cause 1 strike. |

## CueResolver Resolution Rules (`RES-01..09`)

`CueResolver` MUST output exactly one `ResolutionResult` per matchstick input:

1. **`RES-01`**: Input MUST be rejected (`InputRejected`) if encounter is inactive, player controls are locked, match is not ready, or cooldown is active.
2. **`RES-02`**: Resolver MUST only evaluate active, unexpired cues.
3. **`RES-03`**: If multiple cues overlap, encounter-defined priority determines the single deterministic outcome.
4. **`RES-04`**: Threat cue does not automatically trigger an attack; resolver must check current `ThreatState`.
5. **`RES-05`**: Environment cues MUST NEVER cause strikes or player death.
6. **`RES-06`**: Decoys trigger a strike only if the specific `CueId` was previously introduced in an earlier encounter.
7. **`RES-07`**: Event deduplication prevents duplicate responses from a single input event.
8. **`RES-08`**: Resolver MUST NOT depend on Unity APIs (pure C# for EditMode testability).
9. **`RES-09`**: Resolver MUST be strictly deterministic.

## Reset and Respawn Rules (`RST-01..07`)

When restarting an encounter or respawning from death:
- `RST-01`: Stop all running audio cues.
- `RST-02`: Cancel all active coroutines and timers.
- `RST-03`: Reset threat state to `Dormant` and reset strikes to 0.
- `RST-04`: Restore AudioMixer snapshot to normal.
- `RST-05`: Suppress lingering events from the old encounter.
- `RST-06`: Entire retry cycle must complete in under 5.0 seconds (`P-RETRY-MAX`).
- `RST-07`: Player exposure history for `CueId` is preserved across retries within a playthrough so that first mistakes on new cues remain forgiving.
