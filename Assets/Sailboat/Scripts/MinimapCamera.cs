using System.Collections;
using UnityEngine;

namespace Sailboat
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class MinimapCamera : MonoBehaviour
    {
        Camera cam;
        private void OnEnable()
        {
            initialize();
        }

        private void OnValidate()
        {
            initialize();
        }

        void initialize()
        {
            cam = GetComponent<Camera>();
        }

        public void UpdateMinimap(Minimap minimap)
        {
            cam.targetTexture = minimap.texture;
            cam.cullingMask = minimap.renderLayers;
        }
    }
}