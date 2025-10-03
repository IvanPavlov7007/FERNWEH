using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class SimpleRaftController : VehicleController
    {
        protected override void pushVisuals()
        {
            var direction = (Vector2)GameCursor.Instance.transform.position - rb.position;
            playPaddleSound();
            ParticlesManager.Instance.directedSplashParticles(rb.position, -direction.normalized,0.2f);
        }

        protected override void push()
        {
            var direction = (Vector2)GameCursor.Instance.transform.position - rb.position;
            rb.AddForce(direction.normalized * pushForce);

        }
    }
}