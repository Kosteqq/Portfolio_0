using ProjectPortfolio.Global;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class InteractionSelectionBounds : MonoBehaviour
{
    [SerializeField] private Bounds _bounds;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public bool IsInSelection(in Rect p_screenSpaceRect)
    {
        var rect = RectExtensions.WorldPointsIntoScreenRect(_bounds.GetWorldCorners(transform), _camera);
        return p_screenSpaceRect.Overlaps(rect);
    }

    private void OnDrawGizmos()
    {
        var camera = Camera.main;
        foreach (Vector3 corner in _bounds.GetWorldCorners(transform))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(corner, Vector3.one * 0.5f);
        }

        var rect = RectExtensions.WorldPointsIntoScreenRect(_bounds.GetWorldCorners(transform), camera);
        var bounds = new Bounds();
        bounds.SetMinMax(camera.ScreenToWorldPoint(new Vector3(rect.xMin, rect.yMin, 2f)), camera.ScreenToWorldPoint(new Vector3(rect.xMax, rect.yMax, 2f)));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    [CustomEditor(typeof(InteractionSelectionBounds))]
    private class ComponentEditor : Editor
    {
        private BoxBoundsHandle _boundsHandle;
        
        private void OnSceneGUI()
        {
            ref Bounds bounds = ref ((InteractionSelectionBounds)target)._bounds;
            _boundsHandle ??= new BoxBoundsHandle();
            
            _boundsHandle.size = bounds.size;
            _boundsHandle.center = bounds.center;

            Handles.matrix = ((MonoBehaviour)target).transform.localToWorldMatrix;
            _boundsHandle.SetColor(Color.cyan);
            _boundsHandle.DrawHandle();

            if (_boundsHandle.size != bounds.size || _boundsHandle.center != bounds.center)
            {
                bounds.size = _boundsHandle.size;
                bounds.center = _boundsHandle.center;
                EditorUtility.SetDirty(target);
            }
        }
    }
}
