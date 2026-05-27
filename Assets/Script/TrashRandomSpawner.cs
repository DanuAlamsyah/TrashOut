using UnityEngine;
using System.Collections.Generic;

public class TrashRandomSpawner : MonoBehaviour
{
    [Header("Prefab Sampah")]
    public GameObject[] trashPrefabs;

    [Header("Jumlah Sampah Per Area Spawn")]
    public int trashPerSpawnPoint = 5;

    [Header("Jarak Minimal Antar Sampah")]
    public float minDistance = 1f;

    [Header("Random Posisi di Sekitar Spawn Point")]
    public float randomRadius = 4f;

    [Header("Raycast")]
    public float raycastHeight = 30f;
    public LayerMask groundLayer;

    [Header("Offset dari Permukaan Jalan")]
    public float yOffset = 0.02f;

    [Header("Random Rotation")]
    public bool randomRotation = true;

    private List<Vector3> usedPositions = new List<Vector3>();
    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        GetSpawnPointsFromChildren();
        SpawnTrashPerArea();
    }

    void GetSpawnPointsFromChildren()
    {
        spawnPoints.Clear();

        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        Debug.Log("Jumlah spawn point ditemukan: " + spawnPoints.Count);
    }

    void SpawnTrashPerArea()
    {
        if (trashPrefabs.Length == 0)
        {
            Debug.LogWarning("Prefab sampah belum dimasukkan!");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("Tidak ada SpawnPoint di dalam TrashSpawner!");
            return;
        }

        int totalSpawned = 0;

        foreach (Transform spawnPoint in spawnPoints)
        {
            int spawnedInThisArea = 0;
            int attempts = 0;
            int maxAttempts = trashPerSpawnPoint * 100;

            while (spawnedInThisArea < trashPerSpawnPoint && attempts < maxAttempts)
            {
                attempts++;

                Vector2 randomCircle = Random.insideUnitCircle * randomRadius;

                Vector3 rayStart = new Vector3(
                    spawnPoint.position.x + randomCircle.x,
                    spawnPoint.position.y + raycastHeight,
                    spawnPoint.position.z + randomCircle.y
                );

                if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundLayer))
                {
                    continue;
                }

                if (!IsPositionValid(hit.point))
                {
                    continue;
                }

                GameObject selectedTrash = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

                Quaternion rotation = randomRotation
                    ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)
                    : Quaternion.identity;

                GameObject spawnedTrash = Instantiate(selectedTrash, hit.point, rotation);

                SnapBottomToGround(spawnedTrash, hit.point.y);

                usedPositions.Add(hit.point);
                spawnedInThisArea++;
                totalSpawned++;
            }

            Debug.Log(spawnPoint.name + " berhasil spawn: " + spawnedInThisArea);
        }

        Debug.Log("Total sampah berhasil dispawn: " + totalSpawned);
    }

    void SnapBottomToGround(GameObject trash, float groundY)
    {
        Renderer renderer = trash.GetComponentInChildren<Renderer>();

        if (renderer == null)
        {
            trash.transform.position = new Vector3(
                trash.transform.position.x,
                groundY + yOffset,
                trash.transform.position.z
            );

            return;
        }

        float bottomY = renderer.bounds.min.y;
        float difference = groundY - bottomY;

        trash.transform.position += new Vector3(0f, difference + yOffset, 0f);
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 usedPosition in usedPositions)
        {
            if (Vector3.Distance(position, usedPosition) < minDistance)
            {
                return false;
            }
        }

        return true;
    }
}