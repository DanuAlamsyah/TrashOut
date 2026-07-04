using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public static ButtonSound Instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}