using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("State")]
    public bool isDead = false;

    private Animator animator;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private WeaponToggle weaponToggle;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        weaponToggle = GetComponent<WeaponToggle>();

        Debug.Log("PlayerHealth aktif. HP Player: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Player terkena damage: " + damage + ". Sisa HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("Player mati.");

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Die");
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerShoot != null)
        {
            playerShoot.enabled = false;
        }

        if (weaponToggle != null)
        {
            weaponToggle.enabled = false;
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }
    }
}