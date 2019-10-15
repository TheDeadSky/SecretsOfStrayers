using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controller
{
    [RequireComponent(typeof(PlayerStates))]
    public class InputHandler : MonoBehaviour
    {

        Vector2 inputDir;

        PlayerStates pstates;

        void Start()
        {
            pstates = GetComponent<PlayerStates>();
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.F))
                pstates.ViewModeSwitch();

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                pstates.SwitchAlwaysRun();
            }

            pstates.SwitchSprinting(Input.GetKey(KeyCode.LeftShift));

            if (Input.GetKeyDown(KeyCode.Space))
                pstates.Jump(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

            pstates.Crouch((bool)Input.GetKey(KeyCode.LeftControl));

            if (Input.GetKeyDown(KeyCode.C))
                pstates.SwitchAlwaysCrouch();

            pstates.CameraLook();
        }

        void FixedUpdate()
        {
            pstates.MoveInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }
    }
}
