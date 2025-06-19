using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

namespace ProjectPortfolio.Global
{
    [Serializable]
    public partial struct Bounds2D
    {
        public Vector2 Center;
        public Vector2 Size;
        [HideInInspector] public float Rotation;

        public Vector2 Min => GetMin();
        public Vector2 Max => GetMax();

        public Bounds2D(Vector2 p_center, Vector2 p_size, float p_rotation)
        {
            Center = p_center;
            Size = p_size;
            Rotation = p_rotation;
            
#if UNITY_EDITOR
            _editorCachedBoundsHandle = null;
#endif
        }

        public readonly Bounds2D Transform(Transform p_transform)
        {
            return new Bounds2D
            {
                Center = p_transform.TransformPoint(Center.ToXZ()).GetXZ(),
                Size = Size * p_transform.lossyScale.GetXZ(),
                Rotation = Rotation + p_transform.eulerAngles.y,
            };
        }

        private readonly Vector2 GetMin()
        {
            Vector2 min = Vector2.positiveInfinity;
            foreach (Vector2 corner in this.GetCorners())
            {
                min = Vector2.Min(min, corner);
            }

            return min;
        }

        private readonly Vector2 GetMax()
        {
            Vector2 max = Vector2.zero;
            foreach (Vector2 corner in this.GetCorners())
            {
                max = Vector2.Max(max, corner);
            }

            return max;
        }

#if UNITY_EDITOR
        private BoxBoundsHandle _editorCachedBoundsHandle;
        
        public bool DrawEditorHandle(Transform p_parent = null)
        {
            Handles.matrix = p_parent != null
                ? p_parent.localToWorldMatrix 
                : Matrix4x4.identity;
            
            Handles.color = Color.green;

            if (_editorCachedBoundsHandle == null)
            {
                _editorCachedBoundsHandle = new BoxBoundsHandle();
                _editorCachedBoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Z;
            }
            
            _editorCachedBoundsHandle.size = Size.ToXZ();
            _editorCachedBoundsHandle.center = Center.ToXZ();
            _editorCachedBoundsHandle.DrawHandle();

            if (_editorCachedBoundsHandle.center == Center.ToXZ()
                && _editorCachedBoundsHandle.size == Size.ToXZ())
            {
                return false;
            }
            
            Center = _editorCachedBoundsHandle.center.GetXZ();
            Size = _editorCachedBoundsHandle.size.GetXZ();
            return true;
        }
#endif
    }
}