using UnityEngine;

public class TrashCollectible : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            if (TrashManager.Instance != null)
            {
                TrashManager.Instance.AddTrash();
            }

            Debug.Log("Sampah berhasil diambil");

            Destroy(gameObject);
        }
    }
}