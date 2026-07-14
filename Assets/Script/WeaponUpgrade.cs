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
        Debug.Log("Weapon_Upgraded = " +
            PlayerPrefs.GetInt("Weapon_Upgraded", 0));

        Debug.Log("Damage Upgrade Awal = " + damageUpgrade);
        Debug.Log("FireRate Upgrade Awal = " + fireRateUpgrade);

        if (PlayerPrefs.GetInt("Weapon_Upgraded", 0) == 1)
        {
            UpgradeFireRate();
            UpgradeDamage();
        }

        Debug.Log("Damage Upgrade Akhir = " + damageUpgrade);
        Debug.Log("FireRate Upgrade Akhir = " + fireRateUpgrade);
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