﻿using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public bool useTimer = true;
    public float totalTimeInSeconds = 180f; // Default: 3 minutes

    public bool IsFinite { get; private set; }
    public float TotalTime { get; private set; }
    public float TimeRemaining { get; private set; }
    public bool IsOver { get; private set; }

    private bool raceStarted = false;

    public static Action<float> OnAdjustTime;
    public static Action<int, bool, GameMode> OnSetTime;
    public static event Action OnTimeOver;

    private void Awake()
    {
        TotalTime = totalTimeInSeconds;
        TimeRemaining = TotalTime;
        IsFinite = useTimer;
        IsOver = false;
    }

    private void Start()
    {
        // Automatically start the timer on game start
        StartRace();
    }

    void OnEnable()
    {
        OnAdjustTime += AdjustTime;
        OnSetTime += SetTime;
    }

    private void OnDisable()
    {
        OnAdjustTime -= AdjustTime;
        OnSetTime -= SetTime;
    }

    private void AdjustTime(float delta)
    {
        TimeRemaining += delta;
        TimeRemaining = Mathf.Clamp(TimeRemaining, 0, TotalTime);
    }

    private void SetTime(int time, bool isFinite, GameMode gameMode)
    {
        TotalTime = time;
        IsFinite = isFinite;
        TimeRemaining = TotalTime;
    }

    void Update()
    {
        if (!raceStarted || !IsFinite || IsOver) return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;
            IsOver = true;

            TimelineController controller = FindObjectOfType<TimelineController>();
            if (controller != null)
            {
                controller.endingType = TimelineController.EndingType.Timeout;
                controller.EndSequence();
            }
        }

    }

    public void StartRace()
    {
        raceStarted = true;
    }

    public void StopRace()
    {
        raceStarted = false;
    }
}
