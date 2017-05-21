using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DiskDistribution))]
public class DiskDistributionInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		DiskDistribution targetScript = (DiskDistribution)target;

		if (GUILayout.Button ("Show Points")) {
			targetScript.showPointsOnly();
		}
	}

}
