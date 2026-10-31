# Networking Guidelines — LAST SHIFT

## Scope Authority

LAST SHIFT is designed and architected as a **strictly single-player offline psychological horror experience** (`last-shift-gdd/00-overview.md`, `PIL-06`, `17-scope-and-risks.md`).

## Network Architecture Status

- **Type:** None (Single-Player Standalone).
- **Framework:** None. Netcode for GameObjects, Mirror, Photon, and other multiplayer SDKs are **strictly out of scope** (`17-scope-and-risks.md`).
- **Transport:** None.
- **Rules for AI Agents:**
  - MUST NOT add multiplayer, networked synchronization, or peer-to-peer logic without explicit product owner approval and a dedicated `DEC-*` entry in `last-shift-gdd/19-open-decisions.md`.
  - All game state, progression, and save systems are strictly local.

## Local Communication Interfaces

The only communication layers active in this project are local developer tooling bridges:
1. **Unity MCP Bridge (`com.anklebr.unity-mcp`):** Local HTTP server bound exclusively to localhost (`127.0.0.1:7890`) for Editor inspection and AI agent pair-programming tools.
2. **Local Playtest Telemetry (`TelemetryService`):** Writes event metrics exclusively to local CSV files for QA validation (ACC-19, PERF-05). Completely offline and disabled in release builds.
