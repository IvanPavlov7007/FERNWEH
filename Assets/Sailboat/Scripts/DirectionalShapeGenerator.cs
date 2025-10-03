using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DirectionalShapeGenerator : MonoBehaviour
{
    [Header("Shape Settings")]
    public int segments = 64;
    public float radius = 5f;
    public Vector2 direction = Vector2.zero; // (0,0) = circle
    [Range(-1f, 1f)] public float bias = 0f; // -1 = origin-heavy, 1 = tip-heavy

    private Mesh mesh;

    void Update()
    {
        GenerateShape();
    }

    void GenerateShape()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[segments + 2]; // center + segments + loop close
        int[] triangles = new int[segments * 3];

        // Center point
        vertices[0] = Vector3.zero;

        Vector2 dir = direction.normalized;
        float angleToDir = Mathf.Atan2(dir.y, dir.x);

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = t * Mathf.PI * 2f;

            float angleDiff = Mathf.DeltaAngle(Mathf.Rad2Deg * angle, Mathf.Rad2Deg * angleToDir);
            float weight = Mathf.Cos(Mathf.Deg2Rad * angleDiff);
            weight = Mathf.Clamp01((weight + 1f) / 2f); // remap to [0,1]

            // Apply bias shaping
            if (bias > 0)
                weight = Mathf.Pow(weight, 1f / (1f + bias));
            else if (bias < 0)
                weight = Mathf.Pow(weight, 1f - bias);

            float r = radius * weight;
            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;

            vertices[i + 1] = new Vector3(x, y, 0f);

            // Triangle fan
            if (i < segments)
            {
                triangles[i * 3 + 0] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
