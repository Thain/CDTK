using UnityEngine;
using System.Collections;

namespace CDTK
{
	//a class representing a line in any capacity: either a boundary of the level or a drawn edge
    public class Line2D
    {
        //the endpoints of the line segment
        public Vertex p1;
        public Vertex p2;
		public Region cw = null;
		public Region ccw = null;

        //constructor
        public Line2D(Vector2 q1, Vector2 q2)
        {
            this.p1 = new Vertex(q1);
            this.p2 = new Vertex(q2);
        }
		 public Line2D(Vertex q1, Vertex q2)
        {
            this.p1 = q1;
            this.p2 = q2;
        }

		public Vertex otherPt(Vertex v){
			if(v == p1) return p2;
			else if (v == p2) return p1;
			else {Debug.Log("HELP"); return null;}
		}

		public void setCW(Region r){
			cw = r;
		}

		public void setCCW(Region r){
			ccw = r;
		}

		//checks if the given point is on this line segment or not
		public bool OnSegment(Vertex pt)
		{
			if (pt.GetValue().x <= Mathf.Max(this.p1.GetValue().x, this.p2.GetValue().x) && pt.GetValue().x >= Mathf.Min(this.p1.GetValue().x, this.p2.GetValue().x) &&	pt.GetValue().y <= Mathf.Max(this.p1.GetValue().y, this.p2.GetValue().y) && pt.GetValue().y >= Mathf.Min(this.p1.GetValue().y, this.p2.GetValue().y)) return true;
			else return false;
		}
		
		//finds out if the intersection between this line and the given one is at a corner, but ensures not at two, making them collinear
		public bool CornerIntersect(Line2D other){
			if((other.p1.GetValue().Equals(this.p1.GetValue())||other.p1.GetValue().Equals(this.p2.GetValue()))^(other.p2.GetValue().Equals(this.p1.GetValue())||other.p2.GetValue().Equals(this.p2.GetValue()))) return true;
			else return false;
		}

		//figures out if two lines intersect, with special specifications for our purposes
		//for us, an intersection where the two lines share one but not both vertices is not an intersection.
		public bool Intersect(Line2D other)
		{
			int o1 = Vertex.Orientation(this.p1, this.p2, other.p1);
		 	int o2 = Vertex.Orientation(this.p1, this.p2, other.p2);
		 	int o3 = Vertex.Orientation(other.p1, other.p2, this.p1);
		 	int o4 = Vertex.Orientation(other.p1, other.p2, this.p2);

			// Special cases, for when they're collinear (one line is on another)
			if (o1 == 0 && this.OnSegment(other.p1)) if(!CornerIntersect(other)) return true; else return false;
			if (o2 == 0 && this.OnSegment(other.p2)) if(!CornerIntersect(other)) return true; else return false;
			if (o3 == 0 && other.OnSegment(this.p1)) if(!CornerIntersect(other)) return true; else return false;
			if (o4 == 0 && other.OnSegment(this.p2)) if(!CornerIntersect(other)) return true; else return false;

			// General case
			if (o1 != o2 && o3 != o4) return true;

			return false;
		}
		
		//checks if either of the endpoints of the given line are on an infinite line in y = mx + b form formed by this line
		public bool onInfLine(Line2D check){
			float m = (this.p1.GetValue().y - this.p2.GetValue().y)/(this.p1.GetValue().x - this.p2.GetValue().x);
			float b = this.p1.GetValue().y - (this.p1.GetValue().x * m);
			if(Mathf.Abs((check.p1.GetValue().x * m) + b - check.p1.GetValue().y) < .000001 || Mathf.Abs((check.p2.GetValue().x * m) + b - check.p2.GetValue().y) < .000001) return true;
			else return false;
		}

		public float calcLength(){
			Vector2 p = this.p1.GetValue(); Vector2 q = this.p2.GetValue();
			return Mathf.Sqrt(Mathf.Pow((p.x - q.x), 2) + Mathf.Pow((p.y - q.y),2));
		}

		public override string ToString(){
			return "x1: " + this.p1.GetValue().x + ", y1: " + this.p1.GetValue().y + "\nx2: " + this.p2.GetValue().x + ", y2: " + this.p2.GetValue().y;
		}
    }
}
