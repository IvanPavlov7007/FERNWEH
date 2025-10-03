using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System.Linq;

namespace Sailboat
{
    [RequireComponent(typeof(ConstantWind))]
    public class WindManager : Singleton<WindManager>
    {
        public LayerMask windLayerMask = 6; //: WindTrigger
        public float windGridCellSize = 100f;

        public Dictionary<Vector2Int, List<WindArea>> windGrid = new();

        [Header("Constant Wind")]
        public bool useConstantWind = true;
        [Header("Debug")]
        public bool logNoAreaPosition = false;

        public ConstantWind constantWind;
        Vector2Int WorldToGrid(Vector2 worldPos)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPos.x / windGridCellSize), Mathf.FloorToInt(worldPos.y / windGridCellSize));
        }

        public void RegisterWindArea(WindArea area)
        {
            var collider = area.GetComponent<Collider2D>();
            if (collider == null) return;

            Bounds bounds = collider.bounds;

            Vector2Int minCell = WorldToGrid(bounds.min);
            Vector2Int maxCell = WorldToGrid(bounds.max);

            for (int x = minCell.x; x <= maxCell.x; x++)
            {
                for (int y = minCell.y; y <= maxCell.y; y++)
                {
                    Vector2 worldCenter = new Vector2(x + 0.5f, y + 0.5f) * windGridCellSize;
                    //if (collider.OverlapPoint(worldCenter))
                    if (Physics2D.OverlapBoxAll(worldCenter, Vector2.one * windGridCellSize, 0f, windLayerMask).Contains(collider))
                    {
                        Vector2Int key = new Vector2Int(x, y);
                        if (!windGrid.ContainsKey(key))
                        {
                            windGrid[key] = new List<WindArea>();
                        }
                        windGrid[key].Add(area);
                    }
                }
            }
        }

        public Vector2 getWindSpeed(Vector2 worldPosition)
        {
            if (useConstantWind)
                return constantWind.CurrentWind;
            else
                return getWindSpeedFromAreas(worldPosition);
            
        }

        public Vector2 getWindSpeedFromAreas(Vector2 worldPosition)
        {
            var key = WorldToGrid(worldPosition);

            if (windGrid.TryGetValue(key, out var areasInCell))
            {
                var overlappingAreas = areasInCell.Where(a => a.OverlapPoint(worldPosition));
                if (!overlappingAreas.Any())
                {
                    return LogNoAreaAtPos(worldPosition, key);
                }
                int maxPrio = overlappingAreas.Max(a => a.priority);
                var blendAreas = overlappingAreas.Where(a => a.priority == maxPrio).ToArray();

                Vector2 targ = Vector2.zero;
                if (blendAreas.Length > 0)
                {
                    foreach (var wind in blendAreas)
                    {
                        targ += wind.getWindSpeed(worldPosition);
                    }
                    //Maybe use interpolation instead
                    targ /= blendAreas.Length;
                }
                return targ;
            }
            else
            {
                return LogNoAreaAtPos(worldPosition, key);
            }
        }

        private Vector2 LogNoAreaAtPos(Vector2 worldPosition, Vector2Int cell)
        {
            if(logNoAreaPosition)
                Debug.Log("No area at position: " + worldPosition.ToString() + " Cell: " + cell);
            return Vector2.zero;
        }
    }
}