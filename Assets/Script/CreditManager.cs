using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    public float creditDuration = 20f; // sesuaikan dengan lama animasi

    void Start()
    {
        Invoke(nameof(LoadMainMenu), creditDuration);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}