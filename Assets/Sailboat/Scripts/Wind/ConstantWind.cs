using System;
using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class ConstantWind : MonoBehaviour
    {
        [SerializeField] Vector2 constantWind = Vector2.right;
        public float ConstantWindMultiplier = 1f;
        public Vector2 CurrentWind => constantWind * ConstantWindMultiplier;
        // Raised whenever constant wind value changes (after each step of a transition, and at the end).
        public event Action<Vector2> WindChanged;

        [Tooltip("Default easing used when transitioning constant wind.")]
        public AnimationCurve defaultEasing = AnimationCurve.Linear(0, 0, 1, 1);

        [Tooltip("Use unscaled time for constant wind transitions.")]
        public bool useUnscaledTime = false;

        Coroutine windChangeRoutine;

        public void SetWindImmediate(Vector2 newWind)
        {
            if (windChangeRoutine != null)
            {
                StopCoroutine(windChangeRoutine);
                windChangeRoutine = null;
            }
            constantWind = newWind;
            WindChanged?.Invoke(constantWind);
        }

        public void ChangeWind(Vector2 newWind, float duration, AnimationCurve easing = null)
        {
            if (duration <= 0f)
            {
                SetWindImmediate(newWind);
                return;
            }

            if (windChangeRoutine != null)
            {
                StopCoroutine(windChangeRoutine);
            }

            var curveToUse = (easing != null && easing.length > 0) ? easing : defaultEasing;
            windChangeRoutine = StartCoroutine(Co_ChangeWind(constantWind, newWind, duration, curveToUse));
        }

        IEnumerator Co_ChangeWind(Vector2 from, Vector2 to, float duration, AnimationCurve easing)
        {
            float t = 0f;
            while (t < 1f)
            {
                float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                t += dt / Mathf.Max(0.0001f, duration);
                float e = easing != null ? easing.Evaluate(Mathf.Clamp01(t)) : Mathf.Clamp01(t);

                // Use Slerp to get a nicer direction transition; falls back to Lerp if magnitudes are zero
                Vector3 f = from;
                Vector3 tt = to;
                Vector2 blended = (f.sqrMagnitude > 0.0001f && tt.sqrMagnitude > 0.0001f)
                    ? (Vector2)Vector3.Slerp(f, tt, e)
                    : Vector2.Lerp(from, to, e);

                constantWind = blended;
                WindChanged?.Invoke(constantWind);
                yield return null;
            }
            constantWind = to;
            WindChanged?.Invoke(constantWind);
            windChangeRoutine = null;
        }
    }
}