using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotorbikeController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 25f;
    public float turnSpeed = 50f;
    public float leanAngle = 15f;

    [Header("Environmental Effects")]
    public bool isRaining = false;
    public float rainSlipperiness = 0.5f; // 0 (grippy) to 1 (slippery)

    [Header("Fatigue Effects")]
    public bool isDrowsy = false;
    public float maxBlurAmount = 2f;
    public Camera mainCamera;

    private Rigidbody rb;
    private float inputHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        // Apply drowsiness blur (visual, can be expanded)
        if (isDrowsy && mainCamera != null)
        {
            float blurStrength = Mathf.PingPong(Time.time, maxBlurAmount);
            // Hook to post-processing or camera shake here
            // Example: PostProcessingController.SetBlur(blurStrength);
        }
    }

    void FixedUpdate()
    {
        // Constant forward motion
        Vector3 forwardMove = transform.forward * forwardSpeed;
        rb.velocity = new Vector3(forwardMove.x, rb.velocity.y, forwardMove.z);

        // Steering and leaning
        float effectiveTurnSpeed = turnSpeed * (1f - (isRaining ? rainSlipperiness : 0f));
        float steer = inputHorizontal * effectiveTurnSpeed * Time.fixedDeltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, steer, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Leaning visual effect
        float targetLean = -inputHorizontal * leanAngle;
        transform.localRotation = Quaternion.Euler(0f, transform.localEulerAngles.y, targetLean);
    }
}
