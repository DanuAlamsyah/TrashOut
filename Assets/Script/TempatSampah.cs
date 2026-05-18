using UnityEngine;

public class TempatSampah : MonoBehaviour
{
    // Cek apakah objek ini bertag Organik atau Anorganik
    private void OnTriggerEnter(Collider other)
    {
        Sampah sampah = other.GetComponent<Sampah>();

        if (sampah != null)
        {
            // Mencocokkan jenis sampah dengan TAG tempat sampah ini
            if ((sampah.jenis == Sampah.JenisSampah.Organik && gameObject.CompareTag("Organik")) ||
                (sampah.jenis == Sampah.JenisSampah.Anorganik && gameObject.CompareTag("Anorganik")))
            {
                Debug.Log("Sip! Sortir Benar.");
                GameManagerSortir.Instance.TambahSkor(10);
            }
            else
            {
                Debug.Log("Waduh! Sortir Salah.");
                GameManagerSortir.Instance.TambahSkor(-5);
            }

            // Hancurkan sampah setelah masuk tempat sampah
            Destroy(other.gameObject);
        }
    }
}