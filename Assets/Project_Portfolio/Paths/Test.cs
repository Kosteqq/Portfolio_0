using System;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private PathsManager _pathsManager;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                var tile = _pathsManager.GetTile(transform.position.GetXZ());
                var tileBounds = _pathsManager.GetTileWorldBounds(in tile);
                Gizmos.DrawWireCube(tileBounds.center, tileBounds.size);
            }
        }
    }
}