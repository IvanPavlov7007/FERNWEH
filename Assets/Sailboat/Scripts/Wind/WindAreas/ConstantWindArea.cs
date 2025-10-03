using UnityEngine;

namespace Sailboat
{
    public class ConstantWindArea : WindArea
    {
        public Vector2 direction;
        public bool localSpace = false;

        public override Vector2 getWindSpeed(Vector2 worldPos)
        {
            if (localSpace)
                return transform.TransformDirection(direction);
            return direction;
        }

        public void setWindDirection(Vector2 dir)
        {
            direction = dir;
        }
}
}