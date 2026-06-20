using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public MonsterHealth monsterHealth;
    public Image healthFill;

    private int maxHealth;

    void Start()
    {
        maxHealth = monsterHealth.maxHealth;
    }


    void Update()
    {
        float current =
            monsterHealth.GetCurrentHealth();

        healthFill.fillAmount =
            current / maxHealth;
    }
}