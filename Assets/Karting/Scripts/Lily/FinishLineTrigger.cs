using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (TimelineController.HasEnded) return;

		if (other.CompareTag("Player"))
		{
			var controller = FindObjectOfType<TimelineController>();
			if (controller != null)
			{
				controller.TryEndSequence(TimelineController.EndingType.Good);
				Debug.Log("Player reached finish line");
			}
		}
	}
}
