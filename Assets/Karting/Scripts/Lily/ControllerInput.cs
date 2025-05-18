using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    [Header("Output")]
    public float throttleInput;
    public float steeringInput;

    void Update()
    {
        // Use Input Manager axes for PS4 controller
        throttleInput = Input.GetAxis("Vertical");   // Left stick Y (up = 1, down = -1)
        steeringInput = Input.GetAxis("Horizontal"); // Left stick X (right = 1, left = -1)
    }
}
