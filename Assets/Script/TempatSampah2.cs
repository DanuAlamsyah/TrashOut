using UnityEngine;

public class TempatSampah2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. CEK KHUSUS JIKA YANG MASUK ADALAH BATU
        if (other.CompareTag("Batu"))
        {
            Debug.Log("Waduh! Malah masukin Batu.");
            
            // Otomatis deteksi mau di Level 2 atau Level 3
            if (GameManagerSortir2.Instance != null) GameManagerSortir2.Instance.KurangiWaktuBatu();
            else if (GameManagerSortir3.Instance != null) GameManagerSortir3.Instance.KurangiWaktuBatu();

            Destroy(other.gameObject);
            return; 
        }

        // 2. CEK JIKA YANG MASUK ADALAH SAMPAH BIASA
        Sampah sampah = other.GetComponent<Sampah>();

        if (sampah != null)
        {
            // Mencocokkan jenis sampah dengan TAG tempat sampah ini (Organik / Anorganik)
            if ((sampah.jenis == Sampah.JenisSampah.Organik && gameObject.CompareTag("Organik")) ||
                (sampah.jenis == Sampah.JenisSampah.Anorganik && gameObject.CompareTag("Anorganik")))
            {
                Debug.Log("Sip! Sortir Benar.");
                
                // SUNTIKAN PINTAR: Kirim skor ke GameManager level yang sedang aktif
                if (GameManagerSortir2.Instance != null) GameManagerSortir2.Instance.TambahSkor(10);
                else if (GameManagerSortir3.Instance != null) GameManagerSortir3.Instance.TambahSkor(10);
            }
            else
            {
                Debug.Log("Waduh! Sortir Salah.");
                
                if (GameManagerSortir2.Instance != null) GameManagerSortir2.Instance.TambahSkor(-5);
                else if (GameManagerSortir3.Instance != null) GameManagerSortir3.Instance.TambahSkor(-5);
            }

            // Hancurkan sampah setelah masuk tempat sampah
            Destroy(other.gameObject);
        }
    }
}