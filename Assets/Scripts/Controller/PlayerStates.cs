using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controller
{
    public enum ViewMode { FPerson, TPerson };

    public class PlayerStates : MonoBehaviour
    {

        public GameObject FPCam { get { return fpCam; } }
        public GameObject TPCam { get { return tpCam; } }

        private Transform target;

        private float force = 1000f;
        private float maxSpeed = 5f;
        private float maxSlape = 70f;

        private ViewMode viewMode;

        //private readonly bool IsGrounded;

        [SerializeField] private GameObject fpCam, tpCam; // game objects of first person and third person cameras

        private float turnSmoothTime = 0.2f;
        private float turnSmoothVelocity;

        private float currentSpeed, speedSmoothVelocity;
        private float speedSmoothTime = 0.3f;
        private float crouchSmoothTime = 0.3f;

        private Animator animator;

        private bool isGrounded = true;
        private bool alwaysRun = true;
        private bool alwaysCrouch = false;
        private bool sprinting = false;

        private Rigidbody rb;

        [SerializeField] private Vector3 movementDirectSpeed;

        public void SwitchAlwaysCrouch() { alwaysCrouch = !alwaysCrouch; }
        public void SwitchAlwaysRun() { alwaysRun = !alwaysRun; }


        private void Start()
        {
            animator = GetComponent<Animator>();

            tpCam.GetComponent<Camera>().enabled = false;

            rb = GetComponent<Rigidbody>();
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
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed - (maxSpeed * .3f), ref speedSmoothVelocity, Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed - (maxSpeed * .5f), ref speedSmoothVelocity, speedSmoothTime);
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
            float targetRotation = fpCam.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        public void Jump(Vector2 inputDir)
        {
            if (isGrounded)
            {
                if (inputDir != Vector2.zero)
                {
                    rb.AddForce(transform.TransformDirection(inputDir.x, 1, inputDir.y) * 13000);
                }
                else
                    rb.AddForce(transform.up * 13000);
                animator.SetTrigger("Jump");
                Debug.Log(transform.TransformDirection(inputDir.x, 1, inputDir.y) * currentSpeed * 3000);
                Debug.Log($"Current Speed {currentSpeed}");
            }
        }

        public void ViewModeSwitch()
        {
            switch (viewMode)
            {
                case ViewMode.FPerson:
                    viewMode = ViewMode.TPerson;
                    fpCam.GetComponent<Camera>().enabled = false;
                    tpCam.GetComponent<Camera>().enabled = true;
                    break;
                case ViewMode.TPerson:
                    viewMode = ViewMode.FPerson;
                    fpCam.GetComponent<Camera>().enabled = true;
                    tpCam.GetComponent<Camera>().enabled = false;
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

        private void MovementAnimation(Vector2 inputDir)
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

        private void OnCollisionStay(Collision collision)
        {
            foreach (ContactPoint con in collision.contacts)
            {
                if (Vector3.Angle(con.normal, Vector3.up) < maxSlape)
                {
                    isGrounded = true;
                    animator.SetBool("Grounding", true);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            isGrounded = false;
            animator.SetBool("Jump", isGrounded);
            animator.SetBool("Grounding", false);
        }
    }
}

