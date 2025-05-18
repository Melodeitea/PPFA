using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MotorbikeController : MonoBehaviour
{
    [Header("Input")]
    public ControllerInput input; // Reference to your DualShock controller input script

    [Header("Movement Settings")]
    public float baseSpeed = 10f;
    public float maxSpeed = 50f;
    public float acceleration = 20f;
    public float brakeForce = 15f;
    public float turnSpeed = 100f;
    public float driftFactor = 0.9f;

    [Header("Suspension")]
    public Transform[] wheelPoints;
    public float suspensionDistance = 0.5f;
    public float suspensionStrength = 20000f;
    public float wheelRadius = 0.35f;
    public LayerMask groundLayer;

    [Header("FX & Visuals")]
    public Transform[] wheelMeshes;
    public ParticleSystem driftFX;
    public TrailRenderer[] driftTrails;

    private Rigidbody rb;
    private bool isDrifting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (input == null)
        {
            Debug.LogError("ControllerInput reference is not assigned on " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        if (input == null) return;

        HandleSuspension();
        ApplyMovement();
        HandleDrift();
        UpdateVisuals();
    }

    private void ApplyMovement()
    {
        Vector3 forward = transform.forward;

        // Use input from DualShock controller
        float throttle = Mathf.Clamp01(input.throttleInput); // Up = 1, Down = -1
        float brake = Mathf.Clamp01(-input.throttleInput);   // Down = -1 becomes 1
        float steering = input.steeringInput;

        float targetSpeed = baseSpeed + throttle * acceleration;

        Vector3 forwardVelocity = Vector3.Project(rb.velocity, forward);

        if (forwardVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(forward * targetSpeed, ForceMode.Acceleration);
        }

        if (brake > 0f)
        {
            rb.AddForce(-forward * brakeForce * brake, ForceMode.Acceleration);
        }

        float steerAmount = steering * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnOffset = Quaternion.Euler(0f, steerAmount, 0f);
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    private void HandleSuspension()
    {
        foreach (Transform wheel in wheelPoints)
        {
            Ray ray = new Ray(wheel.position, -wheel.up);
            if (Physics.Raycast(ray, out RaycastHit hit, suspensionDistance + wheelRadius, groundLayer))
            {
                float suspensionForce = (suspensionDistance - hit.distance) / suspensionDistance;
                suspensionForce = Mathf.Clamp01(suspensionForce);
                Vector3 force = wheel.up * suspensionForce * suspensionStrength;
                rb.AddForceAtPosition(force, wheel.position);
            }
        }
    }

    private void HandleDrift()
    {
        // Simple drift: press PS4 X button (mapped as "Jump" in Input Manager or manually via Input.GetKey)
        isDrifting = Input.GetButton("Jump"); // Make sure "Jump" is mapped to "joystick button 1"

        if (isDrifting)
        {
            Vector3 forwardVelocity = Vector3.Project(rb.velocity, transform.forward);
            Vector3 sidewaysVelocity = Vector3.Project(rb.velocity, transform.right);
            rb.velocity = forwardVelocity + sidewaysVelocity * driftFactor;

            if (driftFX != null && !driftFX.isPlaying)
                driftFX.Play();

            foreach (var trail in driftTrails)
                trail.emitting = true;
        }
        else
        {
            if (driftFX != null && driftFX.isPlaying)
                driftFX.Stop();

            foreach (var trail in driftTrails)
                trail.emitting = false;
        }
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < wheelPoints.Length && i < wheelMeshes.Length; i++)
        {
            Ray ray = new Ray(wheelPoints[i].position, -wheelPoints[i].up);
            if (Physics.Raycast(ray, out RaycastHit hit, suspensionDistance + wheelRadius, groundLayer))
            {
                wheelMeshes[i].position = hit.point + Vector3.up * wheelRadius;
            }
        }
    }

    public float GetSpeed() => rb.velocity.magnitude;
    public bool IsDrifting() => isDrifting;
}
