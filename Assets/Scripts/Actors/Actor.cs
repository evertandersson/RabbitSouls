using Debug;
using Level;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Actors {
    public class Actor : MonoBehaviour {
        public event Action OnStartedMove;
        public event Action OnReachedDestination;

        [SerializeField] private float speed = 3f;
        [SerializeField] private int nodesPerMove = 3;

        [Header("Debug")]
        [SerializeField] private bool debugGeneratePath;
        [SerializeField] private Vector2Int debugTargetPos;

        private GridManager grid;
        private PathNode[] path;
        private int currentNodeIndex;

        private Vector2Int GridPosition => grid.WorldToGridPos(transform.position);

        public int NodesPerMove => nodesPerMove;

        public int NodeDistanceToTarget(Vector3 target) {
            Vector2Int startPosition = GridPosition;
            Vector2Int endPosition = grid.WorldToGridPos(target);
            return AStar.GetNodeDistance(grid, startPosition, endPosition);
        }

        private Vector3 NextPosition() {
            currentNodeIndex++;
            if(currentNodeIndex >= path.Length)
                return transform.position;

            Vector2Int gridPosition = path[currentNodeIndex].Position;
            Vector3 worldPosition = grid.GridToWorldPos(gridPosition);
            return worldPosition;
        }

        private void Awake() {
            grid = FindObjectOfType<GridManager>();

            transform.position = Vector3Int.RoundToInt(transform.position);
        }

        private void Start() {
            if(debugGeneratePath) {
                Vector3 targetWorldPos = grid.GridToWorldPos(debugTargetPos);
                bool generatedPath = TryGeneratePath(targetWorldPos);
                string result = generatedPath ? "success!" : "failure..";
                SafeLogger.Log($"Generation {result}");
            }
        }

        public bool TryGeneratePath(Vector3 targetWorldPos, bool overwrite = false) {
            bool tileIsBlocked = grid.TileIsBlocked(targetWorldPos);
            if(tileIsBlocked)
                return false;

            Vector2Int startPos = GridPosition;
            Vector2Int endPos = grid.WorldToGridPos(targetWorldPos);

            if(!AStar.TryGetPath(grid, startPos, endPos, out PathNode[] validPath)) {
                return false;
            }

            if(overwrite) {
                path = validPath;
                currentNodeIndex = 0;
            }

            return true;
        }

        public void Move() => StartCoroutine(PathMove());
        private IEnumerator PathMove() {
            OnStartedMove?.Invoke();

            PathNode endNode = path.Last();
            int nodesMoved = 0;
            Vector3 finalPosition = grid.GridToWorldPos(endNode.Position);
            Vector3 targetPosition = NextPosition();

            do {
                if(path.Length == 0)
                    break;

                if(currentNodeIndex == path.Length)
                    break;

                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                if(distanceToTarget < float.Epsilon) {
                    transform.position = targetPosition;

                    nodesMoved++;
                    if(nodesMoved < nodesPerMove) {
                        targetPosition = NextPosition();
                    }
                }
                else {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                }

                yield return null;

            } while(Vector3.Distance(transform.position, finalPosition) > float.Epsilon && nodesMoved < nodesPerMove);

            SafeLogger.Log($"{gameObject} reached destination");
            OnReachedDestination?.Invoke();
        }

        private void OnDrawGizmos() {
            if(debugGeneratePath) {
                if(!grid)
                    grid = FindObjectOfType<GridManager>();

                float gridPosY = grid.transform.position.y;

                Vector3 selfPos = Vector3Int.RoundToInt(new Vector3(transform.position.x, gridPosY, transform.position.z));
                ColoredGizmos.DrawSphere(selfPos, .5f, Color.red);
                Vector3 targetGridPos = grid.GridToWorldPos(debugTargetPos);
                ColoredGizmos.DrawSphere(targetGridPos, .5f, Color.green);

                if (path == null)
                    return;

                if(path.Length > 0) {
                    for(int i = 0; i < path.Length; i++) {
                        path[i].DrawGizmo(grid);
                    }
                }
            }
        }

        private void OnValidate() {
            if(debugGeneratePath) {
                if(!grid)
                    grid = FindObjectOfType<GridManager>();

                debugTargetPos.Clamp(Vector2Int.zero, grid.MapSize - Vector2Int.one);
            }
        }
        public bool CheckPositionIsBlocked(Vector3 pos) {
            return grid.TileIsBlocked(pos);
        }
    }
}
