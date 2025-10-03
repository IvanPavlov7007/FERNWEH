using System.Collections;
using UnityEngine;
using Pixelplacement;
using TMPro;
using System;
using UnityEngine.Localization.Components;

namespace Sailboat.Tutorial
{
    public class TutorialManager : Singleton<TutorialManager>
    {
        public Tutorial currentTutorial;
        [SerializeField]
        TextMeshProUGUI textMesh;
        [SerializeField]
        LocalizeStringEvent textEvent;

        ITransparencyController textTransparency;

        public event Action<TutorialAction> actionStarted;
        public event Action<TutorialAction> actionEnded;


        private bool currentNodeSkipped = false;
        private bool allowedToProceed = true;

        private void Awake()
        {
            textTransparency = Transparency.GetController(
                textMesh.GetComponentInParent<CanvasGroup>());
            textTransparency.Alpha = 0f;
        }

        private void OnEnable()
        {
            if(TutorialInputController.Instance != null)
                TutorialInputController.onSkip += onSkip;
        }

        private void OnDisable()
        {
            if(TutorialInputController.Instance != null)
                TutorialInputController.onSkip -= onSkip;
        }

        private void onSkip()
        {
            currentNodeSkipped = true;
        }

        public Coroutine Play()
        {
            return StartCoroutine(process(currentTutorial));
        }

        IEnumerator process(Tutorial tutorial)
        {
            GameManager.Instance.TutorialOn(true);
            for (int i = 0; i < tutorial.nodes.Length; i++)
            {
                yield return processNodeStart(tutorial,tutorial.nodes[i]);
                yield return waitUntilSkipOrTime(tutorial.nodes[i].shownTime);
                yield return processNodeEnd(tutorial,tutorial.nodes[i]);
                yield return new WaitForSeconds(tutorial.transitionTime);
            }
            GameManager.Instance.TutorialOn(false);
        }

        private IEnumerator waitUntilSkipOrTime(float time)
        {
            currentNodeSkipped = false;
            SimpleTimer timer = new SimpleTimer(time);
            while (!timer.tick(Time.deltaTime) && !currentNodeSkipped)
            {
                yield return null;
            }
        }

        private IEnumerator processNodeStart(Tutorial tutorial, TutorialNode node)
        {
            if(node.stringReference != null)
            {
                textEvent.StringReference = node.stringReference;
                Tween.Value(0f, 1f, x => textTransparency.Alpha = x, tutorial.textTransitionTime, 0f);
            }

            if ((node.actions & TutorialAction.OverviewCamera) != 0)
            {
                actionStarted?.Invoke(TutorialAction.OverviewCamera);
            }

            if ((node.actions & TutorialAction.showHint) != 0)
            {
                actionStarted?.Invoke(TutorialAction.showHint);
            }

            yield return null;
        }

        private IEnumerator processNodeEnd(Tutorial tutorial, TutorialNode node)
        {
            if (node.stringReference != null)
            {
                Tween.Value(1f, 0f, x => textTransparency.Alpha = x, tutorial.textTransitionTime, 0f);
            }
            if ((node.actions & TutorialAction.OverviewCamera) != 0)
            {
                actionEnded?.Invoke(TutorialAction.OverviewCamera);
            }

            if ((node.actions & TutorialAction.showHint) != 0)
            {
                actionEnded?.Invoke(TutorialAction.showHint);
            }

            yield return null;
        }

    }
}