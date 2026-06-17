using UnityEngine;
using TMPro; // Wajib ada untuk mengontrol teks armor di layar

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Armor System (Reward Sortir)")]
    public int currentArmor = 0; 
    public TextMeshProUGUI teksArmorLayar; // Tarik objek UI Text ke sini di Inspector

    [Header("State")]
    public bool isDead = false;

    private Animator animator;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private WeaponToggle weaponToggle;

    void Start()
    {
        // 🛡️ Ambil data armor hasil belanja sortir. Jika gak beli, otomatis 0.
        currentArmor = PlayerPrefs.GetInt("NilaiArmorPemain", 0);

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        weaponToggle = GetComponent<WeaponToggle>();

        // Tampilkan teks armor di awal level
        UpdateTeksArmorLayar();

        Debug.Log("PlayerHealth aktif. HP Player: " + currentHealth + " | Armor: " + currentArmor);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // 🛡️ Logika Filter Tameng Armor
        if (currentArmor > 0)
        {
            currentArmor -= damage;
            Debug.Log("Armor menahan serangan! Sisa Armor: " + currentArmor);

            if (currentArmor < 0)
            {
                int sisaDamage = Mathf.Abs(currentArmor);
                currentArmor = 0;
                currentHealth -= sisaDamage;
                Debug.Log("Armor hancur! Sisa damage memotong HP. Sisa HP: " + currentHealth);
            }
        }
        else
        {
            currentHealth -= damage;
            Debug.Log("Player terkena damage langsung. Sisa HP: " + currentHealth);
        }

        // 💾 Simpan perubahan sisa armor secara jujur ke memori agar terbawa ke level berikutnya
        PlayerPrefs.SetInt("NilaiArmorPemain", currentArmor);
        PlayerPrefs.Save();

        // Perbarui visual teks di layar HP
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
                teksArmorLayar.gameObject.SetActive(true); // Muncul jika armor ada
            }
            else
            {
                teksArmorLayar.text = "Armor: 0";
                teksArmorLayar.gameObject.SetActive(false); // Sembunyi jika armor habis
            }
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;
        currentArmor = 0;
        
        // Reset data armor di memori karena player sudah mati/gagal
        PlayerPrefs.SetInt("NilaiArmorPemain", 0);
        PlayerPrefs.Save();
        
        UpdateTeksArmorLayar();

        Debug.Log("Player mati.");

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Die");
        }

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerShoot != null) playerShoot.enabled = false;
        if (weaponToggle != null) weaponToggle.enabled = false;
        if (characterController != null) characterController.enabled = false;
    }
}