using UnityEngine;

public class SortirManager : MonoBehaviour
{
    void Start()
    {
        // 1. Mencari objek yang memiliki Tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 2. Menonaktifkan Player agar script StarterAssets tidak bisa mengunci kursor lagi
            player.SetActive(false);
        }

        // 3. Memaksa kursor untuk muncul dan bisa digerakkan bebas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
