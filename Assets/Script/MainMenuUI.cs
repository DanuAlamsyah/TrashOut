using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void KlikPlay()
    {
        Debug.Log("Tombol Play diklik! Mengecek GameManager...");

        // Memanggil fungsi dari GameManager yang sedang aktif
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            Debug.LogWarning("Waduh, GameManager tidak ditemukan di scene Main Menu!");
        }
    }

    public void KlikContinue()
    {
        Debug.Log("Tombol Continue diklik! Mengecek GameManager...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
            Debug.LogWarning("Waduh, GameManager tidak ditemukan di scene Main Menu!");
        }
    }

    public void KlikQuit()
    {
        Debug.Log("Tombol Quit diklik! Mengecek GameManager...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
        else
        {
            Debug.LogWarning("Waduh, GameManager tidak ditemukan di scene Main Menu!");
        }
    }

    public void KlikCredits()
    {
        Debug.Log("Tombol Credits diklik! Mengecek GameManager...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OpenCredits();
        }
        else
        {
            Debug.LogWarning("Waduh, GameManager tidak ditemukan di scene Main Menu!");
        }
    }
}