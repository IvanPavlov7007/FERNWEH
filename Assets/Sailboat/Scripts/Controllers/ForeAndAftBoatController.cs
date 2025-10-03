using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class ForeAndAftBoatController : VehicleController
    {
        [Space]
        public bool useMotor = false;
        public float motorRange = 5f;
        public float maxSteering = 20f;
        public float maxSailRotation = 1f;

        [Space]
        [Tooltip("Speed at which boat turns most easily")]
        public float maxTurningSpeed = 10f;

        [Space]
        public float windSpeedToAccel = 100f;
        public float liftSlope, dragBase, dragFactor, apparentSpeedLimit;

        private void FixedUpdate()
        {

            if (useMotor)
            {
                rb.AddForce(transform.up * motorRange * input.motor);
            }
            

            Vector2 velocity = rb.linearVelocity;
            
            SailFixedUpdate(velocity);

            float vel_magnitude = velocity.magnitude;
            float angular_velocity = rb.angularVelocity;

            //from 0 to 1
            float velocity_ranged = Mathf.InverseLerp(0f, maxTurningSpeed, vel_magnitude) + 0.1f;

            rb.AddTorque(input.steering * maxSteering * velocity_ranged);


        }

        //private void SailFixedUpdate(Vector2 boat_velocity)
        //{
        //    Vector2 sailDir = sailHingeJoint.transform.right;  // sail’s chord direction (edge to edge)
        //    Vector2 apparentWind = G.Instance.windManager.getWindSpeed(sailHingeJoint.transform.position) - boat_velocity;

        //    float apparentSpeed = Mathf.Min(apparentSpeedLimit, apparentWind.magnitude);
        //    if (apparentSpeed < 0.01f) return;

        //    Vector2 awDir = apparentWind.normalized;

        //    // ---- Compute Lift + Drag on Sail ----
        //    // Angle between sail and apparent wind
        //    float angleOfAttack = Vector2.SignedAngle(sailDir, awDir) * Mathf.Deg2Rad;

        //    // Simplified lift/drag coefficients (tweak for feel)
        //    float CL = liftSlope * Mathf.Sin(2 * angleOfAttack); // lift curve
        //    float CD = dragBase + dragFactor * (1 - Mathf.Cos(angleOfAttack));

        //    float q = apparentWind.sqrMagnitude; // dynamic pressure proxy

        //    Vector2 liftDir = new Vector2(-apparentWind.y, apparentWind.x); // 90° rotated
        //    if (angleOfAttack < 0) liftDir = -liftDir;   // flip if AoA negative

        //    Vector2 lift = liftDir.normalized * (CL * q * windSpeedToAccel);
        //    Vector2 drag = -apparentWind.normalized * (CD * q * windSpeedToAccel);

        //    Vector2 totalForce = lift + drag;
        //    // Apply to boat
        //    sailHingeJoint.attachedRigidbody.AddForce(totalForce);
        //    sailRb.AddTorque(input.deltaSailRotate * maxSailRotation);

        //    canvasVisuals.UpdateVisuals(CL, awDir);
        //}

        //private void SailFixedUpdate(Vector2 boat_velocity)
        //{
        //    Vector2 sailUpDir = sailHingeJoint.transform.up;

        //    Vector2 windSpeed = G.Instance.windManager.getWindSpeed(sailHingeJoint.transform.position);
        //    Vector2 apparentWind = windSpeed - boat_velocity;

        //    Vector2 windAccel = apparentWind * windSpeedToAccel;

        //    float wind_sail_dot = Vector2.Dot(windAccel, sailUpDir);
        //    Vector2 wind_sail_proj = wind_sail_dot * sailUpDir;
        //    Vector2 sail_boat_proj = Vector2.Dot(wind_sail_proj, transform.up) * transform.up;

        //    //Applying force directly onto the sail
        //    sailHingeJoint.attachedRigidbody.AddForce(sail_boat_proj);
        //    sailRb.AddTorque(input.deltaSailRotate * maxSailRotation);

        //    canvasVisuals.UpdateVisuals(wind_sail_dot, windAccel.normalized);
        //}

        private void SailFixedUpdate(Vector2 boat_velocity)
        {
            Vector2 sailUpDir;
            Vector2 sailRightDir = sailHingeJoint.transform.right;

            Vector2 windSpeed = G.Instance.windManager.getWindSpeed(sailHingeJoint.transform.position);
            Vector2 apparentWind = windSpeed - boat_velocity;

            Vector2 apparentWindAccel = apparentWind * windSpeedToAccel;
            Vector2 windAccel = windSpeed * windSpeedToAccel;

            float wind_sail_dot = Vector2.Dot(apparentWindAccel, sailRightDir);
            if (wind_sail_dot > 0f)
            {
                sailUpDir = Quaternion.Euler(0f,0f, 
                    Mathf.Sign(Vector2.SignedAngle(apparentWind, sailRightDir)) * 90f)
                    * sailRightDir;
            }
            else
            {
                sailUpDir = Quaternion.Euler(0f, 0f,
                    Mathf.Sign(Vector2.SignedAngle(windSpeed, sailRightDir)) * 90f)
                    * sailRightDir;
                wind_sail_dot = Vector2.Dot(windAccel, sailUpDir);
            }

            Vector2 wind_sail_proj = wind_sail_dot * sailUpDir;
            Debug.DrawRay(sailHingeJoint.transform.position, wind_sail_proj * 0.01f);

            Vector2 sail_boat_proj = Vector2.Dot(wind_sail_proj, transform.up) * transform.up;

            //Applying force directly onto the sail
            if (sailsUp)
            {
                sailHingeJoint.attachedRigidbody.AddForce(sail_boat_proj);
            }
            sailRb.AddTorque(input.deltaSailRotate * maxSailRotation);
            canvasVisuals.UpdateVisuals(wind_sail_dot, apparentWindAccel.normalized, sailUpDir, shown: sailsUp);
        }
    }
}