using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public CanvasGroup dialogCanvasGroup;
    public Text dialogText;
    public Text speakerNameText;
    public float fadeDuration = 0.3f;
    public float textDisplayDuration = 4f;
    public AudioSource voiceAudioSource;

    Coroutine currentDialogRoutine;

    void Awake()
    {
        dialogCanvasGroup.alpha = 0;
        dialogCanvasGroup.gameObject.SetActive(false);
    }

    public void ShowDialog(string speaker, string message, AudioClip voiceClip = null)
    {
        if (currentDialogRoutine != null)
            StopCoroutine(currentDialogRoutine);

        currentDialogRoutine = StartCoroutine(DisplayDialogRoutine(speaker, message, voiceClip));
    }

    IEnumerator DisplayDialogRoutine(string speaker, string message, AudioClip voiceClip)
    {
        dialogCanvasGroup.gameObject.SetActive(true);
        dialogText.text = message;
        speakerNameText.text = speaker;

        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(dialogCanvasGroup, 0f, 1f));

        // Play voice line
        if (voiceClip != null && voiceAudioSource != null)
        {
            voiceAudioSource.clip = voiceClip;
            voiceAudioSource.Play();
        }

        yield return new WaitForSeconds(textDisplayDuration);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(dialogCanvasGroup, 1f, 0f));
        dialogCanvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            cg.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }
}
