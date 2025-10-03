using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sailboat.Scripts.Controllers.Realistic
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SpriteSquareCanvasVisuals : MonoBehaviour
    {
        [System.Serializable]
        public struct BezierPoint
        {
            public Vector3 position;
            public Vector3 control1;
            public Vector3 control2;
        }

        public BezierPoint[] bezierPoints = new BezierPoint[4];

        private Mesh mesh;

        void Awake()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            UpdateMesh();
        }

        public void UpdateMesh()
        {
            // Sample Bezier curves between each point
            List<Vector3> vertices = new List<Vector3>();
            int segments = 10; // Increase for smoother curves

            for (int i = 0; i < 4; i++)
            {
                BezierPoint p0 = bezierPoints[i];
                BezierPoint p1 = bezierPoints[(i + 1) % 4];

                for (int j = 0; j <= segments; j++)
                {
                    float t = j / (float)segments;
                    Vector3 point = CalculateCubicBezierPoint(t, p0.position, p0.control2, p1.control1, p1.position);
                    vertices.Add(point);
                }
            }

            // Triangulate the quad (simple fan triangulation)
            List<int> triangles = new List<int>();
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }

            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0; //first term
            p += 3 * uu * t * c0; //second term
            p += 3 * u * tt * c1; //third term
            p += ttt * p1; //fourth term

            return p;
        }
    }
}