using UnityEngine;
using UnityEngine.UI;

namespace Sailboat
{
    [ExecuteAlways]
    public class MotorManager : MonoBehaviour
    {
        public PlayerInputController sailboatInput;
        public Slider motorSlider;

        private void OnValidate()
        {
            if (sailboatInput == null)
            {
                sailboatInput = FindFirstObjectByType<PlayerInputController>();
            }

            if (motorSlider == null)
            {
                motorSlider = GameObject.Find("Motor Slider").GetComponent<Slider>();
            }

            if(motorSlider != null && sailboatInput != null)
            {
                motorSlider.onValueChanged.AddListener(sailboatInput.applyMotor);
            }
            else
            {
                if (motorSlider == null)
                    Debug.LogError(this.name + " couldn't find motor slider");

                if(sailboatInput == null)
                    Debug.LogError(this.name + " couldn't find sailboat input");
            }
        }
    }
}
