using UnityEngine;
using System.Collections;

namespace CDTK
{
	//a class representing a line in any capacity: either a boundary of the level or a drawn edge
    public class Line
    {
        //the endpoints of the line segment
        public Vertex a;
        public Vertex b;
		public Region cw = null;
		public Region ccw = null;

        //constructor
        public Line(Vector2 q1, Vector2 q2)
        {
            this.a = new Vertex(q1);
            this.b = new Vertex(q2);
        }
		 public Line(Vertex q1, Vertex q2)
        {
            this.a = q1;
            this.b = q2;
        }

		public Vertex otherPt(Vertex v){
			if(v == a) return b;
			else if (v == b) return a;
			else {Debug.Log("ERROR ON OTHERPT FUNC"); return null;}
		}

		public Line NextLine(Vertex v){
			return otherPt(v).NextLine(this);
		}
		public Line PrevLine(Vertex v){
			return otherPt(v).PrevLine(this);
		}
		
		//checks if the given point is on this line segment or not
		public bool OnSegment(Vertex pt)
		{
			if (pt.val.x <= Mathf.Max(this.a.val.x, this.b.val.x) && pt.val.x >= Mathf.Min(this.a.val.x, this.b.val.x) &&	pt.val.y <= Mathf.Max(this.a.val.y, this.b.val.y) && pt.val.y >= Mathf.Min(this.a.val.y, this.b.val.y)) return true;
			else return false;
		}
		
		//finds out if the intersection between this line and the given one is at a corner, but ensures not at two, making them collinear
		public bool CornerIntersect(Line other){
			if((other.a.val.Equals(this.a.val)||other.a.val.Equals(this.b.val))^(other.b.val.Equals(this.a.val)||other.b.val.Equals(this.b.val))) return true;
			else return false;
		}

		//figures out if two lines intersect, with special specifications for our purposes
		//for us, an intersection where the two lines share one but not both vertices is not an intersection.
		public bool Intersect(Line other)
		{
			int o1 = Vertex.Orientation(this.a, this.b, other.a);
		 	int o2 = Vertex.Orientation(this.a, this.b, other.b);
		 	int o3 = Vertex.Orientation(other.a, other.b, this.a);
		 	int o4 = Vertex.Orientation(other.a, other.b, this.b);

			// Special cases, for when they're collinear (one line is on another)
			if (o1 == 0 && this.OnSegment(other.a)) if(!CornerIntersect(other)) return true; else return false;
			if (o2 == 0 && this.OnSegment(other.b)) if(!CornerIntersect(other)) return true; else return false;
			if (o3 == 0 && other.OnSegment(this.a)) if(!CornerIntersect(other)) return true; else return false;
			if (o4 == 0 && other.OnSegment(this.b)) if(!CornerIntersect(other)) return true; else return false;

			// General case
			if (o1 != o2 && o3 != o4) return true;

			return false;
		}
		
		//checks if either of the endpoints of the given line are on an infinite line in y = mx + b form formed by this line
		public bool onInfLine(Line check){
			float m = (this.a.val.y - this.b.val.y)/(this.a.val.x - this.b.val.x);
			float b = this.a.val.y - (this.a.val.x * m);
			if(Mathf.Abs((check.a.val.x * m) + b - check.a.val.y) < .000001 || Mathf.Abs((check.b.val.x * m) + b - check.b.val.y) < .000001) return true;
			else return false;
		}

		public float calcLength(){
			Vector2 p = this.a.val; Vector2 q = this.b.val;
			return Mathf.Sqrt(Mathf.Pow((p.x - q.x), 2) + Mathf.Pow((p.y - q.y),2));
		}

		public override string ToString(){
			return this.a.val.x + ", " + this.a.val.y + " --> " + this.b.val.x + ", " + this.b.val.y;
		}
    }
}
