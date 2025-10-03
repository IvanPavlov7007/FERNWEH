using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Sailboat.UI
{
    public class SoundSettings : MonoBehaviour
    {
        Slider slider;
        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            slider.value = AudioController.Instance.GetMasterVolume();
            slider.onValueChanged.AddListener(valueChanged);
        }

        void valueChanged(float value)
        {
            AudioController.Instance.SetMasterVolume(value);
        }
    }
}