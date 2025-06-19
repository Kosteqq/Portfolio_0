using System;
using System.Collections.Generic;
using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.VFX;

namespace ProjectPortfolio.Features.Combat
{
    internal class CombatRayVisualizer : MonoBehaviour
    {
        private static readonly int RayAlphaProperty = Shader.PropertyToID("_Alpha");
        
        [SerializeField] private LineRenderer _rayRenderer;
        [SerializeField] private VisualEffect _hitParticle;
        [SerializeField] private float _rayTime;
        [SerializeField] private float _particlesDisappearTime;
        [SerializeField] private float _lineHeight;

        private MaterialPropertyBlock _rayPropertyBlock;
        private float _rayTimer;
        private float _particlesDisappearTimer;

        private readonly List<VisualEffect> _hitParticles = new(8);
        private int _lineIndex;

        public event Action<CombatRayVisualizer> OnReleased;

        public void Awake()
        {
            _rayPropertyBlock = new MaterialPropertyBlock();
            _rayTimer = _rayTime;
        }

        public void Clear()
        {
            _rayTimer = _rayTime;
            _particlesDisappearTimer = _particlesDisappearTime;

            foreach (VisualEffect particle in _hitParticles)
            {
                Destroy(particle.gameObject);
            }
            _hitParticles.Clear();
        }

        private void Update()
        {
            if (_rayTimer > 0f)
            {
                _rayTimer = Mathf.Max(_rayTimer - Time.deltaTime, 0f);
                _rayPropertyBlock.SetFloat(RayAlphaProperty, _rayTimer / _rayTime);
                _rayRenderer.SetPropertyBlock(_rayPropertyBlock, 0);
                return;
            }

            if (_particlesDisappearTimer > 0f)
            {
                _particlesDisappearTimer = Mathf.Max(_particlesDisappearTimer - Time.deltaTime, 0f);
                foreach (VisualEffect particle in _hitParticles)
                {
                    particle.Stop();
                }
            }

            if (_rayTimer <= 0f && _particlesDisappearTimer <= 0f)
            {
                OnReleased?.Invoke(this);
            }
        }

        public void Setup(in Ray p_ray, IReadOnlyList<HitHistory> p_hitHistory)
        {
            _rayRenderer.positionCount = p_hitHistory.Count + 1;
            
            var positions = new Vector3[p_hitHistory.Count + 1];
            positions[0] = p_ray.Origin.ToXZ(_lineHeight);
            
            for (int i = 0; i < p_hitHistory.Count; i++)
            {
                positions[i + 1] = p_hitHistory[i].WorldPosition.ToXZ(_lineHeight);
                CreateHitEffect(p_hitHistory[i]);
            }
            
            _rayRenderer.SetPositions(positions);
        }
        
        public void CreateHitEffect(in HitHistory p_hit)
        {
            VisualEffect effect = Instantiate(
                _hitParticle,
                p_hit.WorldPosition.ToXZ(_lineHeight),
                Quaternion.LookRotation(p_hit.WorldHitNormal.ToXZ()),
                transform);
            
            _hitParticles.Add(effect);
            effect.Play();
        }
    }
}