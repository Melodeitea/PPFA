using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotorbikeController : MonoBehaviour
{
    [Header("Input")]
    public ControllerInput input;


    [Header("Movement")]
    public float acceleration = 1000f;
    public float brakeForce = 800f;
    public float turnSpeed = 50f;
    public float maxSpeed = 50f;

    [Header("Stability")]
    public float uprightForce = 5000f;
    public float leanAngle = 25f;
    public float leanSmoothing = 5f;

    private float gripMultiplier = 1f;
    private Rigidbody rb;
    private float currentSpeed = 10f;
    private bool hasCrashed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Lower CoM for stability
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyStabilization();
        ApplyLeaning();
        ClampMaxSpeed();
    }

    private void ApplyMovement()
    {
        Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        float throttle = Mathf.Clamp01(input.throttleInput);
        float brake = Mathf.Clamp01(input.brakeInput);
        float steering = input.steeringInput;

        float targetSpeed = throttle * maxSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 desiredVelocity = flatForward * currentSpeed;
        Vector3 velocityDiff = desiredVelocity - flatVelocity;

        Vector3 force = velocityDiff * gripMultiplier;
        rb.AddForce(force, ForceMode.Acceleration);

        if (brake > 0f)
        {
            Vector3 forwardVel = Vector3.Project(flatVelocity, flatForward);
            Vector3 brakeForceVec = -forwardVel * brake * brakeForce * Time.fixedDeltaTime;
            rb.AddForce(brakeForceVec, ForceMode.VelocityChange);

            Vector3 lateralVel = Vector3.Project(flatVelocity, transform.right);
            rb.AddForce(-lateralVel * 0.5f * brake, ForceMode.VelocityChange);
        }

        float steerAmount = steering * turnSpeed * Time.fixedDeltaTime;
        Quaternion steerRotation = Quaternion.Euler(0f, steerAmount, 0f);
        rb.MoveRotation(rb.rotation * steerRotation);
    }




    private void ApplyStabilization()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(
            rb.angularVelocity.magnitude * Mathf.Rad2Deg * uprightForce / rb.mass,
            rb.angularVelocity
        ) * transform.up;

        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        rb.AddTorque(torqueVector * uprightForce * Time.fixedDeltaTime);
    }

    private void ApplyLeaning()
    {
        if (rb.velocity.magnitude > 1f)
        {
            float leanInput = input.steeringInput * input.throttleInput * gripMultiplier;
            float targetLean = Mathf.Clamp(leanInput * leanAngle, -30f, 30f);

            Quaternion currentRotation = rb.rotation;
            Quaternion leanRotation = Quaternion.Euler(
                currentRotation.eulerAngles.x,
                currentRotation.eulerAngles.y,
                -targetLean
            );

            rb.MoveRotation(Quaternion.Slerp(currentRotation, leanRotation, Time.fixedDeltaTime * leanSmoothing));
        }
    }

    private void ClampMaxSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 clampedFlat = flatVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(clampedFlat.x, rb.velocity.y, clampedFlat.z);
        }

    }

    public void SetGripMultiplier(float value) => gripMultiplier = value;
    public void ResetGripMultiplier() => gripMultiplier = 1f;

    void OnCollisionEnter(Collision col)
    {
        if (hasCrashed) return;

        if (col.collider.CompareTag("Trees") || col.collider.CompareTag("Car") || col.collider.CompareTag("Animal"))
        {
            hasCrashed = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) rb.velocity = Vector3.zero;

            GameEndingManager endingManager = FindObjectOfType<GameEndingManager>();
            if (endingManager != null)
            {
                endingManager.TriggerBadEnding_Crash();
            }

            // Optionally: disable controls, add camera shake, etc.
            // For example: enabled = false;
        }
    }
}
