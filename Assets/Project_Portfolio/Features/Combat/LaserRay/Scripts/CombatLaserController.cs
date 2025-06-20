using System.Collections.Generic;
using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectPortfolio.Features.Combat
{
    internal class CombatLaserController : MonoBehaviour
    {
        [SerializeField] private CombatRayVisualizer _rayEffect;

        private ObjectPool<CombatRayVisualizer> _rayVisualizerPool;
        
        private readonly List<(Vector2, Vector2, bool)> _debug = new();

        private void Awake()
        {
            var visualizersRoot = new GameObject("| Rays |");
            _rayVisualizerPool = new ObjectPool<CombatRayVisualizer>(
                () =>
                {
                    CombatRayVisualizer visualizer = Instantiate(_rayEffect, visualizersRoot.transform);
                    visualizer.OnReleased += _rayVisualizerPool.Release;
                    return visualizer;
                },
                p_visualizer =>
                {
                    p_visualizer.gameObject.SetActive(true);
                },
                p_visualizer =>
                {
                    p_visualizer.gameObject.SetActive(false);
                    p_visualizer.Clear();
                },
                p_visualizer =>
                {
                    Destroy(p_visualizer.gameObject);
                },
                defaultCapacity: 20);
        }

        private float _timer;
        private void Update()
        {
            if (_timer <= 0f)
            {
                _timer = 3f;
                ShootRay(new Ray
                {
                    Origin = new Vector2(10, 10),
                    Direction = new Vector2(1, 1).normalized,
                });
            }

            _timer -= Time.deltaTime;
        }

        public void ShootRay(in Ray p_ray)
        {
            List<HitHistory> hitHistory = CastRay(p_ray);
            
            var visualizer = _rayVisualizerPool.Get();
            visualizer.Setup(in p_ray, hitHistory);
        }

        private bool CheckRayHit(in Ray p_ray, out HitResult p_hitResult)
        {
            ICombatRayTarget closestHitTarget = null;
            float closestHitDist = float.MaxValue;
            HitResult closestHit = default;
                
            foreach (MonoBehaviour mono in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (mono is ICombatRayTarget target
                    && target.IsHit(in p_ray, out HitResult targetHit)
                    && targetHit.Distance < closestHitDist)
                {
                    closestHitDist = targetHit.Distance;
                    closestHit = targetHit;
                    closestHitTarget = target;
                }
            }
            
            if (closestHitTarget == null)
            {
                p_hitResult = default;
                return false;
            }

            p_hitResult = closestHit;
            return true;
        }

        private List<HitHistory> CastRay(Ray p_ray)
        {
            var history = new List<HitHistory>(16);

            while (true)
            {
                if (!CheckRayHit(in p_ray, out HitResult hitResult))
                {
                    p_ray.Origin += p_ray.Direction * 1000;
                    history.Add(new HitHistory { WorldPosition = p_ray.Origin });
                    break;
                }

                history.Add(new HitHistory(in hitResult));
                p_ray.Origin = hitResult.Position;
                p_ray.Direction = hitResult.OutDirection;

                if (hitResult.IsBlocked)
                {
                    break;
                }
            }

            return history;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _debug.Count; i++)
            {
                // Gizmos.color = Color.HSVToRGB((float)i / _debug.Count, 0.5f, 1f);
                Gizmos.color = _debug[i].Item3 ? Color.red : Color.blue;
                Gizmos.DrawLine(_debug[i].Item1.ToXZ(1f), _debug[i].Item2.ToXZ(1f));
            }
            _debug.Clear();
        }
    }
}