using System;
using ProjectPortfolio.Global;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

namespace ProjectPortfolio.Paths
{
    public class PathsObstacleComponent : MonoBehaviour
    {
        [SerializeField] private PathsManager _pathsManager;
        [SerializeField] private Bounds2D _localBounds;
        private Bounds2D _prevWorldBounds;

        internal Bounds2D PrevBounds => _prevWorldBounds;
        internal Bounds2D Bounds => _localBounds.Transform(transform);

        private void Start()
        {
            _pathsManager.RegisterObstacle(this);
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                
                _pathsManager.ObstacleUpdated(this);
                _prevWorldBounds = Bounds;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(PathsObstacleComponent))]
        private class ObstacleEditor : Editor
        {
            private BoxBoundsHandle _boundsHandle;
            
            private void OnSceneGUI()
            {
                var obstacle = (PathsObstacleComponent)target;
                ref Bounds2D bounds = ref obstacle._localBounds;

                Handles.matrix = obstacle.transform.localToWorldMatrix;
                Handles.color = Color.green;

                if (bounds.DrawEditorHandle(obstacle.transform))
                {
                    EditorUtility.SetDirty(target);
                }

                Handles.matrix = Matrix4x4.identity;
                foreach (var node in obstacle._pathsManager._pathfinders[0]._grid.Nodes)
                {
                    Handles.color = Color.white;
                    if (node.G < float.MaxValue)
                    {
                        Handles.Label(node.GridNode.WorldBounds.Center.ToXZ(), Mathf.Min(node.G, node.Rhs).ToString("F1"));
                    }
                }
            }
        }
#endif
    }
}