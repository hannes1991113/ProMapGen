using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightCalc))]
public class HeightCalcInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		HeightCalc targetScript = (HeightCalc)target;
		if (GUILayout.Button ("Calculate Heights")) {
			targetScript.CalcHeights ();
		}
	}

}
