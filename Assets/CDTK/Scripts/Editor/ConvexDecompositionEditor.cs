﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{

    [CustomEditor(typeof(ConvexDecomposition))]
    public class ConvexDecompositionEditor : Editor
    {
		static bool decomposed = false;
		static bool triangulated = false;
		static bool delTriang = false;

        public override void OnInspectorGUI()
        {
            // INITIALIZATION
            ConvexDecomposition cd = target as ConvexDecomposition;

			EditorGUILayout.BeginHorizontal();
        	cd.level = (GameObject) EditorGUILayout.ObjectField(cd.level, typeof(GameObject), true);
			EditorGUILayout.EndHorizontal();

            // DRAW THE UI
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool triangButtonResult = GUILayout.Button("Triangulate Level", GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool decompButtonResult = GUILayout.Button("Decompose Level", GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            bool delTriangButtonResult = GUILayout.Button("Execute Delaunay Triangulation", GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			bool resetButtonResult = GUILayout.Button("Reset", GUILayout.Width(300), GUILayout.Height(20));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

            // HANDLE THE INPUT
			if (triangButtonResult) {
				if (triangulated) 
					Debug.Log ("Level already triangulated!");
				else {
					cd.TriangulateLevel ();
					triangulated = true;
				}
			} 
			else if (decompButtonResult) {
				if (decomposed) 
					Debug.Log ("Level already decomposed!");
				else if (!triangulated)
					Debug.Log ("Level must be triangulated first!");
				else{
					cd.DecomposeLevel ();
					decomposed = true;
				}
			} 
			else if (delTriangButtonResult) {
				if (delTriang) 
					Debug.Log ("Delaunay Triangulation already completed!");
				else if (!triangulated)
					Debug.Log ("Level must be triangulated first!");
				else if (decomposed)
					Debug.Log ("No support for completing Delaunay Triangulation on decomposed levels.");
				else{
					cd.DelaunayTriangulate ();
					delTriang = true;
				}
			} 
			else if (resetButtonResult)
			{
				// Destroy all child polygons
				foreach (VertexGraph graphs in cd.gameObject.GetComponents<VertexGraph>())
				{
					DestroyImmediate(graphs);
				}
				if (triangulated)
					triangulated = false;
				if (decomposed)
					decomposed = false;
				GUIUtility.ExitGUI();
			}
        }
    }
}
