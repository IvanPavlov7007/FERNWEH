using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat.Misc
{
    public class GhostSailboatController : SimpleTweenPosition
    {
        [Header("Sailboat Settings")]
        [SerializeField]
        Transform body;
        [SerializeField]
        Transform sail;
        [SerializeField]
        [Tooltip("Angle-overshot relative to the body")]
        float sailCorrectionAngle = 15f;
        [SerializeField]
        [Tooltip("Angle-overshot to the perpendicular to wind")]
        float courseCorrectionAngle = 15f;
        [SerializeField]
        [Tooltip("Which direction boat should try to go in world coordinates")]
        Vector2 intendedCourse = new Vector2(1, 0);

        protected override void OnEnable()
        {
            Tween.LocalPosition(body.transform, startPos, endPos, duration, delay, curve, loop: loopType);
        }

        private void Update()
        {
            // Get normalized wind direction at the boat's position
            Vector2 windDirection = WindManager.Instance.getWindSpeed(body.transform.position).normalized;

            // Calculate intended course direction (normalized)
            Vector2 courseDir = intendedCourse.normalized;

            // Find the signed angle from wind to intended course
            float windToCourseAngle = Vector2.SignedAngle(windDirection, courseDir);

            // Determine the sign for correction: positive if intendedCourse is to the right of wind, negative if to the left
            float correctionSign = Mathf.Sign(windToCourseAngle);

            // Overshoot the course against the wind by courseCorrectionAngle in the correct direction
            float correctedCourseAngle = correctionSign * courseCorrectionAngle;

            // Rotate intendedCourse by correctedCourseAngle around Z axis
            Vector2 finalCourseDir = Quaternion.AngleAxis(correctedCourseAngle, Vector3.forward) * courseDir;

            // Set body.transform.right to look towards the final course direction
            transform.right = new Vector3(finalCourseDir.x, finalCourseDir.y, 0f);

            // Sail overshoots relative to body's right, always away from wind (same sign as windToCourseAngle)
            Vector2 bodyRight2D = new Vector2(body.transform.right.x, body.transform.right.y);
            Vector2 sailDir = Quaternion.AngleAxis(-correctionSign * sailCorrectionAngle, Vector3.forward) * bodyRight2D;

            // Set sail.transform.up to the sail direction
            sail.transform.up = new Vector3(sailDir.x, sailDir.y, 0f);
        }
    }
}