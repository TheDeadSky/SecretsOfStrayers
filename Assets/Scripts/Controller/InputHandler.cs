using UnityEngine;


namespace Controller
{
    [RequireComponent(typeof(PlayerStates))]
    public class InputHandler : MonoBehaviour
    {

        Vector2 inputDir;

        PlayerStates playerStates;

        void Start()
        {
            playerStates = GetComponent<PlayerStates>();
        }

        void Update()
        {
            if (GameMenuActions.GamePaused)
                return;
            if (Input.GetKeyDown(KeyCode.F))
                playerStates.ViewModeSwitch();

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                playerStates.SwitchAlwaysRun();
            }

            playerStates.SwitchSprinting(Input.GetKey(KeyCode.LeftShift));

            if (Input.GetKeyDown(KeyCode.Space))
                playerStates.Jump(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

            playerStates.Crouch((bool)Input.GetKey(KeyCode.LeftControl));

            if (Input.GetKeyDown(KeyCode.C))
                playerStates.SwitchAlwaysCrouch();

            playerStates.CameraLook();
        }

        void FixedUpdate()
        {
            if (GameMenuActions.GamePaused)
                return;
            playerStates.MoveInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        }
    }
}
