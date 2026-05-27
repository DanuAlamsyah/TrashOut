using UnityEngine;
using UnityEngine.SceneManagement; // Dibutuhkan jika nanti ingin pindah scene saat game selesai

public class GameManagerSortir : MonoBehaviour
{
    public static GameManagerSortir Instance;

    [Header("Pengaturan Sampah")]
    public Transform spawnPoint;       
    public GameObject[] prefabSampah;  // Kamu bisa isi sampai 20 atau lebih prefab di sini
    public float jedaSpawn = 2.5f;
    private float hitungMundurSpawn;

    [Header("Sistem Waktu (Timer)")]
    public float waktuBermain = 60f; // 60 detik = 1 menit (bisa diubah di Inspector)
    private bool gameSelesai = false;

    [Header("Sistem Skor & Koin")]
    public int skorSaatIni = 0;
    public int totalKoinDidapat = 0;
    public int konversiSkorKeKoin = 1; // Misal: 1 skor = 1 koin (bisa disesuaikan)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        hitungMundurSpawn = jedaSpawn;
        gameSelesai = false;
    }

    void Update()
    {
        // Jika game sudah selesai, hentikan semua logika di bawahnya
        if (gameSelesai) return;

        // 1. LOGIKA TIMER (PENGURANG WAKTU)
        if (waktuBermain > 0)
        {
            waktuBermain -= Time.deltaTime;
            
            // Tampilkan waktu di Console (Nanti bisa dihubungkan ke UI Text)
            // Debug.Log("Sisa Waktu: " + Mathf.CeilToInt(waktuBermain) + " detik");
        }
        else
        {
            WaktuHabis();
        }

        // 2. LOGIKA SPAWN SAMPAH (Hanya berjalan kalau waktu masih ada)
        hitungMundurSpawn -= Time.deltaTime;
        if (hitungMundurSpawn <= 0f)
        {
            SpawnSampah();
            hitungMundurSpawn = jedaSpawn;
        }
    }

    void SpawnSampah()
    {
        if (prefabSampah.Length == 0 || spawnPoint == null) return;

        // Pilih acak dari list sampah (bisa menampung hingga puluhan variasi)
        int randomSampah = Random.Range(0, prefabSampah.Length);

        // Munculkan klon sampah
        GameObject sampahBaru = Instantiate(prefabSampah[randomSampah], spawnPoint.position, Quaternion.identity);

        // Suntikkan komponen gerak otomatis khusus minigame
        SampahMinigame komponenGerak = sampahBaru.AddComponent<SampahMinigame>();
        komponenGerak.kecepatanJalan = 1f; 
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return;

        skorSaatIni += nilai;
        
        // Mencegah skor minus jika pemain salah sortir terus-terusan
        if (skorSaatIni < 0) skorSaatIni = 0; 

        Debug.Log("Skor: " + skorSaatIni);
    }

    void WaktuHabis()
    {
        gameSelesai = true;
        waktuBermain = 0;

        // 3. LOGIKA KONVERSI SKOR MENJADI KOIN
        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;

        Debug.Log("--- MINIGAME SELESAI ---");
        Debug.Log("Total Skor Akhir: " + skorSaatIni);
        Debug.Log("Koin yang Kamu Dapatkan: " + totalKoinDidapat);

        // TIPS UNTUK GAME UTAMA:
        // Di sini kamu bisa menyimpan koinnya ke PlayerPrefs agar bisa dibaca di scene utama:
        // PlayerPrefs.SetInt("KoinPemain", PlayerPrefs.GetInt("KoinPemain", 0) + totalKoinDidapat);
        
        // Panggil fungsi untuk memunculkan UI Selesai / Pindah Scene di sini
        SelesaiDanKembaliKeGameUtama();
    }

    void SelesaiDanKembaliKeGameUtama()
    {
        // Contoh: Jika ingin otomatis balik ke game utama setelah 3 detik
        // Invoke("MuatSceneGameUtama", 3f);
    }

    void MuatSceneGameUtama()
    {
        // SceneManager.LoadScene("NamaSceneGameUtamaKamu");
    }
}