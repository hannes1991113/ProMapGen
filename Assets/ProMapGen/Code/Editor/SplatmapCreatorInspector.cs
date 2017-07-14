using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProMapGen{
[CustomEditor(typeof(SplatmapCreator))]
public class SplatmapCreatorInspector : Editor {
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
}