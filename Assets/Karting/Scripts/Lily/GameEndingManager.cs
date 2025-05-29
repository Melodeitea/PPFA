using System.Collections;
using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
	private bool hasEnded = false;

	public DialogSequence goodEndingDialog;
	public DialogSequence crashEndingDialog;
	public DialogSequence timeoutEndingDialog;

	private DialogManager dialogManager;

	public CanvasGroup endingOverlay; // Black screen UI
	public AudioSource musicSource;
	public AudioClip badEndingMusic_Crash;
	public AudioClip badEndingMusic_Timeout;
	public AudioClip goodEndingMusic;

	void Start()
	{
		dialogManager = FindObjectOfType<DialogManager>();
	}

	public void TriggerGoodEnding()
	{
		if (hasEnded) return;
		hasEnded = true;
		StartCoroutine(HandleEnding(goodEndingDialog, goodEndingMusic));
		Debug.Log("Good ending triggered!");
	}

	public void TriggerBadEnding_Crash()
	{
		if (hasEnded) return;
		hasEnded = true;
		StartCoroutine(HandleEnding(crashEndingDialog, badEndingMusic_Crash));
		Debug.Log("Crash ending triggered!");
	}

	public void TriggerBadEnding_Timeout()
	{
		if (hasEnded) return;
		hasEnded = true;
		StartCoroutine(HandleEnding(timeoutEndingDialog, badEndingMusic_Timeout));
		Debug.Log("Timeout ending triggered!");
	}


	private IEnumerator HandleEnding(DialogSequence dialog, AudioClip music)
	{
		// Fade to black
		yield return StartCoroutine(FadeInBlackOverlay());

		// Play music
		if (musicSource && music)
		{
			musicSource.clip = music;
			musicSource.loop = false;
			musicSource.Play();
		}

		// Small delay before dialog (ensures canvas is visible)
		yield return new WaitForSeconds(0.5f);

		// Start dialog
		if (dialogManager && dialog != null)
		{
			dialogManager.PlayDialog(dialog);
		}
	}


	private IEnumerator FadeInBlackOverlay()
	{
		if (endingOverlay == null) yield break;

		endingOverlay.gameObject.SetActive(true);

		float duration = 2f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			endingOverlay.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
			yield return null;
		}

		endingOverlay.alpha = 1f;

	}

}
