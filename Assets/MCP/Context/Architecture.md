# Technical Architecture — LAST SHIFT

Authority: `last-shift-gdd/12-technical-architecture.md` and `last-shift-gdd/13-data-contracts-and-resolution.md`.

## System Overview

LAST SHIFT employs a clean, decoupled architecture separating pure C# business and game logic from Unity's presentation layer:
- **Separation of Concerns:** Core rules (such as cue resolution and threat state transitions) are implemented as pure C# classes without `MonoBehaviour` dependencies (ARC-05).
- **No Global Singletons:** Global singletons and `FindObjectOfType` calls are strictly prohibited (ARC-02).
- **Explicit Interfaces & Events:** Inter-module communication uses explicit interfaces and event subscriptions (ARC-01).
- **Data-Driven Configuration:** All gameplay definitions, audio cue profiles, and tuning parameters are serialized via `ScriptableObject` assets (ARC-04) reading values strictly from `14-parameters.md`.

## Core Systems & Component Map

| Component | Responsibility | Invariants / Prohibitions |
|---|---|---|
| `CueResolver` (`ICueResolver`) | Pure C# class evaluating audio cue snapshots and threat phase upon match ignition. Returns deterministic `ResolutionResult` (RES-01..RES-09). | Pure C# without UnityEngine API; cannot play audio or spawn VFX (RES-08). |
| `MatchstickController` (`IMatchstickService`) | Handles player ignition inputs, burn timer (1.8s `P-MATCH-BURN`), cooldown (1.0s `P-MATCH-COOLDOWN`), and requests AudioMixer ducking transitions (MAT-01..07). | Must not classify audio cues; delegates resolution to `CueResolver`. |
| `ThreatStateController` (`IThreatStateService`) | Tracks threat states (`Dormant`, `Approaching`, `Verifying`, `Attacking`, `Disengaged`), strike accumulation, and strike cooldown recovery (THR-04..09). | Does not accept direct player input. |
| `AudioCueController` (`IAudioCueService`) | Spawns and manages 3D spatialized audio playback, cue lifetimes, and positional tracking (`MovesTowardPlayer`). | Must not compute gameplay resolution results. |
| `ThreatResponse` (`IThreatResponse`) | Applies resolver results: triggers visual hints, plays proximity audio warnings, applies strikes, or triggers monster attacks. | Must not alter cue classification rules. |
| `EncounterController` & `GameFlow` | Sequences level progression, sets active cue pools, and manages triggers for scripted events and chase sequences. | Must not directly manipulate individual audio sources. |
| `CheckpointController` | Executes clean state reset upon failure or respawn within 5 seconds (`P-RETRY-MAX`) (RST-01..07). | Stops previous cues, cancels running timers/coroutines, and restores normal audio mixer snapshot. |
| `TelemetryService` (`ITelemetryService`) | Logs playtest event timestamps, cue responses, and failure rates to a local CSV file. | Disabled in release builds (PERF-05); never influences gameplay logic. |

## Runtime Data Flow

1. `EncounterController` starts an encounter and registers active `AudioCue` ScriptableObjects.
2. `AudioCueController` instantiates spatial audio playback with assigned `CueId`, duration, and movement characteristics.
3. Player triggers matchstick ignition via `MatchstickController.TryIgnite()`.
4. `MatchstickController` switches `AudioMixer` to the ducked snapshot (-12 dB ducking, 1500 Hz low-pass filter).
5. `CueResolver.Resolve()` inspects the valid cue snapshot, exposure history, and current threat phase.
6. Resolver outputs a single deterministic `ResolutionResult` (`ThreatConfirmed`, `EnvironmentConfirmed`, `DecoyRevealed`, `AttackTriggered`, or `InputRejected`).
7. `ThreatResponse` applies the result (revealing visual clues or adding a strike).
8. Match burns out -> `AudioMixer` returns to the normal snapshot via a 0.4s release transition (`P-MATCH-DUCK-RELEASE`).
9. On player failure, `CheckpointController` executes `RST-01..07` to clean up all audio and state before respawning.

## Data Contracts (`13-data-contracts-and-resolution.md`)

- `IAudioCueService`: `PlayCue(cueId)`, `StopCue(instanceId)`, events `CueStarted`, `CueEnded`.
- `IAudioMixerService`: `TransitionToSnapshot(snapshotId, duration)`.
- `IThreatStateService`: `CurrentState`, `Strikes`, `SetState()`, `AddStrike()`, event `ThreatStateChanged`.
- `IMatchstickService`: `TryIgnite()`, `Extinguish()`, `CurrentState`.
- `ICueResolver`: `Resolve(inputContext)` returning `ResolutionResult`.
- `IThreatResponse`: `Apply(result, encounterContext)`.
- `IEncounterService`: `StartEncounter(id)`, `CompleteEncounter()`, `ResetEncounter()`.

## Dependencies & Tech Stack

- **Unity Version:** 6000.3.8f1 (Unity 6).
- **Render Pipeline:** Universal Render Pipeline (URP 17.3.0).
- **Input System:** com.unity.inputsystem 1.18.0.
- **Audio:** Unity Audio Engine with hierarchical `AudioMixer` routing.
- **Testing:** com.unity.test-framework 1.6.0 (EditMode tests for all `RES-01..09` rules).
- **Editor AI Bridge:** AB Unity MCP plugin (`com.anklebr.unity-mcp`).

## Build Targets

- **Primary Platform:** PC (Windows Standalone, x64).
- **Graphics API:** DirectX 11 / 12, Vulkan.
- **Target Framerate:** 60 FPS stable; profiling on standalone builds rather than Editor only (PERF-06).
