using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TimelineController : MonoBehaviour
{
    public float totalDuration = 180f;
    private float timer;

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
            EndSequence();
        }
    }

    void EndSequence()
    {
        Debug.Log("[Timeline] Sequence ended.");
        // Trigger anything here like fade, end cutscene, etc.
    }

    public void ResetTimeline()
    {
        timer = 0f;
        foreach (var e in events)
        {
            e.hasTriggered = false;
        }
    }
}
