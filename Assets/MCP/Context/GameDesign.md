# Game Design Document — LAST SHIFT

Authority: `last-shift-gdd/` (Single Source of Truth v1.1).

## Game Overview

- **Title:** LAST SHIFT
- **Genre:** First-person Psychological Horror, Audio-based Survival, Exploration, Chase
- **Target Audience:** Fans of psychological horror, atmospheric tension, and deduction
- **Platform:** PC (Single-player, 20–30 minute targeted runtime)
- **High Concept:** An exhausted office worker hallucinates a building fire and flees through an infinite ventilation loop while stalked by a monster, only to discover the entire escape was a terminal hallucination and they passed away at their work desk.
- **Key Binding Principles:**
  - The player never fights. Survival is achieved strictly through observation, decision-making, and movement (PIL-03).
  - All audio and environmental cues follow consistent, learnable rules despite the protagonist's unreliable perception (PIL-01, PIL-02).

## Core Gameplay Loop

1. **Explore & Observe:** Navigate claustrophobic office spaces and ventilation ducts using first-person movement (PLR-01..PLR-05).
2. **Listen & Discern:** Hear spatialized 3D audio cues to evaluate whether sounds represent environmental noise, a decoy, or an approaching threat (05-audio-cues.md).
3. **Decision Point:** Choose between waiting, moving forward, or striking a match.
4. **Matchstick Sensory Trade-off:** Striking a match provides brief visual clarity but immediately ducks environmental and cue audio by 12 dB with a 1500 Hz low-pass filter (MAT-02), blinding hearing while illuminating vision.
5. **Threat & Chase Navigation:** If a threat closes in, navigate looping corridors, manage sprint bursts, and avoid strikes (06-threat-and-chase.md).
6. **False Exit:** Reach apparent salvation that loops back or shifts subtly.
7. **Recontextualization:** Experience the narrative transition to the office desk, realizing the true nature of the events (08-ending-sequence.md).

## Design Pillars

| ID | Pillar | Core Rule |
|---|---|---|
| PIL-01 | Uncertain Perception | Player hears threats before seeing them. Cues are learnable, not random. |
| PIL-02 | Learnable Rules | Cues have fixed signatures. First mistake on a new cue is never penalized. |
| PIL-03 | Vulnerable Player | No weapons, no combat abilities, no power fantasy. |
| PIL-04 | Controlled Tension | Rhythmic pacing with deliberate rest zones (LVL-11, P-REST-DURATION: 30-45s). |
| PIL-05 | Narrative Recontextualization | Subtle clues (personal items, mother's messages) gain poignant meaning at the twist. |
| PIL-06 | Compact Scope | One office, one vent system, one monster, tightly curated encounters. |

## Key Mechanics & Tuning Parameters

Tuning numbers are owned strictly by `last-shift-gdd/14-parameters.md`:

- **Matchstick System (`04-matchstick.md`):**
  - Burn Duration (`P-MATCH-BURN`): 1.8 seconds.
  - Cooldown (`P-MATCH-COOLDOWN`): 1.0 second.
  - Audio Ducking (`P-MATCH-DUCK-DB`): 12 dB reduction.
  - Low-Pass Filter (`P-MATCH-LPF-HZ`): 1500 Hz cutoff.
  - Ducking Attack / Release: 0.1s (`P-MATCH-DUCK-ATTACK`) / 0.4s (`P-MATCH-DUCK-RELEASE`).
  - Capacity: Unlimited matchsticks (DEC-01 prototype default).
- **Audio Cue Classification (`05-audio-cues.md`):**
  - **Threat:** Moves toward player (`MovesTowardPlayer = true`). Max 3 variants (`P-CUE-MAX-THREAT`).
  - **Environment:** Fixed source, steady rhythm (`MovesTowardPlayer = false`). Max 4 variants (`P-CUE-MAX-ENV`). Never causes strikes or death (CUE-08).
  - **Decoy:** Organic sound resembling threat, but static source position (`MovesTowardPlayer = false`). Max 3 variants (`P-CUE-MAX-DECOY`). First encounter introduces without strike (CUE-04).
- **Threat State & Chase (`06-threat-and-chase.md`):**
  - States: `Dormant` -> `Approaching` -> `Verifying` -> `Attacking` -> `Disengaged`.
  - Approach Window (`P-THREAT-APPROACH`): 8.0 seconds.
  - Max Strikes (`P-STRIKE-MAX`): 2 strikes before attack.
  - Strike Recovery (`P-STRIKE-RECOVERY`): 20.0 seconds without detection.
  - Telegraph Warning (`P-TELEGRAPH-MIN`): Minimum 1.5s telegraph before lethal attack.
  - Full Jumpscares (`P-JUMPSCARE-CAP`): Maximum 2 per playthrough (THR-12).
  - Fast Retry (`P-RETRY-MAX`): Restart / retry cycle must complete under 5 seconds (THR-10, RST-06).

## UI/UX & Accessibility Guidelines

- **HUD:** Diegetic, minimal, no gamey crosshairs or health bars (09-ui-ux-accessibility.md).
- **Accessibility Options:**
  - Optional visual sound indicator (UI-06) indicating presence/direction without revealing internal cue classification.
  - Optional haptic feedback for heartbeats / proximity (UI-07).
  - Mono audio mix option for single-ear playability.
  - Configurable camera bob / sway reduction to combat motion sensitivity.
