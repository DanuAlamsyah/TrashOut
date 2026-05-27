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