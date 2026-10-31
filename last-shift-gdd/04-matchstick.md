---
id: 04-matchstick
title: Mekanik Korek
owns: [MAT-*, state-machine-korek]
depends_on: [05-audio-cues, 14-parameters]
---

# Mekanik Korek

Korek adalah sumber cahaya sementara dan alat verifikasi persepsi. Pemain mendengar cue, menilai sumbernya, lalu memutuskan apakah akan menyalakan korek.

Selama api menyala, ambience dan cue audio diredam dan difilter, sehingga pemain harus memilih antara melihat dan mendengar. Biaya ini membuat setiap penggunaan korek menjadi keputusan, tanpa sistem batang korek terbatas (lihat `DEC-01`).

## Aturan

| ID | Aturan |
|---|---|
| MAT-01 | Korek MUST menolak input jika state tidak valid, kontrol terkunci, atau cooldown aktif. |
| MAT-02 | Saat korek menyala, mixer MUST berpindah ke snapshot redaman. Redaman dan filter memakai `P-MATCH-DUCK-DB` dan `P-MATCH-LPF-HZ`. |
| MAT-03 | Saat korek padam, mixer MUST kembali ke snapshot normal, termasuk saat respawn atau pergantian scene. |
| MAT-04 | Durasi nyala memakai `P-MATCH-BURN`. Cooldown memakai `P-MATCH-COOLDOWN`. |
| MAT-05 | Durasi dan cooldown MUST berhenti saat game pause dan MUST pulih konsisten saat resume. |
| MAT-06 | Visual korek (api, cahaya, asap, animasi tangan) MUST dikelola komponen terpisah dari logika ignition. |
| MAT-07 | Korek MUST NOT mengklasifikasikan cue. Klasifikasi dilakukan `CueResolver` (lihat `13-data-contracts-and-resolution.md`). |

## Hasil penggunaan korek

| Kondisi | Hasil |
|---|---|
| Threat cue valid dan monster belum menyerang | Korek menyala. Tampilkan petunjuk visual singkat tentang posisi atau bentuk ancaman, tanpa selalu menampilkan bentuk penuh. |
| Environment cue | Tidak ada serangan. Durasi korek terpakai dan pendengaran teredam selama api menyala. Pemain mendapat konfirmasi visual sumber suara. |
| Decoy cue, kemunculan pertama | Tidak ada strike. Pemain melihat bahwa sumber suara tidak ada, sehingga ia belajar membedakan decoy. |
| Decoy cue, kemunculan berikutnya | Pemain mendapat satu strike dan waktu pendekatan monster berkurang. |
| Monster dalam fase Attacking | Serangan terjadi jika kondisi serangan valid. |
| Pemain menunggu | Pada encounter tertentu ancaman mendekat. Menunggu tidak selalu aman. |
| Korek padam | Pendengaran kembali normal dengan transisi singkat (`P-MATCH-DUCK-RELEASE`). Visual hilang sampai korek dinyalakan lagi dan cooldown selesai. |

## State machine korek

| State | Transisi |
|---|---|
| Unlit | Input valid, lalu Igniting. |
| Igniting | Berhasil, lalu Burning. Gagal, lalu Unlit. |
| Burning | Timer habis atau dimatikan, lalu Extinguishing. Mixer berada pada snapshot redaman. |
| Extinguishing | Efek selesai, mixer kembali ke snapshot normal, lalu Unlit. |
