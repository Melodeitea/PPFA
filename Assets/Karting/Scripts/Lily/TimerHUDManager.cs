using System;
using UnityEngine;
using TMPro;

public class TimerHUDManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject player;

    private TimeManager m_TimeManager;
    private bool hasTriggeredEnding = false;

    private void Start()
    {
        m_TimeManager = FindObjectOfType<TimeManager>();
        DebugUtility.HandleErrorIfNullFindObject<TimeManager, TimerHUDManager>(m_TimeManager, this);

        if (!m_TimeManager.IsFinite)
        {
            timerText.text = "";
            timerText.gameObject.SetActive(false);
        }
        else
        {
            timerText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!m_TimeManager.IsFinite) return;

        int timeRemaining = Mathf.CeilToInt(m_TimeManager.TimeRemaining);

        // Debug info
        Debug.Log($"[TimerHUD] Time remaining: {timeRemaining}s");

        // Update UI
        timerText.text = string.Format("{0}:{1:00}", timeRemaining / 60, timeRemaining % 60);

        // Trigger end only once
        if (m_TimeManager.IsOver && !hasTriggeredEnding)
        {
            hasTriggeredEnding = true;
            TriggerTimeoutEnding();
        }
    }

    private void TriggerTimeoutEnding()
    {
        Debug.Log("[TimerHUDManager] Timer ran out. Triggering timeout ending.");

        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb) rb.velocity = Vector3.zero;
        }

        GameEndingManager endingManager = FindObjectOfType<GameEndingManager>();
        if (endingManager != null)
        {
            endingManager.TriggerBadEnding_Timeout();
        }
    }
}
