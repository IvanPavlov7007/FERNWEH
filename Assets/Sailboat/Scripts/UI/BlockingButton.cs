using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sailboat.UI
{
    public class BlockingButton : MonoBehaviour
    {
        public Button button;

        Action currentCallback;
        public void Show(Action onClick)
        {
            gameObject.SetActive(true);
            currentCallback = onClick;
        }

        public void clicked()
        {
            gameObject.SetActive(false);
            currentCallback();
        }
    }
}