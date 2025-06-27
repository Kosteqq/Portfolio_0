using ProjectPortfolio.Global;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectPortfolio.Features.Combat
{
    internal class CombatRayShield : MonoBehaviour, ICombatRayTarget
    {
        [SerializeField] private float _radius;

        // source: https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection.html
        public bool IsHit(in Ray p_ray, out HitResult p_hit)
        {
            Vector2 delta = transform.position.GetXZ() - p_ray.Origin;
            float deltaSquareLenght = Vector2.Dot(delta, delta);
            bool isInside = deltaSquareLenght < _radius * _radius;

            if (isInside)
            {
                p_hit = default;
                return false;
            }

            float tca = Vector2.Dot(p_ray.Direction, delta);

            if (tca < 0f)
            {
                p_hit = default;
                return false;
            }

            float squareD = deltaSquareLenght - tca * tca;

            if (squareD > _radius * _radius)
            {
                p_hit = default;
                return false;
            }

            float thc = Mathf.Sqrt(_radius * _radius - squareD);
            float t0 = tca - thc;
            float t1 = tca + thc;
            
            if (t0 < 0f && t1 < 0f)
            {
                p_hit = default;
                return false;
            }

            float distToIntersect = t0 >= 0f ? t0 : t1;
            Vector2 intersect = p_ray.Origin + p_ray.Direction * distToIntersect;

            Vector2 normal = (intersect - transform.position.GetXZ()).normalized;
            p_hit = new HitResult
            {
                Position = transform.position.GetXZ() + 1.01f * _radius * normal,
                Normal = normal,
                Distance = Vector2.Distance(p_ray.Origin, intersect),
            };
            
            if (RayMath.TryReflect(p_ray.Direction, normal, out Vector2 reflectedDirection))
            {
                p_hit.OutDirection = reflectedDirection;
                p_hit.IsBlocked = false;
                return true;
            }
                
            p_hit.OutDirection = p_ray.Direction;
            p_hit.IsBlocked = true;
            return true;
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(CombatRayShield))]
        private class Editor : UnityEditor.Editor
        {
            private void OnSceneGUI()
            {
                var component = (CombatRayShield)target;
                
                Handles.color = Color.green;
                Handles.DrawWireDisc(component.transform.position, Vector3.up, component._radius);
            }
        }
#endif
    }
}