using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // 1. Membebaskan kursor agar tidak terkunci di tengah layar game
        Cursor.lockState = CursorLockMode.None;

        // 2. Membuat kursor kembali terlihat
        Cursor.visible = true;
    }
}
