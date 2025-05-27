using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    [Header("Output")]
    public float throttleInput;  // RT
    public float brakeInput;     // LT
    public float steeringInput;  // Left Stick X
    public float cameraInput;    // Right Stick X

    void Update()
    {
        // RT (Axis 10) — 0 to 1
        throttleInput = Mathf.Clamp01(Input.GetAxis("RT"));

        // LT (Axis 9) — 0 to 1
        brakeInput = Mathf.Clamp01(Input.GetAxis("LT"));

        // Steering — Left Stick X (Axis 1)
        steeringInput = Input.GetAxis("LeftStickX");

        // Camera — Right Stick X (Axis 4)
        cameraInput = Input.GetAxis("RightStickX");
    }
}
