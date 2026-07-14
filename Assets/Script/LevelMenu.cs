using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [Header("Buttons Level 1-5")]
    public Button[] buttons;
    [System.Serializable]
    public class SceneMapping
    {
        public string levelScene;
        public string cutsceneScene;
    }

[Header("Scene Mapping")]
public SceneMapping[] sceneMappings;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"[LevelMenu] Awake dijalankan. Level yang sudah terbuka di PlayerPrefs: Level {unlockedLevel}");

        // Kunci semua level
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // Buka level yang sudah unlock
        int countActivated = 0;
        for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
            countActivated++;
        }
        Debug.Log($"[LevelMenu] Berhasil mengaktifkan {countActivated} tombol level di UI.");
    }

   public void OpenLevel(int levelId)
    {
        if (ButtonSound.Instance != null)
        {
            ButtonSound.Instance.PlayClick();
        }

        Debug.Log($"[LevelMenu] Tombol Level {levelId} diklik.");

        PlayerPrefs.SetInt("CurrentLevel", levelId);
        PlayerPrefs.Save();

        if(levelId <= 0 || levelId > sceneMappings.Length)
        {
            Debug.LogError("Level tidak valid!");
            return;
        }

        SceneMapping mapping = sceneMappings[levelId - 1];

        string cutsceneKey = $"SudahNontonCutscene{levelId}";

        bool sudahNonton =
            PlayerPrefs.GetInt(cutsceneKey, 0) == 1;

        if(!sudahNonton)
        {
            Debug.Log($"Load Cutscene : {mapping.cutsceneScene}");

            SceneManager.LoadScene(mapping.cutsceneScene);
        }
        else
        {
            Debug.Log("Cutscene sudah pernah ditonton.");

            SceneManager.LoadScene(mapping.levelScene);
        }
    }
    public static void UnlockCurrentLevelNext()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"[LevelMenu] Memanggil UnlockCurrentLevelNext(). Level Sekarang: {currentLevel}, Batas Unlock: {unlockedLevel}");

        if (currentLevel >= unlockedLevel)
        {
            int nextLevel = currentLevel + 1;
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();

            Debug.Log($"[LevelMenu] BERHASIL! Level {nextLevel} sekarang terbuka di PlayerPrefs!");
        }
        else
        {
            Debug.Log($"[LevelMenu] Level berikutnya tidak di-unlock karena level sekarang ({currentLevel}) lebih kecil dari batas unlock ({unlockedLevel}).");
        }
    }

    public static void ResetProgress()
    {
        if (ButtonSound.Instance != null)
        {
            ButtonSound.Instance.PlayClick();
        }
        Debug.Log("[LevelMenu] Fungsi ResetProgress() dipanggil secara statik!");

        // 🧼 1. RESET PROGRESS UTAMA BAWAAN TIM
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);

        // 🧼 2. RESET PENANDA CUTSCENE AGAR MUNCUL LAGI SAAT MENCET LEVEL 1
       // Reset seluruh status cutscene
        for (int i = 1; i <= 5; i++)
        {
            PlayerPrefs.DeleteKey($"SudahNontonCutscene{i}");
        }

        // 🧼 3. RESET TOTAL POIN GLOBAL & REWARD TOKO RAHMA
        PlayerPrefs.DeleteKey("TotalPoinGlobal");
        PlayerPrefs.DeleteKey("NilaiArmorPemain");
        PlayerPrefs.DeleteKey("PunyaPistolAngin");

        // 🧼 4. RESET STATUS TOMBOL BIAR GAK AUTO-HANGUS (BISA DIBELI LAGI)
        PlayerPrefs.DeleteKey("Hati1_Terbeli");
        PlayerPrefs.DeleteKey("Weapon_Upgraded");
        PlayerPrefs.DeleteKey("Hati2_Terbeli");

        // 🧼 5. RESET REKOR SKOR MAKSIMAL TIAP SCENE SORTIR
        PlayerPrefs.DeleteKey("Poin_sortir1");
        PlayerPrefs.DeleteKey("Poin_sortir2");
        PlayerPrefs.DeleteKey("Poin_sortir3");
        PlayerPrefs.DeleteKey("Poin_sortir4");

        // Paksa simpan semua penghapusan ke dalam brankas komputer
        PlayerPrefs.Save();
        Debug.Log("[LevelMenu] Progress, Poin, Armor, Reward, dan Cutscene berhasil DI-RESET TOTAL!");

        // Reload UI Menu agar player bisa memencet Level 1 manual
        Debug.Log("[LevelMenu] Reloading UI Menu...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Memuat scene WinCondition (dipanggil mis. saat menyelesaikan Level 5)
    public static void LoadWinCondition()
    {
        Debug.Log("[LevelMenu] Memuat scene WinCondition dan memproses unlock level berikutnya...");

        // Pastikan level saat ini di-unlock sebelum berpindah ke WinCondition
        UnlockCurrentLevelNext();

        SceneManager.LoadScene("cutscene8");
    }

    // Instance wrapper bila dipanggil dari komponen non-statik
    public void LoadWinConditionInstance()
    {
        LoadWinCondition();
    }

    public void KembaliKeMainMenu()
    {
        if (ButtonSound.Instance != null)
        {
            ButtonSound.Instance.PlayClick();
        }
        Debug.Log("[LevelMenu] Tombol Back diklik! Langsung memuat scene: mainMenu");
        SceneManager.LoadScene("mainMenu");
    }
}
