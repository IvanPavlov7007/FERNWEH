using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class CollectiblesRespawner : MonoBehaviour
    {
        [SerializeField]
        GameObject collectilePrefab;
        public bool spawnOnAwake;
        public float respawnTime;
        public Collectible currentCollectible;

        SimpleTimer respawnTimer;

        private void Awake()
        {
            if (spawnOnAwake)
                Spawn();
        }

        public void Spawn()
        {
            currentCollectible = 
                Instantiate(collectilePrefab, transform.position, Quaternion.identity, transform)
                .GetComponent<Collectible>();
            currentCollectible.onCollected.AddListener(onCollected);
        }

        void onCollected(Collectible collectible)
        {
            collectible.onCollected.RemoveListener(onCollected);
            respawnTimer = new SimpleTimer(respawnTime);
        }

        private void Update()
        {
            if (respawnTimer != null && respawnTimer.tick(Time.deltaTime))
            {
                respawnTimer = null;
                Spawn();
            }
        }

    }
}