using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField]
        protected Rigidbody2D rb;
        [SerializeField]
        protected HingeJoint2D sailHingeJoint;
        [SerializeField]
        protected SailCanvasVisuals canvasVisuals;
        protected Rigidbody2D sailRb;

        protected PlayerInputController input;
        [Space]
        public float pushForce = 1000f;
        public float pushDuration = 0.2f;
        public float pushCooldown = 0.4f;

        private SimpleTimer pushTimer;

        [Space]
        protected bool sailsUp;

        protected virtual void Start()
        {
            //crutch TODO move sail to its own component
            if(sailHingeJoint != null)
                sailRb = sailHingeJoint.attachedRigidbody;
            input = G.Instance.playerInputController;
            input.pushed += onPush;
        }

        protected virtual void Update()
        {
            if (!GameManager.Instance.WorldInteractive)
                return;
            //checking if push cooldown is over
            if (pushTimer != null && pushTimer.tick(Time.deltaTime))
                pushTimer = null;
            sailsUp = input.sailsUp;
        }

        protected virtual void pushVisuals()
        {
            playPaddleSound();
            ParticlesManager.Instance.directedSplashParticles(transform.position, -transform.up);
        }

        protected virtual void push()
        {
            rb.AddForce(transform.up * pushForce);
            
        }

#if UNITY_EDITOR
        int d_frame = 0;
        int d_idleFrames = 36;
        string d_speed;
        public bool drawGUI = false;

        protected virtual void OnGUI()
        {
            if (!drawGUI)
                return;
            if (d_frame++ > d_idleFrames)
            {
                d_frame = 0;
                d_speed = rb.linearVelocity.magnitude.ToString();
            }

            GUI.TextArea(new Rect(0f, 0f, 200f, 100f), "speed = " + d_speed + '\n');
        }
#endif
        void onPush()
        {
            if (gameObject.activeInHierarchy && pushTimer == null)
            {
                pushTimer = new SimpleTimer(pushCooldown);
                StartCoroutine(pushing());
                pushVisuals();
            }
        }

        IEnumerator pushing()
        {
            SimpleTimer timer = new SimpleTimer(pushDuration);
            while (!timer.tick(Time.fixedDeltaTime))
            {
                push();
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        protected void playPaddleSound()
        {
            AudioController.Instance.PlaySoundFlat("water_paddle",volume: 0.3f, pitch: new AudioParams.Pitch(AudioParams.Variation.Small), randomization: new AudioParams.Randomization(true));
        }
    }
}