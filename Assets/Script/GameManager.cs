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

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape)) // Jika memencet tombol Escape
    //     {
    //         string currentScene = SceneManager.GetActiveScene().name;

    //         // Jika sedang di MainMenu -> keluar game
    //         if (currentScene == "mainMenu")
    //         {
    //             Application.Quit();
    //             Debug.Log("Quit Game");
    //         }
    //         // Jika sedang di scene lain -> kembali ke MainMenu
    //         else
    //         {
    //             SceneManager.LoadScene("mainMenu");
    //         }
    //     }
    // }

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
}