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
        private readonly PathfinderGrid _grid;
        private readonly PathfinderQueue _queue;
        
        private readonly PathfinderNode _destinationNode;
        private PathfinderNode _startNode;
        private float _km = 0f;

        private readonly List<Vector2> _path = new();

        public IReadOnlyList<Vector2> Path => _path;

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

            _startNode = newStartNode;

            Initialize();
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
                    Debug.LogError("Max iterations exceeded");
                    break;
                }

                if (_queue.IsEmpty
                    || (_queue.First.Key > CalculateKey(_queue.First) && _startNode.Rhs < _startNode.G))
                {
                    _startNode.G = _startNode.Rhs;
                    break;
                }

                PathfinderNode currentNode = _queue.Dequeue();
                var currentNodeNewKey = CalculateKey(currentNode);

                if (currentNode.Key < currentNodeNewKey)
                {
                    _queue.Enqueue(currentNode, currentNodeNewKey);
                    continue;
                }

                if (currentNode.G > currentNode.Rhs)
                {
                    currentNode.G = currentNode.Rhs;

                    foreach (PathfinderNode neighbour in currentNode.Neighbours)
                    {
                        if (neighbour != _destinationNode)
                        {
                            neighbour.Rhs = Mathf.Min(neighbour.Rhs, currentNode.G + CalculateCost(neighbour, currentNode));
                        }
                        UpdateNode(neighbour);
                    }
                    continue;
                }

                float oldNodeG = currentNode.G;
                currentNode.G = float.MaxValue;

                foreach (PathfinderNode neighbour in currentNode.Neighbours.Concat(new[] { currentNode }))
                {
                    if (!neighbour.Rhs.IsApprox(oldNodeG + CalculateCost(neighbour, currentNode)))
                    {
                        UpdateNode(neighbour);
                        continue;
                    }

                    if (neighbour != _destinationNode)
                    {
                        neighbour.Rhs = float.MaxValue;
                    }

                    foreach (PathfinderNode secondNeighbour in neighbour.Neighbours)
                    {
                        neighbour.Rhs = Mathf.Min(neighbour.Rhs, CalculateCost(neighbour, secondNeighbour) + secondNeighbour.G);
                    }
                    
                    UpdateNode(neighbour);
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
                    if (neighbour.G < minCost)
                    {
                        minCost = neighbour.G;
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
            Vector2Int delta = (p_node.GridNode.Position - p_target.GridNode.Position).Abs();

            return Mathf.Max(delta.x, delta.y) * 0.41f + Mathf.Min(delta.x, delta.y);
        }

        private float CalculateCost(PathfinderNode p_node, PathfinderNode p_target)
        {
            return Vector2Int.Distance(p_node.GridNode.Position, p_target.GridNode.Position);
        }
    }
}