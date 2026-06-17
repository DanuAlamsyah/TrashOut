using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Instance dibuat. Objek didaftarkan ke DontDestroyOnLoad.");
        }
        else
        {
            Debug.LogWarning("[GameManager] Duplikasi terdeteksi! Menghancurkan game object tiruan.");
            Destroy(gameObject);
        }
    }

    // =========================
    // LEVEL SYSTEM
    // =========================

    public void UnlockNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"[GameManager] Memanggil UnlockNextLevel(). Current: {currentLevel}, Unlocked: {unlockedLevel}");

        if (currentLevel >= unlockedLevel)
        {
            int nextLevel = currentLevel + 1;
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();

            Debug.Log($"[GameManager] SUKSES: Level {nextLevel} terbuka!");
        }
        else
        {
            Debug.Log("[GameManager] Level berikutnya sudah pernah terbuka sebelumnya.");
        }
    }

    // =========================
    // MENU
    // =========================

    public void NewGame()
    {
        Debug.Log("[GameManager] Tombol New Game ditekan. Mereset data progress...");
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.Save();
        
        Debug.Log("[GameManager] Memuat scene LevelSelect...");
        SceneManager.LoadScene("LevelSelect");
    }

    public void ContinueGame()
    {
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log($"[GameManager] Tombol Continue Game ditekan. Progress terakhir player: Level {currentUnlocked}. Memuat scene LevelSelect...");
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenCredits()
    {
        Debug.Log("[GameManager] Membuka Scene Credits...");
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Keluar dari Game (Application.Quit)...");
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("[GameManager] Menghentikan Play Mode di Unity Editor.");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // =========================
    // NAVIGATION
    // =========================

    public void BackToLevelSelect()
    {
        Debug.Log("[GameManager] Navigasi: Kembali ke LevelSelect.");
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToPilahSampah(string sceneName)
    {
        Debug.Log($"[GameManager] Navigasi: Pergi ke scene Pilah Sampah -> {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextLevel(string sceneName)
    {
        Debug.Log($"[GameManager] Navigasi: Pindah ke Level Berikutnya -> {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void Back()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"[GameManager] Tombol Back ditekan dari scene: {currentScene}");

        if (currentScene == "LevelSelect" || currentScene == "Credits")
        {
            Debug.Log("[GameManager] Mengarahkan kembali ke mainMenu.");
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            Debug.Log("[GameManager] Mengarahkan kembali ke LevelSelect.");
            SceneManager.LoadScene("LevelSelect");
        }
    }
}