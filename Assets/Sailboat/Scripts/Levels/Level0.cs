using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class Level0 : LevelScene
    {
        [SerializeField]
        ReachTarget target;

        protected override IEnumerator Start()
        {
            target.areaReached += x => targetReached();
            ArrowHintsManager.Instance.CreatePersistentArrowHint(target.transform, ArrowHintType.Help, Color.green);
            yield return base.Start();
        }

        void targetReached()
        {
            StartCoroutine(Progression());
        }

        IEnumerator Progression()
        {
            playTargetReachedSound();
            Destroy(target.gameObject);
            yield return new WaitForSeconds(0.5f);
            yield return MoveToTheNextScene();
        }
    }
}