using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimelineController controller = FindObjectOfType<TimelineController>();
            if (controller != null)
            {
                controller.endingType = TimelineController.EndingType.Good;
                controller.EndSequence();
            }
        }
    }
}
