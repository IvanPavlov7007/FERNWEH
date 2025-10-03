using UnityEngine;
using UnityEngine.InputSystem;

namespace Sailboat
{
    public class DebugInputController : MonoBehaviour
    {
        PlayerInput playerInput;
        public bool debugMode = false;
        public Vector2 moveValue;
        public event System.Action onChangeVehicle;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if(debugMode)
                playerInput.SwitchCurrentActionMap("Debug Mode");
        }

        public void OnDebugMode(InputValue value)
        {
#if UNITY_EDITOR
            if (value.isPressed)
            {
                playerInput.SwitchCurrentActionMap("DebugMode");
                Debug.Log("DEBUG MODE ON");
                debugMode = true;
            }
#endif
        }

        public void OnChangeVehicle(InputValue value)
        {
            if (value.isPressed)
            {
                onChangeVehicle?.Invoke();
            }
        }

        public void OnNormalMode(InputValue value)
        {
            if (value.isPressed)
            {
                playerInput.SwitchCurrentActionMap("Sailing");
                Debug.Log("Normal MODE ON");
                debugMode = false;
            }
        }

        public void OnMovement(InputValue value)
        {
            moveValue = value.Get<Vector2>();
        }
    }
}