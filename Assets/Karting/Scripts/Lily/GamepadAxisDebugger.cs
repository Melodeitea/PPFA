using UnityEngine;

public class GamepadAxisDebugger : MonoBehaviour
{
    void Update()
    {
        Debug.Log($"LeftStickX (Axis 1): {Input.GetAxis("LeftStickX")}");
        Debug.Log($"RightStickX (Axis 4): {Input.GetAxis("RightStickX")}");
        Debug.Log($"LT (Axis 9): {Input.GetAxis("LT")}");
        Debug.Log($"RT (Axis 10): {Input.GetAxis("RT")}");
    }
}
