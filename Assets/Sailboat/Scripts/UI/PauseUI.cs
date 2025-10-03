using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PauseUI : Singleton<PauseUI>
    {
        CanvasGroup group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            group.alpha = 1f;
            group.interactable = true;
        }

        public void Hide()
        {
            group.alpha = 0f;
            group.interactable = false;
        }
    }
}