using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Pixelplacement;
using System;

namespace Sailboat
{
    public class MinimapManager : Singleton<MinimapManager>
    {
        public MinimapCamera minimapCamera;
        public Minimap minimap;
        [SerializeField]
        GameObject miniMapIcon;

        /// <summary>
        /// transform - realObject, GameObject - icon
        /// </summary>
        Dictionary<Transform, GameObject> minimapIcons = new Dictionary<Transform, GameObject>();

        public event Action MinimapUpdated;

        private void Awake()
        {
            CreateNewMininmap();
        }

        private void CreateNewMininmap()
        {
            minimap.CreateRenderTexture();
            minimapCamera.UpdateMinimap(minimap);
            MinimapUpdated?.Invoke();
        }

        public GameObject CreatePersistentMiniMapIcon(Transform target, Color color)
        {
            var destroyTracker = DestroyTracker.GetTracker(target.gameObject);
            var icon = Instantiate(miniMapIcon, target.position,target.rotation,target);
            var sr = icon.GetComponentInChildren<SpriteRenderer>();
            sr.color = color;

            minimapIcons.Add(target, icon);
            destroyTracker.destroyed += onTargetDestroyed;

            return icon;
        }

        void onTargetDestroyed(DestroyTracker destroyTracker)
        {
            if (minimapIcons.TryGetValue(destroyTracker.transform, out var icon))
            {
                if (icon != null) // Unity destroyed objects check
                {
                    Destroy(icon.gameObject);
                }
                minimapIcons.Remove(destroyTracker.transform);
            }
            destroyTracker.destroyed -= onTargetDestroyed;
        }
    }
}