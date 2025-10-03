using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class WindTrailsManager : MonoBehaviour
    {
        public WindTrail particlePrefab;
        public int maxParticles = 100;
        public float spawnInterval = 0.1f;
        public float spawnMargin = 2f;

        private float spawnTimer = 0f;
        private List<WindTrail> pool = new List<WindTrail>();
        private Camera cam;

        void Start()
        {
            cam = Camera.main;

            // Prepopulate pool
            for (int i = 0; i < maxParticles; i++)
            {
                var p = Instantiate(particlePrefab);
                p.gameObject.SetActive(false);
                pool.Add(p);
            }
        }

        void Update()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnParticle();
            }
        }

        void SpawnParticle()
        {
            Vector3 spawnPos = GetRandomVisiblePosition();

            WindTrail particle = GetPooledParticle();
            if (particle != null)
            {
                particle.gameObject.SetActive(true);
                particle.Initialize(spawnPos);
            }
        }

        Vector3 GetRandomVisiblePosition()
        {
            Vector3 camPos = cam.transform.position;
            Vector2 halfSize = worldScreenHalfSize(cam);
            float height = halfSize.y * 2;
            float width = height * cam.aspect;

            float x = Random.Range(camPos.x - width / 2 - spawnMargin, camPos.x + width / 2 + spawnMargin);
            float y = Random.Range(camPos.y - height / 2 - spawnMargin, camPos.y + height / 2 + spawnMargin);

            return new Vector3(x, y, 0);
        }


        public static Vector2 worldScreenHalfSize(Camera cam)
        {
            if(cam.orthographic)
            {
                float height = cam.orthographicSize;
                float width = height * cam.aspect;

                return new Vector2(width, height);
            }
            else
            {
                // For perspective, use field of view and distance
                float height = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * -cam.transform.position.z;
                float width = height * cam.aspect;
                return new Vector2(width, height);
            }
        }

        public static Rect cameraWorldSizeRect(Camera cam)
        {
            var halfSize = worldScreenHalfSize(cam);
            Rect rect = new Rect(Vector2.zero, halfSize * 2f);
            rect.center = cam.transform.position;
            return rect;
        }

        WindTrail GetPooledParticle()
        {
            foreach (var p in pool)
            {
                if (!p.gameObject.activeSelf)
                    return p;
            }

            return null; // no free particle available
        }
    }
}