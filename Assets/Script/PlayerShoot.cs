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

    [Header("Aim Shoot Direction")]
    public float aimRayDistance = 100f;
    public LayerMask aimLayerMask = ~0;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shotSound;
    [Range(0f, 3f)] public float shotVolume = 2f;

    private Animator animator;
    private float nextFireTime = 0f;
    private bool isAiming = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f; // 0 = suara 2D
            audioSource.volume = 1f;
        }

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

        bool isRunning = false;

        if (animator != null)
        {
            isRunning = animator.GetBool("isRunning");

            // Animasi shoot hanya diputar saat diam
            if (!isRunning)
            {
                animator.SetTrigger("Shoot");
            }
        }

        Vector3 shootDirection;

        if (isAiming)
        {
            shootDirection = GetAimDirection();
        }
        else
        {
            shootDirection = firePoint.forward;
        }

        Quaternion bulletRotation = Quaternion.LookRotation(shootDirection);

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            bulletRotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }

        if (audioSource != null && shotSound != null)
        {
            audioSource.PlayOneShot(shotSound, shotVolume);
        }
    }

    Vector3 GetAimDirection()
    {
        if (playerCamera == null)
        {
            return firePoint.forward;
        }

        // Ray dari tengah layar/crosshair kamera
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, aimRayDistance, aimLayerMask, QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * aimRayDistance;
        }

        Vector3 direction = targetPoint - firePoint.position;
        direction.Normalize();

        return direction;
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