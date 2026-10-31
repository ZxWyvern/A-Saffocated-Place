---
id: 16-playtest-and-validation
title: Playtest dan Validasi
owns: [VAL-*, telemetri, kuesioner, target-hipotesis]
depends_on: [14-parameters, 05-audio-cues]
---

# Playtest dan Validasi

Tuning parameter dan aturan cue MUST berdasarkan data, bukan perkiraan. Telemetri dan kuesioner dipasang sejak prototipe pertama. Gunakan minimal lima pemain per iterasi.

## Telemetri

| ID | Data per sesi |
|---|---|
| VAL-01 | `CueId`, kategori, waktu cue dimainkan, dan jarak sumber. Ditulis ke CSV. |
| VAL-02 | Waktu pemain menyalakan korek terhadap waktu cue, dan hasil resolusi. |
| VAL-03 | Jumlah strike, jumlah kematian, dan waktu per encounter. |
| VAL-04 | Titik pemain berhenti, mengulang, atau keluar dari game. |
| VAL-05 | Apakah pemain memeriksa tiap benda pribadi di bagian Overtime (NAR-ITEM-01..03). |

## Kuesioner setelah bermain

Skala 1 sampai 5:

| ID | Pernyataan |
|---|---|
| VAL-06 | Saya merasa takut selama bermain. |
| VAL-07 | Saya merasa kegagalan saya adil dan dapat saya pelajari. |
| VAL-08 | Saya memahami apa yang terjadi di akhir game. |
| VAL-09 | Saya peduli dengan nasib karakter. |

## Target hipotesis iterasi pertama

Target ini adalah hipotesis dan MUST direvisi setelah data dari lima pemain pertama terkumpul.

| ID | Metrik | Target awal | Sumber data |
|---|---|---|---|
| VAL-10 | Akurasi identifikasi cue pada paparan ketiga | 80 persen atau lebih | VAL-01, VAL-02 |
| VAL-11 | Median kematian per encounter | 3 atau kurang | VAL-03 |
| VAL-12 | Skor rasa adil | 3,5 atau lebih dari 5 | VAL-07 |
| VAL-13 | Skor peduli pada karakter | 3,5 atau lebih dari 5 | VAL-09 |
| VAL-14 | Pemain yang memahami twist tanpa bantuan | 80 persen atau lebih | VAL-08 |

## Aturan penggunaan data

- Hasil playtest MUST mengubah `14-parameters.md` dan `CHANGELOG.md`, bukan ditanam di kode.
- Cue dengan akurasi identifikasi di bawah target MUST direvisi tekstur atau mix-nya, bukan kategorinya (CUE-02).
