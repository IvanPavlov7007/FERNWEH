using System.Collections;
using UnityEngine;

namespace Sailboat
{
    [RequireComponent(typeof(StayArea))]
    public class WindChangeTrigger : MonoBehaviour
    {
        [Header("Target Wind")]
        public Vector2 targetWind = Vector2.right;
        [Tooltip("If true, adds targetWind to the current wind instead of replacing it.")]
        public bool relative = false;

        [Header("Transition")]
        [Min(0f)] public float transitionTime = 1f;
        public AnimationCurve easing = null;

        [Header("Behavior")]
        public bool triggerOnce = false;
        public bool revertOnExit = false;

        StayArea stayArea;
        Vector2? previousWind;
        bool triggered = false;

        // Use this for initialization
        void Awake()
        {
            stayArea = GetComponent<StayArea>();
        }

        private void OnEnable()
        {
            stayArea.onStay.AddListener(OnStay);
            stayArea.onExited.AddListener(OnExit);
        }

        private void OnDisable()
        {
            stayArea.onStay.RemoveListener(OnStay);
            stayArea.onExited.RemoveListener(OnExit);
        }

        private void OnStay(Entity entity)
        {
            if (triggered && triggerOnce) return;
            if (!Player.Instance.entityIsPlayer(entity)) return;

            var constantWind = WindManager.Instance.constantWind;
            if (constantWind == null) return;

            previousWind = constantWind.CurrentWind;

            Vector2 desired = relative ? constantWind.CurrentWind + targetWind : targetWind;
            constantWind.ChangeWind(desired, transitionTime, easing);

            triggered = true;
        }

        private void OnExit(Entity entity)
        {
            if (!revertOnExit) return;
            if (!Player.Instance.entityIsPlayer(entity)) return;

            var constantWind = WindManager.Instance.constantWind;
            if (constantWind == null) return;

            if (previousWind.HasValue)
            {
                constantWind.ChangeWind(previousWind.Value, transitionTime, easing);
                previousWind = null;
            }
            triggered = false;
        }
    }
}