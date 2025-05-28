using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;

public class CheckpointTrigger : MonoBehaviour
{
    [Header("Checkpoint ID")]
    public string checkpointID;

    [Header("Dialog")]
    public DialogSequence dialogToPlay;

    [Header("Optional Effects")]
    public UnityEvent onCheckpointEnter;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        // Play dialog
        if (dialogToPlay != null)
        {
            var manager = FindObjectOfType<DialogManager>();
            if (manager != null)
                manager.PlayDialog(dialogToPlay);
        }

        // Trigger any additional effects (VFX, sound, gameplay changes)
        onCheckpointEnter?.Invoke();

        Debug.Log($"[Checkpoint] Triggered checkpoint: {checkpointID}");
    }
}
