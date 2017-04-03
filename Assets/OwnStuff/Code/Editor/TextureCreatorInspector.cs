using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		TextureCreator targetScript = (TextureCreator)target;
		if (GUILayout.Button ("Fill Texture")) {
			targetScript.FillTexture ();
		}
	}

}
