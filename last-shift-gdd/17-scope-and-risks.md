---
id: 17-scope-and-risks
title: Scope, Risiko, dan Di Luar Scope
owns: [RISK-*, scope-vertical-slice, di-luar-scope]
depends_on: [01-design-pillars]
---

# Scope, Risiko, dan Di Luar Scope

## Scope vertical slice

- Satu area kantor kecil, satu jaringan ventilasi modular, satu monster, satu chase, satu ending.
- Satu mekanik utama: audio cue dan korek.
- Satu loop environment dengan beberapa perubahan scripted dan satu ruang jeda.
- Interaksi objek terbatas: komputer, dokumen, kartu identitas, kisi ventilasi, dan tiga benda pribadi.

## Di luar scope

Item berikut MUST NOT dikerjakan tanpa persetujuan eksplisit dan entri `DEC-*`.

- Combat, senjata, crafting, skill tree, banyak monster, branching narrative besar, multiplayer.
- AI monster bebas yang mengejar pemain di seluruh level.
- Sistem penyakit atau diagnosis medis yang disimulasikan secara detail.
- Batang korek terbatas, sprint khusus, dan lebih dari dua jalur bercabang di The Pursuit. Ketiganya dievaluasi ulang hanya jika data playtest menunjukkan kebutuhan.

## Risiko dan mitigasi

| ID | Risiko | Dampak | Mitigasi |
|---|---|---|---|
| RISK-01 | Cue sulit dibedakan | Pemain menganggap kegagalan acak dan berhenti takut. | Batasi jumlah jenis cue, pakai pembeda perilaku (CUE-03), uji blind audio, revisi tekstur dan mix. |
| RISK-02 | Kegagalan berulang membosankan | Rasa takut hilang saat retry. | Sistem strike, jumpscare dibatasi (THR-12), retry cepat (THR-10). |
| RISK-03 | Twist terasa dingin | Ending dipahami tetapi tidak menyentuh. | Benda pribadi di awal dan pesan ibu yang masih terbuka di ending (NAR-ITEM-*). |
| RISK-04 | Twist tidak terbaca | Ending terasa tanpa dasar. | Tanam petunjuk visual dan audio sejak awal (NAR-FS-*). |
| RISK-05 | Ketegangan tanpa jeda | Pemain menjadi kebal terhadap tekanan. | Ruang jeda di The Loop (LVL-11) dan rasa lega pada False Exit. |
| RISK-06 | Loop membingungkan | Frustrasi navigasi. | Pertahankan landmark dan ubah detail secara terkontrol (LVL-07..LVL-09). |
| RISK-07 | Mekanik bergantung pada audio | Pemain dengan gangguan pendengaran atau speaker buruk tidak dapat bermain. | Indikator visual dan haptic opsional (UI-06, UI-07), serta opsi mono. |
| RISK-08 | Scope membesar | Vertical slice tidak selesai. | Pertahankan satu monster dan encounter terkurasi (PIL-06, THR-03). |
| RISK-09 | State bocor setelah retry | Bug, serangan ganda, atau softlock. | Reset lifecycle eksplisit (RST-*) dan acceptance test (ACC-11). |
