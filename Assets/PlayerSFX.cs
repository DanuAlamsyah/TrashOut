using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip shotSound;
    public AudioClip jumpSound;

    [Header("Volume")]
    [Range(0f, 1f)] public float shotVolume = 1f;
    [Range(0f, 1f)] public float jumpVolume = 1f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayShotSound()
    {
        if (shotSound != null)
        {
            audioSource.PlayOneShot(shotSound, shotVolume);
        }
    }

    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }
}
