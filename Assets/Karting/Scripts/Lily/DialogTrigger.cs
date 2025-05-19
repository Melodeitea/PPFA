using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public string speakerName;
    [TextArea]
    public string dialogLine;
    public AudioClip voiceClip;

    public DialogManager dialogManager;

    public void TriggerDialog()
    {
        if (dialogManager != null)
            dialogManager.ShowDialog(speakerName, dialogLine, voiceClip);
    }
}
