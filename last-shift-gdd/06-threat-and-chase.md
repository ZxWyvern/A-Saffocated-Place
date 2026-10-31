---
id: 06-threat-and-chase
title: Monster, Strike, dan Chase
owns: [THR-*, state-ancaman, strike, jumpscare, failure]
depends_on: [05-audio-cues, 04-matchstick, 14-parameters]
---

# Monster dan Chase

## Desain

| ID | Aspek | Desain |
|---|---|---|
| THR-01 | Presentasi | Utamakan suara, bayangan, siluet, dan gerak singkat. Monster MUST NOT diperlihatkan terlalu sering. |
| THR-02 | AI | Encounter terkurasi dengan trigger, waypoint, dan state eksplisit. MUST NOT ada AI bebas yang mengejar pemain di seluruh level. |
| THR-03 | Jumlah | Satu monster untuk seluruh game. |

## State ancaman

| State | Transisi utama |
|---|---|
| Dormant | Encounter dimulai, lalu Approaching. |
| Approaching | Cue ancaman aktif, lalu Verifying. Tekanan maksimum atau waktu pendekatan (`P-THREAT-APPROACH`) habis, lalu Attacking. |
| Verifying | Respons pemain sesuai aturan, lalu Disengaged atau fase berikutnya. Strike mencapai batas atau kondisi serangan terpenuhi, lalu Attacking. |
| Attacking | Serangan selesai, lalu Disengaged atau failure. |
| Disengaged | Strike berkurang seiring waktu. Encounter berikutnya, lalu Approaching, atau selesai. |

## Strike

| ID | Aturan |
|---|---|
| THR-04 | Maksimal strike per encounter: `P-STRIKE-MAX`. |
| THR-05 | Strike pertama MUST menghasilkan peringatan audio dari jarak dekat. |
| THR-06 | Strike yang mencapai batas maksimum MUST memicu fase Attacking. |
| THR-07 | Satu strike hilang setelah pemain tidak terdeteksi selama `P-STRIKE-RECOVERY`. |
| THR-08 | Strike diberikan oleh `ThreatResponse` berdasarkan hasil `CueResolver`, bukan oleh komponen lain. |
| THR-09 | Decoy hanya memberi strike jika `CueId` sudah pernah dipaparkan sebelumnya (lihat CUE-04). |

## Failure dan retry

| ID | Aturan |
|---|---|
| THR-10 | Kegagalan mengulang encounter dari awal. Retry MUST selesai dalam waktu kurang dari `P-RETRY-MAX`. |
| THR-11 | Setiap encounter MUST menentukan secara eksplisit apakah kegagalan berarti restart encounter atau kembali ke checkpoint. |

## Jumpscare

| ID | Aturan |
|---|---|
| THR-12 | Jumpscare penuh MUST NOT melebihi `P-JUMPSCARE-CAP` kali per playthrough. |
| THR-13 | Dua momen jumpscare penuh: (a) satu scripted tidak mematikan di bagian The Loop, (b) satu pada kegagalan pertama di The Pursuit. |
| THR-14 | Kegagalan berikutnya MUST memakai serangan singkat tanpa jumpscare penuh. |
| THR-15 | Setiap serangan MUST memiliki guard agar jumpscare hanya aktif sekali per serangan. |
| THR-16 | Durasi jumpscare penuh memakai `P-JUMPSCARE-DURATION`. |

## Fairness

| ID | Aturan |
|---|---|
| THR-17 | Pemain MUST mendapat telegraph audio atau visual minimal `P-TELEGRAPH-MIN` sebelum serangan, kecuali pada momen yang sengaja dirancang sebagai kejutan dan terdaftar di tabel di bawah. |
| THR-18 | Timer ancaman MUST NOT berjalan saat game pause, loading, atau cutscene yang mengunci gameplay. |

### Pengecualian telegraph yang terdaftar

| Momen | Alasan |
|---|---|
| Jumpscare scripted di The Loop | Tidak mematikan. Berfungsi sebagai pengenalan rasa takut, bukan ujian. |
