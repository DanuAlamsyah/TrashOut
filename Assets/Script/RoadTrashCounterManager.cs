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

    void Start()
    {
        collectedTrash = 0;
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
            Debug.Log("Semua sampah sudah dikumpulkan!");
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