using Debug;
using UnityEngine;
using Util;

namespace Pathfinding
{
    public class GridManager : MonoBehaviour {

        [SerializeField] private Vector2Int mapSize = Vector2Int.one * 25;
        [SerializeField] private LayerMask blockMask;
        [SerializeField, Range(0.01f, 1f)] private float blockAccuracy = 0.99f;
        [SerializeField] private bool keepPresetTiles;

        [Header("Debug")]
        [SerializeField] private bool[] tiles;

        private Transform planeTransform;

        public Vector3 CellSize => new(1, 0 ,1);

        public Vector3 WorldOffset => new (mapSize.x / 2f, 0, mapSize.y / 2f);

        public Vector2Int MapSize => mapSize;

        public bool TileIsBlocked(Vector2Int gridPos) {
            if (!TryPosToIndex(gridPos, out int index)) {
                return true;
            }

            if(index < 0 || index >= tiles.Length)
                return true;

            return tiles[index];
        }

        public bool TileIsBlocked(Vector3 worldPos) {
            return TileIsBlocked(WorldToGridPos(worldPos));
        }

        private void Awake() {
            if(keepPresetTiles) {
                return;
            }

            BuildTilesFromGeometry();
        }
        public void BuildTilesFromGeometry() {
            if(planeTransform == null) {
                planeTransform = GetComponentInChildren<MeshRenderer>().transform;
            }
            planeTransform.localScale = new(mapSize.x * 0.1f, 1f, mapSize.y * 0.1f);

            tiles = new bool[mapSize.x * mapSize.y];

            for(int i = 0; i < tiles.Length; i++) {
                if(!TryIndexToPos(i, out Vector2Int pos)) {
                    SafeLogger.LogError("Index out of range", this);
                    return;
                }

                Collider[] geometry = Physics.OverlapBox(GridToWorldPos(pos) + Vector3.up, Vector3.one * (blockAccuracy * 0.5f), Quaternion.identity, blockMask);
                bool tileIsBlocked = geometry.Length > 0;
                tiles[i] = tileIsBlocked;
            }
        }
        private bool TryIndexToPos(int i, out Vector2Int pos) {
            if(i < 0 || i >= tiles.Length) {
                pos = default;
                return false;
            }

            pos = new Vector2Int(i % mapSize.x, i / mapSize.x);
            return true;
        }
        private bool TryPosToIndex(Vector2Int pos, out int index) {
            bool xOutOfBounds = pos.x < 0 || pos.x >= mapSize.x;
            bool yOutOfBounds = pos.y < 0 || pos.y >= mapSize.y;
            if(xOutOfBounds || yOutOfBounds) {
                index = default;
                return false;
            }

            index = pos.y * mapSize.x + pos.x;
            return true;
        }

        private void OnDrawGizmos() {
            Color prevGizmoColor = Gizmos.color;

            for(int z = 0; z < mapSize.y; z++) {
                for(int x = 0; x < mapSize.x; x++) {
                    Vector3 center = transform.position + new Vector3(x, 0, z) - WorldOffset + CellSize / 2;
                    ColoredGizmos.DrawWireCube(center, CellSize, Color.cyan);

                    if(tiles.Length == 0) {
                        continue;
                    }

                    if(!TryPosToIndex(new Vector2Int(x, z), out int posIndex)) {
                        continue;
                    }

                    if (posIndex >= tiles.Length)
                        continue;

                    ColoredGizmos.DrawCube(center, CellSize, tiles[posIndex] ? Color.black : Color.white);
                }
            }

            Gizmos.color = prevGizmoColor;
        }

        public bool Contains(Vector2Int position) {
            bool xInBounds = position.x >= 0 && position.x < mapSize.x;
            bool yInBounds = position.y >= 0 && position.y < mapSize.y;
            return xInBounds && yInBounds;
        }

        public Vector3 GridToWorldPos(Vector2Int v) {
            return new Vector3(v.x, transform.position.y, v.y) - WorldOffset + CellSize / 2f;
        }

        public Vector2Int WorldToGridPos(Vector3 v) {
            return (v + WorldOffset - CellSize / 2f).XZToVector2Int();
        }

        public void SetTile(Vector3 worldPos, bool isBlocked) {
            Vector2Int gridPos = WorldToGridPos(worldPos);
            if (!TryPosToIndex(gridPos, out int index)) {
                return;
            }

            tiles[index] = isBlocked;
        }
    }
}
