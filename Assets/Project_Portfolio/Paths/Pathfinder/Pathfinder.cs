using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class Pathfinder
    {
        private const int MAX_COMPUTE_ITERATIONS = 10_000;

        private readonly Action<Pathfinder> _releaseCallback;
        internal readonly PathfinderGrid _grid;
        private readonly PathfinderQueue _queue;
        
        private readonly PathfinderNode _destinationNode;
        private PathfinderNode _startNode;
        private float _km = 0f;

        private readonly List<Vector2> _path = new();

        public IReadOnlyList<Vector2> Path => _path;

        internal List<PathfinderNode> PrevUpdatedNodes = new();

        internal Pathfinder(Vector3 p_destination, PathfinderGrid p_grid, Action<Pathfinder> p_releaseCallback)
        {
            _grid = p_grid;
            _releaseCallback = p_releaseCallback;
            _destinationNode = p_grid.GetNode(p_destination);
            _destinationNode.Rhs = 0f;

            _queue = new PathfinderQueue();
        }

        public void Release()
        {
            _releaseCallback?.Invoke(null);
        }

        private void Initialize()
        {
            foreach (PathfinderNode node in _grid.Nodes)
            {
                node.Key = default;
                node.Rhs = float.MaxValue;
                node.G = float.MaxValue;
            }
            
            _destinationNode.Rhs = 0f;
            _queue.Clear();
            _queue.Enqueue(_destinationNode, CalculateKey(_destinationNode));
        }
        
        public void ResetStartNode(Vector3 p_startPosition)
        {
            PathfinderNode newStartNode = _grid.GetNode(p_startPosition);

            if (newStartNode == null)
            {
                Debug.LogError("New start position is invalid.");
                return;
            }
            
            if (_startNode != null && _startNode != newStartNode)
            {
                _km += CalculateOctileHeuristic(_startNode, newStartNode);
            }

            _startNode = newStartNode;

            Initialize();
            CalculatePath();
            ParsePath();
        }

        internal void UpdateNodes(List<GridNode> p_changedGridNodes)
        {
            _destinationNode.Rhs = 0f;
            PrevUpdatedNodes.Clear();
            
            foreach (GridNode changedGridNode in p_changedGridNodes)
            {
                var changed = _grid.GetNode(changedGridNode);
                changed.G = float.MaxValue;
                changed.Rhs = float.MaxValue;
                
                foreach (GridNode gridNode in changedGridNode.Neighbours.Concat(new[] { changedGridNode }))
                {
                    var node = _grid.GetNode(gridNode);

                    if (_startNode != node)
                    {
                        UpdateNodeRhs(node);
                    }
                    
                    UpdateNode(node);
                }
            }

            CalculatePath();
            ParsePath();
        }

        private void CalculatePath()
        {
            int iterations = 0;

            while (true)
            {
                if (iterations++ >= MAX_COMPUTE_ITERATIONS)
                {
                    _queue.Clear();
                    Debug.LogError("Max iterations exceeded");
                    break;
                }

                if (_queue.IsEmpty
                    || (_queue.First.Key > CalculateKey(_startNode) && _startNode.Rhs <= _startNode.G))
                {
                    _startNode.G = _startNode.Rhs;
                    break;
                }

                PathfinderNode currentNode = _queue.Dequeue();
                var currentNodeNewKey = CalculateKey(currentNode);
                PrevUpdatedNodes.Add(currentNode);

                if (currentNode.Key < currentNodeNewKey)
                {
                    _queue.Enqueue(currentNode, currentNodeNewKey);
                    continue;
                }

                if (currentNode.G > currentNode.Rhs)
                {
                    currentNode.G = currentNode.Rhs;
                    UpdateNodeNeighboursRhs(currentNode);

                    foreach (PathfinderNode neighbour in currentNode.Neighbours)
                        UpdateNode(neighbour);
                    
                    continue;
                }

                float oldNodeG = currentNode.G;
                currentNode.G = float.MaxValue;

                foreach (PathfinderNode neighbour in currentNode.Neighbours.Concat(new[] { currentNode }))
                {
                    if (neighbour.Rhs.IsApprox(oldNodeG + CalculateCost(neighbour, currentNode)))
                    {
                        UpdateNodeRhs(neighbour);
                    }

                    UpdateNode(neighbour);
                }
            }
        }

        private void UpdateNodeRhs(PathfinderNode p_node)
        {
            if (p_node != _destinationNode)
            {
                p_node.Rhs = float.MaxValue;
            }
            
            foreach (PathfinderNode neighbour in p_node.Neighbours)
            {
                p_node.Rhs = Mathf.Min(p_node.Rhs, CalculateCost(p_node, neighbour) + neighbour.G);
            }
        }

        private void UpdateNodeNeighboursRhs(PathfinderNode p_node)
        {
            foreach (PathfinderNode neighbour in p_node.Neighbours)
            {
                if (neighbour != _destinationNode)
                {
                    neighbour.Rhs = Mathf.Min(neighbour.Rhs, CalculateCost(p_node, neighbour) + p_node.G);
                }
            }
        }

        private void ParsePath()
        {
            _path.Clear();
            PathfinderNode currentNode = _startNode;
            int iterations = 0;

            while (true)
            {
                if (iterations++ >= MAX_COMPUTE_ITERATIONS)
                {
                    Debug.LogError("Max iterations exceeded");
                    break;
                }
                
                if (currentNode == _destinationNode)
                {
                    break;
                }
                
                _path.Add(currentNode.GridNode.WorldPosition);
                PathfinderNode bestNeighbour = null;
                float minCost = float.MaxValue;

                foreach (PathfinderNode neighbour in currentNode.Neighbours)
                {
                    if (Mathf.Min(neighbour.G, neighbour.Rhs) < minCost)
                    {
                        minCost = Mathf.Min(neighbour.G, neighbour.Rhs);
                        bestNeighbour = neighbour;
                    }
                }

                if (bestNeighbour == null)
                {
                    Debug.LogError("Failed to find path");
                    return;
                }

                currentNode = bestNeighbour;
            }

            _path.Add(_destinationNode.GridNode.WorldPosition);
        }

        private void UpdateNode(PathfinderNode p_node)
        {
            if (!p_node.IsRhsEqualG && !_queue.Contains(p_node))
            {
                _queue.Enqueue(p_node, CalculateKey(p_node));
                return;
            }

            if (p_node.IsRhsEqualG && _queue.Contains(p_node))
            {
                _queue.Remove(p_node);
                return;
            }

            if (_queue.Contains(p_node))
            {
                _queue.Update(p_node, CalculateKey(p_node));
            }
        }

        private PathfinderNodeKey CalculateKey(PathfinderNode p_node)
        {
            return new PathfinderNodeKey(
                Mathf.Min(p_node.G, p_node.Rhs) + CalculateOctileHeuristic(p_node, _startNode) + _km,
                Mathf.Min(p_node.G, p_node.Rhs)
            );
        }

        private float CalculateOctileHeuristic(PathfinderNode p_node, PathfinderNode p_target)
        {
            if (p_node.GridNode.IsBlocked || p_target.GridNode.IsBlocked)
            {
                return float.PositiveInfinity;
            }
            
            Vector2Int delta = (p_node.GridNode.Position - p_target.GridNode.Position).Abs();

            return Mathf.Max(delta.x, delta.y) * 0.41f + Mathf.Min(delta.x, delta.y);
        }

        private float CalculateCost(PathfinderNode p_node, PathfinderNode p_target)
        {
            if (p_node.GridNode.IsBlocked || p_target.GridNode.IsBlocked)
            {
                return float.PositiveInfinity;
            }
            
            return Vector2Int.Distance(p_node.GridNode.Position, p_target.GridNode.Position);
        }
    }
}