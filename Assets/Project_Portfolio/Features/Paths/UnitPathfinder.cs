using System.Collections.Generic;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class UnitPathfinder : MonoBehaviour, IUnitPathDriver
    {
        private readonly List<Vector2> _path = new(128);

        public IReadOnlyList<Vector2> PathNodes => _path;

        private Pathfinder _pathfinder;
        
        private void Awake()
        {
            var manager = FindAnyObjectByType<PathsManager>();
            _pathfinder = manager.CreatePathfinder(_path, () => transform.position.GetXZ());
        }

        public void SetTarget(Vector2 p_worldPosition)
        {
            _pathfinder.SetDestinationPosition(p_worldPosition);
        }

        public bool HasNextNode()
        {
            return !_path.IsEmpty();
        }

        public Vector2 PeekNextNode()
        {
            return _path[0];
        }

        public Vector2 PopNextNode()
        {
            Vector2 position = _path[0];
            _path.RemoveAt(0);
            return position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < _path.Count - 1; i++)
            {
                Gizmos.DrawCube(_path[i].ToXZ(), Vector3.one);
            }
            
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _path.Count - 1; i++)
            {
                Gizmos.DrawLine(_path[i].ToXZ(), _path[i + 1].ToXZ());
            }
        }
    }
}