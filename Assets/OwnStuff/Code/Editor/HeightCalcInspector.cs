using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeightCalcCopy))]
public class HeightCalcInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		HeightCalcCopy heightCalcScript = (HeightCalcCopy)target;
		if (GUILayout.Button ("Calculate Heights")) {
			heightCalcScript.CalcHeights ();
		}
	}

}
