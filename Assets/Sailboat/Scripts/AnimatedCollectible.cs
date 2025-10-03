using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class AnimatedCollectible : Collectible
    {
        public float[] tickTimers = { 0.2f, 0.15f, 0.1f, 0.04f, 0.04f, 0.04f };

        float currentTimer = 0;
        float currentTime = 0f;
        bool isStaying;
        bool finihed = false;

        protected override void Start()
        {
            base.Start();
            onExited.AddListener(AreaExited);
        }

        protected override void AreaStayed(Entity entity)
        {
            if (!G.Instance.player.entityIsPlayer(entity))
                return;
            isStaying = true;
        }

        protected virtual void AreaExited(Entity entity)
        {
            if (!G.Instance.player.entityIsPlayer(entity))
                return;
            Reset();
        }


        private void Reset()
        {
            isStaying = false;
            currentTimer = 0;
            currentTime = 0f;
            updateAccumulatedTime(out _);
        }

        private void Update()
        {
            if (isStaying && !finihed)
            {
                currentTime += Time.deltaTime;
                if(currentTime >= currentTimer)
                {
                    int i;
                    updateAccumulatedTime(out i);
                    AudioController.Instance.PlaySoundFlat("drop_002",0.6f);
                    if (i == tickTimers.Length - 1)
                    {
                        Collect();
                        finihed = true;
                    }
                    else
                        tickAnimation();
                }
            }
        }

        private void tickAnimation()
        {
            ParticlesManager.Instance.splashParticles(transform.position);
        }

        private void updateAccumulatedTime(out int currentTimerIndex)
        {
            float sum = 0f;
            for (currentTimerIndex = 0; currentTimerIndex < tickTimers.Length; currentTimerIndex++)
            {
                sum += tickTimers[currentTimerIndex];
                if (sum > currentTime)
                {
                    currentTimer = sum;
                    break;
                }
            }

        }
    }
}