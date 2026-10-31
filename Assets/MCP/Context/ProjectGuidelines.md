# Project Guidelines — LAST SHIFT

Authority: `last-shift-gdd/` and `docs/ai-workflow/`.

## Single Source of Truth (SSOT v1.1)

1. `last-shift-gdd/` is the authoritative source for all gameplay rules and design intent.
2. **Numbers only in `14-parameters.md`:** All gameplay constants, durations, decibels, and distances must be loaded via ScriptableObject configs referencing `P-*` parameter IDs. Never hardcode numeric gameplay values in scripts.
3. If code, tickets, or discussions conflict with the GDD, the GDD prevails.
4. Unresolved choices reside in `19-open-decisions.md` with explicit prototype defaults (`DEC-*`). Implement the prototype default without inventing behaviors.

## Naming Conventions

- **Classes, Structs, Enums:** `PascalCase` (e.g. `CueResolver`, `ThreatStateController`).
- **Interfaces:** `PascalCase` with leading `I` (e.g. `ICueResolver`, `IMatchstickService`).
- **Methods & Public Properties:** `PascalCase` (e.g. `TryIgnite()`, `ResolutionResult`).
- **Private & Protected Fields:** `_camelCase` with underscore prefix (e.g. `_burnDuration`, `_threatState`).
- **Constants:** `UPPER_SNAKE_CASE` or `PascalCase`.
- **ScriptableObject Assets:** `PascalCase` with descriptive category suffix (e.g. `AudioCue_PipeRattle_Env.asset`).

## Code Style & Architecture Rules

- **ARC-01:** Use explicit interfaces, events, and clear ownership boundaries.
- **ARC-02:** Do not use global singletons (`Instance`) or `FindObjectOfType()`. Inject dependencies or bind in setup.
- **ARC-03:** If using dependency injection (e.g., VContainer), register services on appropriate `LifetimeScope` and prevent circular dependencies.
- **ARC-04:** Use `ScriptableObject` for configuration data.
- **ARC-05:** Core logic rules (especially `CueResolver`) MUST be pure C# decoupled from Unity APIs (`UnityEngine.Object`, scenes, components) to enable fast EditMode unit testing without scene loading.
- **PERF-01:** Reuse `AudioSource` instances or object pools for repeated one-shot playback.
- **PERF-02:** Zero allocations (strings, LINQ, collections) on `Update()` hot paths.
- **PERF-03:** All timers and coroutines must have clear lifecycles and be cancellable on respawn (RST-02).
- **PERF-05:** Playtest telemetry and debug logging must be excluded or disabled in release builds.

## Project Structure

```text
Assets/
  Scripts/
    Runtime/         # Gameplay scripts, controllers, and services
    Editor/          # Editor extensions and tooling
  Tests/
    EditMode/        # Pure C# unit tests (ACC-24: RES-01..09)
    PlayMode/        # Scene and integration tests
  Settings/          # URP and Global Settings
  Scenes/            # Game and dev scenes
  Prefabs/           # Character, environment, and interactive prefabs
  Audio/             # Clips and AudioMixer assets
  MCP/Context/       # Project documentation for MCP AI agents
```

## Git Workflow & AI Safety

- Branching: Feature branches from `main`.
- AI Handoffs: Each agent writes its own session handoff in `docs/ai-workflow/sessions/<branch-slug>__<actor-slug>.md`. Never edit a shared handoff.
- Verification Commands:
  - `python docs/ai-workflow/tools/validate_pack.py .`
  - `python docs/ai-workflow/tools/verify_compile.py .`
- Protected Areas: Respect `P-001` through `P-005` in `project-context.md`.
