---
id: README
title: LAST SHIFT SSOT Index
version: 1.1
status: pre-production
---

# LAST SHIFT: Single Source of Truth (SSOT)

Direktori ini adalah satu-satunya sumber otoritatif untuk desain dan spesifikasi teknis game LAST SHIFT. Dokumen dipecah per topik agar agent hanya perlu memuat file yang relevan dengan tugasnya.

- Versi: 1.1
- Status: Pre-production
- Engine: Unity 6, C#
- Platform: PC
- Bahasa dokumen: Indonesia. Identifier, nama state, dan nama kelas memakai bahasa Inggris dan harus dipakai persis seperti tertulis.

## Aturan SSOT untuk agent

1. **Prioritas.** Jika kode, tiket, atau percakapan bertentangan dengan direktori ini, direktori ini yang benar. Jangan mengubah perilaku game tanpa mengubah file pemiliknya.
2. **Satu pemilik per fakta.** Setiap fakta didefinisikan di tepat satu file (lihat kolom "Memiliki" di indeks). File lain hanya merujuk lewat ID. Jangan menyalin nilai atau aturan ke file lain.
3. **Angka hanya di `14-parameters.md`.** Durasi, jarak, desibel, dan batas hitung gameplay ada di sana dengan ID `P-*`. Kode wajib membaca nilai itu dari konfigurasi, bukan hardcode. Pengecualian: durasi bagian level dimiliki `07-level-flow.md`.
4. **ID stabil.** Aturan memakai ID seperti `CUE-03`, `MAT-02`, `THR-05`. ID tidak pernah dipakai ulang. Aturan yang dihapus ditandai `DEPRECATED`, bukan dihapus.
5. **Kata kunci.** MUST dan MUST NOT adalah wajib. SHOULD adalah default yang boleh disimpang dengan alasan tertulis. MAY adalah opsional.
6. **Keputusan terbuka.** Hal yang belum diputuskan ada di `19-open-decisions.md` dengan default prototipe. Implementasikan default itu dan jangan mengarang perilaku lain.
7. **Batas scope.** Jangan menambah fitur yang ada di daftar "Di luar scope" (`17-scope-and-risks.md`) tanpa persetujuan eksplisit dari pemilik produk.
8. **Perubahan.** Ubah file pemilik, tambahkan entri di `CHANGELOG.md`, dan naikkan versi bila perilaku berubah.
9. **Ketidakjelasan.** Jika aturan tidak mencakup suatu kasus, catat sebagai asumsi eksplisit di tiket atau PR dan tambahkan entri `DEC-*` baru. Jangan menebak diam-diam.

## Urutan baca per tugas

| Tugas | File yang dimuat |
|---|---|
| Orientasi umum | `00-overview.md`, `01-design-pillars.md`, `GLOSSARY.md` |
| Gameplay korek | `04-matchstick.md`, `05-audio-cues.md`, `14-parameters.md` |
| Resolusi cue dan strike | `05-audio-cues.md`, `06-threat-and-chase.md`, `13-data-contracts-and-resolution.md`, `14-parameters.md` |
| AI dan chase | `06-threat-and-chase.md`, `07-level-flow.md`, `14-parameters.md` |
| Level dan encounter | `07-level-flow.md`, `06-threat-and-chase.md`, `02-narrative.md` |
| Narasi dan ending | `02-narrative.md`, `08-ending-sequence.md` |
| Audio (desain dan mix) | `05-audio-cues.md`, `11-audio-direction.md`, `14-parameters.md` |
| UI dan aksesibilitas | `09-ui-ux-accessibility.md`, `05-audio-cues.md` |
| Arsitektur dan kode | `12-technical-architecture.md`, `13-data-contracts-and-resolution.md`, `14-parameters.md` |
| QA | `15-acceptance-criteria.md`, `16-playtest-and-validation.md` |
| Perencanaan | `17-scope-and-risks.md`, `18-production-plan.md`, `19-open-decisions.md` |

## Indeks file

| File | Memiliki |
|---|---|
| `00-overview.md` | Ringkasan, high concept, tujuan versi |
| `01-design-pillars.md` | Pilar desain `PIL-*` |
| `02-narrative.md` | Premis, karakter, tema, aturan naratif `NAR-*`, benda pribadi, foreshadowing |
| `03-player-and-core-loop.md` | Core loop, kontrol pemain `PLR-*` |
| `04-matchstick.md` | Mekanik korek `MAT-*`, state machine korek |
| `05-audio-cues.md` | Kategori cue, aturan cue `CUE-*` |
| `06-threat-and-chase.md` | Monster, state ancaman, strike, jumpscare, failure `THR-*` |
| `07-level-flow.md` | Bagian level, durasi, loop environment, ruang jeda `LVL-*` |
| `08-ending-sequence.md` | Urutan ending `END-*` |
| `09-ui-ux-accessibility.md` | HUD, opsi, aksesibilitas `UI-*` |
| `10-art-direction.md` | Arah visual `ART-*` |
| `11-audio-direction.md` | Arah audio, layer, mixer `AUD-*` |
| `12-technical-architecture.md` | Komponen, alur runtime, performa `ARC-*`, `PERF-*` |
| `13-data-contracts-and-resolution.md` | Skema data, kontrak kode, aturan resolusi `RES-*`, reset `RST-*` |
| `14-parameters.md` | Semua parameter tuning `P-*` |
| `15-acceptance-criteria.md` | Kriteria penerimaan `ACC-*` dan keterlacakan |
| `16-playtest-and-validation.md` | Telemetri, kuesioner, target hipotesis `VAL-*` |
| `17-scope-and-risks.md` | Scope, di luar scope, risiko `RISK-*` |
| `18-production-plan.md` | Tahap produksi dan Definition of Done |
| `19-open-decisions.md` | Keputusan terbuka `DEC-*` |
| `GLOSSARY.md` | Istilah |
| `CHANGELOG.md` | Riwayat versi |
