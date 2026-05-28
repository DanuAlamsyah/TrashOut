using UnityEngine;
using TMPro;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    [Header("UI")]
    public TextMeshProUGUI trashCounterText;

    [Header("Jumlah Sampah")]
    public int collectedTrash = 0;
    public int totalTrash = 20;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateTrashUI();
    }

    public void AddTrash()
    {
        collectedTrash++;

        UpdateTrashUI();

        Debug.Log("Sampah terkumpul: " + collectedTrash + " / " + totalTrash);
    }

    void UpdateTrashUI()
    {
        if (trashCounterText != null)
        {
            trashCounterText.text = "Sampah: " + collectedTrash + " / " + totalTrash;
        }
    }
}