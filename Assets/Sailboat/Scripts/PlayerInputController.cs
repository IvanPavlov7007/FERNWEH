using UnityEngine;
using UnityEngine.InputSystem;
using Pixelplacement;

namespace Sailboat
{
    public class PlayerInputController : MonoBehaviour
    {
        public InputActionMap map;

        public float steering;
        public float motor;
        public float deltaSailRotate;
        public Vector2 cursorPosition;
        public bool sailsUp;
        public bool tightenSails;
        public bool loosenSails;
        public event System.Action pushed;

        [SerializeField]
        float marginZ = 1f;
        public void OnCursorPosition(InputValue value)
        {
            Vector2 vec2 = value.Get<Vector2>();
            Vector3 screenPosition = new Vector3(vec2.x, vec2.y, marginZ);
            cursorPosition = G.Instance.mainCam.ScreenToWorldPoint(screenPosition);
        }

        public void OnSteering(InputValue value)
        {
            steering = value.Get<float>();
        }

        public void OnSailRotate(InputValue value)
        {
            deltaSailRotate = value.Get<Vector2>().x;
        }

        public void OnSailsUp(InputValue value)
        {
            sailsUp = !sailsUp;
            if (sailsUp)
                AudioController.Instance.PlaySoundFlat("cloth2");
            else
                AudioController.Instance.PlaySoundFlat("cloth3");
        }

        public void OnPush(InputValue value)
        {
            pushed?.Invoke();
        }

        public void OnTightenSails(InputValue value)
        {
            tightenSails = value.isPressed;
        }

        public void OnLoosenSails(InputValue value)
        {
            loosenSails = value.isPressed;
        }

        public void applyMotor(float value)
        {
            motor = value;
        }
    }
}