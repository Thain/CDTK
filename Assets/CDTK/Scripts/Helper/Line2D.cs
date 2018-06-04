using UnityEngine;
using System.Collections;

namespace CDTK
{
	//a class representing a line in any capacity: either a boundary of the level or a drawn edge
    public class Line2D
    {
        //the endpoints of the line segment
        public Vector2 p1;
        public Vector2 p2;

        //constructor
        public Line2D(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

		//checks if the given point is on this line segment or not
		public bool OnSegment(Vector2 pt)
		{
			if (pt.x <= Mathf.Max(this.p1.x, this.p2.x) && pt.x >= Mathf.Min(this.p1.x, this.p2.x) &&	pt.y <= Mathf.Max(this.p1.y, this.p2.y) && pt.y >= Mathf.Min(this.p1.y, this.p2.y)) return true;
			else return false;
		}
		
		//finds out if the intersection between this line and the given one is at a corner, but ensures not at two, making them collinear
		public bool CornerIntersect(Line2D other){
			if((other.p1.Equals(this.p1)||other.p1.Equals(this.p2))^(other.p2.Equals(this.p1)||other.p2.Equals(this.p2))) return true;
			else return false;
		}

		//checks the orientation of three points. 0 for collinear, 1 for clockwise, 2 for counterclockwise
		public int Orientation(Vector2 pt)
		{
			double val = (this.p2.y - this.p1.y) * (pt.x - this.p2.x) - (this.p2.x - this.p1.x) * (pt.y - this.p2.y);
			if(val == 0) return 0;
			return (val > 0)? 1: -1;
		}

		//figures out if two lines intersect, with special specifications for our purposes
		//for us, an intersection where the two lines share one but not both vertices is not an intersection.
		public bool Intersect(Line2D other)
		{
			int o1 = this.Orientation(other.p1);
		 	int o2 = this.Orientation(other.p2);
		 	int o3 = other.Orientation(this.p1);
		 	int o4 = other.Orientation(this.p2);

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
			float m = (this.p1.y - this.p2.y)/(this.p1.x - this.p2.x);
			float b = this.p1.y - (this.p1.x * m);
			if((check.p1.x * m) + b == check.p1.y || (check.p2.x * m) + b == check.p2.y) return true;
			else return false;
		}
    }
}
