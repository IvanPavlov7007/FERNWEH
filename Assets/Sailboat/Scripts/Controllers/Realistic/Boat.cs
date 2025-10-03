using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Sailboat.Scripts.Controllers.Realistic
{
    public class Boat : MonoBehaviour
    {
        [Tooltip("The speed at which the boat can turn at full speed")]
        public float maxTurningSpeed = 10f;
        [Tooltip("The maximum steering torque applied to the boat")]
        public float maxSteering = 1000f;
        public float sailToBoatForceMultiplier = 100f;
        public float backwardsMultiplier = 0.1f;
        public IBoatController boatController;
        public Vector2 windToSailForce { get; set; } //set by sail

        Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            Vector2 velocity = rb.linearVelocity;
            float vel_magnitude = velocity.magnitude;
            float angular_velocity = rb.angularVelocity;

            //from 0 to 1
            float velocity_ranged = Mathf.InverseLerp(0f, maxTurningSpeed, vel_magnitude) + 0.1f;
            //float idealTorque = boatController.steering * maxSteering * velocity_ranged;
            //float currentTorque = angular_velocity * rb.inertia;
            //rb.AddTorque(idealTorque - currentTorque);
            rb.AddTorque(boatController.steering * maxSteering * velocity_ranged);

            //Reduce sideways velocity
            Vector2 right = Quaternion.Euler(0f, 0f, rb.rotation - 90f) * Vector2.up;
            right.Normalize();
            rb.linearVelocity -= Vector2.Dot(rb.linearVelocity, right) * right * 0.9f;

            Vector2 up = transform.up;
            Vector2 sail_boat_proj = Vector2.Dot(windToSailForce, up) * up;
            if(sail_boat_proj.magnitude < 0f)
                sail_boat_proj *= backwardsMultiplier;
            rb.AddForce(sail_boat_proj * sailToBoatForceMultiplier);
        }
    }

    public interface IBoatController
    {
        float steering { get; }
    }
}