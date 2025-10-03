using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Pixelplacement;

namespace Sailboat
{
    public class TutorialInputController : Singleton<TutorialInputController>
    {
        public static event Action onSkip;

        public void OnSkip(InputValue value)
        {
            if (value.isPressed)
            {
                onSkip?.Invoke();
            }
        }
    }
}