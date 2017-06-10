using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(Combination))]
public class CombinationInspector : PropertyDrawer {
//	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
//	{
//		
//	}

//	public override void OnInspectorGUI(){
//		DrawDefaultInspector ();
//
//		weight = serializedObject.FindProperty ("weight");
//
//		EditorGUILayout.FloatField(weight.floatValue);
//
//
//		Combination combiScript = (Combination)target;
//
//		switch (combiScript.preprocessType) {
//		case Combination.PreprocessType.Curve:
//			showCurve (combiScript);
//			break;
//		case Combination.PreprocessType.Exponent:
//			showExponentBase (combiScript);
//			break;
//		case Combination.PreprocessType.None:
//		default:
//			break;
//		}
//
//		switch (combiScript.combinationType) {
//		case Combination.CombinationType.Add:
//			combiScript.weight = EditorGUILayout.Slider("Weight", combiScript.weight, 0,1);
//			break;
//		case Combination.CombinationType.Exponent:
//		default:
//			break;
//		}
//	}

	void showCurve(Combination combiScript){
		switch (combiScript.combinationType) {
		case Combination.CombinationType.Add:
			combiScript.addCurve = EditorGUILayout.CurveField (combiScript.addCurve);
			break;
		case Combination.CombinationType.Exponent:
		default:
			combiScript.expCurve = EditorGUILayout.CurveField (combiScript.expCurve);
			break;
		}
	}

	void showExponentBase(Combination combiScript){
		switch (combiScript.combinationType) {
		case Combination.CombinationType.Add:
			combiScript.addBase = EditorGUILayout.FloatField (combiScript.addBase);
			break;
		case Combination.CombinationType.Exponent:
		default:
			combiScript.exponentBase = EditorGUILayout.FloatField (combiScript.exponentBase);
			break;
		}
	}
}
