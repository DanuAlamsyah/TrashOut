using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        ButtonSound.Instance.PlayClick();
        Debug.Log("[MainMenu] New Game dipilih.");

        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("SudahNontonCutscene1", 0);

        PlayerPrefs.Save();

        SceneManager.LoadScene("LevelSelect");
    }

    public void ContinueGame()
    {
        ButtonSound.Instance.PlayClick();
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        Debug.Log($"[MainMenu] Continue Game. Unlocked Level = {unlocked}");

        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenCredits()
    {
        ButtonSound.Instance.PlayClick();
        Debug.Log("[MainMenu] Membuka Credits.");

        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        ButtonSound.Instance.PlayClick();
        Debug.Log("[MainMenu] Keluar Game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}