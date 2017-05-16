using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoissonDiskResultHelper))]
public class PoissonDiskInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		PoissonDiskResultHelper targetScript = (PoissonDiskResultHelper)target;
		if (GUILayout.Button ("Show Points")) {
			targetScript.showPointsOnly();
		}
	}

}
