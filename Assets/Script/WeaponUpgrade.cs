using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    [Header("Upgrade Status")]
    public bool fireRateUpgrade = false;
    public bool damageUpgrade = false;

    [Header("Fire Rate")]
    public float fireRateMultiplier = 0.5f;

    [Header("Damage")]
    public int damageMultiplier = 2;

    void Start()
    {
        // Mengecek apakah pemain sudah membeli upgrade di toko sebelumnya
        if (PlayerPrefs.GetInt("Weapon_Upgraded", 0) == 1)
        {
            // Jika sudah dibeli, otomatis aktifkan kedua efek upgrade ini
            UpgradeFireRate();
            UpgradeDamage();
        }
    }

    public void UpgradeFireRate()
    {
        fireRateUpgrade = true;
        Debug.Log("Fire Rate aktif");
    }

    public void UpgradeDamage()
    {
        damageUpgrade = true;
        Debug.Log("Damage aktif");
    }
}