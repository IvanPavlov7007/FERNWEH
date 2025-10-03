using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class Collectible : StayArea
    {
        [Header("Item")]
        public string itemId;
        public int amount = 1;

        public UnityEngine.Events.UnityEvent<Collectible> onCollected;

        protected override void Start()
        {
            base.Start();
            onStay.AddListener(AreaStayed);
        }

        protected virtual void AreaStayed(Entity entity)
        {
            if (Player.Instance.entityIsPlayer(entity))
            {
                Collect();
            }
        }

        protected virtual void Collect()
        {
            // Trigger global GameEvent
            G.Instance.inventory.AddItem(itemId, amount,true);

            AudioController.Instance.PlaySoundFlat("drop_004", 0.6f);

            // Trigger UnityEvent for inspector-driven reactions (sound, VFX)
            onCollected?.Invoke(this);

            ParticlesManager.Instance.splashParticles(transform.position, 0.5f);

            // Destroy or disable collectible
            Destroy(gameObject);
        }
    }
}