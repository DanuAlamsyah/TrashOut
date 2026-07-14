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
    public Button tombolUpgradeSenjata; // Diubah nama variabel agar lebih jelas
    public Button tombolHati2;

    [Header("Visual Gembok Objek")]
    public GameObject gembokHati1;
    public GameObject gembokUpgradeSenjata; // Diubah nama variabel agar lebih jelas
    public GameObject gembokHati2;

    [Header("Nama Scene Tujuan Berikutnya")]
    public string namaSceneBerikutnya; 

    private int totalPoinTabungan;

    void Awake()
    {
        Instance = this;
        if (panelReward != null) panelReward.SetActive(false); 
    }

    public void MunculkanPopUpReward(int poinBaruDariLevelIni, string namaLevelSortirIni)
    {
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

        if (panelReward != null) panelReward.SetActive(true);
        
        UpdateVisualToko();
    }

    public void UpdateVisualToko()
    {
        if (teksTotalPoin != null) teksTotalPoin.text = "Poin Kamu: " + totalPoinTabungan;

        // ==========================================
        // 🔒 LOGIKA UNTUK ITEM 1: ARMOR LV 1 (70 POIN)
        // ==========================================
        if (PlayerPrefs.GetInt("Hati1_Terbeli", 0) == 1)
        {
            tombolHati1.interactable = false;
            if (gembokHati1 != null) gembokHati1.SetActive(false);
        }
        else
        {
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
        // 🔒 LOGIKA UNTUK ITEM 2: UPGRADE SENJATA (200 POIN) -> ✨ SUDAH DIPERBAIKI VISUALNYA
        // ==========================================
        if (PlayerPrefs.GetInt("Weapon_Upgraded", 0) == 1) 
        {
            tombolUpgradeSenjata.interactable = false; 
            if (gembokUpgradeSenjata != null) gembokUpgradeSenjata.SetActive(false);
        }
        else
        {
            if (totalPoinTabungan >= 200) // Pengecekan visual diubah ke 200
            {
                tombolUpgradeSenjata.interactable = true;
                if (gembokUpgradeSenjata != null) gembokUpgradeSenjata.SetActive(false);
            }
            else
            {
                tombolUpgradeSenjata.interactable = false;
                if (gembokUpgradeSenjata != null) gembokUpgradeSenjata.SetActive(true);
            }
        }

        // ==========================================
        // 🔒 LOGIKA UNTUK ITEM 3: ARMOR LV 2 (130 POIN) -> ✨ SUDAH DIPERBAIKI VISUALNYA
        // ==========================================
        if (PlayerPrefs.GetInt("Hati2_Terbeli", 0) == 1)
        {
            tombolHati2.interactable = false;
            if (gembokHati2 != null) gembokHati2.SetActive(false);
        }
        else
        {
            if (totalPoinTabungan >= 130) // Pengecekan visual diubah ke 130
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

    public void TukarHati1()
    {
        if (ButtonSound.Instance != null) ButtonSound.Instance.PlayClick();
        if (totalPoinTabungan < 70) return;
        totalPoinTabungan -= 70;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        PlayerPrefs.SetInt("NilaiArmorPemain", 5);
        PlayerPrefs.SetInt("Hati1_Terbeli", 1);
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut(); 
    }

    public void TukarHati2()
    {
        if (ButtonSound.Instance != null) ButtonSound.Instance.PlayClick();
        if (totalPoinTabungan < 130) return;
        totalPoinTabungan -= 130;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        PlayerPrefs.SetInt("NilaiArmorPemain", 15);
        PlayerPrefs.SetInt("Hati2_Terbeli", 1);
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut();
    }

    public void TukarUpgradeSenjata()
    {
        if (ButtonSound.Instance != null) ButtonSound.Instance.PlayClick();
        if (totalPoinTabungan < 200) return;

        totalPoinTabungan -= 200;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        PlayerPrefs.SetInt("Weapon_Upgraded", 1);
        PlayerPrefs.Save();

        UpdateVisualToko();
        TombolSkipAtauLanjut();
    }

    public void TombolSkipAtauLanjut()
    {
        if (ButtonSound.Instance != null) ButtonSound.Instance.PlayClick();
        if (!string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            SceneManager.LoadScene(namaSceneBerikutnya);
        }
        else
        {
            Debug.LogError("Nama scene berikutnya kosong! Isi dulu di Inspector.");
        }
    }

   public void TombolKembaliKeLevelSelect()
    {
        if (ButtonSound.Instance != null) ButtonSound.Instance.PlayClick();
        ButtonSound.Instance.PlayClick();

        LevelMenu.UnlockCurrentLevelNext();

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