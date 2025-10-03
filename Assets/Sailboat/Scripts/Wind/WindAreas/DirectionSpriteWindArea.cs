using System.Collections;
using UnityEngine;

namespace Sailboat
{
    public class DirectionSpriteWindArea : WindArea
    {
        [SerializeField]
        SpriteRenderer spriteRenderer;
        public float magnification = 10f;

        private void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public override Vector2 getWindSpeed(Vector2 worldPos)
        {
            Color color = GetSpriteColorAtWorldPosition(spriteRenderer, worldPos);
            Vector2 speed = new Vector2(Mathf.Lerp(-1F,1F, color.r), Mathf.Lerp(-1f,1f, color.g)) * magnification;
            return speed;
        }

        Color GetSpriteColorAtWorldPosition(SpriteRenderer renderer, Vector2 worldPos)
        {
            Sprite sprite = renderer.sprite;
            Texture2D texture = sprite.texture;

            // Convert world position to local space of the sprite
            Vector2 localPos = renderer.transform.InverseTransformPoint(worldPos);

            // Convert local position to pixel position
            Rect rect = sprite.textureRect;
            Vector2 pivot = sprite.pivot;
            float pixelsPerUnit = sprite.pixelsPerUnit;

            // Shift by pivot and scale to texture pixels
            float px = (localPos.x * pixelsPerUnit) + pivot.x;
            float py = (localPos.y * pixelsPerUnit) + pivot.y;

            // Convert to texture coordinates
            int texX = Mathf.FloorToInt(rect.x + px);
            int texY = Mathf.FloorToInt(rect.y + py);

            // Check bounds
            if (texX < 0 || texX >= texture.width || texY < 0 || texY >= texture.height)
                return Color.clear;

            return texture.GetPixel(texX, texY); // or GetPixelBilinear(u, v)
        }
    }
}