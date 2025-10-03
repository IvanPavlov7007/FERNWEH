using UnityEngine;

namespace Sailboat
{
    public class TargetCompass : MonoBehaviour
    {
        public Transform origin;
        public Transform target;
        public Transform UI_Arrow;

        public bool usePlayerAsOrigin = true;
        void Start()
        {
            if (usePlayerAsOrigin)
                origin = G.Instance.player.bodyTransform;
    }

        void Update()
        {
            UI_Arrow.up = (target.position - origin.position).normalized;
        }
    }
}