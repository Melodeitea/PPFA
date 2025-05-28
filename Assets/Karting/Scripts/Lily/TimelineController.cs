using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TimelineController : MonoBehaviour
{
	public float totalDuration = 180f;
	private float timer;
	private bool ended = false;

	public enum EndingType { Good, Timeout, Crash }
	public EndingType endingType = EndingType.Timeout;

	[System.Serializable]
	public class TimedEvent
	{
		public string eventName;
		public float triggerTime;
		public UnityEvent onTrigger;
		public bool hasTriggered = false;
	}

	public List<TimedEvent> events = new List<TimedEvent>();

	void Update()
	{
		if (ended) return;

		timer += Time.deltaTime;

		foreach (var e in events)
		{
			if (!e.hasTriggered && timer >= e.triggerTime)
			{
				Debug.Log($"[Timeline] Triggering: {e.eventName} at {timer:F2}s");
				e.onTrigger.Invoke();
				e.hasTriggered = true;
			}
		}

		if (timer >= totalDuration)
		{
			endingType = EndingType.Timeout;
			EndSequence();
		}
	}

	public void EndSequence()
	{
		if (ended) return;
		ended = true;

		Debug.Log($"[Timeline] Sequence ended with: {endingType}");

		GameEndingManager manager = FindObjectOfType<GameEndingManager>();
		if (manager != null)
		{
			switch (endingType)
			{
				case EndingType.Good:
					manager.TriggerGoodEnding();
					break;
				case EndingType.Crash:
					manager.TriggerBadEnding_Crash();
					break;
				case EndingType.Timeout:
					manager.TriggerBadEnding_Timeout();
					break;
			}
		}
	}

	public void ResetTimeline()
	{
		timer = 0f;
		ended = false;
		foreach (var e in events)
		{
			e.hasTriggered = false;
		}
	}
}
