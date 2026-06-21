using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 12f;
    public float fireRate = 0.4f;


    [Header("Damage")]
    public int bulletDamage = 1;



    [Header("Weapon Upgrade")]
    public WeaponUpgrade weaponUpgrade;



    [Header("Aim")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 35f;
    public float aimSpeed = 10f;



    [Header("Aim Shoot Direction")]
    public float aimRayDistance = 100f;
    public LayerMask aimLayerMask = ~0;



    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shotSound;
    [Range(0f, 3f)]
    public float shotVolume = 2f;



    private Animator animator;
    private float nextFireTime = 0f;
    private bool isAiming = false;



    void Start()
    {
        animator = GetComponent<Animator>();


        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }


        if(audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = 1f;
        }


        if(playerCamera == null)
        {
            playerCamera = Camera.main;
        }


        if(playerCamera != null)
        {
            normalFOV = playerCamera.fieldOfView;
        }
    }





    void Update()
    {
        HandleAim();
        HandleShoot();
    }






    void HandleAim()
    {
        isAiming = Input.GetMouseButton(1);


        if(playerCamera != null)
        {
            float targetFOV =
            isAiming ? aimFOV : normalFOV;


            playerCamera.fieldOfView =
            Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                aimSpeed * Time.deltaTime
            );
        }
    }







    void HandleShoot()
    {
        if(Input.GetMouseButtonDown(0)
        && Time.time >= nextFireTime)
        {

            Shoot();



            float currentFireRate = fireRate;



            // Upgrade fire rate
            if(weaponUpgrade != null &&
               weaponUpgrade.fireRateUpgrade)
            {
                currentFireRate *=
                weaponUpgrade.fireRateMultiplier;
            }



            nextFireTime =
            Time.time + currentFireRate;
        }
    }









    void Shoot()
    {
        if(bulletPrefab == null ||
           firePoint == null)
        {
            Debug.LogWarning(
            "Bullet Prefab atau Fire Point belum diisi."
            );

            return;
        }



        bool isRunning = false;



        if(animator != null)
        {
            isRunning =
            animator.GetBool("isRunning");


            if(!isRunning)
            {
                animator.SetTrigger("Shoot");
            }
        }






        Vector3 shootDirection;



        if(isAiming)
        {
            shootDirection =
            GetAimDirection();
        }
        else
        {
            shootDirection =
            firePoint.forward;
        }





        SpawnBullet(
            shootDirection,
            firePoint.position
        );

    }









    void SpawnBullet(
        Vector3 direction,
        Vector3 position
    )
    {

        Quaternion bulletRotation =
        Quaternion.LookRotation(direction);



        GameObject bullet =
        Instantiate(
            bulletPrefab,
            position,
            bulletRotation
        );




        // Set damage peluru
        BulletDamage damageScript =
        bullet.GetComponent<BulletDamage>();


        if(damageScript != null)
        {
            int damage = bulletDamage;



            if(weaponUpgrade != null &&
               weaponUpgrade.damageUpgrade)
            {
                damage *=
                weaponUpgrade.damageMultiplier;
            }



            damageScript.damage = damage;
        }






        Rigidbody rb =
        bullet.GetComponent<Rigidbody>();


        if(rb != null)
        {
            rb.velocity =
            direction * bulletSpeed;
        }






        if(audioSource != null &&
           shotSound != null)
        {
            audioSource.PlayOneShot(
                shotSound,
                shotVolume
            );
        }

    }









    Vector3 GetAimDirection()
    {

        if(playerCamera == null)
        {
            return firePoint.forward;
        }



        Ray ray =
        playerCamera.ViewportPointToRay(
        new Vector3(0.5f,0.5f,0f)
        );



        Vector3 targetPoint;



        if(Physics.Raycast(
            ray,
            out RaycastHit hit,
            aimRayDistance,
            aimLayerMask,
            QueryTriggerInteraction.Ignore))
        {
            targetPoint =
            hit.point;
        }
        else
        {
            targetPoint =
            ray.origin +
            ray.direction *
            aimRayDistance;
        }




        Vector3 direction =
        targetPoint -
        firePoint.position;



        direction.Normalize();



        return direction;

    }








    public void ForceStopAim()
    {
        isAiming = false;


        if(playerCamera != null)
        {
            playerCamera.fieldOfView =
            normalFOV;
        }
    }

}