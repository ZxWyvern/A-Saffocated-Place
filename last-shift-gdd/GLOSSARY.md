---
id: GLOSSARY
title: Glosarium
owns: [istilah]
depends_on: []
---

# Glosarium

| Istilah | Definisi |
|---|---|
| Cue | Satu suara dalam game yang memberi informasi gameplay. Memiliki `CueId` dan kategori tetap. |
| CueId | Identitas stabil sebuah cue. Penentu kategori. |
| Threat cue | Cue yang menandakan monster mendekat. Sumber bergerak mendekat. |
| Environment cue | Cue yang membangun suasana. Sumber tetap, ritme stabil. Tidak pernah menghukum. |
| Decoy cue | Cue yang menyerupai threat dalam tekstur tetapi sumbernya tidak berpindah. |
| Strike | Hitungan kesalahan dalam satu encounter. Mencapai `P-STRIKE-MAX` memicu serangan. |
| Telegraph | Peringatan audio atau visual sebelum serangan. |
| Encounter | Unit gameplay terkurasi dengan trigger, cue, dan state ancaman sendiri. |
| Snapshot redaman | Snapshot mixer yang meredam ambience dan cue saat korek menyala. |
| Ruang jeda | Segmen The Loop tanpa ancaman aktif untuk memulihkan ketegangan. |
| Halusinasi | Seluruh pengalaman dari Fire Alarm sampai False Exit, yang bukan kejadian objektif. |
| False Exit | Bagian yang menciptakan resolusi palsu sebelum Reveal. |
| Reveal | Bagian ending yang mengungkap kenyataan. |
| SSOT | Single Source of Truth: direktori ini. |
| EditMode test | Unit test Unity yang berjalan tanpa Play Mode, dipakai untuk menguji `CueResolver`. |
