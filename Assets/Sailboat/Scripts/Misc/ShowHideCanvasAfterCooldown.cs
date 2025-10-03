using System.Collections;
using UnityEngine;
using Pixelplacement;
using TMPro;

namespace Assets.Sailboat.Scripts.Misc
{
    public class ShowHideCanvasAfterCooldown : MonoBehaviour
    {
        public float transitionTime;
        public float shownTime;

        CanvasGroup canvas;
        ITransparencyController transparencyController;
        private void OnEnable()
        {
            canvas = GetComponentInChildren<CanvasGroup>();
            transparencyController = Transparency.GetController(canvas);

            Tween.Value(0f, 1f, x => transparencyController.Alpha = x, transitionTime, 0f);
            Tween.Value(1f, 0f, x => transparencyController.Alpha = x, transitionTime, shownTime + transitionTime);
        }
    }
}