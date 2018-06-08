using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{
    // Class to represent 
    public class Region : MonoBehaviour
    {
        // the raw data
		List<Vector2> vertices;		
		List<Line2D> edges;

        // Constructor for making a region out of the vertices that make it up: generally to be used for creating triangles
        public Region(List<Vector2> verts){
            vertices = verts;
        }
        
        // Constructor for combining to smaller regions to make a bigger one: main constructor for the convex decomposition. 
        public Region(Region r1, Region r2){
            //Will need to add all vertices without adding duplicates. Also needs to list them counterclockwise order.
            //Best approach: find the 2 shared vertices, and start on one polygon adding counterclockwise order. when one of the two
            //vertices is reached, start adding for the other polygon FROM THAT VERTEX. Then, go until the other vertex is reached,
            //and switch back to adding from the first polygon until it loops back to the beginning.
        }
        
        // Checks if this Region is a convex polygon
        public bool IsConvex(){
            // just for now, so the compiler doesn't complain.
            return true;
        }
        // Adds a vertex
        public void AppendVertex(Vector2 vert){vertices.Add(vert);}
        // Removes a  vertex
        public void RemoveVertex(Vector2 vert){vertices.Remove(vert);}

        // Returns the vertices, listed counterclockwise
        public Vector2[] GetVertices() {return vertices.ToArray();}

        // NOTE: TO BE EDITED LATER
		#region Editor
		// private void OnDrawGizmosSelected()
		// {
		// 	Vector3[] poly = GetRaw3();
		// 	int i;
		// 	for (i = 0; i < poly.Length; i++)
		// 	{
		// 		Gizmos.color = Color.cyan;
		// 		Gizmos.DrawSphere(poly[i], 0.1f);
		// 		Gizmos.color = Color.red;
		// 		Gizmos.DrawLine(poly[i], poly[(i + 1) % poly.Length]);
		// 	}
		// }
		#endregion
       
    }
}