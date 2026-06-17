using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 

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
    public TextMeshProUGUI teksSkor; 
    public TextMeshProUGUI teksWaktu; 

    // --- TAMBAHKAN VARIABEL INI ---
    [Header("Transisi Level Berikutnya")]
    [Tooltip("Masukkan nama scene level selanjutnya (contoh: Level2, Level3, mainMenu)")]
    public string namaLevelBerikutnya;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void Start()
    {
        SpawnSampah();
        hitungMundurSpawn = jedaSpawn;
        gameSelesai = false;
        
        UpdateTeksSkorLayar(); 
        UpdateTeksWaktuLayar(); 
    }

    void Update()
    {
        if (gameSelesai) return;

        if (waktuBermain > 0)
        {
            waktuBermain -= Time.deltaTime;
            UpdateTeksWaktuLayar(); 
        }
        else
        {
            WaktuHabis();
        }

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
        komponenGerak.kecepatanJalan = 2f; 
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return;

        skorSaatIni += nilai;
        if (skorSaatIni < 0) skorSaatIni = 0; 

        Debug.Log("Skor di Console: " + skorSaatIni);
        UpdateTeksSkorLayar(); 
    }

    void UpdateTeksSkorLayar()
    {
        if (teksSkor != null)
        {
            teksSkor.text = "Skor: " + skorSaatIni;
        }
    }

    void UpdateTeksWaktuLayar()
    {
        if (teksWaktu != null)
        {
            teksWaktu.text = "Waktu: " + Mathf.CeilToInt(waktuBermain) + "s";
        }
    }

    void WaktuHabis()
    {
        if (gameSelesai) return;

        gameSelesai = true;
        waktuBermain = 0;
        UpdateTeksWaktuLayar(); 

        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;

        Debug.Log("--- MINIGAME SORTIR 1 SELESAI ---");
        Debug.Log("Total Skor Akhir: " + skorSaatIni);
        Debug.Log("Koin Didapat: " + totalKoinDidapat);

        // 🔒 1. Panggil fungsi tim di background (Buka kunci level selanjutnya)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnlockNextLevel();
        }

        // 💥 2. Munculkan Pop-Up Toko Reward kamu (Layar berhenti di sini, gak langsung pindah!)
        if (GlobalRewardManager.Instance != null)
        {
            GlobalRewardManager.Instance.MunculkanPopUpReward(skorSaatIni, "sortir1");
        }
        else
        {
            // Hanya jadi cadangan kalau kamu beneran lupa pasang prefab Toko di scene
            SelesaiDanKembaliKeGameUtama(); 
        }
    }

    // Fungsi backup bawaan dari tim (hanya dipanggil kalau toko reward tidak ada)
    void SelesaiDanKembaliKeGameUtama()
    {
        SceneManager.LoadScene("LevelSelect");
    }

}