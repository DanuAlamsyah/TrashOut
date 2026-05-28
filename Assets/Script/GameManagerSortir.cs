using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; // WAJIB ada untuk mengontrol TextMeshPro UI

public class GameManagerSortir : MonoBehaviour
{
    public static GameManagerSortir Instance;

    [Header("Pengaturan Sampah")]
    public Transform spawnPoint;       
    public GameObject[] prefabSampah;  
    public float jedaSpawn = 2.5f;
    private float hitungMundurSpawn;

    [Header("Sistem Waktu (Timer)")]
    public float waktuBermain = 60f; 
    private bool gameSelesai = false;

    [Header("Sistem Skor & Koin")]
    public int skorSaatIni = 0;
    public int totalKoinDidapat = 0;
    public int konversiSkorKeKoin = 1; 

    [Header("Komponen UI Screen")]
    public TextMeshProUGUI teksSkor; // Kotak tempat narik objek UI teks di Inspector

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    void Start()
    {
        // Panggil fungsi spawn di sini agar sampah pertama LANGSUNG muncul saat play
        SpawnSampah();

        // Sisa hitung mundur untuk sampah kedua dan seterusnya tetap berjalan normal
        hitungMundurSpawn = jedaSpawn;
        gameSelesai = false;
        
        UpdateTeksSkorLayar(); 
    }

    void Update()
    {
        if (gameSelesai) return;

        // 1. LOGIKA TIMER (PENGURANG WAKTU)
        if (waktuBermain > 0)
        {
            waktuBermain -= Time.deltaTime;
        }
        else
        {
            WaktuHabis();
        }

        // 2. LOGIKA SPAWN SAMPAH
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

        int randomSampah = Random.Range(0, prefabSampah.Length);
        GameObject sampahBaru = Instantiate(prefabSampah[randomSampah], spawnPoint.position, Quaternion.identity);

        SampahMinigame komponenGerak = sampahBaru.AddComponent<SampahMinigame>();
        komponenGerak.kecepatanJalan = 1f; 
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return;

        skorSaatIni += nilai;
        
        // Mencegah skor minus jika pemain salah sortir terus-terusan
        if (skorSaatIni < 0) skorSaatIni = 0; 

        Debug.Log("Skor di Console: " + skorSaatIni);
        
        // UPDATE UI DI LAYAR HP/MONITOR
        UpdateTeksSkorLayar(); 
    }

    // Fungsi pembantu untuk memperbarui visual teks di Canvas UI
    void UpdateTeksSkorLayar()
    {
        if (teksSkor != null)
        {
            teksSkor.text = "Skor: " + skorSaatIni;
        }
    }

    void WaktuHabis()
    {
        gameSelesai = true;
        waktuBermain = 0;

        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;

        Debug.Log("--- MINIGAME SELESAI ---");
        Debug.Log("Total Skor Akhir: " + skorSaatIni);
        Debug.Log("Koin yang Kamu Dapatkan: " + totalKoinDidapat);
        
        SelesaiDanKembaliKeGameUtama();
    }

    void SelesaiDanKembaliKeGameUtama()
    {
        // Tempat naruh logika UI Game Over kamu nanti
    }
}