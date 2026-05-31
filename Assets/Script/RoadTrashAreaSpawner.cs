using System.Collections.Generic;
using UnityEngine;

public class RoadTrashAreaSpawner : MonoBehaviour
{
    [Header("Trash Prefabs")]
    public GameObject[] trashPrefabs;

    [Header("Road Areas")]
    public BoxCollider[] roadAreas;

    [Header("Spawn Settings")]
    public int trashCount = 15;
    public float spawnYOffset = 0.1f;
    public float minDistanceBetweenTrash = 1.5f;
    public int maxSpawnAttempts = 100;

    [Header("Ground Check")]
    public bool useRaycastToGround = true;
    public LayerMask groundLayer = ~0;

    [Header("Parent")]
    public Transform spawnedTrashParent;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        SpawnAllTrash();

        RoadTrashCounterManager counterManager = FindObjectOfType<RoadTrashCounterManager>();

        if (counterManager != null)
        {
            counterManager.SetTotalTrash(trashCount);
        }
    }

    void SpawnAllTrash()
    {
        if (trashPrefabs == null || trashPrefabs.Length == 0)
        {
            Debug.LogWarning("Trash Prefabs belum diisi di RoadTrashAreaSpawner.");
            return;
        }

        if (roadAreas == null || roadAreas.Length == 0)
        {
            Debug.LogWarning("Road Areas belum diisi di RoadTrashAreaSpawner.");
            return;
        }

        for (int i = 0; i < trashCount; i++)
        {
            SpawnOneTrash();
        }
    }

    void SpawnOneTrash()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            BoxCollider selectedArea = roadAreas[Random.Range(0, roadAreas.Length)];

            if (selectedArea == null) continue;

            Vector3 spawnPosition = GetRandomPointInArea(selectedArea);

            if (IsTooCloseToOtherTrash(spawnPosition))
            {
                continue;
            }

            GameObject selectedTrashPrefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

            Quaternion randomRotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject trash = Instantiate(
                selectedTrashPrefab,
                spawnPosition,
                randomRotation,
                spawnedTrashParent
            );

            PrepareTrashObject(trash);

            spawnedPositions.Add(spawnPosition);

            return;
        }

        Debug.LogWarning("Gagal spawn sampah. Coba perbesar RoadArea atau kurangi Trash Count.");
    }

    Vector3 GetRandomPointInArea(BoxCollider area)
    {
        Bounds bounds = area.bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 5f,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        if (useRaycastToGround)
        {
            Ray ray = new Ray(randomPoint, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                return hit.point + Vector3.up * spawnYOffset;
            }
        }

        return new Vector3(
            randomPoint.x,
            area.transform.position.y + spawnYOffset,
            randomPoint.z
        );
    }

    bool IsTooCloseToOtherTrash(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenTrash)
            {
                return true;
            }
        }

        return false;
    }

    void PrepareTrashObject(GameObject trash)
    {
        if (trash == null) return;

        Collider trashCollider = trash.GetComponent<Collider>();

        if (trashCollider != null)
        {
            trashCollider.isTrigger = true;
        }

        Rigidbody rb = trash.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Mematikan script lama TrashCollectible kalau masih ada,
        // supaya tidak auto-pickup dari sistem lama.
        MonoBehaviour[] scripts = trash.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != null && script.GetType().Name == "TrashCollectible")
            {
                script.enabled = false;
            }
        }

        RoadTrashPickupE pickupScript = trash.GetComponent<RoadTrashPickupE>();

        if (pickupScript == null)
        {
            trash.AddComponent<RoadTrashPickupE>();
        }
    }
}