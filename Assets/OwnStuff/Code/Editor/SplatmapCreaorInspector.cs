using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplatmapCreator))]
public class SplatmapCreaorInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		SplatmapCreator targetScript = (SplatmapCreator)target;
		if (GUILayout.Button ("Create Splatmap")) {
			targetScript.CreateSplatmap ();
		}

//		if (GUILayout.Button ("Set Waterlevel")) {
//			targetScript.setWaterLevel ();
//		}
	}

}
