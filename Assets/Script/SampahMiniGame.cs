using UnityEngine;

public class SampahMinigame : MonoBehaviour
{
    [Header("Pergerakan Otomatis")]
    public float kecepatanJalan = 3f;
    private Transform targetAkhir;
    private bool sedangDiDrag = false;

    private Rigidbody rb;
    private Camera kameraUtama;
    private float jarakKeKamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        kameraUtama = Camera.main;
        
        // Cari otomatis objek penampung di ujung jalur
        GameObject goTarget = GameObject.Find("TargetPoint");
        if (goTarget != null)
        {
            targetAkhir = goTarget.transform;
        }
    }

    void Update()
    {
        // Jika sedang tidak di-drag, sampah jalan otomatis ke arah TargetPoint
        if (!sedangDiDrag && targetAkhir != null)
        {
            Vector3 arah = (targetAkhir.position - transform.position).normalized;
            transform.Translate(arah * kecepatanJalan * Time.deltaTime, Space.World);

            // Jika lolos sampai ujung, hancurkan objeknya
            if (Vector3.Distance(transform.position, targetAkhir.position) < 0.5f)
            {
                // 💥 TAMBAHAN KHUSUS LEVEL 4: Cek jika yang lolos ke ujung adalah BOM!
                if (gameObject.CompareTag("Bom"))
                {
                    if (GameManagerSortir4.Instance != null)
                    {
                        GameManagerSortir4.Instance.LedakanBomGameOver();
                    }
                }

                Destroy(gameObject);
            }
        }
    }

    // --- MEKANIK DRAG AND DROP 3D ---
    void OnMouseDown()
    {
        sedangDiDrag = true;
        jarakKeKamera = kameraUtama.WorldToScreenPoint(gameObject.transform.position).z;
        
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // 🌟 KUNCI BALIKIN WARNA 3D SAAT DIKLIK:
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
        if (mr != null)
        {
            mr.material.color = Color.white; // Kembalikan ke warna asli materialnya!
        }
    }

    void OnMouseDrag()
    {
        Vector3 posisiMouseLayar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, jarakKeKamera);
        Vector3 posisiDunia3D = kameraUtama.ScreenToWorldPoint(posisiMouseLayar);
        transform.position = posisiDunia3D;
    }

    void OnMouseUp()
    {
        sedangDiDrag = false;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}