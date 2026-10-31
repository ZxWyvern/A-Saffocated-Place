---
id: 05-audio-cues
title: Kategori dan Aturan Audio Cue
owns: [CUE-*, kategori-cue]
depends_on: [01-design-pillars, 14-parameters]
---

# Kategori dan Aturan Audio Cue

Audio cue adalah informasi gameplay utama. Aturan di file ini menjamin bahwa pemain yang memperhatikan selalu dapat membedakan ancaman, lingkungan, dan decoy (PIL-02).

## Kategori

| Kategori | Jumlah jenis maksimal | Karakter tetap | Fungsi desain |
|---|---|---|---|
| Threat | `P-CUE-MAX-THREAT` | Tekstur organik atau gesekan. Sumber bergerak mendekat selama cue berlangsung. | Menandakan monster mendekat atau berada di sekitar. |
| Environment | `P-CUE-MAX-ENV` | Ritme stabil dan berulang. Posisi sumber tetap. | Membangun suasana dan menguji perhatian pemain. |
| Decoy | `P-CUE-MAX-DECOY` | Menyerupai threat dalam tekstur, tetapi posisi dan jarak sumber tidak berubah. | Mengaburkan batas ancaman dan lingkungan secara terkontrol. |

## Aturan cue

| ID | Aturan |
|---|---|
| CUE-01 | Setiap `CueId` MUST memiliki satu tekstur, satu ritme, dan satu kategori tetap di seluruh game. |
| CUE-02 | Kategori cue MUST NOT diubah secara acak atau antar encounter. |
| CUE-03 | Pembeda decoy adalah perilakunya: sumber Threat bergerak mendekat (`MovesTowardPlayer = true`), sumber Decoy dan Environment tidak berpindah (`MovesTowardPlayer = false`). |
| CUE-04 | Kesalahan pertama pada setiap `CueId` baru MUST NOT menghasilkan strike. Cue diperkenalkan sebelum diuji (`IntroductionEncounter`). |
| CUE-05 | Hasil sebuah `CueId` MUST identik di seluruh encounter. Yang boleh berubah antar encounter hanya tekanan di sekitarnya: jarak dan waktu pendekatan. |
| CUE-06 | Cue kritis MUST tetap terbaca pada headphone stereo umum. |
| CUE-07 | Pembeda antar cue MUST berasal dari tekstur, pola, dan perubahan jarak, bukan perbedaan loudness yang ekstrem. |
| CUE-08 | Environment cue MUST NOT menyebabkan strike atau game over. |
| CUE-09 | Satu jenis cue baru diperkenalkan pada satu waktu, dimulai dari yang paling jelas (lihat `07-level-flow.md`). |
| CUE-10 | Kategori dan `CueId` MUST NOT ditampilkan di build pemain. Debug view hanya di editor. |
