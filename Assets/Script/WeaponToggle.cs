using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    [Header("Weapon Object")]
    public GameObject weaponObject;

    [Header("Shooting Script")]
    public PlayerShoot playerShoot;

    public bool hasWeapon = false;

    void Start()
    {
        SetWeaponActive(hasWeapon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            hasWeapon = !hasWeapon;
            SetWeaponActive(hasWeapon);
        }
    }

    void SetWeaponActive(bool active)
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(active);
        }

        if (playerShoot != null)
        {
            playerShoot.enabled = active;
        }
    }
}