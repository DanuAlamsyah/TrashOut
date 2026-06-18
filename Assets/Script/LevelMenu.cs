using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [Header("Buttons Level 1-5")]
    public Button[] buttons;

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
        Debug.Log($"[LevelMenu] Tombol Level {levelId} KLIKS! Mencoba memuat scene...");
        
        // Simpan level yang sedang dimainkan
        PlayerPrefs.SetInt("CurrentLevel", levelId);
        PlayerPrefs.Save();
        Debug.Log($"[LevelMenu] PlayerPrefs 'CurrentLevel' disimpan dengan nilai: {levelId}");

        // Menggunakan "level" (huruf kecil) sesuai nama asset scene kamu
        string sceneToLoad = "level" + levelId;
        Debug.Log($"[LevelMenu] Memuat Scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
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
        Debug.Log("[LevelMenu] Fungsi ResetProgress() dipanggil secara statik!");
        
        // 🧼 1. RESET PROGRESS UTAMA BAWAAN TIM
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);

        // 🧼 2. RESET TOTAL POIN GLOBAL & REWARD TOKO RAHMA
        PlayerPrefs.DeleteKey("TotalPoinGlobal");
        PlayerPrefs.DeleteKey("NilaiArmorPemain");
        PlayerPrefs.DeleteKey("PunyaPistolAngin");

        // 🧼 3. RESET STATUS TOMBOL BIAR GAK AUTO-HANGUS (BISA DIBELI LAGI)
        PlayerPrefs.DeleteKey("Hati1_Terbeli");
        PlayerPrefs.DeleteKey("Senjata_Terbeli");
        PlayerPrefs.DeleteKey("Hati2_Terbeli");

        // 🧼 4. RESET REKOR SKOR MAKSIMAL TIAP SCENE SORTIR
        PlayerPrefs.DeleteKey("Poin_sortir1");
        PlayerPrefs.DeleteKey("Poin_sortir2");
        PlayerPrefs.DeleteKey("Poin_sortir3");
        PlayerPrefs.DeleteKey("Poin_sortir4");

        // Paksa simpan semua penghapusan ke dalam brankas komputer
        PlayerPrefs.Save();
        Debug.Log("[LevelMenu] Progress, Poin, Armor, dan Reward Toko Sortir berhasil DI-RESET TOTAL! Memuat ulang scene...");
        
        // Memuat ulang scene level select biar tombol-tombolnya langsung nge-lock lagi jadi Level 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Tambahkan fungsi ini di dalam LevelMenu.cs
    public void KembaliKeMainMenu()
    {
        Debug.Log("[LevelMenu] Tombol Back diklik! Langsung memuat scene: mainMenu");
        
        // Langsung tembak ke scene mainMenu tanpa lewat GameManager
        SceneManager.LoadScene("mainMenu"); 
    }
}