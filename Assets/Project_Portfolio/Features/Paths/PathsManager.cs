using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using Unity.Collections;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private int _gridSize;

        private bool _initialized;
        private readonly List<GridNode> _nodes = new();
        public readonly List<Pathfinder> _pathfinders = new();

        private readonly List<Obstacle> _obstacles = new(256);

        public Pathfinder CreatePathfinder(List<Vector2> p_path, Func<UnitPosition> p_getPositionFunc)
        {
            if (!_initialized)
                InitializeGrid();
            
            var grid = new PathfinderGrid(_nodes, _gridSize, UnitPosition.LOCAL_TO_WORLD);
            var pathfinder = new Pathfinder(p_path, p_getPositionFunc, grid, ReleasedPathfinder);
            _pathfinders.Add(pathfinder);
            return pathfinder;
        }

        private void ReleasedPathfinder(Pathfinder p_releasedPathfinder)
        {
            _pathfinders.Remove(p_releasedPathfinder);
        }

        private void InitializeGrid()
        {
            _initialized = true;
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    _nodes.Add(new GridNode(new UnitPosition(x, y)));
                }
            }
            
            for (var tileIndex = 0; tileIndex < _nodes.Count; tileIndex++)
            {
                var gridTile = _nodes[tileIndex];
                AddNodeNeighbour(gridTile, new Vector2Int(1, 0));
                AddNodeNeighbour(gridTile, new Vector2Int(-1, 0));
                AddNodeNeighbour(gridTile, new Vector2Int(0, 1));
                AddNodeNeighbour(gridTile, new Vector2Int(0, -1));
                AddNodeNeighbour(gridTile, new Vector2Int(1, 1));
                AddNodeNeighbour(gridTile, new Vector2Int(1, -1));
                AddNodeNeighbour(gridTile, new Vector2Int(-1, 1));
                AddNodeNeighbour(gridTile, new Vector2Int(-1, -1));
            }
        }
        
        private void AddNodeNeighbour(GridNode p_node, Vector2Int p_direction)
        {
            Vector2Int nodeGridPosition = p_node.Position.ToVec2() + p_direction;

            if (nodeGridPosition.x >= 0
                && nodeGridPosition.y >= 0
                && nodeGridPosition.x <= _gridSize - 1
                && nodeGridPosition.y <= _gridSize - 1)
            {
                p_node.Neighbours.Add(_nodes[nodeGridPosition.y * _gridSize + nodeGridPosition.x]);
            }
        }
        
        internal ObstacleHandle CreateObstacle(in Bounds2D p_bounds)
        {
            var changedNodes = new List<GridNode>(32);
            
            foreach (GridNode node in NodesInsideBounds(p_bounds))
            {
                if (!node.IsBlocked)
                {
                    node.IsBlocked = true;
                    changedNodes.Add(node);
                }
            }

            var newObstacleID = new ObstacleHandle { Index = _obstacles.Count };
            var newObstacle = new Obstacle(newObstacleID, p_bounds);
            
            _obstacles.Add(newObstacle);
            UpdatePathfinders(changedNodes);
            
            return newObstacleID;
        }
        
        internal void DestroyObstacle(ObstacleHandle p_destroyedObstacleHandle)
        {
            if (!p_destroyedObstacleHandle.IsValid)
            {
                Debug.LogError("Passed handle is no longer valid!");
                return;
            }
            
            var changedNodes = new List<GridNode>(32);

            Obstacle destroyedObstacle = _obstacles[p_destroyedObstacleHandle.Index];
            int destroyedIndex = p_destroyedObstacleHandle.Index;
            p_destroyedObstacleHandle.Index = -1;

            if (destroyedIndex == _obstacles.Count - 1)
            {
                _obstacles.RemoveAt(destroyedIndex);
            }
            else
            {
                _obstacles.RemoveAtSwapBack(destroyedIndex);
                _obstacles[destroyedIndex].Handle.Index = destroyedIndex; 
            }

            foreach (GridNode node in NodesInsideBounds(destroyedObstacle.OccupiedBounds))
            {
                bool anyIsOccupied = _obstacles.Any(obstacle => obstacle.OccupiedBounds.IsIntersectWith(node.GridBounds));
                
                if (!anyIsOccupied)
                {
                    node.IsBlocked = false;
                    changedNodes.Add(node);
                }
            }
            
            UpdatePathfinders(changedNodes);
        }

        private IEnumerable<GridNode> NodesInsideBounds(Bounds2D p_bounds)
        {
            Vector2Int gridPosition = Vector2Int.FloorToInt(p_bounds.Min / UnitPosition.LOCAL_TO_WORLD);
            Vector2Int gridMaxPosition = Vector2Int.CeilToInt(p_bounds.Max / UnitPosition.LOCAL_TO_WORLD);
            Vector2Int obstacleSize = gridMaxPosition - gridPosition;

            for (int yOffset = 0; yOffset < obstacleSize.y; yOffset++)
            {
                for (int xOffset = 0; xOffset < obstacleSize.x; xOffset++)
                {
                    Vector2Int nodePosition = gridPosition + new Vector2Int(xOffset, yOffset);
                    GridNode node = GetNode(nodePosition);

                    if (node.GridBounds.IsIntersectWith(p_bounds))
                    {
                        yield return node;
                    }
                }
            }
        }

        private void UpdatePathfinders(List<GridNode> p_changedNodes)
        {
            foreach (var pathfinder in _pathfinders)
            {
                pathfinder.UpdateNodes(p_changedNodes);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (GridNode node in _nodes)
            {
                Gizmos.color = node.IsBlocked ? Color.red : Color.white;
                Gizmos.DrawWireCube(node.GridBounds.Center.ToXZ() + new Vector3(0.1f, 0f, 0.1f), node.GridBounds.Size.ToXZ(1f) - new Vector3(0.2f, 0f, 0.2f));
            }

            foreach (var pathfinder in _pathfinders)
            {
                float x = 1f / pathfinder.PrevUpdatedNodes.Count;
                for (var index = 0; index < pathfinder.PrevUpdatedNodes.Count; index++)
                {
                    var updated = pathfinder.PrevUpdatedNodes[index];
                    Gizmos.color = new Color(1f, 0f, 1f) * x * index;
                    Gizmos.DrawCube(updated.GridNode.GridBounds.Center.ToXZ() + new Vector3(0.2f, 0f, 0.2f),
                        updated.GridNode.GridBounds.Size.ToXZ(1f) - new Vector3(0.4f, 0f, 0.4f));
                    
                    Gizmos.color = new Color(1f, 0f, 1f);
                    Gizmos.DrawWireCube(updated.GridNode.GridBounds.Center.ToXZ() + new Vector3(0.2f, 0f, 0.2f),
                        updated.GridNode.GridBounds.Size.ToXZ(1f) - new Vector3(0.4f, 0f, 0.4f));
                }
            }
        }

        public bool IsPositionValid(UnitPosition p_position)
        {
            return p_position.X < _gridSize && p_position.Y < _gridSize;
        }

        private GridNode GetNode(Vector2Int p_position)
        {
            return _nodes[p_position.y * _gridSize + p_position.x];
        }

        private GridNode GetNode(UnitPosition p_position)
        {
            return _nodes[p_position.Y * _gridSize + p_position.X];
        }

        public bool IsNodeAvailable(UnitPosition p_position)
        {
            return !GetNode(p_position).IsBlocked;
        }
    }
}
