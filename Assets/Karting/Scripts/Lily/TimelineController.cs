using UnityEngine;
using System.Collections.Generic;

public class TimelineController : MonoBehaviour
{
    public float totalDuration = 180f; // 3 minutes
    private float timer;

    [System.Serializable]
    public class TimedEvent
    {
        public float triggerTime;
        public UnityEngine.Events.UnityEvent onTrigger;
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
        Debug.Log("Game finished or transition to end scene.");
        // e.g., trigger final cinematic or result screen
    }
}
