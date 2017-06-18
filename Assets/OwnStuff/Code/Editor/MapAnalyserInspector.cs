using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProMapGen{
[CustomEditor(typeof(MapAnalyser))]
public class MapAnalyserInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		MapAnalyser targetScript = (MapAnalyser)target;
		if (GUILayout.Button ("Analyse")) {
			targetScript.analyseMap ();
		}
	}

}
}