using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {

    [Range(1, 10)]
    public float mouseSensitivity = 10;

    //public Transform playerRotation;

    Vector2 pitchClamp = new Vector2(-40, 85);
    //Vector2 yawClamp = new Vector2(-85, 80);

    float yaw, pitch;
    Vector3 currentRotation;

	void LateUpdate ()
    {
        if (GameMenuActions.GamePaused)
            return;
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);
        
		Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;
	}
}
