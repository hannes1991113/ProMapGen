using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProMapGen{
[CustomEditor(typeof(IslandDistribution))]
public class DiskDistributionInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		IslandDistribution targetScript = (IslandDistribution)target;

		if (GUILayout.Button ("Show Points")) {
			targetScript.showPointsOnly();
		}
	}

}
}