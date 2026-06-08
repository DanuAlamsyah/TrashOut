using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using System.Collections; // Dibutuhkan untuk menjalankan Coroutine (jeda waktu)

public class GameManagerSortir2 : MonoBehaviour
{
    public static GameManagerSortir2 Instance;

    [Header("Pengaturan Sampah & Batu")]
    public Transform spawnPoint;       
    public GameObject[] prefabSampah;  
    public GameObject prefabBatu;       
    [Range(0, 100)] public int peluangMunculBatu = 25; 
    public float jedaSpawn = 2.5f;
    private float hitungMundurSpawn;

    [Header("Sistem Waktu (Timer)")]
    public float waktuBermain = 60f; 
    private bool gameSelesai = false;
    private bool sudahBoost = false;    

    [Header("Sistem Skor & Koin")]
    public int skorSaatIni = 0;
    public int totalKoinDidapat = 0;
    public int konversiSkorKeKoin = 1; 

    [Header("Komponen UI Screen")]
    public TextMeshProUGUI teksSkor; 
    public TextMeshProUGUI teksWaktu;
    public TextMeshProUGUI teksPeringatan; 

    [Header("Transisi Level Berikutnya")]
    [Tooltip("Masukkan nama scene level selanjutnya (contoh: Level3)")]
    public string namaLevelBerikutnya;

    [HideInInspector] public float kecepatanSampahGlobal = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        kecepatanSampahGlobal = 1f; 
        sudahBoost = false;

        if (teksPeringatan != null) teksPeringatan.text = "";

        SpawnSampahAtauBatu(); 
        hitungMundurSpawn = jedaSpawn;
        gameSelesai = false;
        UpdateTeksSkorLayar(); 
    }

    void Update()
    {
        if (gameSelesai) return;

        if (waktuBermain > 0)
        {
            waktuBermain -= Time.deltaTime;
            UpdateTeksWaktuLayar(); 

            if (waktuBermain <= 15f && !sudahBoost)
            {
                sudahBoost = true;
                jedaSpawn = 1.2f; 
                kecepatanSampahGlobal = 2.2f; 
                
                TampilkanPopUpPeringatan("SPEED BOOST!", 1f);
            }
        }
        else
        {
            WaktuHabis();
        }

        hitungMundurSpawn -= Time.deltaTime;
        if (hitungMundurSpawn <= 0f)
        {
            SpawnSampahAtauBatu();
            hitungMundurSpawn = jedaSpawn;
        }
    }

    void SpawnSampahAtauBatu()
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

        SampahMinigame komponenGerak = objekBaru.AddComponent<SampahMinigame>();
        komponenGerak.kecepatanJalan = kecepatanSampahGlobal; 
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return;
        skorSaatIni += nilai;
        if (skorSaatIni < 0) skorSaatIni = 0; 
        UpdateTeksSkorLayar(); 
        
        if (nilai < 0)
        {
            TampilkanPopUpPeringatan("SALAH SORTIR! -5", 0.5f);
        }
    }

    public void KurangiWaktuBatu()
    {
        if (gameSelesai) return;
        waktuBermain -= 5f; 
        if (waktuBermain < 0) waktuBermain = 0;
        
        UpdateTeksWaktuLayar();
        TampilkanPopUpPeringatan("-5s HINDARI BATU!", 1f);
    }

    public void TampilkanPopUpPeringatan(string pesan, float durasiTampil)
    {
        if (teksPeringatan != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(ProsesPopUp(pesan, durasiTampil));
        }
    }

    // --- DI SINI SUDAH DIPERBAIKI MENJADI ProsesPopUp ---
    IEnumerator ProsesPopUp(string pesan, float durasi)
    {
        teksPeringatan.text = pesan;
        yield return new WaitForSeconds(durasi); 
        teksPeringatan.text = ""; 
    }

    void UpdateTeksSkorLayar()
    {
        if (teksSkor != null) teksSkor.text = "Skor: " + skorSaatIni;
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
        gameSelesai = true;
        waktuBermain = 0;
        if (teksPeringatan != null) teksPeringatan.text = "WAKTU HABIS!";
        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;
        
        SelesaiDanKembaliKeGameUtama();
    }

    void SelesaiDanKembaliKeGameUtama()
    {
        Debug.Log("Minigame selesai! Memuat level berikutnya: " + namaLevelBerikutnya);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToNextLevel(namaLevelBerikutnya);
        }
        else
        {
            Debug.LogWarning("GameManager utama tidak ditemukan di scene ini. Menggunakan SceneManager biasa.");
            SceneManager.LoadScene(namaLevelBerikutnya);
        }
    }
}