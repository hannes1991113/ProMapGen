using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplatmapCreator))]
public class CombinationInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();
	}

}
