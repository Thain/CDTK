using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{
    // Class to represent 
    public class Region
    {
        // the raw data
		List<Vertex> vertices;	

        // not sure if I even need this. Keeping it around just in case	
		List<Line2D> edges;

        // Constructor for making a region out of the vertices that make it up: generally to be used for creating triangles
        public Region(Vertex v1, Vertex v2, Vertex v3){
            vertices = new List<Vertex>(new Vertex[]{v1, v2, v3});
        }
        
        // Constructor for combining to smaller regions to make a bigger one: main constructor for the convex decomposition. 
        public Region(Region r1, Region r2, Line2D border){
            // These two points represent the vertices where the polygons intersect. They will be important later.
            Vertex interP = border.p1; Vertex interQ = border.p2;

            Vertex cur;
            // Addrange approach (is there a get for lists?)
            for(int i = 0; i < r1.vertices.Count; i++){
                cur = r1.vertices[i];
                if(cur.Equals(interP)){

                }
                if(cur.Equals(interQ)){
                    AppendVertex(cur);
                    for()
                    //initiate another for loop, going from one intersection to the other on the other polygon.
                }
                else AppendVertex(cur);
            }
            // Will need to add all vertices without adding duplicates. Also needs to list them counterclockwise order.
            // Best approach: find the 2 shared vertices, and start on one polygon adding counterclockwise order. when one of the two
            // vertices is reached, start adding for the other polygon FROM THAT VERTEX. Then, go until the other vertex is reached,
            // and switch back to adding from the first polygon until it loops back to the beginning.
        }
        
        // Checks if this Region is a convex polygon
        public bool IsConvex(){
            Vertex prev; Vertex cur; Vertex next; int len = vertices.Count;
            bool conv = true;
            for(int i = 0; i < vertices.Count; i++){
                cur = vertices[i];
                if(i == 0){
                    next = vertices[i+1];
                    prev = vertices[len-1];
                }
                else if(i == len - 1) {
                    next = vertices[0];
                    prev = vertices[i-1];
                }
                else {
                    next = vertices[i+1];
                    prev = vertices[i-1];
                }
                //MAKE SURE THAT REGIONS ARE ALWAYS LISTED CCW
                if(Vertex.Orientation(prev, cur, next) == 1){
                    conv = false;
                    break;
                }
            }
            return conv;
        }
        // Adds a vertex
        public void AppendVertex(Vertex vert){vertices.Add(vert);}
        // Removes a  vertex
        public void RemoveVertex(Vertex vert){vertices.Remove(vert);}

        // Returns the vertices, listed counterclockwise
        public Vertex[] GetVertices() {return vertices.ToArray();}

        public override string ToString(){
            string ret = " ";
            foreach(Vertex v in vertices){
                ret = ret + v.ToString() + " ";
            }
            return ret;
        }
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