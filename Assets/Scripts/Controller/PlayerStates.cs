using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controller
{
    public enum ViewMode { FPerson, TPerson };

    public class PlayerStates : MonoBehaviour
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
        bool alwaysRun = true;
        bool alwaysCrouch = false;
        bool sprinting = false;

        Rigidbody rb;

        public void SwitchAlwaysCrouch() { alwaysCrouch = !alwaysCrouch; }
        public void SwitchAlwaysRun() { alwaysRun = !alwaysRun; }

        [Header("DEBUG")]
        public Vector3 movementDirectSpeed;

        void Start()
        {
            animator = GetComponent<Animator>();
            cameraFPT = Camera.main.transform;

            TPCam.GetComponent<Camera>().enabled = false;

            rb = GetComponent<Rigidbody>();
        }

        void MovementAnimation(Vector2 inputDir)
        {
            float inputY = 0;
            float inputX = 0;

            if (inputDir.y > 0)
            {
                inputY = ((alwaysRun) ? 1 : 0.5f) * inputDir.magnitude;
            }
            else if (inputDir.y < 0)
            {
                inputY = ((alwaysRun) ? -1 : -0.5f) * inputDir.magnitude;
            }

            if (inputDir.x > 0)
            {
                inputX = (alwaysRun) ? 1 : 0.5f * inputDir.magnitude;
            }
            else if (inputDir.x < 0)
            {
                inputX = (alwaysRun) ? -1 : -0.5f * inputDir.magnitude;
            }

            inputY = (sprinting && !alwaysCrouch) ? 1.5f * inputDir.magnitude : inputY;

            animator.SetFloat("InputY", inputY, speedSmoothTime, Time.deltaTime);
            animator.SetFloat("InputX", inputX, speedSmoothTime, Time.deltaTime);
        }

        public void SwitchSprinting(bool s)
        {
            sprinting = s;
            
            if(s == true)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref speedSmoothVelocity, Time.deltaTime);
            }
            else if(alwaysRun && !alwaysCrouch)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed - 3, ref speedSmoothVelocity, Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed - 5, ref speedSmoothVelocity, speedSmoothTime);
            }
        }

        public void MoveInput(Vector2 inputDir)
        {
            Vector3 movement = new Vector3(inputDir.x, 0, inputDir.y);

            if (isGrounded)
            {
                movementDirectSpeed = transform.position + transform.TransformDirection(movement) * currentSpeed * Time.deltaTime;
                rb.MovePosition(movementDirectSpeed);
            }

            MovementAnimation(inputDir);
        }

        public void CameraLook()
        {
            float targetRotation = cameraFPT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        public void Jump(Vector2 inputDir)
        {
            if(isGrounded)
            {
                if(inputDir != Vector2.zero)
                    rb.AddForce(transform.TransformDirection(inputDir.x, 1, inputDir.y) * currentSpeed * 3000);
                else
                    rb.AddForce(transform.up * 10000);
                animator.SetTrigger("Jump");
            }
        }

        public void ViewModeSwitch()
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

        public void Crouch(bool c)
        {
            if (c || alwaysCrouch)
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
            foreach (ContactPoint con in collision.contacts)
            {
                if (Vector3.Angle(con.normal, Vector3.up) < maxSlape)
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

