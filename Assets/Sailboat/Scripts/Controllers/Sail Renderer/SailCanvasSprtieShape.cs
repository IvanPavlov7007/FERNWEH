using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

namespace Sailboat
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class SailCanvasSprtieShape : SailCanvasVisuals
    {
        [SerializeField]
        SpriteShapeController spriteShapeController;

        public float halfBalkLength = 1f;
        public float maxDisplayedWindForce = 2000f;
        public float maxFilledArea = 8f;
        public float partsPow = 2f;
        public float zeroDotTolerance = 0.02f;

        Spline spline;

        private void Start()
        {
            spline = spriteShapeController.spline;
        }

        private void OnDestroy()
        {
            spline = null;
        }

        private void setShowed(bool showed)
        {
            gameObject.SetActive(showed);
        }

        public override void UpdateVisuals(float windSailDot, Vector2 windDirection, Vector2 sailUp, bool shown)
        {
            base.UpdateVisuals(windSailDot, windDirection, sailUp, shown);
            if (!shown)
                return;
            Vector2 sailDirection = transform.up;
            Vector2 sailLeft = -transform.right;
            Vector2 sailRight = transform.right;

            float nDot = Vector2.Dot(sailDirection.normalized, windDirection.normalized);
            float nDotPositive = Mathf.Abs(Mathf.Pow(nDot, partsPow));

            Vector3[] verticies = new Vector3[4];
            Vector3[] tangents = new Vector3[2];

            if (-zeroDotTolerance < nDot && nDot < zeroDotTolerance)
            {
                setShowed(false);
                return;
            }
            else
            {
                setShowed(true);
            }

            float biggerEdgeportion = 1f / (1f + nDotPositive);
            float smallerEdgePortion = biggerEdgeportion * nDotPositive;
            float leftPortion = 0f;
            float rightPortion = 0f;

            if (Vector2.Dot(sailLeft, windDirection) >= Vector2.Dot(sailRight, windDirection))
            {
                leftPortion = biggerEdgeportion;
                rightPortion = smallerEdgePortion;
            }
            else
            {
                leftPortion = smallerEdgePortion;
                rightPortion = biggerEdgeportion;
            }

            float edgeLength = combinedEdgesLength(windSailDot);

            verticies[0] = new Vector3(halfBalkLength, 0f);
            verticies[3] = new Vector3(-halfBalkLength, 0f);

            verticies[1] = verticies[0] + transform.InverseTransformDirection(windDirection) * rightPortion * edgeLength;
            verticies[2] = verticies[3] + transform.InverseTransformDirection(windDirection) * leftPortion * edgeLength;

            tangents[0] = transform.InverseTransformDirection(windDirection) * rightPortion * edgeLength;
            tangents[1] = transform.InverseTransformDirection(windDirection) * leftPortion * edgeLength;

            redrawSail(verticies, tangents);
        }

        float combinedEdgesLength(float windForce)
        {
            float interpolatedArea = Mathf.InverseLerp(0f, maxDisplayedWindForce, Mathf.Abs(windForce)) * maxFilledArea;
            return interpolatedArea / halfBalkLength;
        }

        void redrawSail(Vector3[] positions, Vector3[] tangents)
        {
            if (spline == null)
                return;
            for (int i = 0; i < positions.Length; i++)
            {
                try
                {
                    spline.SetPosition(i, positions[i]);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            try {
                spline.SetTangentMode(1, ShapeTangentMode.Broken);
                spline.SetTangentMode(2, ShapeTangentMode.Broken);
                spline.SetRightTangent(1, tangents[0]);
                spline.SetLeftTangent(2, tangents[1]);
            }
            catch (System.Exception ex){ }
        }
    }
}