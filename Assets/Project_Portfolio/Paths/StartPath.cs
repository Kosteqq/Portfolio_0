using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class StartPath : MonoBehaviour
    {
        [SerializeField] private GameObject Target;

        private List<Vector2> _path;
        private Pathfinder _pathfinder;

        private void Start()
        {
            var manager = FindAnyObjectByType<PathsManager>();
            _pathfinder = manager.CreatePathfinder(transform.position);
        }

        private void OnDestroy()
        {
            _pathfinder?.Release();
            _pathfinder = null;
        }

        private void Update()
        {
            _pathfinder.ResetStartNode(Target.transform.position);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            
            Gizmos.color = Color.blue;
            foreach (Vector2 path in _pathfinder.Path)
            {
                Gizmos.DrawCube(new Vector3(path.x, 0f, path.y) + new Vector3(2.5f, 0f, 2.5f),
                    Vector3.one);
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawLineStrip(new ReadOnlySpan<Vector3>(_pathfinder.Path.Select(point => point.ToXZ() + new Vector3(2.5f, 0f, 2.5f)).ToArray()), false);
        }
    }
}