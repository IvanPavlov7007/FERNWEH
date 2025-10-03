using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sailboat
{
    public class Level4 : LevelScene
    {
        [SerializeField]
        ReachTarget[] reachTargets;
        [SerializeField]
        GameObject[] fisherMenToShow;
        [SerializeField]
        ReachTarget finalTarget;
        [SerializeField]
        string passangerItemId;

        List<ReachTarget> targetsToReach = new List<ReachTarget>();
        Dictionary<ReachTarget, GameObject> targetToFishermanMap = new Dictionary<ReachTarget, GameObject>();

        protected override IEnumerator Start()
        {
            Debug.Assert(fisherMenToShow.Length == reachTargets.Length);
            for(int i = 0; i < fisherMenToShow.Length; i++)
            {
                targetToFishermanMap.Add(reachTargets[i], fisherMenToShow[i]);
            }

            finalTarget.gameObject.SetActive(false);
            G.Instance.inventory.AddItem(passangerItemId, reachTargets.Length);

            MinimapManager.Instance.CreatePersistentMiniMapIcon(G.Instance.player.bodyTransform, Color.green);
            foreach (var target in reachTargets)
            {
                targetsToReach.Add(target);
                target.areaReached += onTargetReach;
                activateReachTarget(target, Color.yellow);
            }
            yield return base.Start();
            yield return sequence();
        }

        bool allTargetsReached = false;

        void onTargetReach(ReachTarget reachTarget)
        {
            G.Instance.inventory.RemoveItem(passangerItemId);
            targetsToReach.Remove(reachTarget);
            targetToFishermanMap[reachTarget].SetActive(true);
            Destroy(reachTarget.gameObject);
            playTargetReachedSound();
            if(targetsToReach.Count == 0)
                allTargetsReached = true;
        }

        private void activateReachTarget(ReachTarget target, Color color)
        {
            MinimapManager.Instance.CreatePersistentMiniMapIcon(target.transform, Color.yellow);
            //ArrowHintsManager.Instance.CreatePersistentArrowHint(target.transform,ArrowHintType.Normal, color);
            target.gameObject.SetActive(true);
        }

        IEnumerator sequence()
        {
            yield return new WaitUntil(()=>allTargetsReached);

            bool finalAreaReached = false;
            finalTarget.areaReached += x => finalAreaReached = true;
            activateReachTarget(finalTarget, Color.green);
            yield return new WaitUntil(()=>finalAreaReached);
            playTargetReachedSound();
            Destroy(finalTarget.gameObject);
            yield return new WaitForSeconds(0.5f);
            yield return MoveToTheNextScene();
        }
    }
}