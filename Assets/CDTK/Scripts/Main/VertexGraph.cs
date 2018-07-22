using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK {
	public class VertexGraph : MonoBehaviour
	{
		private List<Line2D> boundEdges = new List<Line2D>();
		private List<Line2D> drawnEdges = new List<Line2D>();
		List<Vertex> vertices = new List<Vertex>();

		public void AddPolygon(Vector2 [] v){
			Vertex first = new Vertex(v[0]);
			Vertex cur = first; Vertex next; Line2D curLine;
			for(int i = 0; i < v.Length; i++){
				vertices.Add(cur);
				if (i == v.Length - 1) next = first;
				else next = new Vertex(v[i+1]);
				curLine = new Line2D(cur, next);
				AddBoundEdge(curLine);
				cur = next;
				}
		}

		public void AddBoundEdge(Line2D edge){
			boundEdges.Add(edge);
			edge.p1.AddNeighbor(edge); edge.p2.AddNeighbor(edge);
		}
		public void DrawEdge(Line2D edge){
			drawnEdges.Add(edge);
			edge.p1.AddNeighbor(edge); edge.p2.AddNeighbor(edge);
		}
		//unnecessary for my specific implementation.
//		public void AddBoundEdge(Vector2 a, Vector2 b){
//			boundEdges.Add(new Line2D(a,b));
//		}
//		public void DrawEdge(Vector2 a, Vector2 b){
//			drawnEdges.Add(new Line2D(a,b));
//		}

		public void EraseEdge(Line2D edge){
			drawnEdges.Remove(edge);
			edge.p1.RemoveNeighbor(edge); edge.p2.RemoveNeighbor(edge);
			}

		// Dont think an isEmpty method is necessary but can be easily implemented

		public Line2D[] GetAllEdges() {
			List<Line2D> ret = new List<Line2D>(boundEdges);
			ret.AddRange (drawnEdges);
			return ret.ToArray();
		}
		public Line2D[] GetDrawnEdges() {return drawnEdges.ToArray();}
		public Line2D[] GetBoundEdges() {return boundEdges.ToArray();}
		public Vertex[] GetVertices() {return vertices.ToArray();}

		public float maximumX(){
			float cur = 0;
			foreach(Vertex next in vertices)
				if(next.GetValue().x - cur > .000001)
					cur = next.GetValue().x;
			return cur;
		}
		public float maximumY(){
			float cur = 0;
			foreach(Vertex next in vertices)
				if(next.GetValue().y - cur > .000001)
					cur = next.GetValue().y;
			return cur;
		}

		// Tests if the proposed line a intersects with any given line in the level, b
		public bool IntersectsNothing(Line2D a){
			foreach (Line2D b in GetAllEdges())
				if (a.Intersect (b))
					return false;
			return true;
		}

		public void createRegions(){
			foreach(Line2D draw in drawnEdges)
			{
				if(draw.cw == null){
					Line2D edge1 = draw.p1.GetSortedEdges().Get(draw).prev.value;
					Line2D edge2 = draw.p2.GetSortedEdges().Get(draw).next.value;
					Region rcw = new Region(draw.p2, draw.p1, edge1.otherPt(draw.p1));
					draw.cw = rcw;
					//need to figure out how collinearity plays into this
					if(Vertex.Orientation(edge2.p1, edge2.p2, draw.p1) == 1) edge2.cw = rcw;
					else edge2.ccw = rcw;
					if(Vertex.Orientation(edge1.p1, edge1.p2, draw.p2) == 1) edge2.cw = rcw;
					else edge2.ccw = rcw;
				}
				if(draw.ccw == null){
					Line2D edge1 = draw.p1.GetSortedEdges().Get(draw).next.value;
					Line2D edge2 = draw.p2.GetSortedEdges().Get(draw).prev.value;
					Region rccw = new Region(draw.p1, draw.p2, edge2.otherPt(draw.p2));
					draw.ccw = rccw;
					//need to figure out how collinearity plays into this
					if(Vertex.Orientation(edge2.p1, edge2.p2, draw.p1) == 1) edge2.cw = rccw;
					else edge2.ccw = rccw;
					if(Vertex.Orientation(edge1.p1, edge1.p2, draw.p2) == 1) edge2.cw = rccw;
					else edge2.ccw = rccw;
				}
			}
		}

		#region Editor
		//draws edges at all times
		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(new Vector2(0,-100), new Vector2(0,1000));
			Gizmos.DrawLine(new Vector2(-100,0), new Vector2(100,0));
			foreach (Line2D e in boundEdges) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine (e.p1.GetValue(), e.p2.GetValue());
			}
			foreach (Line2D e in drawnEdges) {
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (e.p1.GetValue(), e.p2.GetValue());
			}
		}
		//only draws vertices when selected
		private void OnDrawGizmosSelected()
		{
			foreach (Line2D e in GetAllEdges()) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere (e.p1.GetValue(), 0.1f);
			}
		}
		#endregion
	}
}
