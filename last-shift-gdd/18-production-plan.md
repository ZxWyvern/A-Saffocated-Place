---
id: 18-production-plan
title: Rencana Produksi dan Definition of Done
owns: [tahap-produksi, DoD]
depends_on: [15-acceptance-criteria, 16-playtest-and-validation]
---

# Rencana Produksi

| Tahap | Pekerjaan | Kriteria selesai |
|---|---|---|
| Greybox | Kantor, ventilasi, controller, jalur dasar. | Pemain dapat menyelesaikan alur tanpa art final. |
| Audio prototype | Kategori cue, aturan perilaku cue, spatial audio, debug view, telemetri dasar. | Pemain dapat membedakan cue awal dalam blind test dengan akurasi sesuai VAL-10. |
| Matchstick prototype | Ignition, timer, snapshot redaman, resolver dengan EditMode test. | Aturan input dan respons konsisten dan lulus test (ACC-01..ACC-03, ACC-24). |
| Threat encounter | Threat states, strike, chase, failure dan reset. | Tidak ada serangan ganda atau softlock. Retry kurang dari `P-RETRY-MAX` (ACC-09, ACC-11). |
| Narrative pass | Benda pribadi, foreshadowing, ruang jeda, urutan ending. | Ending dipahami tanpa dialog eksposisi panjang (ACC-22, VAL-14). |
| Polish dan QA | Mix audio, pencahayaan, kamera, aksesibilitas, performa. | Seluruh `ACC-*` lulus pada build. |

# Definition of Done

- Core loop dari kantor hingga ending dapat dimainkan dari awal sampai akhir.
- Setiap encounter memiliki kondisi mulai, sukses, gagal, dan reset yang eksplisit.
- Mekanik korek dan audio cue lulus kriteria penerimaan.
- Tidak ada softlock pada loop, chase, respawn, atau ending.
- Twist dapat disimpulkan dari gabungan petunjuk, dan koran berfungsi sebagai konfirmasi.
- Telemetri dan kuesioner sudah dipakai pada minimal satu putaran playtest dengan lima pemain.
- Semua keputusan terbuka (`19-open-decisions.md`) diberi status sebelum produksi konten penuh.
