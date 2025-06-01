using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private float _nodeSize;
        [SerializeField] private int _gridSize;
        
        private readonly List<GridNode> _nodes = new();
        private readonly List<Pathfinder> _pathfinders = new();
        
        private void Awake()
        {
            InitializeGrid();
        }

        public Pathfinder CreatePathfinder(Vector3 p_worldDestinationPos)
        {
            var grid = new PathfinderGrid(_nodes, _gridSize, _nodeSize);
            var pathfinder = new Pathfinder(p_worldDestinationPos, grid, ReleasedPathfinder);
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
                        new Vector2(x * _nodeSize, y * _nodeSize)));
                }
            }
            
            for (var tileIndex = 0; tileIndex < _nodes.Count; tileIndex++)
            {
                var gridTile = _nodes[tileIndex];
                
                if (gridTile.Position.x > 0)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex - 1]);
                if (gridTile.Position.x < _gridSize - 1)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex + 1]);
                
                if (gridTile.Position.y > 0)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex - _gridSize]);
                if (gridTile.Position.y < _gridSize - 1)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex + _gridSize]);
                
                if (gridTile.Position.x > 0 && gridTile.Position.y > 0)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex - _gridSize - 1]);
                if (gridTile.Position.x > 0 && gridTile.Position.y < _gridSize - 1)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex + _gridSize - 1]);
                if (gridTile.Position.x < _gridSize - 1 && gridTile.Position.y > 0)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex - _gridSize + 1]);
                if (gridTile.Position.x < _gridSize - 1 && gridTile.Position.y < _gridSize - 1)
                    _nodes[tileIndex].Neighbours.Add(_nodes[tileIndex + _gridSize + 1]);
            }
        }

        private void OnDrawGizmos()
        {
            for (int y = 0; y < _gridSize; y++)
            {
                for (int x = 0; x < _gridSize; x++)
                {
                    Gizmos.DrawWireCube(new Vector3(x, 0f, y) * _nodeSize + new Vector3(_nodeSize / 2f, 0f, _nodeSize / 2f),
                        new Vector3(_nodeSize, 0f, _nodeSize));
                }
            }
        }
    }
}
