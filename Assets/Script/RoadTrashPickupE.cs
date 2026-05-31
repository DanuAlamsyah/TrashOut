using UnityEngine;

public class RoadTrashPickupE : MonoBehaviour
{
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
        }

        Destroy(gameObject);
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