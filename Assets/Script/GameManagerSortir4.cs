using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using System.Collections;

public class GameManagerSortir4 : MonoBehaviour
{
    public static GameManagerSortir4 Instance;

    [Header("Pengaturan Objek")]
    public Transform spawnPoint;       
    public GameObject[] prefabSampah;  
    public GameObject prefabBatu;       
    public GameObject prefabBom;        // Tarik prefab bom hitam kamu ke sini nanti
    
    [Range(0, 100)] public int peluangBatu = 15; 
    [Range(0, 100)] public int peluangBom = 15;   // Peluang bom muncul (persen)
    [Range(0, 100)] public int peluangSampahHitam = 40; // Sampah misterius tetap ada
    
    public float jedaSpawn = 2.0f; // Lebih cepat dari sortir 3
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

    [HideInInspector] public float kecepatanSampahGlobal = 1.4f; // Start awal lebih ngebut

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        kecepatanSampahGlobal = 1.4f; 
        sudahBoost = false;
        if (teksPeringatan != null) teksPeringatan.text = "";

        SpawnObjekAcakLevel4(); 
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

            // FINAL RUSH: 15 Detik terakhir conveyor ngebut brutal!
            if (waktuBermain <= 15f && !sudahBoost)
            {
                sudahBoost = true;
                jedaSpawn = 0.8f; 
                kecepatanSampahGlobal = 3.0f; 
                TampilkanPopUpPeringatan("FINAL RUSH!!!", 1.5f);
            }
        }
        else
        {
            WaktuHabis();
        }

        // 2. LOGIKA SPAWN OTOMATIS
        hitungMundurSpawn -= Time.deltaTime;
        if (hitungMundurSpawn <= 0f)
        {
            SpawnObjekAcakLevel4();
            hitungMundurSpawn = jedaSpawn;
        }
    }

    void SpawnObjekAcakLevel4()
    {
        if (spawnPoint == null) return;
        GameObject objekBaru;

        int acak = Random.Range(0, 100);

        // Urutan acak: Bom dulu, lalu Batu, baru Sampah Biasa
        if (acak < peluangBom && prefabBom != null)
        {
            objekBaru = Instantiate(prefabBom, spawnPoint.position, Quaternion.identity);
        }
        else if (acak < (peluangBom + peluangBatu) && prefabBatu != null)
        {
            objekBaru = Instantiate(prefabBatu, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            if (prefabSampah.Length == 0) return;
            int randomSampah = Random.Range(0, prefabSampah.Length);
            objekBaru = Instantiate(prefabSampah[randomSampah], spawnPoint.position, Quaternion.identity);
            
            // Satukan logika Sortir 3: Sampah biasa bisa jadi siluet hitam misterius
            int acakHitam = Random.Range(0, 100);
            if (acakHitam < peluangSampahHitam)
            {
                MeshRenderer mr = objekBaru.GetComponentInChildren<MeshRenderer>();
                if (mr != null) mr.material.color = Color.black;
            }
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
        if (nilai < 0) TampilkanPopUpPeringatan("SALAH SORTIR! -5", 0.5f);
    }

    public void KurangiWaktuBatu()
    {
        if (gameSelesai) return;
        waktuBermain -= 5f; 
        if (waktuBermain < 0) waktuBermain = 0;
        UpdateTeksWaktuLayar();
        TampilkanPopUpPeringatan("-5s AWAS BATU!", 0.7f);
    }

    // 💥 FUNGSI GAME OVER JIKA BOM MELEDAK
    public void LedakanBomGameOver()
    {
        if (gameSelesai) return;
        gameSelesai = true;
        waktuBermain = 0;
        UpdateTeksWaktuLayar();
        
        if (teksPeringatan != null) teksPeringatan.text = "BOOM! GAME OVER!";
        Debug.Log("Game Over! Bom meledak!");
        
        SelesaiDanKembaliKeGameUtama();
    }

    public void TampilkanPopUpPeringatan(string pesan, float durasi)
    {
        if (teksPeringatan != null)
        {
            StopAllCoroutines();
            StartCoroutine(ProsesPopUp(pesan, durasi));
        }
    }

    IEnumerator ProsesPopUp(string pesan, float durasi)
    {
        teksPeringatan.text = pesan;
        yield return new WaitForSeconds(durasi);
        teksPeringatan.text = ""; 
    }

    void UpdateTeksSkorLayar() { if (teksSkor != null) teksSkor.text = "Skor: " + skorSaatIni; }
    void UpdateTeksWaktuLayar() { if (teksWaktu != null) teksWaktu.text = "Waktu: " + Mathf.CeilToInt(waktuBermain) + "s"; }

    void WaktuHabis()
    {
        gameSelesai = true;
        waktuBermain = 0;
        if (teksPeringatan != null) teksPeringatan.text = "MISI SORTIR LULUS!";
        totalKoinDidapat = skorSaatIni * konversiSkorKeKoin;
        SelesaiDanKembaliKeGameUtama();
    }

    void SelesaiDanKembaliKeGameUtama() { }
}