---
id: 15-acceptance-criteria
title: Kriteria Penerimaan dan QA
owns: [ACC-*]
depends_on: [04-matchstick, 05-audio-cues, 06-threat-and-chase, 07-level-flow, 08-ending-sequence, 13-data-contracts-and-resolution]
---

# Kriteria Penerimaan dan QA

Setiap kriteria memiliki ID dan merujuk aturan yang diuji. Fitur dianggap selesai hanya jika kriteria terkait lulus pada build.

| ID | Kriteria | Aturan yang diuji |
|---|---|---|
| ACC-01 | Korek menolak input ketika state tidak valid, kontrol terkunci, atau cooldown aktif. | MAT-01, RES-01 |
| ACC-02 | Durasi api dan cooldown konsisten pada pause, resume, dan pergantian scene. | MAT-04, MAT-05 |
| ACC-03 | Snapshot redaman aktif tepat selama api menyala dan kembali normal setelah api padam, termasuk saat respawn. | MAT-02, MAT-03, RST-04 |
| ACC-04 | Audio 3D memiliki arah dan jarak yang benar, dan cue penting dapat dikenali pada headphone stereo. | CUE-06, AUD-02 |
| ACC-05 | Cue yang overlap menghasilkan satu resolusi deterministik per input. | RES-03, RES-09 |
| ACC-06 | Kesalahan pertama pada setiap `CueId` baru tidak menghasilkan strike. | CUE-04, RES-06 |
| ACC-07 | Hasil sebuah `CueId` identik di seluruh encounter. | CUE-01, CUE-05 |
| ACC-08 | Environment cue tidak pernah memicu strike atau game over. | CUE-08, RES-05 |
| ACC-09 | Serangan hanya terjadi jika state dan encounter memenuhi syarat, dan satu event tidak memicu serangan ganda. | RES-04, RES-07 |
| ACC-10 | Jumpscare penuh tidak muncul lebih dari dua kali dalam satu playthrough. | THR-12, THR-15 |
| ACC-11 | Respawn membersihkan timer, audio, state, strike, dan event encounter lama, dan retry selesai dalam waktu kurang dari `P-RETRY-MAX`. | RST-01..RST-06, THR-10 |
| ACC-12 | Loop menunjukkan perubahan yang dapat diamati dan tidak menyebabkan softlock. | LVL-08, LVL-09, LVL-10 |
| ACC-13 | Ruang jeda tidak memuat ancaman aktif. | LVL-11 |
| ACC-14 | Jalur keluar dapat dicapai tanpa urutan trigger yang rusak. | 07 (Alur kemajuan) |
| ACC-15 | Indikator visual dan haptic opsional bekerja dan tidak mengungkap kategori cue. | UI-06, UI-07 |
| ACC-16 | Ending mengunci input dengan benar dan transisi kamera dan audio berjalan tanpa konflik. | PLR-04, END-01..END-10 |
| ACC-17 | Pantulan monitor, kartu identitas, foto, pesan, dan wajah korban konsisten secara visual. | 08 (Konsistensi visual) |
| ACC-18 | Koran mengonfirmasi kejadian tanpa menjelaskan seluruh twist. | NAR-04, END-07 |
| ACC-19 | Telemetri menulis file CSV yang valid dan tidak aktif pada build rilis. | PERF-05, VAL-01 |
| ACC-20 | Threat telegraph minimal `P-TELEGRAPH-MIN` sebelum serangan, kecuali momen yang terdaftar. | THR-17 |
| ACC-21 | Timer ancaman tidak berjalan saat pause, loading, atau cutscene yang mengunci gameplay. | THR-18 |
| ACC-22 | Benda pribadi (NAR-ITEM-01..03) hadir di Overtime, muncul kembali di halusinasi, dan konsisten di ending. | NAR-06, NAR-ITEM-* |
| ACC-23 | Kategori dan `CueId` tidak tampil di build pemain. | CUE-10, AUD-06 |
| ACC-24 | `CueResolver` memiliki EditMode test yang mencakup RES-01 sampai RES-09. | RES-08 |
