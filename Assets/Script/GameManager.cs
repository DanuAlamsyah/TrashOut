using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        // Singleton
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            // 1. Jika sedang di MainMenu -> keluar game
            if (currentScene == "mainMenu")
            {
                Application.Quit();
                Debug.Log("Quit Game");
            }
            // 2. Jika sedang di scene Sortir -> JANGAN NGAPA-NGAPAIN
            // (Fitur ini mengecek apakah nama scene dimulai dengan kata "sortir")
            else if (currentScene.StartsWith("sortir"))
            {
                Debug.Log("ESC ditekan di scene sortir. Sistem back ke menu dinonaktifkan.");
                // Di sini kamu biarkan kosong saja, biar ESC cuma ngurusin kursor
            }
            // 3. Jika sedang di scene lain (seperti Level1, Level2, dst) -> kembali ke MainMenu
            else
            {
                SceneManager.LoadScene("mainMenu");
            }
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
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