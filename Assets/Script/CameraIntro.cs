using System.Collections;
using UnityEngine;

public class CameraIntro : MonoBehaviour
{
    public Transform introPoint;
    public Transform dialogPoint;
    public DialogManager dialogManager;

    public float moveDuration = 3f;

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Posisi awal kamera
        transform.position = introPoint.position;
        transform.rotation = introPoint.rotation;

        // Lihat kota dulu
        yield return new WaitForSeconds(2f);

        float timer = 0;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while(timer < moveDuration)
        {
            float t = timer / moveDuration;

            transform.position =
                Vector3.Lerp(
                    startPos,
                    dialogPoint.position,
                    t
                );

            transform.rotation =
                Quaternion.Lerp(
                    startRot,
                    dialogPoint.rotation,
                    t
                );

            timer += Time.deltaTime;

            yield return null;
        }

        transform.position = dialogPoint.position;
        transform.rotation = dialogPoint.rotation;

        dialogManager.StartDialogue();

    }
}