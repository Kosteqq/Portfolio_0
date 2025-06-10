using ProjectPortfolio.Global;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

namespace ProjectPortfolio.Paths
{
    public class StaticObstacle : MonoBehaviour
    {
        [SerializeField] private PathsManager _pathsManager;
        [SerializeField] private GridBounds _localBounds;
        private ObstacleHandle _obstacleHandle; 

        private void Start()
        {
            _obstacleHandle = _pathsManager.CreateObstacle(_localBounds.Transform(transform));
        }

        private void OnDestroy()
        {
            _pathsManager.DestroyObstacle(_obstacleHandle);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(StaticObstacle))]
        private class ObstacleEditor : Editor
        {
            private BoxBoundsHandle _boundsHandle;
            
            private void OnSceneGUI()
            {
                var obstacle = (StaticObstacle)target;
                ref GridBounds bounds = ref obstacle._localBounds;

                Handles.matrix = obstacle.transform.localToWorldMatrix;
                Handles.color = Color.green;

                if (bounds.DrawEditorHandle(obstacle.transform))
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }
#endif
    }
}