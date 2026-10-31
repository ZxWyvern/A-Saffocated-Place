---
id: 12-technical-architecture
title: Arsitektur Teknis
owns: [ARC-*, PERF-*, alur-runtime]
depends_on: [13-data-contracts-and-resolution, 14-parameters]
---

# Arsitektur Teknis

## Prinsip

| ID | Prinsip |
|---|---|
| ARC-01 | Gunakan interface, event, dan ownership yang jelas. |
| ARC-02 | MUST NOT memakai global singleton dan `FindObjectOfType`. |
| ARC-03 | Jika memakai VContainer, daftarkan layanan pada `LifetimeScope` yang sesuai dan hindari circular dependency. |
| ARC-04 | Konfigurasi data memakai ScriptableObject. Parameter tuning dibaca dari konfigurasi (lihat `14-parameters.md`). |
| ARC-05 | Logika aturan (resolusi cue) MUST dipisah dari Unity API agar dapat diuji tanpa scene. |

## Komponen

| Komponen | Tanggung jawab | Tidak boleh |
|---|---|---|
| GameFlow dan EncounterController | Mengatur urutan level dan encounter. | Mengelola visual korek atau memutar setiap audio secara langsung. |
| AudioCueController | Memutar cue, spatial settings, variasi, dan metadata. | Menentukan hasil gameplay dari input pemain. |
| ThreatStateController | Mengelola fase ancaman dan jumlah strike. | Menerima input pemain secara langsung. |
| MatchstickController | Input, ignition, timer, cooldown, dan permintaan perpindahan snapshot mixer. | Mengklasifikasikan audio cue. |
| CueResolver | Mengevaluasi snapshot cue dan fase ancaman. Kelas C# murni tanpa MonoBehaviour. | Menjalankan efek visual atau audio. |
| MatchstickVisuals | Api, cahaya, asap, animasi tangan. | Mengubah state encounter. |
| ThreatResponse | Menerapkan hasil resolusi: reveal, strike, serangan. | Mengklasifikasikan cue. |
| CheckpointController | Reset state saat retry. | Mengetahui detail internal setiap cue. |
| TelemetryService | Menulis log playtest ke file CSV. Nonaktif di build rilis. | Memengaruhi logika gameplay. |

## Alur runtime

1. `EncounterController` mengaktifkan encounter dan konfigurasi cue.
2. `AudioCueController` memutar cue dengan `CueId`, kategori internal, waktu mulai, durasi, dan posisi.
3. `ThreatStateController` memperbarui fase ancaman secara independen.
4. Input pemain meminta `MatchstickController` menyalakan korek.
5. `MatchstickController` memvalidasi state dan cooldown, lalu memindahkan mixer ke snapshot redaman.
6. `CueResolver` membaca snapshot cue relevan, fase ancaman, dan riwayat paparan `CueId` pada waktu input.
7. Resolver mengeluarkan satu hasil (lihat `13-data-contracts-and-resolution.md`).
8. `ThreatResponse` menjalankan hasil tepat satu kali.
9. Saat encounter selesai, gagal, atau respawn, timer, event, dan state sementara dibersihkan (RST-*).

## Performa dan keandalan

| ID | Aturan |
|---|---|
| PERF-01 | Pakai ulang `AudioSource` atau pool untuk audio one-shot berulang jika profiling menunjukkan kebutuhan. |
| PERF-02 | Hindari alokasi string dan collection pada jalur Update yang berjalan setiap frame. |
| PERF-03 | Timer dijalankan dengan coroutine atau state timer yang dapat dibatalkan dan memiliki lifecycle jelas. |
| PERF-04 | MUST NOT memanggil pencarian scene setiap input. Gunakan referensi yang diinjeksi atau diikat saat setup. |
| PERF-05 | Log debug dan telemetri MUST dipisah dari build produksi. |
| PERF-06 | Target performa final ditetapkan setelah spesifikasi PC minimum diketahui (DEC-08). Profiling dilakukan pada build, bukan hanya di Editor. |
