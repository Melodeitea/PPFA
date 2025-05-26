using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CheckpointEventManager : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointEvent
    {
        public string checkpointName;
        public GameObject[] activateObjects;   // Objects that have effects/scripts to Activate
        public GameObject[] deactivateObjects; // Optional: explicitly deactivate others
        public UnityEvent onCheckpointEnter;   // Any extra logic or UI triggers
    }

    public List<CheckpointEvent> events;
    private GameObject currentActiveEffect;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (var evt in events)
        {
            if (evt.checkpointName == this.gameObject.name)
            {
                // Deactivate previous effect
                if (currentActiveEffect != null)
                {
                    var deactivators = currentActiveEffect.GetComponents<MonoBehaviour>();
                    foreach (var d in deactivators)
                    {
                        var m = d.GetType().GetMethod("Deactivate" + d.GetType().Name.Replace("Controller", ""));
                        m?.Invoke(d, null);
                    }
                }

                // Deactivate any listed others
                foreach (var obj in evt.deactivateObjects)
                {
                    var components = obj.GetComponents<MonoBehaviour>();
                    foreach (var c in components)
                    {
                        var m = c.GetType().GetMethod("Deactivate" + c.GetType().Name.Replace("Controller", ""));
                        m?.Invoke(c, null);
                    }
                }

                // Activate new effect(s)
                foreach (var obj in evt.activateObjects)
                {
                    var components = obj.GetComponents<MonoBehaviour>();
                    foreach (var c in components)
                    {
                        var m = c.GetType().GetMethod("Activate" + c.GetType().Name.Replace("Controller", ""));
                        m?.Invoke(c, null);
                    }
                    currentActiveEffect = obj;
                }

                evt.onCheckpointEnter?.Invoke();
                break;
            }
        }
    }
}
