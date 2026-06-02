using UnityEngine;

public class MonsterTrashVomit : MonoBehaviour
{
    [Header("Trash Prefabs")]
    public GameObject[] trashPrefabs;

    [Header("Spawn Settings")]
    public int trashAmountPerVomit = 2;
    public float spawnRadius = 1.5f;
    public float spawnYOffset = 0.1f;

    [Header("Random Time")]
    public float minVomitTime = 5f;
    public float maxVomitTime = 12f;

    [Header("Limit")]
    public int maxVomitCount = 3;

    [Header("Parent")]
    public Transform spawnedTrashParent;

    [Header("Ground Check")]
    public bool useRaycastToGround = true;
    public LayerMask groundLayer = ~0;

    private int currentVomitCount = 0;
    private float nextVomitTime;
    private RoadTrashCounterManager counterManager;
    private MonsterHealth monsterHealth;

    void Start()
    {
        counterManager = FindObjectOfType<RoadTrashCounterManager>();
        monsterHealth = GetComponent<MonsterHealth>();

        SetNextVomitTime();
    }

    void Update()
    {
        if (currentVomitCount >= maxVomitCount) return;

        if (Time.time >= nextVomitTime)
        {
            VomitTrash();
            currentVomitCount++;
            SetNextVomitTime();
        }
    }

    void SetNextVomitTime()
    {
        nextVomitTime = Time.time + Random.Range(minVomitTime, maxVomitTime);
    }

    void VomitTrash()
    {
        if (trashPrefabs == null || trashPrefabs.Length == 0)
        {
            Debug.LogWarning("Trash Prefabs belum diisi di MonsterTrashVomit.");
            return;
        }

        int spawnedCount = 0;

        for (int i = 0; i < trashAmountPerVomit; i++)
        {
            GameObject selectedTrash = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

            Vector3 spawnPosition = GetRandomSpawnPosition();

            Quaternion randomRotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject trash = Instantiate(
                selectedTrash,
                spawnPosition,
                randomRotation,
                spawnedTrashParent
            );

            PrepareTrashObject(trash);

            spawnedCount++;
        }

        if (counterManager != null)
        {
            counterManager.AddTotalTrash(spawnedCount);
        }

        Debug.Log(gameObject.name + " memuntahkan " + spawnedCount + " sampah.");
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

        Vector3 rawPosition = transform.position + new Vector3(
            randomCircle.x,
            2f,
            randomCircle.y
        );

        if (useRaycastToGround)
        {
            Ray ray = new Ray(rawPosition, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                return hit.point + Vector3.up * spawnYOffset;
            }
        }

        return transform.position + new Vector3(
            randomCircle.x,
            spawnYOffset,
            randomCircle.y
        );
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