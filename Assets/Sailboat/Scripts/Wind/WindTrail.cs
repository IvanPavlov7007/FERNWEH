using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class WindTrail : MonoBehaviour
    {
        public float lifetime = 3f;
        private float age = 0f;
        private bool isDead = false;
        private float trailFadeTime = 1.5f;
        private float deathTime;

        private TrailRenderer trail;

        void Awake()
        {
            trail = GetComponent<TrailRenderer>();
        }

        public void Initialize(Vector3 pos)
        {
            transform.position = pos;
            age = 0f;
            isDead = false;

            // 🔥 Critical lines
            trail.Clear();           // clears old trail points
            trail.emitting = true;   // resume trail emission
        }

        void Update()
        {
            if (!isDead)
            {
                age += Time.deltaTime;
                if (age >= lifetime)
                {
                    isDead = true;
                    deathTime = Time.time;
                    // Stop emitting further trail
                    trail.emitting = false;
                    return;
                }

                // Move with wind
                Vector3 wind = WindManager.Instance.getWindSpeed(transform.position);
                transform.position += wind * Time.deltaTime;
            }
            else
            {
                // Wait for trail to fade completely
                if (Time.time - deathTime >= trail.time)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}