using System;
using UnityEngine;
using TMPro;

public class TimerHUDManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public DialogSequence timeoutEndingDialog; // Assign in Inspector
    public GameObject player;                  // Player object with tag "Player"

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

        // Check for timeout once
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

        // Play dialog
        DialogManager dialogManager = FindObjectOfType<DialogManager>();
        if (dialogManager && timeoutEndingDialog != null)
        {
            dialogManager.PlayDialog(timeoutEndingDialog);
        }

        // Optionally add: fade out screen, load menu, etc.
    }
}
