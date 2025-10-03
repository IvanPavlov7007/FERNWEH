using UnityEngine;

namespace Sailboat
{
    public class WalkModeEnabler : MonoBehaviour
    {
        MonoBehaviour controller;
        Rigidbody2D[] rigidbodies2D;
        public float movementSpeed = 10f;

        private void Start()
        {
            rigidbodies2D = transform.parent.GetComponentsInChildren<Rigidbody2D>();

            controller = transform.parent.gameObject.GetComponentInChildren<VehicleController>();
        }

        public void MoveBody(Vector2 newPosition)
        {
            foreach (var rb in rigidbodies2D)
            {
                rb.position = newPosition;
            }
        }

        private void FixedUpdate()
        {
            var input = G.Instance.debugInputController;

            if (input.debugMode)
            {
                controller.enabled = false;

                MoveBody(rigidbodies2D[0].position + input.moveValue * movementSpeed * Time.fixedDeltaTime);

            }
            else
            {
                controller.enabled = true;
            }
        }
    }
}
