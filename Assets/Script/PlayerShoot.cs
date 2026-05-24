using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 12f;
    public float fireRate = 0.4f;

    [Header("Aim")]
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 35f;
    public float aimSpeed = 10f;

    private Animator animator;
    private float nextFireTime = 0f;
    private bool isAiming = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera != null)
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

        if (playerCamera != null)
        {
            float targetFOV = isAiming ? aimFOV : normalFOV;

            playerCamera.fieldOfView = Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                aimSpeed * Time.deltaTime
            );
        }
    }

    void HandleShoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet Prefab atau Fire Point belum diisi di PlayerShoot.");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
    }

    public void ForceStopAim()
    {
        isAiming = false;

        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFOV;
        }
    }
}