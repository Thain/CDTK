using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{
    // Class to represent a closed polygon. This is serializable so we can
    // store it in a scriptable object (LevelGeometry) to save our
    // results from compiling geometry
    public class Polygon : MonoBehaviour
    {
        // the raw data that represents these polygons.
		List<Vector2> vertices;		
		List<Line2D> edges;

        public void SetVertices(List<Vector2> vertices){this.vertices = vertices;}
        // Adds a vertex as a neighbor to the last and first vertex in the loop
        public void AppendVertex(Vector2 vert){vertices.Add(vert);}
        // Removes a given vertex
        public void RemoveVertex(Vector2 vert){vertices.Remove(vert);}

        // Returns this polygon in a way unity can process with meshes
        public Vector2[] GetRaw2() {return vertices.ToArray();}

        public Vector3[] GetRaw3()
        {
            List<Vector3> vec3 = new List<Vector3>();
            foreach (Vector2 vertex in vertices)
            {
                vec3.Add(new Vector3(vertex.x, vertex.y, 0));
            }

            return vec3.ToArray();
        }

		#region Editor
		private void OnDrawGizmosSelected()
		{
			Vector3[] poly = GetRaw3();
			int i;
			for (i = 0; i < poly.Length; i++)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(poly[i], 0.1f);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(poly[i], poly[(i + 1) % poly.Length]);
			}
		}
		#endregion
       
    }
}