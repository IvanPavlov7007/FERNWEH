using System.Collections;
using UnityEngine;
using Sailboat;


namespace Assets.Sailboat.Scripts.Controllers.Realistic
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HingeJoint2D))]
    public class Sail : MonoBehaviour
    {
        [Header("Tuning")]
        public float maxAngle = 90f;                // Absolute maximum allowed
        public float tighteningSpeed = 45f;         // deg/sec tighten/loosen
        [Tooltip("Offset so 0 joint angle aligns with your desired neutral (e.g. 180 if sail sprite points down)")]
        public float referenceAngleOffset = 0f;
        public float windSpeedToForce = 100f;

        [Tooltip("Force application point in sail local space")]
        public Vector2 localCenter = Vector2.down;

        [Header("Control")]
        public ISailController sailController;

        [Header("Debug")]
        [SerializeField] private float currentMaxAngle = 45f;
        [SerializeField] private bool _lineTaut;
        [SerializeField] private float currentAngle; // deg, positive CCW from reference
        public bool lineTaut => _lineTaut;

        private Rigidbody2D rb;
        private HingeJoint2D hinge;
        private Rigidbody2D baseBody;
        private Boat boat;
        private SailCanvasVisuals visuals;

        // Small hysteresis to avoid flickering near the limit
        private const float TautEpsilonDeg = 0.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            hinge = GetComponent<HingeJoint2D>();
            baseBody = hinge.connectedBody;
            boat = transform.parent.GetComponentInChildren<Boat>();
            visuals = GetComponentInChildren<SailCanvasVisuals>();

            // Interpolation helps perceived smoothness if the camera updates in Update()
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            // Ensure we use joint limits rather than forcing rotation
            hinge.useLimits = true;

            // Initialize currentMaxAngle within bounds
            currentMaxAngle = Mathf.Clamp(currentMaxAngle, 0f, maxAngle);
        }
        private void Update()
        {
            if (sailController != null)
            {
                if (sailController.Tightening)
                {
                    currentMaxAngle = Mathf.Max(0f, currentMaxAngle - tighteningSpeed * Time.deltaTime);
                    currentMaxAngle = Mathf.Min(currentMaxAngle, Mathf.Abs(currentAngle));
                }
                else if (sailController.Loosening)
                {
                    currentMaxAngle = Mathf.Min(maxAngle, currentMaxAngle + tighteningSpeed * Time.deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            // Compute current relative joint angle (deg): positive CCW
            // We want an angle centered around our chosen reference
            float rawJointAngle = hinge.jointAngle; // angle of rb relative to connectedBody
            currentAngle = Mathf.DeltaAngle(0f, rawJointAngle - referenceAngleOffset);

            // Drive joint limits to clamp motion without fighting physics
            var limits = hinge.limits;
            limits.min = -currentMaxAngle + referenceAngleOffset;
            limits.max =  currentMaxAngle + referenceAngleOffset;
            hinge.limits = limits;
            hinge.useLimits = true;

            // Decide taut state with a bit of hysteresis
            float upperBound = currentMaxAngle - TautEpsilonDeg;
            float lowerBound = -currentMaxAngle + TautEpsilonDeg;
            bool atLimit = currentAngle > upperBound || currentAngle < lowerBound;
            _lineTaut = atLimit;

            // Sail facing direction in world space (unit)
            // If your sprite's "forward" is down at neutral, referenceAngleOffset should account for that.
            Vector2 sailDir = transform.up; // local up in world space
            // If your sail "faces down" by default, you can flip here instead:
            // sailDir = -transform.up;

            // Normal to sail surface (90 deg CCW)
            Vector2 normal = new Vector2(-sailDir.y, sailDir.x);

            // Force application point in world space
            Vector2 center = (Vector2)transform.TransformPoint(localCenter);

            // Apparent wind at the force application point
            Vector2 windVelocity = WindManager.Instance.getWindSpeed(center);
            Vector2 pointVelocity = rb.GetPointVelocity(center);
            Vector2 apparentWind = windVelocity - pointVelocity;

            // Project wind onto sail normal and scale
            Vector2 windToSailForce = Vector2.Dot(apparentWind, normal) * normal * windSpeedToForce;

            // Apply at the chosen point for realistic torque
            rb.AddForceAtPosition(windToSailForce, center);
            boat.windToSailForce = windToSailForce;
            visuals.UpdateVisuals(Vector2.Dot(apparentWind, normal), apparentWind.normalized, shown: true);
        }
    }

    public interface ISailController
    {
        bool Tightening { get; }
        bool Loosening { get; }
    }
}