using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 1;


    [Header("Hit Effect")]
    public GameObject waterSplashEffect;


    private GameObject owner;

    private bool canHit = false;



    void Start()
    {
        // beri waktu bullet keluar dari player
        Invoke(nameof(EnableHit), 0.15f);
    }



    void EnableHit()
    {
        canHit = true;
    }



    public void SetOwner(GameObject shooter)
    {
        owner = shooter;
    }




    private void OnTriggerEnter(Collider other)
    {

        if(!canHit)
            return;



        // jangan kena player sendiri
        if(other.gameObject == owner)
        {
            return;
        }




        MonsterHealth monster =
        other.GetComponent<MonsterHealth>();


        if(monster != null)
        {
            monster.TakeDamage(damage);
        }




        if(waterSplashEffect != null)
        {
            Instantiate(
                waterSplashEffect,
                transform.position,
                Quaternion.identity
            );
        }



        Destroy(gameObject);

    }
}