using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogSequence sequence;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        DialogManager manager = FindObjectOfType<DialogManager>();
        if (manager && sequence != null)
        {
            manager.PlayDialog(sequence);
            triggered = true;
        }
    }
}
