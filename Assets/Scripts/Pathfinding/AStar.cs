using Debug;
using Level;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding {
    public static class AStar {
        private const int iterationFailSafe = 1000;

        private static readonly Vector2Int[] searchDirections = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public static int GetNodeDistance(GridManager grid, Vector2Int startPosition, Vector2Int endPosition) {
            if(Vector2Int.Distance(startPosition, endPosition) < 1) {
                return 0;
            }

            if(!TryGetPath(grid, startPosition, endPosition, out PathNode[] path)) {
                return Int32.MaxValue;
            }

            return path.Length - 1; // - 1 to not include startposition
        }

        public static bool TryGetPath(GridManager grid, Vector2Int startPosition, Vector2Int endPosition, out PathNode[] path) {
            var pathList = new List<PathNode>();

            if(!grid.Contains(startPosition)) {
                path = default;
                return false;
            }

            if(!grid.Contains(endPosition)) {
                path = default;
                return false;
            }

            PathNode startNode = new(startPosition, 0.0f, 0.0f, null);
            PathNode endNode = new(endPosition, 0.0f, 0.0f, null);

            List<PathNode> open = new();
            List<PathNode> closed = new();

            bool foundPath = false;
            PathNode currentNode = startNode;

            int iterations = 0;

            do {
                for(int i = 0; i < searchDirections.Length; i++) {
                    Vector2Int neighborPos = currentNode.Position + searchDirections[i];

                    bool xOutOfBounds = neighborPos.x < 0 || neighborPos.x >= grid.MapSize.x;
                    bool yOutOfBounds = neighborPos.y < 0 || neighborPos.y >= grid.MapSize.y;
                    if(xOutOfBounds || yOutOfBounds)
                        continue;

                    if(grid.TileIsBlocked(neighborPos))
                        continue;

                    PathNode closedNode = closed.Find(pn => pn.Position.Equals(neighborPos));
                    if(closedNode != null)
                        continue;

                    float g = Vector2.Distance(currentNode.Position, neighborPos) + currentNode.G;
                    float h = Vector2.Distance(neighborPos, endPosition);

                    PathNode openNode = open.Find(pn => pn.Position.Equals(neighborPos));
                    if(openNode != null) {
                        openNode.Update(g, h, currentNode);
                    }
                    else {
                        open.Add(new PathNode(neighborPos, g, h, currentNode));
                    }
                }

                open = open.OrderBy(pn => pn.F).ThenBy(pn => pn.H).ToList();

                // there is no path, so return
                if(open.Count == 0) {
                    path = default;
                    return false;
                }

                PathNode nextNode = open[0];
                closed.Add(nextNode);
                open.RemoveAt(0);

                if(nextNode.Position.Equals(endNode.Position)) {
                    foundPath = true;
                    continue;
                }

                currentNode = nextNode;
                iterations++;
            } while(!foundPath && iterations < iterationFailSafe);

            closed.Reverse();
            PathNode exitNode = closed.First();

            pathList.Add(exitNode);
            PathNode previousNode = exitNode;

            while(previousNode.Parent != null) {
                pathList.Add(previousNode.Parent);
                previousNode = previousNode.Parent;
            }

            pathList.Reverse();
            path = pathList.ToArray();
            return true;
        }
    }
}
