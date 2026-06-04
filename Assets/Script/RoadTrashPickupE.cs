using UnityEngine;

public class RoadTrashPickupE : MonoBehaviour
{
    [Header("Pickup Sound")]
    public AudioClip pickupSound;
    public float pickupVolume = 1f;

    private bool playerInRange = false;
    private RoadTrashCounterManager counterManager;

    void Start()
    {
        counterManager = FindObjectOfType<RoadTrashCounterManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpTrash();
        }
    }

    void PickUpTrash()
    {
        if (counterManager != null)
        {
            counterManager.AddTrash();
            counterManager.ShowInteractText(false);
        }

        PlayPickupSound();

        Destroy(gameObject);
    }

    void PlayPickupSound()
    {
        if (pickupSound == null) return;

        GameObject soundObject = new GameObject("PickupSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = pickupSound;
        audioSource.volume = pickupVolume;

        // 0 = suara 2D, jadi tidak mengecil karena jarak
        audioSource.spatialBlend = 0f;

        audioSource.Play();

        Destroy(soundObject, pickupSound.length);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (counterManager != null)
            {
                counterManager.ShowInteractText(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (counterManager != null)
            {
                counterManager.ShowInteractText(false);
            }
        }
    }
}
