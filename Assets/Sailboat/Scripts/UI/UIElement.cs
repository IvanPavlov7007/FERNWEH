using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIElement : MonoBehaviour
{
    public delegate void UIElementCallback();

    Coroutine currentRoutine;

    public void showForTime(float delay, float time, bool unscaled = false, UIElementCallback callback = null)
    {

    }
}
