using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class Level2 : LevelScene
    {
        [SerializeField]
        ReachTarget reachTarget;
        protected override IEnumerator Start()
        {
            reachTarget.areaReached += x => targetReached();
            ArrowHintsManager.Instance.CreatePersistentArrowHint(reachTarget.transform, ArrowHintType.Help, Color.green);
            yield return base.Start();
        }

        void targetReached()
        {
            playTargetReachedSound();
            Destroy(reachTarget.gameObject);
            Run.After(0.5f, () => StartCoroutine(MoveToTheNextScene()));
        }
    }
}