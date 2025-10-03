using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class SquareRigBoatController : VehicleController
    {
        [Space]
        public bool useMotor = false;
        public float motorRange = 5f;
        public float maxSteering = 20f;
        public float maxSailRotation = 1f;
        public float backwardsMultiplier = 0.3f;

        [Space]
        [Tooltip("Speed at which boat turns most easily")]
        public float maxTurningSpeed = 10f;

        [Space]
        public float windSpeedToAccel = 100f;
        private void FixedUpdate()
        {

            if (useMotor)
            {
                rb.AddForce(transform.up * motorRange * input.motor);
            }

            SailFixedUpdate();

            Vector2 velocity = rb.linearVelocity;
            float vel_magnitude = velocity.magnitude;
            float angular_velocity = rb.angularVelocity;

            //from 0 to 1
            float velocity_ranged = Mathf.InverseLerp(0f, maxTurningSpeed, vel_magnitude) + 0.1f;

            rb.AddTorque(input.steering * maxSteering * velocity_ranged);


        }

        private void SailFixedUpdate()
        {
            Vector2 sailUpDir = sailHingeJoint.transform.up;

            Vector2 windAccel = G.Instance.windManager.getWindSpeed(sailHingeJoint.transform.position) * windSpeedToAccel;

            float wind_sail_dot = Vector2.Dot(windAccel, sailUpDir);
            Vector2 wind_sail_proj = wind_sail_dot * sailUpDir;
            float sail_boat_dot = Vector2.Dot(wind_sail_proj, transform.up);
            Vector2 sail_boat_proj = sail_boat_dot * transform.up;

            if (sailsUp)
            {
                if (sail_boat_dot < 0f)
                    sailHingeJoint.attachedRigidbody.AddForce(sail_boat_proj * backwardsMultiplier);
                else
                    sailHingeJoint.attachedRigidbody.AddForce(sail_boat_proj);
                    
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