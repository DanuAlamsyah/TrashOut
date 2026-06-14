using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         string currentScene = SceneManager.GetActiveScene().name;

    //         if (currentScene == "MainMenu")
    //         {
    //             Application.Quit();
    //         }
    //         else
    //         {
    //             SceneManager.LoadScene("MainMenu");
    //         }
    //     }
    // }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();

            Debug.Log("Level " + (currentLevel + 1) + " terbuka!");
        }
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Ubah fungsi yang paling bawah menjadi seperti ini:
    public void GoToPilahSampah(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Tambahkan fungsi ini di GameManager.cs
    public void GoToNextLevel(string nextLevelName)
    {
        SceneManager.LoadScene(nextLevelName);
    }
}