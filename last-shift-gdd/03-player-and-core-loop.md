---
id: 03-player-and-core-loop
title: Core Loop dan Kontrol Pemain
owns: [core-loop, PLR-*]
depends_on: [01-design-pillars]
---

# Core Gameplay Loop

1. Eksplorasi ruang terbatas dan cari petunjuk lingkungan.
2. Dengarkan audio untuk memperkirakan sumber suara dan ancaman.
3. Pilih menunggu, bergerak, atau menyalakan korek.
4. Jika korek dinyalakan, pemain mendapat informasi visual singkat tetapi tidak bisa mendengar dengan jelas selama api menyala (lihat `04-matchstick.md`).
5. Navigasikan loop dan hindari monster saat chase.
6. Capai jalur keluar yang tampak nyata.
7. Saksikan transisi ke meja kerja dan pengungkapan identitas korban (lihat `08-ending-sequence.md`).

# Kontrol pemain

| ID | Fitur | Persyaratan |
|---|---|---|
| PLR-01 | Movement | Berjalan dan merangkak. Kecepatan disesuaikan dengan ruang sempit. |
| PLR-02 | Sprint | MUST aktif hanya pada bagian chase yang ditentukan dan MUST NOT tersedia tanpa batas. Sprint khusus berada di luar scope kecuali `DEC` baru memutuskan lain. |
| PLR-03 | Interaction | Raycast dan interaction interface untuk ventilasi, kartu identitas, dokumen, benda pribadi, dan objek kunci. |
| PLR-04 | Input lock | Kontrol MUST dapat dikunci selama jumpscare, transisi, dan ending. |
| PLR-05 | Camera | First-person dengan sway ringan yang tidak mengganggu navigasi atau kenyamanan. Opsi pengurangan efek ada di `09-ui-ux-accessibility.md`. |
| PLR-06 | Tanpa combat | Pemain MUST NOT memiliki senjata atau aksi serangan. |
