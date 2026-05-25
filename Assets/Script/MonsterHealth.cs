using System.Collections;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;

    [Header("Damage Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.12f;

    private int currentHealth;

    private Renderer[] renderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        // Mengambil renderer dari object ini dan semua child-nya
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
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashHitColor());

        Debug.Log(gameObject.name + " terkena damage. HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashHitColor()
    {
        SetColor(hitColor);

        yield return new WaitForSeconds(flashDuration);

        RestoreOriginalColor();
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
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        MonsterAI ai = GetComponent<MonsterAI>();
        if (ai != null)
        {
            ai.enabled = false;
        }

        Destroy(gameObject, 1.5f);
    }
}