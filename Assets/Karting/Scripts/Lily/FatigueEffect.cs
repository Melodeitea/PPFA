using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FatigueEffect : MonoBehaviour
{
    [Header("Fatigue Parameters")]
    public float fatigueDuration = 15f;
    public float fatigueGripMultiplier = 0.7f;
    public Light blindingLight;

    [Header("UI Overlay")]
    public CanvasGroup blurCanvasGroup; // Simule la vision floue avec une UI
    public float maxBlurAlpha = 0.8f;

    private float timer;
    private MotorbikeController bike;

    public void ActivateFatigue()
    {
        bike = FindObjectOfType<MotorbikeController>();
        if (bike != null)
            bike.SetGripMultiplier(fatigueGripMultiplier);

        if (blurCanvasGroup != null)
            blurCanvasGroup.alpha = maxBlurAlpha;

        if (blindingLight != null)
            blindingLight.enabled = true;

        timer = fatigueDuration;
        StartCoroutine(FatigueTimer());
    }

    IEnumerator FatigueTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (blindingLight != null)
                blindingLight.intensity = Mathf.PingPong(Time.time * 10f, 3f) + 1f;

            yield return null;
        }

        DeactivateFatigue();
    }

    public void DeactivateFatigue()
    {
        if (bike != null)
            bike.ResetGripMultiplier();

        if (blurCanvasGroup != null)
            blurCanvasGroup.alpha = 0f;

        if (blindingLight != null)
        {
            blindingLight.intensity = 0f;
            blindingLight.enabled = false;
        }
    }
}
