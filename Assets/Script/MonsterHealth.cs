using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;


    [Header("Health Bar")]
    public GameObject healthBar;
    public Image healthFill;
    public float hideHealthDelay = 3f;

    private Coroutine hideHealthCoroutine;



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
    private MonsterAudio monsterAudio;
    private CharacterController characterController;
    private Collider monsterCollider;
    private Rigidbody rigidbodyComponent;


    private Vector3 lockedDeathPosition;



    void Start()
    {
        currentHealth = maxHealth;


        // Sembunyikan health bar saat awal
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }


        animator = GetComponentInChildren<Animator>();
        monsterAI = GetComponent<MonsterAI>();
        monsterAudio = GetComponent<MonsterAudio>();
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

        monsterAudio?.PlayHit();



        currentHealth -= damage;


        Debug.Log(
            gameObject.name +
            " terkena damage. HP: " +
            currentHealth
        );



        // Munculkan health bar
        ShowHealthBar();



        // Update darah
        UpdateHealthBar();




        if (currentHealth <= 0)
        {
            Die();
            return;
        }



        // Animasi kena damage
        if (animator != null && 
            !string.IsNullOrEmpty(takeDamageTrigger))
        {
            animator.SetTrigger(takeDamageTrigger);
        }




        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }


        flashCoroutine =
            StartCoroutine(FlashHitColor());
    }







    void ShowHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(true);
        }


        if (hideHealthCoroutine != null)
        {
            StopCoroutine(hideHealthCoroutine);
        }


        hideHealthCoroutine =
            StartCoroutine(HideHealthBar());
    }






    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(hideHealthDelay);


        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }
    }







    void UpdateHealthBar()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount =
                (float)currentHealth / maxHealth;
        }
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
                mat.SetColor(
                    "_BaseColor",
                    originalColors[i]
                );
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
        monsterAudio?.PlayDeath();
        currentHealth = 0;


        Debug.Log(gameObject.name + " mati.");



        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }


        RestoreOriginalColor();



        lockedDeathPosition =
            transform.position +
            new Vector3(0f, deathYOffset, 0f);



        transform.position = lockedDeathPosition;





        if (animator != null)
        {
            animator.applyRootMotion = false;



            if (!string.IsNullOrEmpty(movingParameter))
            {
                animator.SetBool(
                    movingParameter,
                    false
                );
            }



            if (!string.IsNullOrEmpty(dieTrigger))
            {
                animator.SetTrigger(
                    dieTrigger
                );
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






    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}