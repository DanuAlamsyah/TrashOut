using UnityEngine;

public class WaterHitRing : MonoBehaviour
{
    public float lifeTime = 0.5f;
    public float expandSpeed = 2f;

    private Vector3 startScale;
    private float timer;

    void Start()
    {
        startScale = transform.localScale;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / lifeTime;
        float scaleMultiplier = 1f + progress * expandSpeed;

        transform.localScale = new Vector3(
            startScale.x * scaleMultiplier,
            startScale.y,
            startScale.z * scaleMultiplier
        );
    }
}