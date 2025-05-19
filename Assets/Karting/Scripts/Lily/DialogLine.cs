using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string speakerName;
    [TextArea] public string content;
    public float duration = 3f; // Optional override per line
    public AudioClip voiceClip;
}
