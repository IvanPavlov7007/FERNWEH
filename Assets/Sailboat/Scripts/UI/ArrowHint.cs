using UnityEngine;
using UnityEngine.UI;

namespace Sailboat 
{
    [RequireComponent(typeof(RectTransform))]
    public class ArrowHint : MonoBehaviour
    {
        public Camera cam;
        public Canvas canvas;
        public float screenEdgeMargin = 50f;
        public float targetOnScreenMargin = 50f;
        [Space]
        public Transform origin;
        public Transform target;

        RectTransform rectTransform;

        Image[] images;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            images = GetComponentsInChildren<Image>();
        }

        public void ChangeColor(Color c)
        {
            if (images == null)
                return;
            foreach (var im in images)
            {
                im.color = c;
            }
        }

        void Update()
        {

            Vector2 distanceToTarget = target.position - origin.position;
            Vector2 directionToTarget = distanceToTarget.normalized;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 marginedPos;

            cam = G.Instance.mainCam;

            if (cam == null)
            {
                Debug.LogWarning("No camera registered");
                return;
            }


            if (cameraWorldSizeRect(cam).Contains(target.position))
            {
                Vector2 screenPos = cam.WorldToScreenPoint(target.position);
                marginedPos = screenPos - directionToTarget * targetOnScreenMargin;
            }
            else
            {
                Vector2 edgePoint = GetEdgePoint(directionToTarget, screenSize);
                marginedPos = edgePoint - directionToTarget * screenEdgeMargin;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, marginedPos + screenSize * 0.5f, null, out marginedPos);

            rectTransform.anchoredPosition = marginedPos;
            rectTransform.up = directionToTarget;
        }

        public static Vector2 worldScreenHalfSize(Camera cam)
        {
            if(cam.orthographic)
            {
                float height = cam.orthographicSize;
                float width = height * cam.aspect;

                return new Vector2(width, height);
            }
            else
            {
                // For perspective, use field of view and distance
                float height = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * -cam.transform.position.z;
                float width = height * cam.aspect;
                return new Vector2(width, height);
            }
        }

        public static Rect cameraWorldSizeRect(Camera cam)
        {
            var halfSize = worldScreenHalfSize(cam);
            Rect rect = new Rect(Vector2.zero, halfSize * 2f);
            rect.center = cam.transform.position;
            return rect;
        }

        static Vector2 GetEdgePoint(Vector2 direction, Vector2 screenSize)
        {
            Vector2 screenCenter = screenSize / 2f;

            float tMax = float.MaxValue;

            // Left or Right edge
            if (direction.x != 0f)
            {
                float tx1 = -screenCenter.x / direction.x;
                float tx2 = screenCenter.x / direction.x;
                tMax = Mathf.Min(tx1 > 0f ? tx1 : float.MaxValue, tx2 > 0f ? tx2 : float.MaxValue, tMax);
            }

            // Top or Bottom edge
            if (direction.y != 0f)
            {
                float ty1 = -screenCenter.y / direction.y;
                float ty2 = screenCenter.y / direction.y;
                tMax = Mathf.Min(ty1 > 0f ? ty1 : float.MaxValue, ty2 > 0f ? ty2 : float.MaxValue, tMax);
            }

            Vector2 edgePoint = direction * tMax;
            return screenCenter + edgePoint;
        }
    }
}