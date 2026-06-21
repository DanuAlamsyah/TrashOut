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