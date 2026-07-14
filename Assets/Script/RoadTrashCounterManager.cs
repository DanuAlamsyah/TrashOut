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
            CheckLevelComplete();
        }
    }

    public void ShowInteractText(bool show)
    {
        if (interactText != null)
        {
            interactText.SetActive(show);
        }
    }

    public void CheckLevelComplete()
    {
        bool allMonsterDead =
            MonsterCounter.Instance == null ||
            MonsterCounter.Instance.IsAllMonsterDead();

        if (collectedTrash >= totalTrash &&
            allMonsterDead)
        {
            Debug.Log("Semua syarat level terpenuhi!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GoToPilahSampah(nextSceneName);
            }
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