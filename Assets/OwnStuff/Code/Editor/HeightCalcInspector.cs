using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProMapGen{
[CustomEditor(typeof(Controller))]
public class HeightCalcInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		Controller targetScript = (Controller)target;
		if (GUILayout.Button ("Calculate All")) {
			targetScript.CalcAll ();
		}
	}

}
}