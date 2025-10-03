using UnityEngine;

namespace Sailboat
{

    public class RaftWithSailController : SimpleRaftController
    {
        public float maxSailRotation = 1f;

        [Space]
        public float windSpeedToAccel = 100f;

        private void FixedUpdate()
        {
            SailFixedUpdate();
        }

        private void SailFixedUpdate()
        {
            Vector2 sailUpDir = sailHingeJoint.transform.up;

            Vector2 windAccel = G.Instance.windManager.getWindSpeed(sailHingeJoint.transform.position) * windSpeedToAccel;

            float wind_sail_dot = Vector2.Dot(windAccel, sailUpDir);
            Vector2 wind_sail_proj = wind_sail_dot * sailUpDir;

            if (sailsUp)
            {
                //Applying force directly onto the sail
                sailHingeJoint.attachedRigidbody.AddForce(wind_sail_proj);
            }
            sailRb.AddTorque(calculateTorque());
            

            canvasVisuals.UpdateVisuals(wind_sail_dot, windAccel.normalized, shown: sailsUp);
        }

        float calculateTorque()
        {
            //return input.deltaSailRotate * maxSailRotation;
            Vector2 sailPos = sailRb.position;
            Vector2 cursorPos = GameCursor.Instance.transform.position;

            Vector2 toCursor = cursorPos - sailPos;

            // Desired angle (in degrees)
            float targetAngle = Mathf.Atan2(toCursor.y, toCursor.x) * Mathf.Rad2Deg + 90f;

            // Current angle (in degrees)
            float currentAngle = sailRb.rotation;

            // Smallest angular difference
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);

            // Apply torque proportional to difference
            float torque = angleDiff * maxSailRotation; // rotationStrength = tunable gain
            return torque;
        }
    }
}