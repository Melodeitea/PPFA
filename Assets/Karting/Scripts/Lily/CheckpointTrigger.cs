using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Or "Motorbike" tag
        {
            onEnter.Invoke();
        }
    }
}
