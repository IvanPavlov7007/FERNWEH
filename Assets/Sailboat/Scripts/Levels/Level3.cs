using System.Collections;
using UnityEngine;
using Sailboat.Tutorial;
using Unity.Cinemachine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

namespace Sailboat
{
    public class Level3 : LevelScene
    {
        [SerializeField]
        GameObject[] drawningPeople;
        [SerializeField]
        ReachTarget reachTarget;
        [SerializeField]
        GameObject hintBoat;
        [SerializeField]
        CinemachineCamera overviewCamera;
        [SerializeField]
        CinemachineCamera focusCamera;
        [SerializeField]
        int overviewCameraPrio = 5;
        [SerializeField]
        GameObject textAfterTutorial;

        private int initialDrawningPeopleCount;

        private void Awake()
        {
            hintBoat.SetActive(false);
        }
        protected override IEnumerator Start()
        {
            GameEvents.Instance.OnItemCollected += onItemCollected;
            initialDrawningPeopleCount = drawningPeople.Length;
            reachTarget.gameObject.SetActive(false);
            TutorialManager.Instance.actionEnded += onTutActionEnded;
            TutorialManager.Instance.actionStarted += onTutActionStarted;

            //TODO Stop inputs
            foreach (var person in drawningPeople)
            {
                ArrowHintsManager.Instance.CreatePersistentArrowHint(person.transform, ArrowHintType.Help, Color.yellow);
            }

            yield return base.Start();
            yield return tutorialSequence();
            yield return levelSequence();
        }

        void onItemCollected(string itemId, int amount)
        {
            int actualCount = 0;
            if (G.Instance.inventory.items.TryGetValue("saved_fisherman", out actualCount))
            {
                if(actualCount == initialDrawningPeopleCount)
                    allfishermenCollected = true;
            }
        }


        void onTutActionStarted(TutorialAction action)
        {
            switch (action)
            {
                case TutorialAction.showHint:
                    hintBoat.SetActive(true);
                    break;
                case TutorialAction.OverviewCamera:
                    overviewCamera.Priority.Value = overviewCameraPrio;
                    break;
            }
        }

        void onTutActionEnded(TutorialAction action)
        {
            switch (action)
            {
                case TutorialAction.showHint:
                    //hintBoat.SetActive(false);
                    break;
                case TutorialAction.OverviewCamera:
                    overviewCamera.Priority.Value = 0;
                    break;
            }
        }


        bool allfishermenCollected;
        bool targetReached;

        IEnumerator tutorialSequence()
        {
            yield return TutorialManager.Instance.Play();
            textAfterTutorial.SetActive(true);
            //TODO allow inputs
        }

        IEnumerator levelSequence()
        {
            yield return new WaitUntil(() => allfishermenCollected);
            playTargetReachedSound();
            reachTarget.areaReached += x => targetReached = true;
            reachTarget.gameObject.SetActive(true);
            ArrowHintsManager.Instance.CreatePersistentArrowHint(reachTarget.transform, ArrowHintType.Normal, Color.green);
            yield return new WaitUntil(() => targetReached);
            playTargetReachedSound();
            Destroy(reachTarget.gameObject);
            yield return new WaitForSeconds(0.5f);
            yield return MoveToTheNextScene();
        }
    }
}