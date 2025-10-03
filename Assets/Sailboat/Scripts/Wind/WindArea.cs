using UnityEngine;

namespace Sailboat
{

    public class WindArea : MonoBehaviour
    {
        public int priority = 0;
        Collider2D collider;

        private void Awake()
        {
            WindManager.Instance.RegisterWindArea(this);
            collider = GetComponent<Collider2D>();
        }

        public virtual bool OverlapPoint(Vector2 worldPos)
        {
            return collider.OverlapPoint(worldPos);
        }

        public virtual Vector2 getWindSpeed(Vector2 position)
        {
            return Vector2.zero;
        }
    }
}
