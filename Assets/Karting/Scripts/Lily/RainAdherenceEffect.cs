using UnityEngine;
using System.Collections;


public class RainAdherenceEffect : MonoBehaviour
{
	[Header("Settings")]
	public float reducedGripFactor = 0.5f;

	[Header("Rain Prefab (with particles and sound)")]
	public GameObject rainPrefab;

	private MotorbikeController bike;
	private GameObject activeRainInstance;

	public AudioSource rainAudioSource;
	public AudioClip rainLoop;

	public void ActivateRain()
	{
		if (bike == null) bike = FindObjectOfType<MotorbikeController>();
		if (bike != null) StartCoroutine(AdjustGripOverTime(reducedGripFactor, 1f));

		if (rainPrefab != null && activeRainInstance == null)
			activeRainInstance = Instantiate(rainPrefab, transform);

		if (rainAudioSource && rainLoop)
		{
			rainAudioSource.clip = rainLoop;
			rainAudioSource.loop = true;
			rainAudioSource.Play();
		}
	}

	IEnumerator AdjustGripOverTime(float target, float duration)
	{
		float initial = 1f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			bike.SetGripMultiplier(Mathf.Lerp(initial, target, t));
			yield return null;
		}

		bike.SetGripMultiplier(target);
	}

	public void DeactivateRain()
	{
		if (bike != null)
			bike.ResetGripMultiplier();

		if (activeRainInstance != null)
		{
			Destroy(activeRainInstance);
			activeRainInstance = null;
		}

		if (rainAudioSource)
			rainAudioSource.Stop();
	}
}
