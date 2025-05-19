using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public CanvasGroup dialogCanvas;
    public TMP_Text speakerNameText;
    public TMP_Text dialogText;
    public AudioSource audioSource;
    public float fadeDuration = 0.3f;
    public float typeSpeed = 40f;

    private Coroutine currentDialog;
    private bool isTyping;
    private bool skipTyping;

    void Awake()
    {
        dialogCanvas.alpha = 0;
        dialogCanvas.interactable = false;
        dialogCanvas.blocksRaycasts = false;
    }

    public void PlayDialog(DialogSequence sequence)
    {
        if (currentDialog != null)
            StopCoroutine(currentDialog);

        currentDialog = StartCoroutine(PlaySequence(sequence));
    }

    IEnumerator PlaySequence(DialogSequence sequence)
    {
        yield return FadeCanvas(1);

        foreach (var line in sequence.lines)
        {
            speakerNameText.text = line.speakerName;
            if (line.voiceClip) audioSource.PlayOneShot(line.voiceClip);
            yield return TypeText(line.content);

            yield return new WaitForSeconds(line.duration);
        }

        yield return FadeCanvas(0);
    }

    IEnumerator TypeText(string content)
    {
        isTyping = true;
        skipTyping = false;
        dialogText.text = "";

        foreach (char c in content)
        {
            if (skipTyping) break;
            dialogText.text += c;
            yield return new WaitForSeconds(1f / typeSpeed);
        }

        dialogText.text = content;
        isTyping = false;
    }

    public void SkipOrAdvance()
    {
        if (isTyping)
            skipTyping = true;
    }

    IEnumerator FadeCanvas(float target)
    {
        float start = dialogCanvas.alpha;
        float time = 0;
        dialogCanvas.interactable = true;
        dialogCanvas.blocksRaycasts = true;

        while (time < fadeDuration)
        {
            dialogCanvas.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        dialogCanvas.alpha = target;
        if (target == 0)
        {
            dialogCanvas.interactable = false;
            dialogCanvas.blocksRaycasts = false;
        }
    }
}
