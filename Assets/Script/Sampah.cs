using UnityEngine;

public class Sampah : MonoBehaviour
{
    public enum JenisSampah { Organik, Anorganik }
    public JenisSampah jenis; 

    [Header("Pergerakan")]
    public float kecepatanJalan = 3f;
    private Transform targetAkhir;
    private bool sedangDiDrag = false;

    private Vector3 mOffset;
    private float mZCoord;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Cari otomatis objek bernama TargetPoint di scene
        GameObject goTarget = GameObject.Find("TargetPoint");
        if (goTarget != null)
        {
            targetAkhir = goTarget.transform;
        }
    }

    void Update()
    {
        // Jika sedang tidak di-drag oleh pemain, sampah akan jalan terus ke kanan
        if (!sedangDiDrag && targetAkhir != null)
        {
            Vector3 arah = (targetAkhir.position - transform.position).normalized;
            // Kita gerakkan menggunakan transform agar stabil melayang/berjalan di jalurnya
            transform.Translate(arah * kecepatanJalan * Time.deltaTime, Space.World);

            // Jika lolos dan sampai di ujung target, hancurkan objeknya (Pemain melewatkan sampah)
            if (Vector3.Distance(transform.position, targetAkhir.position) < 0.5f)
            {
                Debug.Log("Sampah lolos! Tidak disortir.");
                Destroy(gameObject);
            }
        }
    }

    // --- MEKANIK DRAG AND DROP ---
    void OnMouseDown()
    {
        sedangDiDrag = true;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        
        if (rb != null) rb.isKinematic = true; 
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }

    void OnMouseUp()
    {
        sedangDiDrag = false;
        if (rb != null) rb.isKinematic = false;
    }
}