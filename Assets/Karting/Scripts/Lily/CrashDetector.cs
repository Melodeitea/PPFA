using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
	private bool hasCrashed = false;

	private void OnCollisionEnter(Collision collision)
	{
		if (hasCrashed || TimelineController.HasEnded) return;

		if (collision.collider.CompareTag("Car") || collision.collider.CompareTag("Trees"))
		{
			var controller = FindObjectOfType<TimelineController>();
			if (controller != null)
			{
				hasCrashed = true;
				controller.TryEndSequence(TimelineController.EndingType.Crash);
				Debug.Log($"Crash detected with {collision.collider.name}");
			}
		}
	}
}
