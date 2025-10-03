using System.Collections;
using UnityEngine;


namespace Sailboat
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SailCanvasMeshVisuals : SailCanvasVisuals
    {
        public float halfBalkLength = 1f;
        public float maxDisplayedWindForce = 2000f;
        public float maxFilledArea = 8f;

        Mesh mesh;
        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            CreatePlaceholderQuad();
        }

        void CreatePlaceholderQuad()
        {
            Vector3[] vertices = new Vector3[4]
            {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
            };

            int[] triangles = new int[6]
            {
            0, 2, 1,
            2, 3, 1
            };

            Vector2[] uv = new Vector2[4]
            {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        public override void UpdateVisuals(float windSailDot, Vector2 windDirection, Vector2 sailUp, bool shown)
        {
            base.UpdateVisuals(windSailDot, windDirection, sailUp, shown);
            Vector2 sailDirection = transform.up;
            Vector2 sailLeft = -transform.right;
            Vector2 sailRight = transform.right;

            float nDot = Vector2.Dot(sailDirection.normalized, windDirection.normalized);

            Vector3[] verticies = new Vector3[4];

            if (Mathf.Approximately(nDot, 0f))
            {
                redrawSail(verticies); //zero positions
                return;
            }

            float biggerEdgeportion = 1f / (1f + nDot);
            float smallerEdgePortion = biggerEdgeportion * nDot;
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

            if(nDot >= 0)
            {
                verticies[0] = new Vector3(halfBalkLength, 0f);
                verticies[3] = new Vector3(-halfBalkLength, 0f);

                verticies[1] = verticies[0] + transform.InverseTransformDirection(windDirection) * rightPortion * edgeLength;
                verticies[2] = verticies[3] + transform.InverseTransformDirection(windDirection) * leftPortion * edgeLength;
            }
            else
            {
                verticies[0] = new Vector3(-halfBalkLength, 0f);
                verticies[3] = new Vector3(halfBalkLength, 0f);

                verticies[1] = verticies[0] + transform.InverseTransformDirection(windDirection) * rightPortion * edgeLength;
                verticies[2] = verticies[3] + transform.InverseTransformDirection(windDirection) * leftPortion * edgeLength;
            }
            redrawSail(verticies);
        }

        float combinedEdgesLength(float windForce)
        {
            float interpolatedArea = Mathf.InverseLerp(0f, maxDisplayedWindForce, Mathf.Abs(windForce)) * maxFilledArea;
            return interpolatedArea / halfBalkLength;
        }

        void redrawSail(Vector3[] positions)
        {
            mesh.vertices = positions;
            mesh.RecalculateBounds();
        }

    }
}