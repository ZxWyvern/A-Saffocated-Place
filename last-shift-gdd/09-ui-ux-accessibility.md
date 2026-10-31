---
id: 09-ui-ux-accessibility
title: UI, UX, dan Aksesibilitas
owns: [UI-*]
depends_on: [05-audio-cues, 11-audio-direction]
---

# UI, UX, dan Aksesibilitas

| ID | Elemen | Aturan |
|---|---|---|
| UI-01 | HUD | Minimal. MUST NOT ada indikator yang memberi tahu kategori cue secara langsung. |
| UI-02 | Interaction prompt | Muncul hanya saat objek dapat diinteraksikan dan hilang saat fokus berpindah. |
| UI-03 | Feedback korek | Animasi tangan, suara gesekan, nyala api, dan redaman audio. Tanpa UI besar. |
| UI-04 | Opsi audio | Slider master, music, dan SFX. Opsi mono compatibility dan subtitle untuk dialog serta kejadian audio penting. |
| UI-05 | Subtitle | MUST NOT menuliskan label seperti "[suara monster]" jika tujuannya agar pemain mengidentifikasi suara sendiri. |
| UI-06 | Indikator visual cue (opsional) | Denyut halus di tepi layar yang menunjukkan arah dan intensitas cue. MUST NOT membedakan kategori, sehingga pemain tetap menilai sendiri apakah cue itu ancaman. Mati secara default (DEC-06). |
| UI-07 | Haptic (opsional) | Getaran kontroler sesuai intensitas dan arah cue, dengan batasan yang sama seperti UI-06. Mati secara default. |
| UI-08 | Kenyamanan | Opsi mengurangi camera shake, vignette, dan efek flicker yang intens. |

## Catatan implementasi aksesibilitas

- Data untuk UI-06 dan UI-07 (arah, intensitas) sudah tersedia dari posisi dan metadata cue. Kategori cue MUST NOT dikirim ke lapisan UI.
- Opsi aksesibilitas tidak boleh mengubah hasil resolusi cue atau parameter encounter.
