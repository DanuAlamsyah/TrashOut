using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [Header("Player Health")]
    public PlayerHealth playerHealth;

    [Header("Heart Fill Images")]
    public Image[] heartFills;

    [Header("Setting")]
    public float hpPerHeart = 2f; // 1 hati = 2 HP

    void Start()
    {
        SetupHeartFills();
        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void SetupHeartFills()
    {
        foreach (Image heart in heartFills)
        {
            if (heart == null) continue;

            heart.type = Image.Type.Filled;
            heart.fillMethod = Image.FillMethod.Horizontal;
            heart.fillOrigin = (int)Image.OriginHorizontal.Left;
            heart.fillAmount = 1f;
            heart.preserveAspect = true;
        }
    }

    void UpdateHearts()
    {
        if (playerHealth == null) return;

        for (int i = 0; i < heartFills.Length; i++)
        {
            if (heartFills[i] == null) continue;

            float hpUntukHeartIni = playerHealth.currentHealth - (i * hpPerHeart);
            float fill = Mathf.Clamp01(hpUntukHeartIni / hpPerHeart);

            heartFills[i].fillAmount = fill;
        }
    }
}