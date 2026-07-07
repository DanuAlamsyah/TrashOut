using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    [Header("Background Music")]
    public AudioSource audioSource;
    [Range(0f, 1f)] public float musicVolume = 0.25f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (audioSource != null)
            {
                audioSource.volume = musicVolume;
                audioSource.loop = true;
                audioSource.playOnAwake = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "mainMenu" ||
            scene.name == "LevelSelect" ||
            scene.name == "credit")
        {
            Destroy(gameObject);
        }
    }
}
