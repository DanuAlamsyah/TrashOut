using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GlobalRewardManager : MonoBehaviour
{
    public static GlobalRewardManager Instance;

    [Header("UI PopUp Panel")]
    public GameObject panelReward;
    public TextMeshProUGUI teksTotalPoin;

    [Header("Komponen Tombol Toko UI")]
    public Button tombolHati1;
    public Button tombolSenjata;
    public Button tombolHati2;

    [Header("Visual Gembok Objek")]
    public GameObject gembokHati1;
    public GameObject gembokSenjata;
    public GameObject gembokHati2;

    [Header("Nama Scene Tujuan Berikutnya")]
    public string namaSceneBerikutnya; 

    private int totalPoinTabungan;

    void Awake()
    {
        Instance = this;
        // Otomatis menyembunyikan panel reward saat game baru mulai berjalan
        if (panelReward != null) panelReward.SetActive(false); 
    }

    // Fungsi dipanggil oleh GameManagerSortir saat waktu bermain habis
    public void MunculkanPopUpReward(int poinBaruDariLevelIni, string namaLevelSortirIni)
    {
        // 1. Logika Akumulasi Skor & Proteksi Replay Level
        int poinLamaLevelIni = PlayerPrefs.GetInt("Poin_" + namaLevelSortirIni, 0);
        totalPoinTabungan = PlayerPrefs.GetInt("TotalPoinGlobal", 0);

        if (poinBaruDariLevelIni > poinLamaLevelIni)
        {
            int selisih = poinBaruDariLevelIni - poinLamaLevelIni;
            totalPoinTabungan += selisih;
            PlayerPrefs.SetInt("Poin_" + namaLevelSortirIni, poinBaruDariLevelIni);
        }

        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        PlayerPrefs.Save();

        // 2. Aktifkan visual panel toko
        if (panelReward != null) panelReward.SetActive(true);
        
        UpdateVisualToko();
    }

    public void UpdateVisualToko()
    {
        // Update teks sisa tabungan koin/poin pemain di layar
        if (teksTotalPoin != null) teksTotalPoin.text = "Poin Kamu: " + totalPoinTabungan;

        // ==========================================
        // 🔒 LOGIKA UNTUK ITEM 1: 1 HATI (70 POIN)
        // ==========================================
        if (PlayerPrefs.GetInt("Hati1_Terbeli", 0) == 1)
        {
            tombolHati1.interactable = false; // Tombol mati/hangus
            if (gembokHati1 != null) gembokHati1.SetActive(false); // Gembok hilang karena sudah dibeli
        }
        else
        {
            // Jika belum dibeli, tombol menyala jika poin cukup, gembok aktif jika poin kurang
            if (totalPoinTabungan >= 70)
            {
                tombolHati1.interactable = true;
                if (gembokHati1 != null) gembokHati1.SetActive(false);
            }
            else
            {
                tombolHati1.interactable = false;
                if (gembokHati1 != null) gembokHati1.SetActive(true);
            }
        }

        // ==========================================
        // 🔒 LOGIKA UNTUK ITEM 2: SENJATA (130 POIN)
        // ==========================================
        if (PlayerPrefs.GetInt("Senjata_Terbeli", 0) == 1)
        {
            tombolSenjata.interactable = false; // Tombol mati/hangus
            if (gembokSenjata != null) gembokSenjata.SetActive(false);
        }
        else
        {
            if (totalPoinTabungan >= 130)
            {
                tombolSenjata.interactable = true;
                if (gembokSenjata != null) gembokSenjata.SetActive(false);
            }
            else
            {
                tombolSenjata.interactable = false;
                if (gembokSenjata != null) gembokSenjata.SetActive(true);
            }
        }

        // ==========================================
        // 🔒 LOGIKA UNTUK ITEM 3: 2 HATI (200 POIN)
        // ==========================================
        if (PlayerPrefs.GetInt("Hati2_Terbeli", 0) == 1)
        {
            tombolHati2.interactable = false; // Tombol mati/hangus
            if (gembokHati2 != null) gembokHati2.SetActive(false);
        }
        else
        {
            if (totalPoinTabungan >= 200)
            {
                tombolHati2.interactable = true;
                if (gembokHati2 != null) gembokHati2.SetActive(false);
            }
            else
            {
                tombolHati2.interactable = false;
                if (gembokHati2 != null) gembokHati2.SetActive(true);
            }
        }
    }

    // ========================================================
    // 🕹️ FUNGSI EKSEKUSI TRIGER ON-CLICK TOMBOL ITEM TOKO
    // ========================================================

    public void TukarHati1()
    {
        if (totalPoinTabungan < 70) return;

        totalPoinTabungan -= 70;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);

        // 🛡️ SEKARANG JADI: BELI ARMOR LV 1 (Menambah 5 Perlindungan)
        PlayerPrefs.SetInt("NilaiArmorPemain", 5);

        PlayerPrefs.SetInt("Hati1_Terbeli", 1); // Status tombol hangus tetap sama
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut(); 
    }

    public void TukarHati2()
    {
        if (totalPoinTabungan < 200) return;

        totalPoinTabungan -= 200;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);

        // 🛡️ SEKARANG JADI: BELI ARMOR LV 2 (Menambah 15 Perlindungan)
        PlayerPrefs.SetInt("NilaiArmorPemain", 15);

        PlayerPrefs.SetInt("Hati2_Terbeli", 1);
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut();
    }

    public void TukarSenjata()
    {
        if (totalPoinTabungan < 130) return;

        totalPoinTabungan -= 130;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);

        // Buka izin kepemilikan senjata di game utama
        PlayerPrefs.SetInt("PunyaPistolAngin", 1);

        // Kunci jatah item ini biar hangus
        PlayerPrefs.SetInt("Senjata_Terbeli", 1);
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut();
    }

    // ========================================================
    // 🚀 FUNGSI TRIGER NAVIGASI / PINDAH SCENE LEVEL
    // ========================================================

    // Dipakai untuk Tombol "Lanjut / Skip"
    public void TombolSkipAtauLanjut()
    {
        if (!string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            SceneManager.LoadScene(namaSceneBerikutnya);
        }
        else
        {
            Debug.LogError("Nama scene berikutnya kosong! Isi dulu di Inspector.");
        }
    }

    // Dipakai untuk Tombol Baru buatanmu "Pilih Level" agar sinkron dengan menu tim
    public void TombolKembaliKeLevelSelect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BackToLevelSelect();
        }
        else
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}