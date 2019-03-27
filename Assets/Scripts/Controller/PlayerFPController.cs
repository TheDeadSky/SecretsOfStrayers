using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum ViewMode { FPerson, TPerson };

[RequireComponent(typeof(Animator))]
public class PlayerFPController : NetworkBehaviour
{
    public Transform target;

    public ViewMode viewMode;

    float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    float currentSpeed, speedSmoothVelocity;
    float speedSmoothTime = 0.3f;
    float crouchSmoothTime = 0.3f;

    Animator animator;

    Transform cameraFPT;

    public GameObject FPCam, TPCam;

    bool isGrounded = true;
    bool running = true;
    bool crouching = false;

    public bool IsGrounded { get {return isGrounded; } }

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraFPT = Camera.main.transform;

        if (isLocalPlayer)
        {
            TPCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            TPCam.GetComponent<Camera>().enabled = false;
            FPCam.GetComponent<Camera>().enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
	
	
	void Update ()
    {
        if (!isLocalPlayer) { return; }
        
        if (Input.GetKeyDown(KeyCode.F))
            ViewModeSwitch();

        if(Input.GetKeyDown(KeyCode.CapsLock))
        {
            running = !running;
        }

        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        MoveInput(inputDir);
        Jump();
        Crouch();

        float animationSpeedPercent = 0;

        if (inputDir.y > 0)
        {
            if (sprinting)
                animationSpeedPercent = 1.5f * inputDir.magnitude;
            else
                animationSpeedPercent = ((running) ? 1 : 0.5f) * inputDir.magnitude;
        }
        else if (inputDir.y < 0)
        {
            animationSpeedPercent = ((running) ? -1 : -0.5f) * inputDir.magnitude;
        }
        else
        {
            if(inputDir.x != 0)
                animationSpeedPercent = (running) ? 1 : 0.5f * inputDir.magnitude;
        }

        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
            crouching = !crouching;
    }

    public void JumpEvent(float jumpForce)
    {
        //GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }

    public void Grounded()
    {
        isGrounded = true;
    }

    void MoveInput(Vector2 inputDir)
    {
        float targetRotation;
        switch (viewMode)
        {
            case ViewMode.FPerson:
                targetRotation = cameraFPT.eulerAngles.y;
                if (inputDir.y > 0)
                {
                    if (inputDir.x > 0)
                        targetRotation += 45;
                    else if (inputDir.x < 0)
                        targetRotation += -45;
                }
                else if (inputDir.y < 0)
                {
                    if (inputDir.x > 0)
                        targetRotation += -45;
                    else if (inputDir.x < 0)
                        targetRotation += 45;
                }
                else
                {
                    if (inputDir.x > 0)
                        targetRotation += 90;
                    else if (inputDir.x < 0)
                        targetRotation += -90;
                }
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                break;
            case ViewMode.TPerson:
                if (inputDir != Vector2.zero)
                {
                    targetRotation = cameraFPT.eulerAngles.y;
                    if (inputDir.y > 0)
                    {
                        if (inputDir.x > 0)
                            targetRotation += 45;
                        else if (inputDir.x < 0)
                            targetRotation += -45;
                    }
                    else if (inputDir.y < 0)
                    {
                        if (inputDir.x > 0)
                            targetRotation += -45;
                        else if (inputDir.x < 0)
                            targetRotation += 45;
                    }
                    else
                    {
                        if (inputDir.x > 0)
                            targetRotation += 90;
                        else if (inputDir.x < 0)
                            targetRotation += -90;
                    }

                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                }
                break;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isLocalPlayer && isGrounded)
        {
            animator.SetBool("Jump", true);
            isGrounded = false;
        }
        if(Input.GetKeyUp(KeyCode.Space) && isLocalPlayer)
        {
            animator.SetBool("Jump", false);
        }
    }

    void ViewModeSwitch()
    {
        switch (viewMode)
        {
            case ViewMode.FPerson:
                viewMode = ViewMode.TPerson;
                FPCam.GetComponent<Camera>().enabled = false;
                TPCam.GetComponent<Camera>().enabled = true;
                break;
            case ViewMode.TPerson:
                viewMode = ViewMode.FPerson;
                FPCam.GetComponent<Camera>().enabled = true;
                TPCam.GetComponent<Camera>().enabled = false;
                break;
        }
    }

    void Crouch()
    {
        if(Input.GetKey(KeyCode.LeftControl) || crouching)
        {
            float crouch = 1 * Mathf.SmoothDamp(animator.GetFloat("Crouch"), 1f, ref crouchSmoothTime, Time.deltaTime);

            animator.SetFloat("Crouch", crouch);
        }
        else
        {
            float crouch = 1 * Mathf.SmoothDamp(animator.GetFloat("Crouch"), 0f, ref crouchSmoothTime, Time.deltaTime);

            animator.SetFloat("Crouch", crouch);
        }
    }
}

