using UnityEngine;

namespace com.ethnicthv
{
    public class Box
    {
        private Vector3[] _vertices = new Vector3[8];
        private int[] _triangles = new int[]
        {
            0, 1, 2, 0, 2, 3, //front
            1, 5, 6, 1, 6, 2, //right
            5, 4, 7, 5, 7, 6, //back
            4, 0, 3, 4, 3, 7, //left
            3, 2, 6, 3, 6, 7, //top
            4, 5, 1, 4, 1, 0  //bottom
        };
    
        /// <summary>
        /// method to apply the vertices to the mesh at the given position
        /// </summary>
        /// <param name="position"> Position that this Box will place on </param>
        /// <returns></returns>
        public (Vector3[], int[]) Apply(Vector3 position)
        {
            //set the vertices of the box
            _vertices[0] = new Vector3(-0.5f, -0.5f, -0.5f) + position;
            _vertices[1] = new Vector3(0.5f, -0.5f, -0.5f) + position;
            _vertices[2] = new Vector3(0.5f, 0.5f, -0.5f) + position;
            _vertices[3] = new Vector3(-0.5f, 0.5f, -0.5f) + position;
            _vertices[4] = new Vector3(-0.5f, -0.5f, 0.5f) + position;
            _vertices[5] = new Vector3(0.5f, -0.5f, 0.5f) + position;
            _vertices[6] = new Vector3(0.5f, 0.5f, 0.5f) + position;
            _vertices[7] = new Vector3(-0.5f, 0.5f, 0.5f) + position;
            
            return (_vertices, _triangles);
        }
    }
}
