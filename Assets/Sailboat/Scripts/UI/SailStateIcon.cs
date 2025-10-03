using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

namespace Sailboat
{
    public class SailStateIcon : MonoBehaviour
    {
        public Sprite open, close;
        public Image img;
        [SerializeField]
        float showingDuration = 0.4f;
        [SerializeField]
        float showTime = 2f;
        [SerializeField]
        float hideTime = 1f;

        ITransparencyController transp;

        private void Awake()
        {
            transp = Transparency.GetController(img);
            transp.Alpha = 0f;
        }
        bool lastStateUp = false;

        private void Start()
        {
            lastStateUp = G.Instance.playerInputController.sailsUp;
        }

        private void Update()
        {
            bool currentStateUp = G.Instance.playerInputController.sailsUp;
            if (currentStateUp && !lastStateUp)
            {
                img.sprite = open;
                ShowIcon();
            }
            else if ( !currentStateUp && lastStateUp)
            {
                img.sprite = close;
                ShowIcon();
            }

            lastStateUp = currentStateUp;
        }

        TweenBase fadeIn,fadeOut;

        private void ShowIcon()
        {
            if(fadeIn != null)
            {
                fadeIn.Stop();
            }

            if(fadeOut != null)
            {
                fadeOut.Stop();
            }
            fadeIn = Tween.Value(0f, 1f, x => transp.Alpha = x, showingDuration, 0f);
            fadeOut = Tween.Value(1f, 0f, x => transp.Alpha = x, hideTime, showTime + showingDuration);
        }
    }
}