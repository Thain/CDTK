using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace CDTK {
	public class Triangulation : MonoBehaviour {
		public GameObject level;

		public void TriangulateLevel () {
			//gets the polygon objects out of the level component, from GRTK. If using a different method of supplying the polygon, this needs to be changed.
			GRTK.Polygon [] polyArray = level.GetComponents<GRTK.Polygon>();

			VertexGraph vg = Undo.AddComponent<VertexGraph>(gameObject);

			//adds all the polygons from the GRTK to the vertex graph. Starts at 1 rather than 0 because the outermost polygon in the GRTK method of level building is not part of the level.
			for(int i = 1; i < polyArray.Length - 1; i++)
				vg.AddPolygon (polyArray[i].GetRaw2());
			
			//multiple debug cases in case they are necessary.
				// Line2D bound = new Line2D (f, g);
				// Line2D draw = new Line2D (g, i);
				// Debug.Log ("false? " + draw.Intersect (bound));

				// Line2D attempt = new Line2D (t3, d);
				// Debug.Log ("true? " + vg.IntersectsNothing (attempt));

			//First, need to draw the lines between vertices, granted they don't intersect any other lines (boundaries or drawn)
			foreach(Vector2 start in vg.GetVertices())
				foreach (Vector2 end in vg.GetVertices()) {
					Line2D possibleEdge = new Line2D (start, end);
					if (vg.IntersectsNothing(possibleEdge) && !end.Equals(start))
						vg.DrawEdge(possibleEdge);

				}

			//Now, to delete all lines which are either in obstacles or exterior to the level.
			//If a point on a drawn line which has an infinite line drawn off in some direction has an even number of intersections with
			//the boundaries of the polygon, it is either outside of it or in an obstacle.
			foreach(Line2D line in vg.GetDrawnEdges())
				if(NumIntersects(line,vg)%2 == 0) vg.EraseEdge(line);
		}

		//counts the number of times an infinite line drawn between the point on the line given and some point
		//outside the polygon intersects the boundaries of the polygon.
		public int NumIntersects(Line2D line, VertexGraph vg){
			int count = 0;
			bool skip = false;

			//creates the "infinite" line, from a point on the drawn line to somewhere outside the polygon
			Line2D infLine = new Line2D(new Vector2(((line.p1.x + line.p2.x)/2),((line.p1.y + line.p2.y)/2)), new Vector2(vg.maximumX() + 1,vg.maximumY() + 1));

			//Debug.Log(pt.x + " " + pt.y + ", " + infPt.x + " " + infPt.y);

			Line2D [] edges = vg.GetBoundEdges();
			foreach(Line2D bound in edges)
			{
				//if the infline intersects with the bound edge, increment counter.
				//however, we want to skip the count if:
				//this is the second of two edges which connect at a vertex the infline intersects
				//(going from inside the shape to outside or vice versa)
				//both lines if its at a vertex where both lines are on the same side of the infline.
				//(staying either inside or outside of the shape)
				//so to generalize:
					//theres no skip business at all if it's not intersecting a vertex.
					//if it is intersecting a vertex, then check if the lines fall on the same side of the line.
						//if they are, skip both this and the next line.
						//if they're not, skip only this one and not the next.
				int ind; if(Array.IndexOf(edges, bound) + 1 == edges.Length -1) ind = Array.IndexOf(edges, bound) + 1; else ind = 0;
				int otherPoint1; int otherPoint2;
				if(infLine.Orientation(bound.p1) == 0) otherPoint1 = infLine.Orientation(bound.p2); else otherPoint1 = infLine.Orientation(bound.p1);
				if(infLine.Orientation(edges[ind].p1) == 0) otherPoint2 = infLine.Orientation(edges[ind].p2); else otherPoint2 = infLine.Orientation(edges[ind].p1);
				
				//this part is tricky.
				//check if the lines are on the same side or opposite sides of the infLine.
				//opposite sides: dont skip this one, but do skip the next.
				//same side: skip both this and the next.
				//therefore, either way, we're skipping the next one. However, if they're on the same side, count++ right now.
						
				if(skip) continue;
				if(infLine.Intersect(bound))
					if(infLine.onInfLine(bound)){
						if(otherPoint1 != otherPoint2) count++;
						skip = true;
					}
					else count++;

			}
			return count;
		}

	}
}
