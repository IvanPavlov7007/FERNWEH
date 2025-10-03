using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using Pixelplacement;

namespace Sailboat
{
    public class EndScene : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI hint;
        [SerializeField]
        string nextScene;


        bool allowKeyPress, keyPressed;
        private IEnumerator Start()
        {
            var op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            ITransparencyController transparencyController = Transparency.GetController(hint);
            transparencyController.Alpha = 0f;
            TransitionUI.Instance.FadeOut();
            yield return new WaitForSeconds(2f);
            Tween.Value(0f, 1f, x => transparencyController.Alpha = x, 2f, 0f);
            allowKeyPress = true;
            yield return new WaitUntil(()=>keyPressed);
            TransitionUI.Instance.FadeIn();
            yield return new WaitForSeconds(0.5f);
            op.allowSceneActivation = true;
        }
        public void OnContinue(InputValue value)
        {
            if (allowKeyPress)
            {
                if (value.isPressed)
                {
                    keyPressed = true;
                }
            }
        }
    }
}
