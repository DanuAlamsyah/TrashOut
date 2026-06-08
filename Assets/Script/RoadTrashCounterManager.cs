using TMPro;
using UnityEngine;

public class RoadTrashCounterManager : MonoBehaviour
{
    [Header("Counter")]
    public int totalTrash;
    public int collectedTrash;

    [Header("UI")]
    public TMP_Text trashCounterText;
    public GameObject interactText;

    // --- TAMBAHKAN VARIABEL INI ---
    [Header("Level Transition")]
    [Tooltip("Masukkan nama scene pilah sampah khusus untuk level ini")]
    public string nextSceneName; 

    void Start()
    {
        UpdateCounterUI();

        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    public void SetTotalTrash(int total)
    {
        totalTrash = total;
        collectedTrash = 0;
        UpdateCounterUI();
    }

    public void AddTotalTrash(int amount)
    {
        totalTrash += amount;
        UpdateCounterUI();
    }

    public void AddTrash()
    {
        collectedTrash++;
        UpdateCounterUI();

        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        if (collectedTrash >= totalTrash)
        {
            Debug.Log("Semua sampah sudah dikumpulkan! Pindah ke: " + nextSceneName);
            
            // --- UBAH BAGIAN INI ---
            if (GameManager.Instance != null)
            {
                // Kirim nama scene yang sudah diatur di Inspector ke GameManager
                GameManager.Instance.GoToPilahSampah(nextSceneName);
            }
            else
            {
                Debug.LogWarning("GameManager tidak ditemukan di scene!");
            }
        }
    }

    public void ShowInteractText(bool show)
    {
        if (interactText != null)
        {
            interactText.SetActive(show);
        }
    }

    void UpdateCounterUI()
    {
        if (trashCounterText != null)
        {
            trashCounterText.text = "Sampah: " + collectedTrash + " / " + totalTrash;
        }
    }
}