using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Sailboat
{
    public class GradientMovingWindArea : WindArea
    {
        [Header("Wind Movement")]
        public SplineContainer windMovementSpline;
        public float scrollSpeed;
        [Range(0f, 1f)] public float scrollValue;
        public bool closed = false;
        public bool localSpace = false;

        [Header("Wind Gradient")]
        public List<GradientWind> gradientWinds;

        [Header("Gizmo Settings")]
        public float multiplyDirectionLength = 10f;
        public float multiplySphereSize = 1f;
        public int resolution = 60;
        public bool logCoordinates = false;

        private void Update()
        {
            scrollValue = Mathf.Repeat(scrollValue + CalculateScrollSpeedFromWorldSpeed(scrollSpeed, windMovementSpline) * Time.deltaTime, 1f);
            UpdateCurrentPositions();
        }

        public override Vector2 getWindSpeed(Vector2 worldPos)
        {
            Vector3 localPlayerPos = windMovementSpline.transform.InverseTransformPoint(worldPos);
            float splineT;
            float3 point;
            var res = SplineUtility.GetNearestPoint(windMovementSpline.Spline, localPlayerPos, out point, out splineT);

#if UNITY_EDITOR
            if (logCoordinates)
            {
                Debug.Log(res.ToString() + " " + splineT.ToString());
                Debug.DrawLine(worldPos, windMovementSpline.transform.TransformPoint(point));
            }
#endif
            //float adjustedT = Mathf.Repeat(splineT + scrollValue, 1f);

            return InterpolateWindAt(splineT);
        }

        public static float CalculateScrollSpeedFromWorldSpeed(float worldSpeed, SplineContainer spline)
        {
            if (spline == null || spline.Spline == null)
                return 0f;

            float length = spline.Spline.GetLength();
            if (length <= 0f) return 0f;

            return worldSpeed / length;
        }

        private void OnValidate()
        {
            UpdateCurrentPositions();
        }

        private void UpdateCurrentPositions()
        {
            if (gradientWinds == null) return;

            for (int i = 0; i < gradientWinds.Count; i++)
            {
                gradientWinds[i] = new GradientWind
                {
                    windSpeed = gradientWinds[i].windSpeed,
                    position = gradientWinds[i].position,
                    currentPosition = Mathf.Repeat(gradientWinds[i].position + scrollValue, 1f)
                };
            }

            gradientWinds.Sort((a, b) => a.currentPosition.CompareTo(b.currentPosition));
        }

        private Vector2 InterpolateWindAt(float t)
        {
            if (gradientWinds == null || gradientWinds.Count == 0)
                return Vector2.zero;

            // Edge cases: before first or after last
            GradientWind first = gradientWinds[0];
            GradientWind last = gradientWinds[gradientWinds.Count - 1];

            if (t <= first.currentPosition)
            {
                if (closed)
                    return getWorldWindSpeed(Vector3.Slerp(last.windSpeed, first.windSpeed, 
                        InverseLerpWrapped(last.currentPosition, first.currentPosition, t)));
                else
                    return getWorldWindSpeed(first.windSpeed);
            }
            if (t >= last.currentPosition)
            {
                if (closed)
                    return getWorldWindSpeed(Vector3.Slerp(last.windSpeed, first.windSpeed,
                        InverseLerpWrapped(last.currentPosition, first.currentPosition, t)));
                else
                    return getWorldWindSpeed(last.windSpeed);
            }
            // In-between
            for (int i = 0; i < gradientWinds.Count - 1; i++)
            {
                GradientWind a = gradientWinds[i];
                GradientWind b = gradientWinds[i + 1];

                if (t >= a.currentPosition && t <= b.currentPosition)
                {
                    float localT = Mathf.InverseLerp(a.currentPosition, b.currentPosition, t);
                    return getWorldWindSpeed( Vector3.Slerp(a.windSpeed, b.windSpeed, localT));
                }
            }

            return getWorldWindSpeed( gradientWinds[0].windSpeed);
        }

        private Vector2 getWorldWindSpeed(Vector2 localWind)
        {
            if (localSpace)
                return transform.TransformDirection(localWind);
            return localWind;
        }

        private static float InverseLerpWrapped(float min, float max, float value, float wrapMax = 1f)
        {
            // Normalize everything to [0, wrapMax)
            min = Mathf.Repeat(min, wrapMax);
            max = Mathf.Repeat(max, wrapMax);
            value = Mathf.Repeat(value, wrapMax);

            float range = Mathf.Repeat(max - min + wrapMax, wrapMax);
            float offset = Mathf.Repeat(value - min + wrapMax, wrapMax);

            return offset / range;
        }

        private void OnDrawGizmos()
        {
            if (windMovementSpline == null || gradientWinds == null) return;

            Gizmos.color = Color.cyan;

            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                float scrolledT = Mathf.Repeat(t + scrollValue, 1f);
                Vector3 localPos = windMovementSpline.Spline.EvaluatePosition(scrolledT);
                Vector3 worldPos = windMovementSpline.transform.TransformPoint(localPos);

                Vector2 wind = InterpolateWindAt(scrolledT);

                Gizmos.DrawLine(worldPos, worldPos + new Vector3(wind.x, wind.y, 0f) * multiplyDirectionLength);
                Gizmos.DrawSphere(worldPos, wind.magnitude * multiplySphereSize);
            }
        }

        [System.Serializable]
        public struct GradientWind
        {
            public Vector2 windSpeed;
            [Range(0f, 1f)] public float position; // original position (0-1)
            [HideInInspector] public float currentPosition; // calculated with scroll
        }
    }
}
