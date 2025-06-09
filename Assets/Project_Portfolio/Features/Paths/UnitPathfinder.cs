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

        private PathsManager _manager;
        private Pathfinder _pathfinder;
        
        private void Awake()
        {
            _manager = FindAnyObjectByType<PathsManager>();
            _pathfinder = _manager.CreatePathfinder(_path, () => UnitPosition.WorldToLocal(transform.position));
        }

        public void SetTarget(UnitPosition p_position)
        {
            _pathfinder.SetDestinationPosition(p_position);
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
            for (int i = 0; i < _path.Count; i++)
            {
                Gizmos.DrawCube(_path[i].ToXZ(), Vector3.one);
            }
            
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _path.Count; i++)
            {
                if (i == 0)
                    Gizmos.DrawLine(transform.position, _path[i].ToXZ());
                else
                    Gizmos.DrawLine(_path[i - 1].ToXZ(), _path[i].ToXZ());
            }
        }
    }
}