using Project_Portfolio.Global;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectPortfolio.Paths
{
    public struct Tile
    {
        public UShortVector2 Position;
    }
    public struct Chunk
    {
        public UShortVector2 Position;
    }
    
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private float _tileWorldSize;
        [SerializeField] private uint _chunkTilesCount;
        [SerializeField] private uint _chunksCount;

        private Tile[] _tiles;
        private float _chunkWorldSize => _chunkTilesCount * _tileWorldSize;
        private uint _tilesInChunk => _chunkTilesCount * _chunkTilesCount;


        private void Awake()
        {
            _tiles = new Tile[(int)Mathf.Pow(_chunksCount * _chunkTilesCount, 2)];

            for (ushort chunkY = 0; chunkY < _chunksCount; chunkY++)
            {
                for (ushort chunkX = 0; chunkX < _chunksCount; chunkX++)
                {
                    for (ushort y = 0; y < _chunkTilesCount; y++)
                    {
                        for (ushort x = 0; x < _chunkTilesCount; x++)
                        {
                            var position = new UShortVector2
                            {
                                X = (ushort)(chunkX * _chunkTilesCount + x),
                                Y = (ushort)(chunkY * _chunkTilesCount + y),
                            };

                            uint index = GetTileIndex(position);
                            
                            _tiles[index] = new Tile
                            {
                                Position = position,
                            };
                        }   
                    }
                }
            }
        }

        public Tile GetTile(Vector2 p_worldPosition)
        {
            UShortVector2 tilePosition = WorldToTilePosition(p_worldPosition);
            uint tileIndex = GetTileIndex(tilePosition);
            return _tiles[tileIndex];
        }

        public Tile GetTile(UShortVector2 p_tilePosition)
        {
            uint tileIndex = GetTileIndex(p_tilePosition);
            return _tiles[tileIndex];
        }

        public uint GetTileIndex(UShortVector2 p_tilePosition)
        {
            UShortVector2 chunk = p_tilePosition / _chunkTilesCount;
            UShortVector2 padding = p_tilePosition % _chunkTilesCount;

            uint chunksOffset = chunk.Y * _chunksCount + chunk.X;
            uint chunkPadding = padding.Y * _chunkTilesCount + padding.X;
            
            return chunksOffset * _tilesInChunk + chunkPadding;
        }

        public Bounds GetTileWorldBounds(in Tile p_tile)
        {
            Bounds bounds = new();
            Vector3 min = new Vector3(p_tile.Position.X * _tileWorldSize, 0f, p_tile.Position.Y * _tileWorldSize);
            
            bounds.SetMinMax(min, min + new Vector3(_tileWorldSize, 0f, _tileWorldSize));
            return bounds;
        }

        private UShortVector2 WorldToTilePosition(Vector2 p_worldPosition)
        {
            Assert.IsTrue(p_worldPosition.x >= 0 && p_worldPosition.y >= 0, "Negative grid values are not supported");

            return new UShortVector2
            {
                X = (ushort)Mathf.FloorToInt(p_worldPosition.x / _tileWorldSize),
                Y = (ushort)Mathf.FloorToInt(p_worldPosition.y / _tileWorldSize),
            };
        }

        private void OnDrawGizmos()
        {
            for (int y = 0; y < _chunksCount; y++)
            {
                for (int x = 0; x < _chunksCount; x++)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(
                        new Vector3((x + 0.5f) * _chunkWorldSize, 0f, (y + 0.5f) * _chunkWorldSize),
                        new Vector3(_chunkWorldSize, 0f, _chunkWorldSize));
                }   
            }
        }
    }
}
