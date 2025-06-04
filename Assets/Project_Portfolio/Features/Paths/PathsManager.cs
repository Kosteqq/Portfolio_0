using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private float _nodeSize;
        [SerializeField] private int _gridSize;
        
        private readonly List<GridNode> _nodes = new();
        public readonly List<Pathfinder> _pathfinders = new();
        private readonly List<PathsObstacleComponent> _obstacles = new();

        private void Awake()
        {
            InitializeGrid();
        }

        public Pathfinder CreatePathfinder(List<Vector2> p_path, Func<Vector2> p_getPositionFunc)
        {
            var grid = new PathfinderGrid(_nodes, _gridSize, _nodeSize);
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
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    _nodes.Add(new GridNode(
                        new Vector2Int(x, y),
                        new Vector2(x * _nodeSize, y * _nodeSize),
                        new Vector2(_nodeSize, _nodeSize)));
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
            Vector2Int nodeGridPosition = p_node.Position + p_direction;

            if (nodeGridPosition.x >= 0
                && nodeGridPosition.y >= 0
                && nodeGridPosition.x <= _gridSize - 1
                && nodeGridPosition.y <= _gridSize - 1)
            {
                p_node.Neighbours.Add(_nodes[nodeGridPosition.y * _gridSize + nodeGridPosition.x]);
            }
        }

        public void RegisterObstacle(PathsObstacleComponent p_obstacle)
        {
            _obstacles.Add(p_obstacle);
            ObstacleUpdated(p_obstacle);
        }

        public void ObstacleUpdated(PathsObstacleComponent p_updatedObstacle)
        {
            var changedNodes = new List<GridNode>(32);
            
            foreach (GridNode prevNode in NodesInsideBounds(p_updatedObstacle.PrevBounds))
            {
                prevNode.IsBlocked = _obstacles.Any(obstacle => obstacle != p_updatedObstacle && obstacle.Bounds.IsIntersectWith(prevNode.WorldBounds));
                changedNodes.Add(prevNode);
            }
            
            foreach (GridNode node in NodesInsideBounds(p_updatedObstacle.Bounds))
            {
                node.IsBlocked = true;
                changedNodes.Add(node);
            }

            UpdatePathfinders(changedNodes);
        }

        private IEnumerable<GridNode> NodesInsideBounds(Bounds2D p_bounds)
        {
            Vector2Int gridPosition = Vector2Int.FloorToInt(p_bounds.Min / _nodeSize);
            Vector2Int gridMaxPosition = Vector2Int.CeilToInt(p_bounds.Max / _nodeSize);
            Vector2Int obstacleSize = gridMaxPosition - gridPosition;

            for (int yOffset = 0; yOffset < obstacleSize.y; yOffset++)
            {
                for (int xOffset = 0; xOffset < obstacleSize.x; xOffset++)
                {
                    Vector2Int nodePosition = gridPosition + new Vector2Int(xOffset, yOffset);
                    GridNode node = _nodes[nodePosition.y * _gridSize + nodePosition.x];

                    if (node.WorldBounds.IsIntersectWith(p_bounds))
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
                Gizmos.DrawWireCube(node.WorldBounds.Center.ToXZ() + new Vector3(0.1f, 0f, 0.1f), node.WorldBounds.Size.ToXZ(1f) - new Vector3(0.2f, 0f, 0.2f));
            }

            foreach (var pathfinder in _pathfinders)
            {
                float x = 1f / pathfinder.PrevUpdatedNodes.Count;
                for (var index = 0; index < pathfinder.PrevUpdatedNodes.Count; index++)
                {
                    var updated = pathfinder.PrevUpdatedNodes[index];
                    Gizmos.color = new Color(1f, 0f, 1f) * x * index;
                    Gizmos.DrawCube(updated.GridNode.WorldBounds.Center.ToXZ() + new Vector3(0.2f, 0f, 0.2f),
                        updated.GridNode.WorldBounds.Size.ToXZ(1f) - new Vector3(0.4f, 0f, 0.4f));
                    
                    Gizmos.color = new Color(1f, 0f, 1f);
                    Gizmos.DrawWireCube(updated.GridNode.WorldBounds.Center.ToXZ() + new Vector3(0.2f, 0f, 0.2f),
                        updated.GridNode.WorldBounds.Size.ToXZ(1f) - new Vector3(0.4f, 0f, 0.4f));
                }
            }
        }
    }
}
