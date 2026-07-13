using UnityEngine;
using System.Collections;

public class MonsterAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip[] walkSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;

    [Header("Walk Sound")]
    public float minWalkInterval = 3f;
    public float maxWalkInterval = 6f;

    private bool isMoving = false;
    private bool isDead = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        StartCoroutine(WalkSoundRoutine());
    }

    IEnumerator WalkSoundRoutine()
    {
        while (!isDead)
        {
            float wait =
                Random.Range(minWalkInterval, maxWalkInterval);

            yield return new WaitForSeconds(wait);

            if (isMoving)
            {
                PlayRandom(walkSounds);
            }
        }
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    public void PlayHit()
    {
        if (isDead) return;

        PlayRandom(hitSounds);
    }

    public void PlayAttack()
    {
        if (isDead) return;

        PlayRandom(attackSounds);
    }

    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;

        PlayRandom(deathSounds);
    }

    void PlayRandom(AudioClip[] clips)
    {
        if (audioSource == null) return;

        if (clips == null || clips.Length == 0) return;

        audioSource.PlayOneShot(
            clips[Random.Range(0, clips.Length)]
        );
    }
}