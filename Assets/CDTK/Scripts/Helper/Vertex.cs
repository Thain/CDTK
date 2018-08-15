using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{
    // Class to represent a singular vertex
    public class Vertex
    {
        // the raw data of the vertex
		public Vector2 val;	
        // The list of all edges stemming from this vertex.
        List<Line> neighbors = new List<Line>();
        LineList sortedEdges = new LineList();

        public Vertex(Vector2 pt){
            val = pt;
        }
        
        // Adds to the list of edges stemming from this vertex.
        public void AddNeighbor(Line line){neighbors.Add(line);}
        // Removes from the list of edges
        public void RemoveNeighbor(Line line){neighbors.Remove(line);}
        // Gets the list of edges stemming from this vertex
        public List<Line> GetEdges(){return neighbors;}
        public LineList GetSortedEdges(){return sortedEdges;}

        public Line NextLine(Line l){return sortedEdges.Get(l).next.value;}
        public Line PrevLine(Line l){return sortedEdges.Get(l).prev.value;}


        public void SortEdges(){
            // First, need to assign numeric value to all edges by calculating their angles relative to any random line.
            Vertex zeroV = neighbors[0].otherPt(this);
            float angle = 0;
            foreach(Line l in neighbors){
                // if this is the zero line, its angle is just zero.
                if(zeroV == l.otherPt(this)) angle = 0;
                // if its not the zero line, there's a more complicated situation to calculate the angle.
                else{
                    int o = Orientation(this, zeroV, l.otherPt(this));
                    if(o == 0) angle = 180;
                    if(o == -1) angle = LawOfCosines(zeroV, this, l.otherPt(this));
                    if(o == 1) angle = 360 - LawOfCosines(zeroV, this, l.otherPt(this));
                }
                sortedEdges.AddSorted(l, angle);
            }
        }

        public float LawOfCosines(Vertex A, Vertex B, Vertex C){
            float a = new Line(B, C).calcLength();
            float b = new Line(A, C).calcLength();
            float c = new Line(A, B).calcLength();
            return (180/Mathf.PI)*(Mathf.Acos((Mathf.Pow(b,2) - Mathf.Pow(a,2) - Mathf.Pow(c,2))/(-2*a*c)));
        }

        //checks the orientation of three points. 0 for collinear, 1 for clockwise, -1 for counterclockwise
		public static int Orientation(Vertex p, Vertex q, Vertex r)
		{
			double val = (q.val.y - p.val.y) * (r.val.x - q.val.x) - (q.val.x - p.val.x) * (r.val.y - q.val.y);
 			if(val == 0) return 0;
			return (val > 0)? 1: -1;
		}

        public override string ToString(){
			return val.x + ", " + val.y;
		}

        public bool Equals(Vertex other){
			if(this.val.Equals(other.val)) return true;
            else return false;
		}
        // // NOTE: TO BE EDITED LATER
		// #region Editor
		// private void OnDrawGizmos()
		// {
		// 	Gizmos.color = Color.cyan;
		// 	Gizmos.DrawSphere(GetValue(), 0.1f);
		// }
		// #endregion
       
    }
}