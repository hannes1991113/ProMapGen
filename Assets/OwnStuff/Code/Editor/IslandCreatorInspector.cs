using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandCreator))]
public class IslandCreatorInspector : Editor {
	public override void OnInspectorGUI(){
//		DrawDefaultInspector ();


		IslandCreator targetScript = (IslandCreator)target;

		targetScript.maxDistance = EditorGUILayout.IntField ("Max Distance", targetScript.maxDistance);

		targetScript.combiner.Length = EditorGUILayout.IntField ("Length", targetScript.combiner.Length);

		for (int i = 0; i < targetScript.combiner.Length; i++) {
			Combination combiScript = targetScript.combiner [i];

			combiScript.preprocessType = (Combination.PreprocessType)EditorGUILayout.EnumPopup ("Preprocess", combiScript.preprocessType);

			switch (combiScript.preprocessType) {
			case Combination.PreprocessType.Curve:
				showCurve (combiScript);
				break;
			case Combination.PreprocessType.Exponent:
				showExponentBase (combiScript);
				break;
			case Combination.PreprocessType.None:
			default:
				break;
			}

			switch (combiScript.combinationType) {
			case Combination.CombinationType.Add:
				combiScript.weight = EditorGUILayout.Slider("Weight", combiScript.weight, 0,1);
				break;
			case Combination.CombinationType.Exponent:
			default:
				break;
			}
		}
	}

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
