using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK {
	public class VertexGraph : MonoBehaviour
	{
		private List<Line> boundEdges = new List<Line>();
		private List<Line> drawnEdges = new List<Line>();
		private List<Region> regions = new List<Region>();
		List<Vertex> vertices = new List<Vertex>();

		public void AddPolygon(Vector2 [] v){
			Vertex first = new Vertex(v[0]);
			Vertex cur = first; Vertex next; Line curLine;
			for(int i = 0; i < v.Length; i++){
				vertices.Add(cur);
				if (i == v.Length - 1) next = first;
				else next = new Vertex(v[i+1]);
				curLine = new Line(cur, next);
				AddBoundEdge(curLine);
				cur = next;
				}
		}

		public void AddRegion(Region r){
			regions.Add(r);
			foreach(Line edge in r.GetEdges()) {
			    if(Vertex.Orientation(edge.a, edge.b, r.GetCenter()) == 1) edge.cw = r;
              	else edge.ccw = r;
           	}
		}
		public void AddBoundEdge(Line edge){
			boundEdges.Add(edge);
			edge.a.AddNeighbor(edge); edge.b.AddNeighbor(edge);
		}
		public void DrawEdge(Line edge){
			drawnEdges.Add(edge);
			edge.a.AddNeighbor(edge); edge.b.AddNeighbor(edge);
		}
		public void EraseEdge(Line edge){
			drawnEdges.Remove(edge);
			edge.a.RemoveNeighbor(edge); edge.b.RemoveNeighbor(edge);
			}

		// Dont think an isEmpty method is necessary but can be easily implemented

		public void RemoveRegion(Region r){
			regions.Remove(r);
		}
		public Line[] GetAllEdges() {
			List<Line> ret = new List<Line>(boundEdges);
			ret.AddRange (drawnEdges);
			return ret.ToArray();
		}
		public Region[] GetRegions() {return regions.ToArray();}
		public Line[] GetDrawnEdges() {return drawnEdges.ToArray();}
		public Line[] GetBoundEdges() {return boundEdges.ToArray();}
		public Vertex[] GetVertices() {return vertices.ToArray();}

		public float maximumX(){
			float cur = 0;
			foreach(Vertex next in vertices)
				if(next.val.x - cur > .000001)
					cur = next.val.x;
			return cur;
		}
		public float maximumY(){
			float cur = 0;
			foreach(Vertex next in vertices)
				if(next.val.y - cur > .000001)
					cur = next.val.y;
			return cur;
		}

		// Tests if the proposed line a intersects with any given line in the level, b
		public bool IntersectsNothing(Line a){
			foreach (Line b in GetAllEdges())
				if (a.Intersect (b))
					return false;
			return true;
		}

			// for each of the lines that we drew, there is a region on each side. We want to see if combining them creates a convex shape.
			// Thus we need to first create this web of regions.
			// Then we need to test each pair.
		public void createRegions(){
			foreach(Line draw in drawnEdges)
			{
				Region r;
				if(draw.cw == null){
					Line edge1 = draw.a.PrevLine(draw);
					Line edge2 = draw.b.NextLine(draw);
					r = new Region(draw.b, draw.a, edge1.otherPt(draw.a), draw, edge2, edge1);
					draw.cw = r;
					AddRegion(r);
				}
				if(draw.ccw == null){
					Line edge1 = draw.a.NextLine(draw);
					Line edge2 = draw.b.PrevLine(draw);
					r = new Region(draw.a, draw.b, edge2.otherPt(draw.b), draw, edge2, edge1);					
					draw.ccw = r;
					AddRegion(r);
				}
			}
		}

		#region Editor
		//draws edges at all times
		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(new Vector2(0,-100), new Vector2(0,1000));
			Gizmos.DrawLine(new Vector2(-100,0), new Vector2(100,0));
			foreach (Line e in boundEdges) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine (e.a.val, e.b.val);
			}
			foreach (Line e in drawnEdges) {
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (e.a.val, e.b.val);
			}
		}
		//only draws vertices when selected
		private void OnDrawGizmosSelected()
		{
			foreach (Line e in GetAllEdges()) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere (e.a.val, 0.05f);
			}
			foreach (Region r in GetRegions()) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere (r.GetCenter().val, 0.02f);
			}
		}
		#endregion
	}
}
