using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace com.ethnicthv
{
    public class BoxPackBehaviour : MonoBehaviour
    {
        private const int BoxCount = 5000;
        private const int GridX = 50;
        readonly LinkedList<int> _dirtyBoxes = new();

        private readonly BoxPositionInfo[] _boxPositionInfos = new BoxPositionInfo[BoxCount];
        
        public Material material;
        
        private Mesh _mesh;
        private RenderParams _rp;
        private readonly Matrix4x4[] _matrices = new Matrix4x4[1];
        private Vector3[] _firstVertices;
        private Vector3[] _secondVertices;
        private int[] _triangles;

        private bool _pingPong = true;

        private readonly int[] _defaultTriangles =
        {
            0, 2, 1, //face front
            0, 3, 2,
            2, 3, 4, //face top
            2, 4, 5,
            1, 2, 5, //face right
            1, 5, 6,
            0, 7, 4, //face left
            0, 4, 3,
            5, 4, 7, //face back
            5, 7, 6,
            0, 6, 7, //face bottom
            0, 1, 6
        };
        

        private readonly Vector3[] _defaultVertices =
        {
            new(-0.5f, -0.5f, -0.5f),
            new(0.5f, -0.5f, -0.5f),
            new(0.5f, 0.5f, -0.5f),
            new(-0.5f, 0.5f, -0.5f),
            new(-0.5f, 0.5f, 0.5f),
            new(0.5f, 0.5f, 0.5f),
            new(0.5f, -0.5f, 0.5f),
            new(-0.5f, -0.5f, 0.5f)
        };

        private void Awake()
        {
            Debug.Log("Awake method called");
            //init the mesh
            var triangles = new int[BoxCount * 36];
            var vertices = new Vector3[BoxCount * 8];

            var seed = UsefulConstant.Seed;
            for (var i = 0; i < BoxCount; i++)
            {
                var numOfGapX = i % GridX;
                var numOfGapY = i / GridX;
                var position = new Vector3(numOfGapX * 1.5f, 0, numOfGapY * 1.5f);
                var verticesIndex = new[]
                {
                    i * 8,
                    i * 8 + 1,
                    i * 8 + 2,
                    i * 8 + 3,
                    i * 8 + 4,
                    i * 8 + 5,
                    i * 8 + 6,
                    i * 8 + 7
                };

                //init the mesh
                for (int j = 0; j < 36; j++)
                {
                    if (j < 8)
                    {
                        vertices[i * 8 + j] = _defaultVertices[j] + position;
                    }

                    triangles[i * 36 + j] = verticesIndex[_defaultTriangles[j]];
                }

                _boxPositionInfos[i] = new BoxPositionInfo(position);
                Random.InitState(seed);
                var d = Random.Range(0, 100);
                _boxPositionInfos[i].MoveWithVelocity(i%7 * 10 + 1, d > 50 ? 1 : 0);
                _dirtyBoxes.AddLast(i);
            }

            //clone the vertices
            _firstVertices = vertices;
            _secondVertices = (Vector3[])vertices.Clone();

            _triangles = triangles;

            _mesh = new Mesh
            {
                vertices = (Vector3[])vertices.Clone(),
                triangles = (int[])triangles.Clone()
            };
            _mesh.RecalculateNormals();
            Debug.Log("Awake method finished");
        }

        private void Start()
        {
            Debug.Log("Start method called");
            material.enableInstancing = true;
            _rp = new RenderParams(material);
            _matrices[0] = Matrix4x4.identity;
            UpdateMesh();
            var i = Random.Range(0, 10);
            var d = Random.Range(0,1);
            _boxPositionInfos[i].MoveWithVelocity(10, d);
            Debug.Log("Start method finished");
        }

        int _c;
        
        private void FixedUpdate()
        {
            if (_c == BoxCount)
            {
                _boxPositionInfos[_c].MoveWithVelocity(60, direction: 1);
                _c++;
            }
            UpdateMesh();
        }

        private void Update()
        {
            _matrices[0] = Matrix4x4.Translate(transform.position);
            Graphics.RenderMeshInstanced(_rp, _mesh, 0, _matrices);
        }

        private void UpdateMesh()
        {
            if (_pingPong)
            {
                RecalculateVertices(_firstVertices);
                _mesh.vertices = _firstVertices;
                _mesh.triangles = _triangles;
                _mesh.RecalculateNormals();
                _pingPong = false;
            }
            else
            {
                RecalculateVertices(_secondVertices);
                _mesh.vertices = _secondVertices;
                _mesh.triangles = _triangles;
                _mesh.RecalculateNormals();
                _pingPong = true;
            }
        }

        private void RecalculateVertices(Vector3[] vertices)
        {
            for (int index = 0; index < BoxCount; index++)
            {
                _boxPositionInfos[index].UpdatePosition();
                var position = _boxPositionInfos[index].Position;
                vertices[index * 8] = new Vector3(-0.5f, -0.5f, -0.5f) + position;
                vertices[index * 8 + 1] = new Vector3(0.5f, -0.5f, -0.5f) + position;
                vertices[index * 8 + 2] = new Vector3(0.5f, 0.5f, -0.5f) + position;
                vertices[index * 8 + 3] = new Vector3(-0.5f, 0.5f, -0.5f) + position;
                vertices[index * 8 + 4] = new Vector3(-0.5f, -0.5f, 0.5f) + position;
                vertices[index * 8 + 5] = new Vector3(0.5f, -0.5f, 0.5f) + position;
                vertices[index * 8 + 6] = new Vector3(0.5f, 0.5f, 0.5f) + position;
                vertices[index * 8 + 7] = new Vector3(-0.5f, 0.5f, 0.5f) + position;
            }
        }
    }
}