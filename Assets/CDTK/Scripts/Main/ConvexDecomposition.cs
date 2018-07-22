using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace CDTK {
	public class ConvexDecomposition : MonoBehaviour {
		public GameObject level;

		public void DecomposeLevel () {
			// Gets the polygon objects out of the level component, from GRTK. If using a different method of supplying the polygon, this needs to be changed.
			GRTK.Polygon [] polyArray = level.GetComponents<GRTK.Polygon>();

			VertexGraph vg = Undo.AddComponent<VertexGraph>(gameObject);

			// adds all the polygons from the GRTK to the vertex graph. Starts at 1 rather than 0 because the outermost polygon in the GRTK method of level building is not part of the level.
			for(int i = 1; i < polyArray.Length; i++)
				vg.AddPolygon (polyArray[i].GetRaw2());

			// First, need to draw the lines between vertices, granted they don't intersect any other lines (boundaries or drawn)
			foreach(Vertex start in vg.GetVertices())
				foreach (Vertex end in vg.GetVertices()) {
					Line2D possibleEdge = new Line2D(start, end);
					if (vg.IntersectsNothing(possibleEdge) && !end.Equals(start))
						vg.DrawEdge(possibleEdge);

				}

			// Now, to delete all lines which are either in obstacles or exterior to the level.
			// If a point on a drawn line which has an infinite line drawn off in some direction has an even number of intersections with
			// the boundaries of the polygon, it is either outside of it or in an obstacle. This completes triangulation.
			foreach(Line2D line in vg.GetDrawnEdges())
				if(NumIntersects(line,vg)%2 == 0) vg.EraseEdge(line);

			// Next, to calculate angles for and sort the edges stemming from each vertex, now that they're "finalized" for the purposes of triangulation.
			foreach(Vertex v in vg.GetVertices())
				v.SortEdges();			
			
			// Then, for each of the lines that we drew, there is a region on each side. We want to see if combining them creates a convex shape.
			// Thus we need to first create this web of regions.
			// Then we need to test each pair.
			vg.createRegions();
			Debug.Log("region vertices: " + vg.GetDrawnEdges()[0].cw);
			Debug.Log("Convex: " + vg.GetDrawnEdges()[0].ccw.IsConvex());
		}

		// Counts the number of times an infinite line drawn between the point on the line given and some point
		// outside the polygon intersects the boundaries of the polygon.
		public int NumIntersects(Line2D line, VertexGraph vg){
			int count = 0;
			bool skip = false;
			int ind;

			// Creates the "infinite" line, from a point on the drawn line to somewhere outside the polygon
			Line2D infLine = new Line2D(new Vector2(((line.p1.GetValue().x + line.p2.GetValue().x)/2),((line.p1.GetValue().y + line.p2.GetValue().y)/2)), new Vector2(vg.maximumX() + 1,vg.maximumY() + 1));

			Line2D [] edges = vg.GetBoundEdges();
			foreach(Line2D bound in edges)
			{
				// If the infline intersects with the bound edge, increment counter.
				// However, we want to skip the count if:
				// This is the second of two edges which connect at a vertex the infline intersects
				// (going from inside the shape to outside or vice versa)
				// Both lines if its at a vertex where both lines are on the same side of the infline.
				//( staying either inside or outside of the shape)
				// So to generalize:
					// Theres no skip business at all if it's not intersecting a vertex. 
					// If it is intersecting a vertex, then check if the lines fall on the same side of the line.
						// If they are, skip both this and the next line.
						// If they're not, skip only this one and not the next.
				if(Array.IndexOf(edges, bound) + 1 == edges.Length - 1) ind = Array.IndexOf(edges, bound) + 1; else ind = 0;
				int otherPoint1; int otherPoint2;
				if(Vertex.Orientation(infLine.p1, infLine.p2, bound.p1) == 0) otherPoint1 = Vertex.Orientation(infLine.p1, infLine.p2, bound.p2); else otherPoint1 = Vertex.Orientation(infLine.p1, infLine.p2, bound.p1);
				if(Vertex.Orientation(infLine.p1, infLine.p2, edges[ind].p1) == 0) otherPoint2 = Vertex.Orientation(infLine.p1, infLine.p2, edges[ind].p2); else otherPoint2 = Vertex.Orientation(infLine.p1, infLine.p2, edges[ind].p1);
				
				// This part is tricky.
				// Check if the lines are on the same side or opposite sides of the infLine.
				// Opposite sides: dont skip this one, but do skip the next.
				// Same side: skip both this and the next.
				// Therefore, either way, we're skipping the next one. However, if they're on the same side, count++ right now.
						
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
