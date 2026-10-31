---
id: 14-parameters
title: Parameter Tuning
owns: [P-*]
depends_on: []
---

# Parameter Tuning

Satu-satunya tempat nilai numerik gameplay. Semua nilai adalah baseline prototipe dan MUST divalidasi melalui playtest (lihat `16-playtest-and-validation.md`). Kode MUST membaca nilai dari konfigurasi dan MUST NOT hardcode.

| ID | Parameter | Baseline | Rentang awal uji | Dipakai di |
|---|---|---|---|---|
| P-MATCH-BURN | Durasi nyala korek | 1,8 detik | 0,8 sampai 3,5 detik | MAT-04 |
| P-MATCH-COOLDOWN | Cooldown ignition | 1,0 detik | 0 sampai 3,0 detik | MAT-04 |
| P-MATCH-DUCK-DB | Redaman ambience dan cue saat korek menyala | 12 dB | 6 sampai 18 dB | MAT-02, AUD-05 |
| P-MATCH-LPF-HZ | Low-pass cutoff saat korek menyala | 1.500 Hz | 800 sampai 3.000 Hz | MAT-02, AUD-05 |
| P-MATCH-DUCK-ATTACK | Waktu transisi masuk snapshot redaman | 0,1 detik | 0,05 sampai 0,3 detik | AUD-05 |
| P-MATCH-DUCK-RELEASE | Waktu transisi keluar snapshot redaman | 0,4 detik | 0,2 sampai 1,0 detik | MAT-02, AUD-05 |
| P-THREAT-APPROACH | Waktu pendekatan ancaman | 8 detik | 4 sampai 15 detik, per encounter | 06 (state Approaching) |
| P-DECISION-GRACE | Jeda toleransi keputusan | 2,0 detik | 0 sampai 4 detik, tidak universal | DEC-09 |
| P-STRIKE-MAX | Strike maksimum per encounter | 2 | 2 sampai 3 | THR-04 |
| P-STRIKE-RECOVERY | Waktu pemulihan satu strike | 20 detik | 10 sampai 40 detik | THR-07 |
| P-TELEGRAPH-MIN | Telegraph minimum sebelum serangan | 1,5 detik | 1,0 sampai 2,5 detik | THR-17 |
| P-RETRY-MAX | Batas durasi retry | Kurang dari 5 detik | Kurang dari 8 detik | THR-10, RST-06 |
| P-REVEAL-DURATION | Durasi reveal visual saat korek | 0,5 sampai 1,5 detik | Sesuaikan dengan keterbacaan cue | 04 (hasil penggunaan korek) |
| P-JUMPSCARE-DURATION | Durasi jumpscare penuh | 0,5 sampai 1,2 detik | Hasil playtest | THR-16 |
| P-JUMPSCARE-CAP | Batas jumpscare penuh per playthrough | 2 | Tetap | THR-12 |
| P-CUE-MAX-THREAT | Jenis cue Threat maksimal | 3 | Tetap | 05 (kategori cue) |
| P-CUE-MAX-ENV | Jenis cue Environment maksimal | 4 | Tetap | 05 (kategori cue) |
| P-CUE-MAX-DECOY | Jenis cue Decoy maksimal | 3 | Tetap | 05 (kategori cue) |
| P-REST-DURATION | Durasi ruang jeda | 30 sampai 45 detik | Hasil playtest | LVL-12 |
| P-LOOP-BRANCH-MAX | Jalur bercabang maksimal di The Pursuit | 2 | Tetap | LVL-04 |

Catatan: `P-MATCH-DUCK-ATTACK` dan `P-MATCH-DUCK-RELEASE` adalah nilai awal yang ditambahkan pada penyusunan SSOT agar transisi snapshot dapat dikonfigurasi. Nilainya belum divalidasi.
