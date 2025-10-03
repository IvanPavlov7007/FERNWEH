using System.Collections;
using UnityEngine;
using System;
using System.Linq;

namespace Sailboat
{
    [CreateAssetMenu(menuName ="Game/Minimap")]
    public class Minimap : ScriptableObject
    {
        public int width;
        public int height;
        public LayerMask renderLayers;
        public RenderTexture texture;

        [ContextMenu("Recreate Render texture")]
        public void CreateRenderTexture()
        {
            texture = new RenderTexture(width, height, 24);
        }
    }
}