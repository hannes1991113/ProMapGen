﻿using System.Collections;
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


			showCombiScript (targetScript.combiner);
			
		}

		void showCombiScript(Combination combiScript){
			combiScript.preprocessType = (Combination.PreprocessType)EditorGUILayout.EnumPopup ("Preprocess", combiScript.preprocessType);

			switch (combiScript.preprocessType) {
			case Combination.PreprocessType.Curve:
			default:
				showCurve (combiScript);
				break;
			case Combination.PreprocessType.Exponent:
				showExponentBase (combiScript);
				break;
			}

			combiScript.combinationType = (Combination.CombinationType)EditorGUILayout.EnumPopup ("Combination", combiScript.combinationType);

			switch (combiScript.combinationType) {
			case Combination.CombinationType.Add:
				combiScript.weightCurve = EditorGUILayout.CurveField ("Weight", combiScript.weightCurve);
				break;
			case Combination.CombinationType.Exponent:
			default:
				break;
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
}