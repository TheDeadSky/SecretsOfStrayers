using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMouseController : MonoBehaviour
{
    [Range(1, 10)]
    public float mouseSensitivity = 10;

    public Transform target;

    [Range(-4, 4)]
    public float distFromTarget = 2;


    Vector2 pitchClamp = new Vector2(-40, 85);
    //Vector2 yawClamp = new Vector2(-60, 60);

    public bool enableYawClamp = false;

    float yaw, pitch;

    public float rotationSmoothTime = .6f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);

        //if (Input.GetKey(KeyCode.LeftAlt) && enableYawClamp)
        //{
        //    yaw = Mathf.Clamp(yaw, yawClamp.x, yawClamp.y);
        //}

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distFromTarget;
    }
}
