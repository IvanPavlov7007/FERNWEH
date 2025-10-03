using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody2D player;

    [SerializeField]
    private float strength;

    [SerializeField]
    AnimationCurve ClosingDistanceToTargetAnimation;

    [Tooltip("To read only")]
    public Vector2 target;

    Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        float height = cam.orthographicSize * 2f;
        float width = cam.aspect * height;

        // Normalization from world coordinates to screen sized grid coordinates
        Vector2Int playerQuadrant = Vector2Int.RoundToInt(player.position / new Vector2(width, height));
        
        target = playerQuadrant * new Vector2(width, height);

        MoveToTarget();
    }

    Vector2 currentTarget, lastTarget;
    float t;
    private void MoveToTarget()
    {
        if(target != currentTarget)
        {
            lastTarget = currentTarget;
            currentTarget = target;
            t = 0f;
        }
        t += Time.deltaTime;
        transform.position = lastTarget + (currentTarget - lastTarget) * ClosingDistanceToTargetAnimation.Evaluate(t);
    }
}
