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

        // Kunci semua level
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        // Buka level yang sudah unlock
        for (int i = 0; i < unlockedLevel && i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelId)
    {
        // Simpan level yang sedang dimainkan
        PlayerPrefs.SetInt("CurrentLevel", levelId);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level" + levelId);
    }

    public static void UnlockCurrentLevelNext()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();

            Debug.Log("Level " + (currentLevel + 1) + " berhasil dibuka!");
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.Save();

        Debug.Log("Progress berhasil direset!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}