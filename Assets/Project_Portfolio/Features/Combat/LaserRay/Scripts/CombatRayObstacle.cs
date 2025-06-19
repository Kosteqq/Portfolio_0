using ProjectPortfolio.Global;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectPortfolio.Features.Combat
{
    internal class CombatRayObstacle : MonoBehaviour, ICombatRayTarget
    {
        [SerializeField] private Bounds2D _hitbox;

        private void Start()
        {
            FindAnyObjectByType<CombatLaserController>()._targets.Add(this);
        }

        public bool IsHit(in Ray p_ray, out HitResult p_hit)
        {
            bool isIntersects = _hitbox
                .Transform(transform)
                .IsIntersectsRay(p_ray.Origin, p_ray.Direction, out Vector2 intersect, out Vector2 normal);

            if (!isIntersects)
            {
                p_hit = default;
                return false;
            }

            p_hit = new HitResult
            {
                Position = intersect,
                Distance = Vector2.Distance(p_ray.Origin, intersect),
                Normal = normal,
                IsBlocked = true,
            };
            return true;
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(CombatRayObstacle))]
        private class Editor : UnityEditor.Editor
        {
            private void OnSceneGUI()
            {
                var component = (CombatRayObstacle)target;

                if (component._hitbox.DrawEditorHandle(component.transform))
                {
                    EditorUtility.SetDirty(component);
                }
            }
        }
#endif
    }
}