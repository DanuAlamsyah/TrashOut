using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class GameManagerSortir3 : MonoBehaviour
{
    public static GameManagerSortir3 Instance;

    [Header("Pengaturan Sampah & Batu")]
    public Transform spawnPoint;       
    public GameObject[] prefabSampah;  
    public GameObject prefabBatu;       // Tarik prefab batu kamu ke sini nanti
    [Range(0, 100)] public int peluangMunculBatu = 20; 
    public float jedaSpawn = 2.2f; // Jeda spawn dibuat agak lebih menantang
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
    public TextMeshProUGUI teksPeringatan;

    // --- TAMBAHKAN VARIABEL INI ---
    [Header("Transisi Level Berikutnya")]
    [Tooltip("Masukkan nama scene level selanjutnya (contoh: Level4)")]
    public string namaLevelBerikutnya;

    [HideInInspector] public float kecepatanSampahGlobal = 2.7f; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        kecepatanSampahGlobal = 2.7f; 
        if (teksPeringatan != null) teksPeringatan.text = "";

        SpawnObjekMisterius(); 
        hitungMundurSpawn = jedaSpawn;
        gameSelesai = false;
        UpdateTeksSkorLayar(); 
        UpdateTeksWaktuLayar();
    }

    void Update()
    {
        if (gameSelesai) return;

        // 1. LOGIKA TIMER
        if (waktuBermain > 0)
        {
            waktuBermain -= Time.deltaTime;
            UpdateTeksWaktuLayar(); 
        }
        else
        {
            WaktuHabis();
        }

        // 2. LOGIKA SPAWN OTOMATIS
        hitungMundurSpawn -= Time.deltaTime;
        if (hitungMundurSpawn <= 0f)
        {
            SpawnObjekMisterius();
            hitungMundurSpawn = jedaSpawn;
        }
    }

    [Header("Persentase Khas Sortir 3")]
    [Range(0, 100)] public int peluangSampahHitam = 50; // Set 50 artinya setengah sampah bakal hitam

    void SpawnObjekMisterius()
    {
        if (spawnPoint == null) return;
        GameObject objekBaru;

        int acak = Random.Range(0, 100);
        if (acak < peluangMunculBatu && prefabBatu != null)
        {
            objekBaru = Instantiate(prefabBatu, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            if (prefabSampah.Length == 0) return;
            int randomSampah = Random.Range(0, prefabSampah.Length);
            objekBaru = Instantiate(prefabSampah[randomSampah], spawnPoint.position, Quaternion.identity);
        }

        // 🌟 DIACAK DI SINI: Cek apakah sampah ini kebagian takdir jadi hitam atau normal
        int acakHitam = Random.Range(0, 100);
        if (acakHitam < peluangSampahHitam)
        {
            MeshRenderer mr = objekBaru.GetComponentInChildren<MeshRenderer>();
            if (mr != null)
            {
                mr.material.color = Color.black; // Muncul sebagai siluet arang hitam
            }
        }
        // Jika acakHitam >= peluangSampahHitam, warnanya dibiarkan normal bawaan prefab!

        SampahMinigame komponenGerak = objekBaru.AddComponent<SampahMinigame>();
        komponenGerak.kecepatanJalan = kecepatanSampahGlobal; 
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return;
        skorSaatIni += nilai;
        if (skorSaatIni < 0) skorSaatIni = 0; 
        UpdateTeksSkorLayar(); 
    }

    public void KurangiWaktuBatu()
    {
        if (gameSelesai) return;
        waktuBermain -= 5f; 
        if (waktuBermain < 0) waktuBermain = 0;
        UpdateTeksWaktuLayar();
        TampilkanPopUpPeringatan("-5s AWAS BATU!", 0.7f);
    }

    public void TampilkanPopUpPeringatan(string pesan, float durasi)
    {
        if (teksPeringatan != null)
        {
            StopAllCoroutines();
            StartCoroutine(ProsesPopUp(pesan, durasi));
        }
    }

    System.Collections.IEnumerator ProsesPopUp(string pesan, float durasi)
    {
        teksPeringatan.text = pesan;
        yield return new WaitForSeconds(durasi);
        teksPeringatan.text = ""; 
    }

    void UpdateTeksSkorLayar() { if (teksSkor != null) teksSkor.text = "Skor: " + skorSaatIni; }
    void UpdateTeksWaktuLayar() { if (teksWaktu != null) teksWaktu.text = "Waktu: " + Mathf.CeilToInt(waktuBermain) + "s"; }

    // Di dalam GameManagerSortir2.cs
    void WaktuHabis()
    {
        if (gameSelesai) return;
        gameSelesai = true;
        waktuBermain = 0;
        UpdateTeksWaktuLayar(); 
        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;

        // 🌟 KONDISI MENANG: Karena berhasil bertahan sampai waktu habis, 
        // BARU BOLEH buka gembok Level 4 game utama!
        int levelTerbuka = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (levelTerbuka < 4) 
        {
            PlayerPrefs.SetInt("UnlockedLevel", 4);
            PlayerPrefs.Save();
        }

        if (GameManager.Instance != null) GameManager.Instance.UnlockNextLevel();

        // Panggil Toko Reward Sortir 3
        if (GlobalRewardManager.Instance != null)
        {
            GlobalRewardManager.Instance.MunculkanPopUpReward(skorSaatIni, "sortir3");
        }
        else
        {
            SceneManager.LoadScene("LevelSelect"); 
        }
    }

    // Fungsi ini dipanggil kalau player keluar atau ada kondisi gagal di sortir 3
    void SelesaiDanKembaliKeGameUtama()
    {
        // 🌟 KONDISI KALAH/NGULANG: Pastikan Level 3 tetap terbuka (jangan turun ke 1 atau 2), 
        // tapi Level 4 JANGAN DI-UNLOCK DULU!
        int levelTerbuka = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (levelTerbuka < 3) 
        {
            PlayerPrefs.SetInt("UnlockedLevel", 3);
            PlayerPrefs.Save();
        }

        // Langsung lempar ke select level biar dia ngulang klik Level 3
        SceneManager.LoadScene("LevelSelect");
    }
}