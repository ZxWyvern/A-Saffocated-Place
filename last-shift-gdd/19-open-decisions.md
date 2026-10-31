---
id: 19-open-decisions
title: Keputusan Desain Terbuka
owns: [DEC-*]
depends_on: []
---

# Keputusan Desain Terbuka

Kolom "Default prototipe" MUST dipakai sampai keputusan berstatus Diputuskan. Jangan mengimplementasikan alternatif lain tanpa mengubah file ini.

| ID | Pertanyaan | Default prototipe | Status |
|---|---|---|---|
| DEC-01 | Apakah jumlah korek terbatas? | Tidak. Biaya pendengaran saat api menyala sudah menjadi tekanan. Evaluasi ulang setelah data playtest. | Terbuka |
| DEC-02 | Apakah salah input selalu jumpscare? | Tidak. Konsekuensi mengikuti strike dan fase ancaman. | Terbuka |
| DEC-03 | Apakah monster selalu terlihat jika cue benar? | Tidak. Tampilkan petunjuk visual yang cukup, tetapi tidak selalu bentuk penuh. | Terbuka |
| DEC-04 | Apakah menunggu aman? | Tidak selalu. Pada encounter tertentu ancaman mendekat saat pemain menunda. | Terbuka |
| DEC-05 | Apakah ending menyebut penyebab medis pasti? | Tidak, kecuali narasi dan riset mendukungnya (NAR-03). | Terbuka |
| DEC-06 | Apakah indikator visual cue aktif secara default? | Tidak. Tersedia sebagai opsi aksesibilitas (UI-06). | Terbuka |
| DEC-07 | Isi pesan dari ibu. | Teks singkat yang menanyakan apakah Raka sudah makan dan pulang jam berapa. Disesuaikan setelah uji narasi. | Terbuka |
| DEC-08 | Spesifikasi PC minimum dan target performa final. | Belum ditetapkan. Profiling pada build sebelum menetapkan target (PERF-06). | Terbuka |
| DEC-09 | Penerapan jeda toleransi keputusan (`P-DECISION-GRACE`). | Belum terpasang pada aturan. Jangan mengimplementasikan efek apa pun dari parameter ini sampai definisinya diputuskan. | Terbuka |
