using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProMapGen{
	[CustomEditor(typeof(IslandCreator))]
	public class IslandCreatorInspector : Editor {

		public override void OnInspectorGUI(){

			DrawDefaultInspector ();

			IslandCreator targetScript = (IslandCreator)target;
			EditorGUILayout.MinMaxSlider(ref targetScript.minSize, ref targetScript.maxSize, 1, 100);
		}
	}
}