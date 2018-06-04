using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{

    [CustomEditor(typeof(Triangulation))]
    public class TriangulationEditor : Editor
    {
		static bool triangulated = false;

        public override void OnInspectorGUI()
        {
            // INITIALIZATION
            Triangulation tri = target as Triangulation;

            // DRAW THE UI
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool triButtonResult = GUILayout.Button("Triangulate Level", GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			bool resetButtonResult = GUILayout.Button("Reset", GUILayout.Width(300), GUILayout.Height(20));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

            // HANDLE THE INPUT
			if (triButtonResult) {
				if (triangulated) 
					Debug.Log ("Level already triangulated!");
				else {
					tri.TriangulateLevel ();
					triangulated = true;
				}
			} 
			else if (resetButtonResult)
			{
				// Destroy all child polygons
				foreach (VertexGraph vg in tri.gameObject.GetComponents<VertexGraph>())
				{
					DestroyImmediate(vg);
				}
				if (triangulated)
					triangulated = false;
				GUIUtility.ExitGUI();
			}
        }
    }
}
