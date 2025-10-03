using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sailboat.UI
{
    public class Minimap_UI : MonoBehaviour
    {
        public RawImage minimapImage;

        private void OnEnable()
        {
            MinimapManager.Instance.MinimapUpdated += onMinimapUpdate;
            Redraw();

        }

        private void OnDisable()
        {
            if (MinimapManager.Instance == null)
                return;
            MinimapManager.Instance.MinimapUpdated -= onMinimapUpdate;
        }

        private void Redraw()
        {
            minimapImage.texture = MinimapManager.Instance.minimap.texture;
        }

        private void onMinimapUpdate()
        {
            Redraw();
        }
    }
}