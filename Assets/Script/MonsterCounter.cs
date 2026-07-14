using UnityEngine;

public class MonsterCounter : MonoBehaviour
{
    public static MonsterCounter Instance;

    private int aliveMonster = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        aliveMonster = 0;
    }

    public void RegisterMonster()
    {
        aliveMonster++;
        Debug.Log("Monster Spawn : " + aliveMonster);
    }

    public void MonsterKilled()
    {
        aliveMonster--;

        if (aliveMonster < 0)
            aliveMonster = 0;

        Debug.Log("Monster Tersisa : " + aliveMonster);
    }

    public bool IsAllMonsterDead()
    {
        return aliveMonster <= 0;
    }

    public int GetAliveMonster()
    {
        return aliveMonster;
    }
}