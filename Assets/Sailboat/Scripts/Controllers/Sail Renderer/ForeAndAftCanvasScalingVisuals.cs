using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class ForeAndAftCanvasScalingVisuals : SailCanvasVisuals
    {
        public Transform canvasTransform;

        public float maxWindForce = 2000f;
        public float maxScale;
        public float minScale;
        public override void UpdateVisuals(float wind_sail_dot, Vector2 windDirection, Vector2 sailUp, bool shown)
        {
            base.UpdateVisuals(wind_sail_dot, windDirection, sailUp, shown);
            float scale_y = maxScale *// how to inverse lerp -1 to 1:
                (Mathf.InverseLerp(-maxWindForce, maxWindForce, wind_sail_dot) * 2f - 1f);
            if (Vector2.Dot(sailUp, transform.up) < 0f)
                scale_y *= -1f;

            transform.localScale = new Vector3(1f, -scale_y, 1f);
        }
    }
}