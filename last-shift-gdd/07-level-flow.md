---
id: 07-level-flow
title: Level Flow dan Encounter
owns: [LVL-*, durasi-bagian, loop-environment, ruang-jeda]
depends_on: [02-narrative, 05-audio-cues, 06-threat-and-chase]
---

# Level Flow dan Encounter

## Bagian level

| ID | Bagian | Durasi target | Tujuan | Gameplay dan hasil |
|---|---|---|---|---|
| LVL-01 | Overtime | 3 sampai 4 menit | Mengenalkan kantor, karakter, dan interaksi. | Periksa dokumen dan komputer. Lihat cangkir kopi dan kartu identitas. Baca foto, pesan dari ibu, dan catatan tenggat (NAR-ITEM-01..03). Bangun rasa lelah tanpa menjelaskan twist. |
| LVL-02 | Fire Alarm | 3 sampai 4 menit | Memulai halusinasi dan mengarahkan ke ventilasi. | Alarm, asap visual, jalur utama tertutup. Cari akses ventilasi. |
| LVL-03 | The Loop | 5 sampai 7 menit | Mengajarkan korek dan pembacaan audio. | Perkenalkan satu jenis cue pada satu waktu, mulai dari yang paling jelas. Satu jumpscare scripted tidak mematikan (THR-13a). Ruang jeda di tengah bagian (LVL-11). Temukan petunjuk kantor. |
| LVL-04 | The Pursuit | 4 sampai 6 menit | Menguji pembacaan audio di bawah tekanan. | Monster mengejar. Maksimal `P-LOOP-BRANCH-MAX` jalur bercabang. Korek memberi informasi singkat dengan harga pendengaran. Capai kisi keluar. |
| LVL-05 | False Exit | 2 sampai 3 menit | Menciptakan resolusi palsu. | Pemain keluar. Suara monster berubah menjadi dengung komputer. Kontrol dan kamera bertransisi. |
| LVL-06 | Reveal | 1 sampai 2 menit | Mengungkap kenyataan dan identitas. | Lihat `08-ending-sequence.md`. |

Total durasi target 20 sampai 30 menit untuk satu playthrough, termasuk retry yang wajar.

## Aturan loop environment

| ID | Aturan |
|---|---|
| LVL-07 | Loop MUST memakai segmen modular dengan landmark yang mudah dikenali. |
| LVL-08 | Loop tidak identik sepenuhnya: ubah satu atau dua detail per iterasi, misalnya nomor panel, posisi objek, suara, atau pencahayaan. |
| LVL-09 | Perubahan MUST menjadi petunjuk. Layout MUST NOT diacak sehingga pemain kehilangan orientasi tanpa alasan. |
| LVL-10 | Loop dikendalikan oleh encounter dan progress flag, bukan teleport acak. |

## Ruang jeda

| ID | Aturan |
|---|---|
| LVL-11 | Satu segmen ventilasi di The Loop MUST menjadi ruang jeda: tanpa ancaman aktif, hanya dengung. |
| LVL-12 | Ruang jeda MUST memakai modul loop yang sudah ada dengan pencahayaan lebih tenang. Durasi: `P-REST-DURATION`. |
| LVL-13 | Kartu identitas (NAR-FS-04) MUST ditempatkan di ruang jeda agar pemain punya alasan berhenti. |
| LVL-14 | Cue ancaman pertama setelah ruang jeda MUST muncul dengan tekanan yang terasa lebih besar. |

## Alur kemajuan

- Jalur keluar MUST dapat dicapai tanpa urutan trigger yang rusak.
- Tidak boleh ada softlock pada loop, chase, respawn, atau ending.
