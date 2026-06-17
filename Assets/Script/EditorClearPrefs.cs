using UnityEngine;
using UnityEditor;

public class EditorClearPrefs
{
    [MenuItem("Tools/Reset Semua Data Game")]
    public static void HapusDataMurni()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("--- BRANKAS PLAYERPREFS SEKARANG SUDAH KEMBALI 0! ---");
    }
}