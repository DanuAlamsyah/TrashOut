using UnityEngine;

public class TempatSampah4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 1. JIKA MASUKIN BOM KE TONG SAMPAH = LANGSUNG BUM! GAME OVER
        if (other.CompareTag("Bom"))
        {
            GameManagerSortir4.Instance.LedakanBomGameOver();
            Destroy(other.gameObject);
            return;
        }

        // 2. JIKA MASUKIN BATU
        if (other.CompareTag("Batu"))
        {
            GameManagerSortir4.Instance.KurangiWaktuBatu();
            Destroy(other.gameObject);
            return; 
        }

        // 3. JIKA MASUKIN SAMPAH BIASA
        Sampah sampah = other.GetComponent<Sampah>();
        if (sampah != null)
        {
            if ((sampah.jenis == Sampah.JenisSampah.Organik && gameObject.CompareTag("Organik")) ||
                (sampah.jenis == Sampah.JenisSampah.Anorganik && gameObject.CompareTag("Anorganik")))
            {
                Debug.Log("Sip! Sortir Benar.");
                GameManagerSortir4.Instance.TambahSkor(10);
            }
            else
            {
                Debug.Log("Waduh! Sortir Salah.");
                GameManagerSortir4.Instance.TambahSkor(-5);
            }

            Destroy(other.gameObject);
        }
    }
}