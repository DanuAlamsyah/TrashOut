using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Wajib ada untuk berpindah Scene

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Armor System (Reward Sortir)")]
    public int currentArmor = 0;
    public TextMeshProUGUI teksArmorLayar;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public string namaSceneMenu = "mainMenu"; // Ketik nama scene menu kamu di Inspector nanti

    [Header("State")]
    public bool isDead = false;

    private Animator animator;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private WeaponToggle weaponToggle;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        currentArmor = PlayerPrefs.GetInt("NilaiArmorPemain", 0);
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        weaponToggle = GetComponent<WeaponToggle>();

        UpdateTeksArmorLayar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (currentArmor > 0)
        {
            currentArmor -= damage;
            if (currentArmor < 0)
            {
                int sisaDamage = Mathf.Abs(currentArmor);
                currentArmor = 0;
                currentHealth -= sisaDamage;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        PlayerPrefs.SetInt("NilaiArmorPemain", currentArmor);
        PlayerPrefs.Save();

        UpdateTeksArmorLayar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateTeksArmorLayar()
    {
        if (teksArmorLayar != null)
        {
            if (currentArmor > 0)
            {
                teksArmorLayar.text = "Armor: " + currentArmor;
                teksArmorLayar.gameObject.SetActive(true);
            }
            else
            {
                teksArmorLayar.text = "Armor: 0";
                teksArmorLayar.gameObject.SetActive(false);
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;
        currentArmor = 0;

        PlayerPrefs.SetInt("NilaiArmorPemain", 0);
        PlayerPrefs.Save();

        UpdateTeksArmorLayar();

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Die");
        }

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerShoot != null) playerShoot.enabled = false;
        if (weaponToggle != null) weaponToggle.enabled = false;
        if (characterController != null) characterController.enabled = false;

        Invoke("ShowGameOverPopUp", 1.5f);
    }

    void ShowGameOverPopUp()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- FUNGSI UNTUK TOMBOL ---

    public void ButtonMainLagi()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonKeluar()
    {
        Debug.Log("Kembali ke Menu Utama...");
        // Memuat scene berdasarkan nama yang kamu tulis di Inspector
        SceneManager.LoadScene(namaSceneMenu);
    }
}
