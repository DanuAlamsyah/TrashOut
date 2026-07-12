using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("[GameManager] Duplikasi terdeteksi, menghancurkan GameManager baru.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[GameManager] Instance dibuat dan dipertahankan.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    // =========================
    // ESC HANDLER
    // =========================

    private void HandleEscape()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"[GameManager] Tombol ESC ditekan pada scene: {currentScene}");

        switch (currentScene)
        {
            // ESC di Main Menu = Keluar Game
            case "mainMenu":
                QuitGame();
                break;

            // ESC di Credits & Level Select = Main Menu
            case "Credits":
            case "LevelSelect":
                SceneManager.LoadScene("mainMenu");
                break;

            // ESC di gameplay = Level Select
            default:
                SceneManager.LoadScene("LevelSelect");
                break;
        }
    }

    // =========================
    // LEVEL SYSTEM
    // =========================

    public void UnlockNextLevel()
    {
        LevelMenu.UnlockCurrentLevelNext();
    }

    // =========================
    // MAIN MENU
    // =========================

    public void NewGame()
    {
        LevelMenu.ResetProgress();
    }
    public void ContinueGame()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        Debug.Log($"[GameManager] Continue Game. Unlocked Level = {unlocked}");

        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenCredits()
    {
        Debug.Log("[GameManager] Membuka Credits.");

        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Keluar Game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // =========================
    // NAVIGATION
    // =========================

    public void Back()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log($"[GameManager] Back ditekan dari {currentScene}");

        if (currentScene == "Credits" || currentScene == "LevelSelect")
        {
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public void BackToLevelSelect()
    {
        Debug.Log("[GameManager] Kembali ke Level Select.");

        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToPilahSampah(string sceneName)
    {
        Debug.Log($"[GameManager] Memuat scene {sceneName}");

        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextLevel(string sceneName)
    {
        Debug.Log($"[GameManager] Next Level -> {sceneName}");

        SceneManager.LoadScene(sceneName);
    }
}