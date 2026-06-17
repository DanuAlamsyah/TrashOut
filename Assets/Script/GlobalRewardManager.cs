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

    [Header("Tombol Item")]
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
        // Otomatis sembunyin panel toko saat game baru mulai, 
        // jadi kamu gak perlu uncheck manual di Unity Editor!
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

        panelReward.SetActive(true);
        UpdateVisualToko();
    }

    void UpdateVisualToko()
    {
        if (teksTotalPoin != null) teksTotalPoin.text = "Poin Kamu: " + totalPoinTabungan;

        // Item 1: 1 Hati (70 Poin)
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

        // Item 2: Senjata (130 Poin)
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

        // Item 3: 2 Hati (200 Poin)
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

    public void TukarHati1()
    {
        if (totalPoinTabungan < 70) return;
        totalPoinTabungan -= 70;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        int sisaHati = PlayerPrefs.GetInt("SisaHatiPemain", 3);
        PlayerPrefs.SetInt("SisaHatiPemain", sisaHati + 1);
        PlayerPrefs.Save();
        UpdateVisualToko();
    }

    public void TukarSenjata()
    {
        if (totalPoinTabungan < 130) return;
        totalPoinTabungan -= 130;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        PlayerPrefs.SetInt("PunyaPistolAngin", 1);
        PlayerPrefs.Save();
        UpdateVisualToko();
    }

    public void TukarHati2()
    {
        if (totalPoinTabungan < 200) return;
        totalPoinTabungan -= 200;
        PlayerPrefs.SetInt("TotalPoinGlobal", totalPoinTabungan);
        int sisaHati = PlayerPrefs.GetInt("SisaHatiPemain", 3);
        PlayerPrefs.SetInt("SisaHatiPemain", sisaHati + 2);
        PlayerPrefs.Save();
        UpdateVisualToko();
    }

    public void TombolSkipAtauLanjut()
    {
        if (!string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            SceneManager.LoadScene(namaSceneBerikutnya);
        }
    }
}