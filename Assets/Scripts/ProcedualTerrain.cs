using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ProcedualTerrain : MonoBehaviour
{
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Vector2Int _mapSize;
    [SerializeField]
    private float _cellSize;
    [SerializeField]
    private Vector2Int _chunkSize;
    [SerializeField]
    private NavMeshSurface _navigationSurface;
    [SerializeField]
    private float _noiseSeed;
    [SerializeField]
    private float _perlinScale;
    [SerializeField]
    private float _perlinPower;
    [SerializeField]
    private float _perlinStrength;

    private void Awake()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.y; x++)
            {
                CreateChunk(new Vector3Int(x, 0, y));
            }
        }
        var centerX = -_mapSize.x * _cellSize * _chunkSize.x / 2;
        var centerZ = -_mapSize.y * _cellSize * _chunkSize.y / 2;
        transform.position = new Vector3(centerX, 0, centerZ);
        _navigationSurface.BuildNavMesh();
    }

    private GameObject CreateChunk(Vector3Int chunkPosition)
    {
        var mesh = GenerateMesh(new Vector2(chunkPosition.x * _chunkSize.x, chunkPosition.z * _chunkSize.y));
        var chunk = new GameObject($"Chunk {chunkPosition}");
        chunk.gameObject.layer = gameObject.layer;
        chunk.transform.SetParent(transform);
        var x = chunkPosition.x * _chunkSize.x * _cellSize;
        var z = chunkPosition.z * _chunkSize.y * _cellSize;
        chunk.transform.position = new Vector3(x, 0, z);
        var filter = chunk.AddComponent<MeshFilter>();
        var renderer = chunk.AddComponent<MeshRenderer>();
        var collider = chunk.AddComponent<MeshCollider>();
        filter.mesh = mesh;
        renderer.sharedMaterial = _material;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        collider.sharedMesh = mesh;
        chunk.isStatic = true;
        return chunk;
    }

    private Mesh GenerateMesh(Vector2 noiseOffset)
    {
        var tris = new List<int>();
        var verts = new List<Vector3>();
        var mesh = new Mesh();
        var center = new Vector3(_chunkSize.x, 0, _chunkSize.y) * _cellSize / 2;
        for (int y = 0; y < _chunkSize.y + 1; y++)
        {
            for (int x = 0; x < _chunkSize.x + 1; x++)
            {
                var sampleX = (x + noiseOffset.x + _noiseSeed) * _perlinScale;
                var sampleY = (y + noiseOffset.y + _noiseSeed) * _perlinScale;
                var height = Mathf.PerlinNoise(sampleX, sampleY);
                height = Mathf.Pow(height, _perlinPower) * _perlinStrength;
                for (int i = 0; i < 6; i++)
                {
                    verts.Add(new Vector3(x * _cellSize, height, y * _cellSize) - center);
                }
            }
        }
        mesh.vertices = verts.ToArray();
        var trisCount = _chunkSize.x * (_chunkSize.y + 1) * 6;
        for (int i = 0; i < trisCount; i += 6)
        {
            if ((i + 6) % ((_chunkSize.x * 6) + 6) == 0)
            {
                continue;
            }
            //tris 1
            tris.Add(GetUniqueValueForVert(i, tris));
            tris.Add(GetUniqueValueForVert((_chunkSize.x + 1) * 6 + i, tris));
            tris.Add(GetUniqueValueForVert((_chunkSize.x + 2) * 6 + i, tris));

            //tris 2
            tris.Add(GetUniqueValueForVert((_chunkSize.x + 2) * 6 + i + 1, tris));
            tris.Add(GetUniqueValueForVert(i + 6, tris));
            tris.Add(GetUniqueValueForVert(i + 1, tris));
        }
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    private int GetUniqueValueForVert(int value, List<int> list)
    {
        var i = 0;
        var maxValue = i + 6;
        while (list.Contains(value))
        {
            i++;
            if (i > maxValue)
            {
                throw new Exception("Max value exceded!!!");
            }
            value++;
        }
        return value;
    }

    [SerializeField]
    private class PopulatedObject
    {
        public string ResourcePath;
        public float SpawnHeight;
    }
}
