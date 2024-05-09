using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace com.ethnicthv
{
    public class MainBehaviour: MonoBehaviour
    {
        
        private Box[] _boxes = new Box[10000];
        private BoxPositionInfo[] _boxPositionInfos = new BoxPositionInfo[10000];
        
        private Mesh _firstMesh;
        private Mesh _secondMesh;
        
        private bool _pingPong = false;
        
        private MeshFilter _meshFilter;
        
        private void Start()
        {
            for (var i = 0; i < 10000; i++)
            {
                _boxes[i] = new Box();
                _boxPositionInfos[i] = new BoxPositionInfo(new Vector3(i % 100, 0, i / 100));
            }
            _firstMesh = new Mesh();
            _firstMesh.triangles = new int[360000];
            _firstMesh.vertices = new Vector3[80000];
            _secondMesh = new Mesh();
            _secondMesh.triangles = new int[360000];
            _secondMesh.vertices = new Vector3[240000];
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void UpdateMesh()
        {
            if (_pingPong)
            {
                RecalculateMesh(_firstMesh);
                _meshFilter.sharedMesh = _firstMesh;
                _pingPong = false;
            }
            else
            {
                RecalculateMesh(_secondMesh);
                _meshFilter.sharedMesh = _secondMesh;
                _pingPong = true;
            }
        }
        
        private void FixedUpdate()
        {
            for (var i = 0; i < 10000; i++)
            {
                if (Random.Range(0, 100) >= 50) continue;
                _boxPositionInfos[i].Accelerate(10, 1);
                _boxPositionInfos[i].UpdatePosition(Time.fixedDeltaTime);
            }
            UpdateMesh();
        }
        
        private void RecalculateMesh(Mesh mesh)
        {
            for (var i = 0; i < 10000; i++)
            {
                //apply the vertices to the mesh
                var (vertices, triangles) = _boxes[i].Apply(_boxPositionInfos[i].Position);
                for(var j = i * 8; j < (i + 1) * 8; j++)
                {
                    mesh.vertices[j] = vertices[j % 8];
                }
                for(var j = i * 36; j < (i + 1) * 36; j++)
                {
                    mesh.triangles[j] = triangles[j % 36];
                }
            }
        }
    }
}