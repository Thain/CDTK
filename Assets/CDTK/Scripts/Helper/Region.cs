using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CDTK
{
    // Class to represent 
    public class Region
    {
        // the raw data
		LinkedList<Vertex> vertices;	
        Vertex center;
		LinkedList<Line> edges;

        // Constructor for making a region out of the vertices that make it up: generally to be used for creating triangles
        public Region(Vertex a, Vertex b, Vertex c, Line lineA, Line lineB, Line lineC){
            vertices = new LinkedList<Vertex>(new Vertex[]{a, b, c});
            edges = new LinkedList<Line>(new Line[]{lineA, lineB, lineC});

            center = new Vertex(new Vector2((a.val.x + b.val.x + c.val.x)/3, (a.val.y + b.val.y + c.val.y)/3));
        }

        // Constructor for combining to smaller regions to make a bigger one: main constructor for the convex decomposition. 
        public Region(Region rCW, Region rCCW, Line border){
            vertices = new LinkedList<Vertex>();
            edges = new LinkedList<Line>();

            // string uh1 = ""; string uh2 = "";
            // foreach(Vertex v in rCW.vertices)
            //     uh1 += v + " // ";
            // foreach(Vertex v in rCCW.vertices)
            //     uh2 += v + " // ";
            // Debug.Log("Combining " + uh1 + " and " + uh2 + " over " + border);

            LinkedListNode<Vertex> cur; LinkedListNode<Vertex> other;
            LinkedListNode<Vertex> borderA = rCCW.vertices.Find(border.a);
            LinkedListNode<Vertex> borderB = rCCW.vertices.Find(border.b); 
            LinkedListNode<Vertex> borderANext = borderA.Next ?? borderA.List.First;
            LinkedListNode<Vertex> borderBNext = borderB.Next ?? borderB.List.First;

            if(borderANext.Value == border.b) {cur = borderB; other = borderA;}
       else if(borderBNext.Value == border.a) {cur = borderA; other = borderB;}
            else {cur = null; other = null; Debug.Log("ERROR ON REGION CREATION AGGREGATE");}

            while(cur.Value != other.Value){
                vertices.AddLast(cur.Value);
                cur = cur.Next ?? cur.List.First;
            }

            // Now switch regions.
            cur = rCW.vertices.Find(other.Value);
            while(cur.Value != vertices.First.Value){
                vertices.AddLast(cur.Value);
                cur = cur.Next ?? cur.List.First;
            }

            LinkedListNode<Line> curLine = rCCW.edges.Find(border).Next ?? rCCW.edges.First;
            LinkedListNode<Line> otherLine = curLine.Previous ?? curLine.List.Last;

            while(curLine.Value != otherLine.Value){
                edges.AddLast(curLine.Value);
                curLine = curLine.Next ?? curLine.List.First;
            }

            // Now switch regions.
            curLine = rCW.edges.Find(border).Next ?? rCW.edges.First;
            otherLine = curLine.Previous ?? curLine.List.Last;
            while(curLine.Value != otherLine.Value){
                edges.AddLast(curLine.Value);
                curLine = curLine.Next ?? curLine.List.First;
            }

            // Averages the old two centers to get the new one.
            center = new Vertex(new Vector2((rCW.center.val.x + rCCW.center.val.x)/2,(rCW.center.val.y + rCCW.center.val.y)/2));

        }

        public Vertex GetCenter(){return center;}
        public Line[] GetEdges(){return edges.ToArray();}
        // Returns the vertices, listed counterclockwise
        public Vertex[] GetVertices() {return vertices.ToArray();}

        
        // Checks if this Region is a convex polygon
        // Chooses three vertices, and sees if the angle that they create is obtuse or not. If it is, then this isn't a convex region.
        public bool IsConvex(){
            LinkedListNode<Vertex> cur = vertices.First; LinkedListNode<Vertex> prev, next;
            bool conv = true;

            while(cur != vertices.Last){
                prev = cur.Previous ?? cur.List.Last;
                next = cur.Next ?? cur.List.First;
                 if(Vertex.Orientation(prev.Value, cur.Value, next.Value) == 1){
                    conv = false;
                    break;
                }
                cur = cur.Next ?? vertices.First;
            }
            
            return conv;
        }
        
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