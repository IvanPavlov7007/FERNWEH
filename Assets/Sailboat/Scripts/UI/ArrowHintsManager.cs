using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Pixelplacement;

namespace Sailboat
{
    public enum ArrowHintType { Normal, Help}
    public class ArrowHintsManager : Singleton<ArrowHintsManager>
    {
        [SerializeField]
        GameObject arrowHintPrefab;

        public Dictionary<Transform, ArrowHint> arrowHints = new Dictionary<Transform, ArrowHint>();

        public ArrowHint CreatePersistentArrowHint(Transform target, ArrowHintType arrowHintType, Color color)
        {
            var destroyTracker = DestroyTracker.GetTracker(target.gameObject);
            var arrow = Instantiate(arrowHintPrefab, GameCanvas.Instance.transform).GetComponent<ArrowHint>();
            arrow.transform.SetSiblingIndex(0);
            arrow.target = target;
            arrow.origin = G.Instance.player.bodyTransform;
            arrow.ChangeColor(color);
            arrow.canvas = GameCanvas.Instance.canvas;

            arrowHints.Add(target, arrow);
            destroyTracker.destroyed += onTargetDestroyed;

            return arrow;
        }

        void onTargetDestroyed(DestroyTracker destroyTracker)
        {
            if (arrowHints.TryGetValue(destroyTracker.transform, out var arrow))
            {
                if (arrow != null) // Unity destroyed objects check
                {
                    Destroy(arrow.gameObject);
                }
                arrowHints.Remove(destroyTracker.transform);
            }
            destroyTracker.destroyed -= onTargetDestroyed;
        }
    }
}