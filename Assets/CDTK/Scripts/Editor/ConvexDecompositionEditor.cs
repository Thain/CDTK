using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace CDTK
{

    [CustomEditor(typeof(ConvexDecomposition))]
    public class ConvexDecompositionEditor : Editor
    {
		static bool decomposed = false;

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
            bool decompButtonResult = GUILayout.Button("Decompose Level", GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			bool resetButtonResult = GUILayout.Button("Reset", GUILayout.Width(300), GUILayout.Height(20));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

            // HANDLE THE INPUT
			if (decompButtonResult) {
				if (decomposed) 
					Debug.Log ("Level already decomposed!");
				else {
					cd.DecomposeLevel ();
					decomposed = true;
				}
			} 
			else if (resetButtonResult)
			{
				// Destroy all child polygons
				foreach (VertexGraph vg in cd.gameObject.GetComponents<VertexGraph>())
				{
					DestroyImmediate(vg);
				}
				if (decomposed)
					decomposed = false;
				GUIUtility.ExitGUI();
			}
        }
    }
}
