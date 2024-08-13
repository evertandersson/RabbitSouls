using Level;
using UnityEngine;
using Util;

namespace Pathfinding {
    [System.Serializable]
    public class PathNode {
        public Vector2Int Position { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;
        public PathNode Parent { get; private set; }

        public PathNode(Vector2Int position, float g, float h, PathNode parent) => (Position, G, H, Parent) = (position, g, h, parent);

        public void Update(float g, float h, PathNode parent) => (G, H, Parent) = (g, h, parent);

        public void DrawGizmo(GridManager grid) {
            Vector3 gridPosition = new Vector3(Position.x, 0, Position.y) - grid.WorldOffset + grid.CellSize / 2f;
            ColoredGizmos.DrawCube(gridPosition, Vector3.one * 0.5f, Color.yellow);
        }
    }
}
