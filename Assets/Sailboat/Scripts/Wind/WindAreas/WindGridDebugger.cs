using UnityEngine;
using System.Collections.Generic;

namespace Sailboat
{
    public class WindGridDebugger : MonoBehaviour
    {
        public Color gridColor = new Color(1, 1, 1, 0.1f);
        public Color activeCellColor = new Color(0, 1, 0, 0.25f);
        public bool drawOccupiedCells = true;
        public bool drawEmptyCells = false;
        public int drawRange = 100; // how many cells in each direction from origin

        private void OnDrawGizmos()
        {
            WindManager windManager = WindManager.Instance;
            if (windManager == null || WindManager.Instance.windGrid == null) return;

            float cellSize = windManager.windGridCellSize;

            Vector2 center = transform.position;

            Vector2Int min = new Vector2Int(-drawRange, -drawRange);
            Vector2Int max = new Vector2Int(drawRange, drawRange);

            for (int x = min.x; x <= max.x; x++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    Vector2 worldPos = (Vector2)cell * cellSize;

                    Rect rect = new Rect(worldPos, Vector2.one * cellSize);

                    bool isOccupied = windManager.windGrid.TryGetValue(cell, out var list) && list.Count > 0;

                    if (isOccupied && drawOccupiedCells)
                    {
                        Gizmos.color = activeCellColor;
                        Gizmos.DrawCube(rect.center, rect.size);
                    }
                    else if (!isOccupied && drawEmptyCells)
                    {
                        Gizmos.color = gridColor;
                        Gizmos.DrawWireCube(rect.center, rect.size);
                    }
                }
            }
        }
    }
}