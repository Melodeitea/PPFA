using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    [Header("Output")]
    public float throttleInput;  // RT
    public float brakeInput;     // LT
    public float steeringInput;  // Right Stick X
    public float cameraInput;    // Left Stick X

    void Update()
    {
        // Triggers - map these correctly in Input Manager
        throttleInput = Input.GetAxis("RT"); // 0 to 1
        brakeInput = Input.GetAxis("LT");    // 0 to 1

        // Right Stick controls bike turning
        steeringInput = Input.GetAxis("RightStickX");

        // Left Stick X controls camera yaw
        cameraInput = Input.GetAxis("Horizontal"); // or a custom "LeftStickX" if needed
    }
}
