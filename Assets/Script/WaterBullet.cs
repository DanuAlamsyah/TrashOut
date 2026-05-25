using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;

    [Header("Hit Effects")]
    public GameObject splashParticlePrefab;
    public GameObject hitRingPrefab;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        MonsterHealth monster = collision.gameObject.GetComponent<MonsterHealth>();

        if (monster != null)
        {
            monster.TakeDamage(damage);
        }

        SpawnHitEffects(collision);

        Destroy(gameObject);
    }

    void SpawnHitEffects(Collision collision)
    {
        if (collision.contacts.Length == 0) return;

        ContactPoint contact = collision.contacts[0];

        if (splashParticlePrefab != null)
        {
            GameObject splash = Instantiate(
                splashParticlePrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            Destroy(splash, 1f);
        }

        if (hitRingPrefab != null)
        {
            GameObject ring = Instantiate(
                hitRingPrefab,
                contact.point + contact.normal * 0.01f,
                Quaternion.FromToRotation(Vector3.up, contact.normal)
            );

            Destroy(ring, 1f);
        }
    }
}