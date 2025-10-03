using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Localization;
using TMPro;
using UnityEngine.Localization.Components;

namespace Sailboat
{

    public class TransitionScene : MonoBehaviour
    {
        public LocalizedString[] transitionTexts;
        public GameObject buttonPressHint;
        public LocalizeStringEvent textEvent;
        const int lastTransition = 5;
        const float minTime = 2f;
        bool okToPressButton;
        bool allowedToContinue;
        SimpleTimer initialTimer;
        public static int transitionIndex = 0;
        public static readonly string SCENE_NAME = "Transition General";

        private void Awake()
        {
            Debug.Assert(transitionIndex < transitionTexts.Length);
            textEvent.StringReference = transitionTexts[transitionIndex];
        }

        IEnumerator Start()
        {
            initialTimer = new SimpleTimer(minTime);
            AsyncOperation op;
            if (transitionIndex == lastTransition)
            {
                op = SceneManager.LoadSceneAsync("Ending");
            }
            else
            {
                string sceneName = $"Level {transitionIndex}";
                op = SceneManager.LoadSceneAsync(sceneName);
            }
            op.allowSceneActivation = false;
            TransitionUI.Instance.FadeOut();
            yield return new WaitUntil(() => Time.timeSinceLevelLoad > 1f && okToPressButton);
            buttonPressHint.SetActive(true);
            yield return new WaitUntil(() => allowedToContinue);
            TransitionUI.Instance.FadeIn();
            yield return new WaitForSeconds(0.5f);
            op.allowSceneActivation = true;
        }

        private void Update()
        {
            if (initialTimer != null && initialTimer.tick(Time.deltaTime))
            {
                okToPressButton = true;
                initialTimer = null;
            }
        }

        public void OnContinue(InputValue value)
        {
            if(okToPressButton)
                allowedToContinue = true;
        }
    }
}