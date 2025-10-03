using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class SailCanvasSpriteScaling : SailCanvasVisuals
    {
        public Transform canvasTransform;

        public float maxWindForce = 2000f;
        public float maxScale;
        public float minScale;

        /// <summary>
        /// Updates visuals based on how strong the force applied to the sail
        /// </summary>
        /// <param name="wind_sail_dot"> The force applied to the sail </param>
        public override void UpdateVisuals(float wind_sail_dot, Vector2 windDirection, Vector2 sailUp, bool shown)
        {
            base.UpdateVisuals(wind_sail_dot, windDirection, sailUp, shown);
            float scale_y = maxScale *// how to inverse lerp -1 to 1:
                (Mathf.InverseLerp(-maxWindForce, maxWindForce, wind_sail_dot) * 2f - 1f);
            transform.localScale = new Vector3(1f, scale_y, 1f);
            transform.localPosition = Vector2.up * scale_y * 0.5f;
        }
    }

    public abstract class SailCanvasVisuals : MonoBehaviour
    {
        public virtual void UpdateVisuals(float wind_sail_dot, Vector2 windDirection, Vector2 sailUp = default, bool shown = true)
        {
            gameObject.SetActive(shown);
        }
    }
}