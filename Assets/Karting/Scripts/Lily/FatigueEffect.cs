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

	public AudioSource audioSource;
	public AudioClip fatigueSound;

	public void ActivateFatigue()
	{
		bike = FindObjectOfType<MotorbikeController>();
		if (bike != null)
			bike.SetGripMultiplier(fatigueGripMultiplier);

		if (blurCanvasGroup != null)
			blurCanvasGroup.alpha = maxBlurAlpha;

		if (blindingLight != null)
			blindingLight.enabled = true;

		if (audioSource && fatigueSound)
			audioSource.PlayOneShot(fatigueSound);

		timer = fatigueDuration;
		StartCoroutine(FatigueTimer());
	}

	IEnumerator FatigueTimer()
	{
		float elapsed = 0f;

		while (elapsed < fatigueDuration)
		{
			elapsed += Time.deltaTime;
			float progress = elapsed / fatigueDuration;

			if (blurCanvasGroup != null)
				blurCanvasGroup.alpha = Mathf.Lerp(maxBlurAlpha, 0f, progress);

			if (blindingLight != null)
				blindingLight.intensity = Mathf.Lerp(4f, 0f, progress) + Mathf.Sin(Time.time * 20f) * 0.5f;

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
