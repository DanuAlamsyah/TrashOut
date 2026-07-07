using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
  public static MusicManager Instance;

  private AudioSource audioSource;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    audioSource = GetComponent<AudioSource>();
    audioSource.loop = true;

    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDestroy()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void Start()
  {
    audioSource.Play();
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    // Kalau bukan scene menu, matikan musik menu
    if (scene.name != "mainMenu" &&
        scene.name != "LevelSelect" &&
        scene.name != "credit")
    {
      audioSource.Stop();
    }
    else
    {
      // Balik ke menu, hidupkan lagi kalau belum nyala
      if (!audioSource.isPlaying)
        audioSource.Play();
    }
  }

  public void StopMusic()
  {
    if (audioSource.isPlaying)
    {
      audioSource.Stop();
    }
  }
}