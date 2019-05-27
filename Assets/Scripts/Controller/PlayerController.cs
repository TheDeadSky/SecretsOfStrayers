using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Controller
{
    public enum ViewMode { FPerson, TPerson };

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public float force = 1000f;
        public float maxSpeed = 5f;
        public float maxSlape = 70f;

        public Transform target;

        public ViewMode viewMode;

        public readonly bool IsGrounded;

        public GameObject FPCam, TPCam;

        float turnSmoothTime = 0.2f;
        float turnSmoothVelocity;

        float currentSpeed, speedSmoothVelocity;
        float speedSmoothTime = 0.3f;
        float crouchSmoothTime = 0.3f;

        Animator animator;

        Transform cameraFPT;

        bool isGrounded = true;
        bool running = true;
        bool crouching = false;

        Rigidbody rb;

        Vector2 inputDir;

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

            rb = GetComponent<Rigidbody>();
        }


        void Update()
        {
            if (!isLocalPlayer) { return; }

            if (Input.GetKeyDown(KeyCode.F))
                ViewModeSwitch();

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                running = !running;
            }

            bool sprinting = Input.GetKey(KeyCode.LeftShift);

            Jump();
            Crouch();

            float inputY = 0;
            float inputX = 0;

            

            if (inputDir.y > 0)
            {
                inputY = ((running) ? 1 : 0.5f) * inputDir.magnitude;
            }
            else if (inputDir.y < 0)
            {
                inputY = ((running) ? -1 : -0.5f) * inputDir.magnitude;
            }

            if (inputDir.x > 0)
            {
                inputX = (running) ? 1 : 0.5f * inputDir.magnitude;
            }
            else if(inputDir.x < 0)
            {
                inputX = (running) ? -1 : -0.5f * inputDir.magnitude;
            }

            inputY = (sprinting && !crouching) ? 1.5f * inputDir.magnitude : inputY;

            animator.SetFloat("InputY", inputY, speedSmoothTime, Time.deltaTime);
            animator.SetFloat("InputX", inputX, speedSmoothTime, Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.C))
                crouching = !crouching;
        }

        void FixedUpdate()
        {
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            CameraLook();
            MoveInput();
        }

        public void JumpEvent(float jumpForce)
        {
            //rb.AddForce(new Vector3(0, jumpForce, 0));
        }

        void MoveInput()
        {
            Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
            if(horizontalMovement.magnitude > maxSpeed)
            {
                horizontalMovement = horizontalMovement.normalized;
                horizontalMovement *= maxSpeed;
            }

            rb.velocity = new Vector3(horizontalMovement.x, rb.velocity.y, horizontalMovement.y);

            if(isGrounded)
                rb.AddRelativeForce(inputDir.x * force, 0, inputDir.y * force);

        }

        void CameraLook()
        {
            float targetRotation = cameraFPT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(0, 10000, 0);
                animator.SetTrigger("Jump");
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
            if (Input.GetKey(KeyCode.LeftControl) || crouching)
            {
                float crouch = 1 * Mathf.SmoothDamp(animator.GetFloat("Crouch"), 1f, ref crouchSmoothTime, Time.deltaTime);
                GetComponent<CapsuleCollider>().height = 1.15f;
                animator.SetFloat("Crouch", crouch);
            }
            else
            {
                float crouch = 1 * Mathf.SmoothDamp(animator.GetFloat("Crouch"), 0f, ref crouchSmoothTime, Time.deltaTime);
                GetComponent<CapsuleCollider>().height = 1.75f;
                animator.SetFloat("Crouch", crouch);
            }
        }

        void OnCollisionStay(Collision collision)
        {
            foreach(ContactPoint con in collision.contacts)
            {
                if(Vector3.Angle(con.normal, Vector3.up) < maxSlape)
                {
                    isGrounded = true;
                }
            }
        }

        void OnCollisionExit(Collision collision)
        {
            isGrounded = false;
            animator.SetBool("Jump", isGrounded);

        }
    }
}
