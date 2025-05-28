using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    private bool hasCrashed = false;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Car") || collision.collider.CompareTag("Trees"))
		{
			TimelineController controller = FindObjectOfType<TimelineController>();
			if (controller != null)
			{
				controller.endingType = TimelineController.EndingType.Crash;
				controller.EndSequence();
			}
		}
	}

}
