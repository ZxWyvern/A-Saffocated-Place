---
id: 13-data-contracts-and-resolution
title: Skema Data, Kontrak Kode, dan Resolusi Cue
owns: [RES-*, RST-*, skema-AudioCue, kontrak]
depends_on: [05-audio-cues, 06-threat-and-chase, 12-technical-architecture]
---

# Skema Data, Kontrak Kode, dan Resolusi Cue

## Skema `AudioCue` (ScriptableObject)

| Field | Tipe konseptual | Kegunaan |
|---|---|---|
| CueId | Unique ID | Identitas cue yang stabil. Dipakai untuk menentukan kategori, bukan nama file atau volume. |
| CueCategory | Enum | Threat, Environment, atau Decoy. |
| AudioClip | AudioClip | Sumber audio. |
| SpatialMode | Enum | 2D atau 3D. |
| MinDistance, MaxDistance | float | Pengaturan jarak audibilitas. |
| CueDuration | float | Masa relevansi cue. |
| MovesTowardPlayer | bool | True untuk Threat. False untuk Decoy dan Environment (CUE-03). |
| IntroductionEncounter | ID atau tag | Encounter tempat cue pertama kali diperkenalkan dan tidak menghasilkan strike (CUE-04). |
| EncounterTag | ID atau tag | Encounter yang boleh memakai cue. |
| ResponseProfile | ScriptableObject reference | Respons yang diizinkan. |

## Hasil resolusi

`CueResolver` MUST mengeluarkan tepat satu hasil per input:

| Hasil | Arti |
|---|---|
| InputRejected | Input ditolak (RES-01). Tidak ada efek gameplay. |
| ThreatConfirmed | Cue Threat valid. Tampilkan petunjuk visual ancaman. |
| EnvironmentConfirmed | Cue Environment. Tampilkan konfirmasi visual sumber. Tanpa strike. |
| DecoyRevealed | Cue Decoy. Strike hanya diberikan jika `CueId` sudah pernah dipaparkan sebelumnya. |
| AttackTriggered | Kondisi serangan terpenuhi (strike mencapai batas atau waktu pendekatan habis). |

## Aturan resolusi

| ID | Aturan |
|---|---|
| RES-01 | Input MUST ditolak jika encounter tidak aktif, kontrol terkunci, korek tidak siap, atau cooldown aktif. |
| RES-02 | Resolver MUST memakai snapshot cue yang masih valid. Cue kedaluwarsa MUST NOT diproses. |
| RES-03 | Jika beberapa cue overlap, gunakan prioritas yang ditetapkan encounter dan keluarkan satu hasil per input. |
| RES-04 | Threat cue tidak otomatis berarti monster menyerang. Resolver MUST memeriksa `ThreatState`. |
| RES-05 | Environment cue MUST NOT menyebabkan strike atau game over (CUE-08). |
| RES-06 | Decoy memberi strike hanya jika `CueId` sudah pernah dipaparkan sebelumnya (CUE-04, THR-09). |
| RES-07 | Resolver MUST memakai event ID dan deduplication agar satu input tidak memicu respons dua kali. |
| RES-08 | Resolver MUST NOT bergantung pada Unity API sehingga seluruh aturan dapat diuji dengan EditMode test. |
| RES-09 | Resolver MUST deterministik: input dan state yang sama menghasilkan hasil yang sama. |

## Kontrak kode yang disarankan

Kontrak adalah batas arsitektur, bukan implementasi final. Sesuaikan signature dan lifecycle dengan codebase.

| Kontrak | Operasi konseptual |
|---|---|
| IAudioCueService | PlayCue(cueId), StopCue(instanceId), event CueStarted dan CueEnded. |
| IAudioMixerService | TransitionToSnapshot(snapshotId, duration). |
| IThreatStateService | CurrentState, Strikes, SetState(), AddStrike(), event ThreatStateChanged. |
| IMatchstickService | TryIgnite(), Extinguish(), CurrentState. |
| ICueResolver | Resolve(inputContext) mengembalikan ResolutionResult. |
| IThreatResponse | Apply(result, encounterContext). |
| IEncounterService | StartEncounter(id), CompleteEncounter(), ResetEncounter(). |
| ITelemetryService | Log(eventName, payload). |

## Reset dan respawn

| ID | Aturan |
|---|---|
| RST-01 | Respawn MUST menghentikan audio cue lama. |
| RST-02 | Respawn MUST membatalkan coroutine dan timer encounter. |
| RST-03 | Respawn MUST mereset threat state dan jumlah strike. |
| RST-04 | Respawn MUST mengembalikan snapshot mixer ke normal (MAT-03). |
| RST-05 | Respawn MUST mencegah event ganda dari encounter lama. |
| RST-06 | Retry MUST selesai dalam waktu kurang dari `P-RETRY-MAX` (THR-10). |
| RST-07 | Riwayat paparan `CueId` MUST dipertahankan melintasi retry dalam satu playthrough, agar kesalahan pertama pada cue baru tetap tidak dihukum tetapi tidak berulang tanpa batas. |
