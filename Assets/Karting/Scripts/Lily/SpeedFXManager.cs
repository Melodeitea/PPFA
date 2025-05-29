// SpeedFXManager.cs
using UnityEngine;
using UnityEngine.UI;

public class SpeedFXManager : MonoBehaviour
{
    [Header("General Settings")]
    public bool speedFXEnabled = true;
    public KeyCode toggleKey = KeyCode.F;

    [Header("Player Movement")]
    public Rigidbody playerRb;
    public float maxSpeed = 10f; // To normalize effects

    [Header("Camera Effects")]
    public Camera mainCamera;
    public float defaultFOV = 60f;
    public float boostedFOV = 90f;
    public float fovLerpSpeed = 5f;

    [Header("Scrolling Motif")]
    public Renderer scrollingGround;
    public float scrollMultiplier = 2f;

    [Header("Parallax Layers")]
    public Transform[] parallaxLayers;
    public float[] parallaxMultipliers;

    [Header("Particle FX")]
    public GameObject speedLinesFX;

    [Header("Audio FX")]
    public AudioSource windSound;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    [Header("UI Feedback")]
    public Slider speedBar;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = playerRb.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            speedFXEnabled = !speedFXEnabled;
            ToggleAllFX(speedFXEnabled);
        }

        float currentSpeed = playerRb.velocity.magnitude;
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);

        if (speedFXEnabled)
        {
            UpdateCameraFOV(speedRatio);
            UpdateScrollingPattern(speedRatio);
            UpdateParallax();
            UpdateAudioFX(speedRatio);
            UpdateSpeedBar(speedRatio);
        }
    }

    void ToggleAllFX(bool state)
    {
        if (speedLinesFX != null)
            speedLinesFX.SetActive(state);

        if (windSound != null)
        {
            if (state)
                windSound.Play();
            else
                windSound.Stop();
        }
    }

    void UpdateCameraFOV(float ratio)
    {
        float targetFOV = Mathf.Lerp(defaultFOV, boostedFOV, ratio);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovLerpSpeed);
    }

    void UpdateScrollingPattern(float ratio)
    {
        if (scrollingGround != null)
        {
            Vector2 offset = new Vector2(Time.time * scrollMultiplier * ratio, 0);
            scrollingGround.material.mainTextureOffset = offset;
        }
    }

    void UpdateParallax()
    {
        Vector3 delta = playerRb.position - lastPosition;
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            if (parallaxLayers[i] != null)
                parallaxLayers[i].Translate(-delta * parallaxMultipliers[i]);
        }
        lastPosition = playerRb.position;
    }

    void UpdateAudioFX(float ratio)
    {
        if (windSound != null)
            windSound.pitch = Mathf.Lerp(minPitch, maxPitch, ratio);
    }

    void UpdateSpeedBar(float ratio)
    {
        if (speedBar != null)
            speedBar.value = ratio;
    }
}
