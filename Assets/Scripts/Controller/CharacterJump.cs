using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public float jumpHeight;
    public bool isGrounded;
    public float gravityStrength;
    Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
	}
	
	void Update ()
    {
        Vector3 gravityS = new Vector3(0, gravityStrength, 0);

        Physics.gravity = gravityS;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, jumpHeight, 0));
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
