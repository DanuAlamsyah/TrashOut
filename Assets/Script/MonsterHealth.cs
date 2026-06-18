using System.Collections;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;

    [Header("Damage Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.12f;

    [Header("Death")]
    public float destroyDelay = 1.5f;

    // Untuk Slime sebelumnya bisa pakai 0.3.
    // Untuk Imp/Boss biasanya mulai dari 0 dulu.
    public float deathYOffset = 0f;

    [Header("Animator Parameters")]
    public string movingParameter = "isMoving";
    public string takeDamageTrigger = "TakeDamage";
    public string dieTrigger = "Die";

    private int currentHealth;
    private bool isDead = false;

    private Renderer[] renderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    private Animator animator;
    private MonsterAI monsterAI;
    private CharacterController characterController;
    private Collider monsterCollider;
    private Rigidbody rigidbodyComponent;

    private Vector3 lockedDeathPosition;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();
        monsterAI = GetComponent<MonsterAI>();
        characterController = GetComponent<CharacterController>();
        monsterCollider = GetComponent<Collider>();
        rigidbodyComponent = GetComponent<Rigidbody>();

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;

            if (mat.HasProperty("_BaseColor"))
            {
                originalColors[i] = mat.GetColor("_BaseColor");
            }
            else if (mat.HasProperty("_Color"))
            {
                originalColors[i] = mat.color;
            }
            else
            {
                originalColors[i] = Color.white;
            }
        }
    }

    void LateUpdate()
    {
        if (isDead)
        {
            transform.position = lockedDeathPosition;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log(gameObject.name + " terkena damage. HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Animasi kena damage untuk monster yang punya animasi Take Damage
        if (animator != null && !string.IsNullOrEmpty(takeDamageTrigger))
        {
            animator.SetTrigger(takeDamageTrigger);
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashHitColor());
    }

    IEnumerator FlashHitColor()
    {
        SetColor(hitColor);

        yield return new WaitForSeconds(flashDuration);

        if (!isDead)
        {
            RestoreOriginalColor();
        }
    }

    void SetColor(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;

            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }
            else if (mat.HasProperty("_Color"))
            {
                mat.color = color;
            }
        }
    }

    void RestoreOriginalColor()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;

            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", originalColors[i]);
            }
            else if (mat.HasProperty("_Color"))
            {
                mat.color = originalColors[i];
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log(gameObject.name + " mati.");

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        RestoreOriginalColor();

        lockedDeathPosition = transform.position + new Vector3(0f, deathYOffset, 0f);
        transform.position = lockedDeathPosition;

        if (animator != null)
        {
            animator.applyRootMotion = false;

            if (!string.IsNullOrEmpty(movingParameter))
            {
                animator.SetBool(movingParameter, false);
            }

            if (!string.IsNullOrEmpty(dieTrigger))
            {
                animator.SetTrigger(dieTrigger);
            }
        }

        if (monsterAI != null)
        {
            monsterAI.enabled = false;
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (monsterCollider != null)
        {
            monsterCollider.enabled = false;
        }

        if (rigidbodyComponent != null)
        {
            rigidbodyComponent.useGravity = false;
            rigidbodyComponent.isKinematic = true;
        }

        Destroy(gameObject, destroyDelay);
    }
}