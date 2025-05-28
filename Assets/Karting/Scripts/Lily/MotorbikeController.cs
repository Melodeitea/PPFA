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

	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip driftSFX;
	public AudioClip brakeSFX;
	public AudioClip crashSFX;
	public AudioClip treeHitSFX;
	public AudioClip animalHitSFX;
	public AudioClip carHitSFX;

	private float gripMultiplier = 1f;
	private Rigidbody rb;
	private bool hasCrashed = false;
	private float currentBrakeAudioCooldown = 0f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = new Vector3(0, -0.5f, 0);
	}

	void FixedUpdate()
	{
		ApplyMovement();
		ApplyStabilization();
		ClampMaxSpeed();
		if (currentBrakeAudioCooldown > 0f)
			currentBrakeAudioCooldown -= Time.fixedDeltaTime;
	}

	private void ApplyMovement()
	{
		Vector3 groundForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

		float throttle = Mathf.Clamp01(input.throttleInput);
		float brake = Mathf.Clamp01(input.brakeInput);
		float steering = input.steeringInput;

		Vector3 flatVelocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
		float currentFlatSpeed = Vector3.Dot(flatVelocity, groundForward);
		float targetSpeed = throttle * maxSpeed;
		float speedDiff = targetSpeed - currentFlatSpeed;

		float accel = Mathf.Sign(speedDiff) * Mathf.Min(Mathf.Abs(speedDiff), acceleration * Time.fixedDeltaTime);
		Vector3 force = groundForward * accel * gripMultiplier;
		rb.AddForce(force, ForceMode.VelocityChange);

		// 🔧 Smoothed braking force
		if (brake > 0f)
		{
			Vector3 forwardVel = Vector3.Project(flatVelocity, groundForward);
			Vector3 brakeVec = -forwardVel.normalized * brake * brakeForce * Time.fixedDeltaTime;
			rb.AddForce(brakeVec * gripMultiplier, ForceMode.Acceleration);

			// Drift behavior: allow lateral movement while braking
			Vector3 lateralVel = Vector3.Project(rb.velocity, transform.right);
			float lateralDrag = Mathf.Clamp(lateralVel.magnitude, 0f, 1f);
			rb.AddForce(-lateralVel * lateralDrag * 0.2f, ForceMode.Acceleration);

			// 🔉 Play brake sound once per brake session
			if (brakeSFX && currentBrakeAudioCooldown <= 0f)
			{
				audioSource?.PlayOneShot(brakeSFX);
				currentBrakeAudioCooldown = 1f;
			}
		}

		// Steering
		float steerAmount = steering * turnSpeed * Time.fixedDeltaTime;
		Quaternion steerRot = Quaternion.Euler(0f, steerAmount, 0f);
		rb.MoveRotation(rb.rotation * steerRot);
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
			rb.velocity = Vector3.zero;

			// 🔉 Play impact sound based on tag
			if (col.collider.CompareTag("Trees") && treeHitSFX)
				audioSource?.PlayOneShot(treeHitSFX);
			else if (col.collider.CompareTag("Car") && carHitSFX)
				audioSource?.PlayOneShot(carHitSFX);
			else if (col.collider.CompareTag("Animal") && animalHitSFX)
				audioSource?.PlayOneShot(animalHitSFX);
			else if (crashSFX)
				audioSource?.PlayOneShot(crashSFX);

			GameEndingManager endingManager = FindObjectOfType<GameEndingManager>();
			if (endingManager != null)
			{
				endingManager.TriggerBadEnding_Crash();
			}
		}
	}
}
