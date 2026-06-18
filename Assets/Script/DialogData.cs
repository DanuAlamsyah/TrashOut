using UnityEngine;

[System.Serializable]
public class DialogData
{
    public string speaker;
    [TextArea(2,5)]
    public string sentence;
}