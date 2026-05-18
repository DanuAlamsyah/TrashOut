using UnityEngine;

public class GameManagerSortir : MonoBehaviour
{
    public static GameManagerSortir Instance;

    public int skor = 0;
    public Transform spawnPoint;       // Seret objek SpawnPoint ke sini
    public GameObject[] prefabSampah;  // Masukkan semua prefab sampahmu di sini
    
    public float jedaSpawn = 2.5f;
    private float hitungMundur;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        hitungMundur = jedaSpawn;
    }

    void Update()
    {
        hitungMundur -= Time.deltaTime;
        if (hitungMundur <= 0f)
        {
            SpawnSampah();
            hitungMundur = jedaSpawn;
        }
    }

    void SpawnSampah()
    {
        if (prefabSampah.Length == 0 || spawnPoint == null) return;

        // Pilih model sampah acak dari array
        int randomSampah = Random.Range(0, prefabSampah.Length);

        // Munculkan di koordinat SpawnPoint
        Instantiate(prefabSampah[randomSampah], spawnPoint.position, Quaternion.identity);
    }

    public void TambahSkor(int nilai)
    {
        skor += nilai;
        Debug.Log("Skor Sekarang: " + skor);
    }
}