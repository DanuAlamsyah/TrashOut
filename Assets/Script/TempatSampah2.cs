using UnityEngine;

public class TempatSampah2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. CEK KHUSUS JIKA YANG MASUK ADALAH BATU
        if (other.CompareTag("Batu"))
        {
            Debug.Log("Waduh! Malah masukin Batu. Waktu berkurang!");
            
            // Sudah benar memanggil GameManagerSortir2
            GameManagerSortir2.Instance.KurangiWaktuBatu();

            Destroy(other.gameObject);
            return; 
        }

        // 2. CEK JIKA YANG MASUK ADALAH SAMPAH BIASA
        Sampah sampah = other.GetComponent<Sampah>();

        if (sampah != null)
        {
            // Mencocokkan jenis sampah dengan TAG tempat sampah ini
            if ((sampah.jenis == Sampah.JenisSampah.Organik && gameObject.CompareTag("Organik")) ||
                (sampah.jenis == Sampah.JenisSampah.Anorganik && gameObject.CompareTag("Anorganik")))
            {
                Debug.Log("Sip! Sortir Benar.");
                // AMAN: Sudah diganti ke GameManagerSortir2
                GameManagerSortir2.Instance.TambahSkor(10); 
            }
            else
            {
                Debug.Log("Waduh! Sortir Salah.");
                // AMAN: Sudah diganti ke GameManagerSortir2
                GameManagerSortir2.Instance.TambahSkor(-5); 
            }

            Destroy(other.gameObject);
        }
    }
}