using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CDTK {
	public class VertexGraph : MonoBehaviour
	{
		private List<Line2D> boundEdges = new List<Line2D>();
		private List<Line2D> drawnEdges = new List<Line2D>();
		List<Vector2> vertices = new List<Vector2>();
		public void AddPolygon(Vector2 [] v){
			vertices.AddRange(v);
			for(int i = 0; i < v.Length; i++){
				if (i == v.Length - 1)
					boundEdges.Add(new Line2D(v[i],v[0]));
				else
					boundEdges.Add(new Line2D(v[i], v[i + 1]));
				}
		}

		public void AddBoundEdge(Line2D edge){
			boundEdges.Add(edge);
		}
		public void DrawEdge(Line2D edge){
			drawnEdges.Add(edge);
		}
		//unnecessary for my specific implementation.
//		public void AddBoundEdge(Vector2 a, Vector2 b){
//			boundEdges.Add(new Line2D(a,b));
//		}
//		public void DrawEdge(Vector2 a, Vector2 b){
//			drawnEdges.Add(new Line2D(a,b));
//		}

		public void EraseEdge(Line2D line){drawnEdges.Remove(line);}

		//dont think an isEmpty method is necessary but can be easily implemented

		public Line2D[] GetAllEdges() {
			List<Line2D> ret = new List<Line2D>(boundEdges);
			ret.AddRange (drawnEdges);
			return ret.ToArray();
		}
		public Line2D[] GetDrawnEdges() {return drawnEdges.ToArray();}
		public Line2D[] GetBoundEdges() {return boundEdges.ToArray();}
		public Vector2[] GetVertices() {return vertices.ToArray();}

		public float maximumX(){
			float cur = 0;
			foreach(Vector2 next in vertices)
				if(next.x > cur)
					cur = next.x;
			return cur;
		}
		public float maximumY(){
			float cur = 0;
			foreach(Vector2 next in vertices)
				if(next.y > cur)
					cur = next.y;
			return cur;
		}

		//tests if the proposed line a intersects with any given line in the level, b
		public bool IntersectsNothing(Line2D a){
			foreach (Line2D b in GetAllEdges())
				if (a.Intersect (b))
					return false;
			return true;
		}

		#region Editor
		//draws edges at all times
		private void OnDrawGizmos()
		{
			foreach (Line2D e in boundEdges) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine (e.p1, e.p2);
			}
			foreach (Line2D e in drawnEdges) {
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (e.p1, e.p2);
			}
		}
		//only draws vertices when selected
		private void OnDrawGizmosSelected()
		{
			foreach (Line2D e in GetAllEdges()) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere (e.p1, 0.1f);
			}
		}
		#endregion
	}
}
