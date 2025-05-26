using System;
using UnityEngine;
using TMPro;

public class TimerHUDManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject player;  // Player object with tag "Player"

    private TimeManager m_TimeManager;
    private bool hasTriggeredEnding = false;

    private void Start()
    {
        m_TimeManager = FindObjectOfType<TimeManager>();
        DebugUtility.HandleErrorIfNullFindObject<TimeManager, TimerHUDManager>(m_TimeManager, this);

        if (m_TimeManager.IsFinite)
        {
            timerText.text = "";
        }
    }

    private void Update()
    {
        if (!m_TimeManager.IsFinite) return;

        int timeRemaining = Mathf.CeilToInt(m_TimeManager.TimeRemaining);

        // Display timer
        timerText.gameObject.SetActive(true);
        timerText.text = string.Format("{0}:{1:00}", timeRemaining / 60, timeRemaining % 60);

        // Trigger timeout ending once when time is up
        if (m_TimeManager.IsOver && !hasTriggeredEnding)
        {
            hasTriggeredEnding = true;
            TriggerTimeoutEnding();
        }
    }

    private void TriggerTimeoutEnding()
    {
        Debug.Log("[TimerHUDManager] Timer ran out. Triggering timeout ending.");

        // Stop the bike
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb) rb.velocity = Vector3.zero;
        }

        // Use centralized game ending logic
        GameEndingManager endingManager = FindObjectOfType<GameEndingManager>();
        if (endingManager != null)
        {
            endingManager.TriggerBadEnding_Timeout();
        }
    }
}
