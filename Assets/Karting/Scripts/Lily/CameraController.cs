using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform bike;
    public float panAmount = 15f;
    public float panSpeed = 5f;
    public ControllerInput input;

    void LateUpdate()
    {
        if (!bike) return;

        float yaw = input.cameraInput * panAmount;
        Quaternion targetRotation = Quaternion.Euler(10f, yaw, 0f); // Tilt cam slightly
        transform.position = bike.position - (bike.forward * 5f) + Vector3.up * 2f;
        transform.rotation = Quaternion.Slerp(transform.rotation, bike.rotation * targetRotation, Time.deltaTime * panSpeed);
    }
}
