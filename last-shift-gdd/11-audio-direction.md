---
id: 11-audio-direction
title: Arah Audio
owns: [AUD-*, layer-audio]
depends_on: [05-audio-cues, 04-matchstick, 14-parameters]
---

# Arah Audio

Audio adalah sistem gameplay utama, bukan lapisan atmosfer tambahan.

## Aturan

| ID | Aturan |
|---|---|
| AUD-01 | Setiap cue penting memiliki ritme, tekstur, dan ruang yang dapat dipelajari. |
| AUD-02 | Gunakan spatial audio untuk arah dan jarak. Cue kritis MUST tetap terbaca pada headphone stereo umum. |
| AUD-03 | Bedakan cue lewat tekstur, pola, dan perubahan jarak, bukan loudness yang ekstrem. |
| AUD-04 | Gunakan ducking dan mixing agar suara penting tidak tertutup musik atau ambience. |
| AUD-05 | Saat korek menyala, mixer berpindah ke snapshot yang meredam ambience dan cue, lalu kembali saat api padam. Parameter: `P-MATCH-DUCK-DB`, `P-MATCH-LPF-HZ`, `P-MATCH-DUCK-ATTACK`, `P-MATCH-DUCK-RELEASE`. |
| AUD-06 | Debug mode yang menampilkan `CueId` dan kategori MUST hanya tersedia di editor, bukan di build pemain. |
| AUD-07 | Layer Feedback (gesekan korek, ignition, api padam) MUST tetap terdengar jelas selama snapshot redaman aktif. |

## Layer audio

| Layer | Contoh |
|---|---|
| Ambience | Dengung AC, listrik, ruang kantor kosong. |
| Threat | Napas, gesekan logam, geraman, gerak di belakang. |
| Hallucination | Keyboard terdistorsi, alarm, suara kantor di ventilasi, notifikasi pesan. |
| Feedback | Gesekan korek, ignition, api padam, impact serangan. |
| Transition | Alarm menghilang menjadi dengung monitor saat ending. |
