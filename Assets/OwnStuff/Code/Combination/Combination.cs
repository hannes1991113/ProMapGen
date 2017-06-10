using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combination{
	public enum CombinationType {
		Add,
		Exponent,
	}

	public enum PreprocessType {
		None,
		Curve,
		Exponent,
	}

	[HideInInspector]
	public PreprocessType preprocessType;
	[HideInInspector]
	public AnimationCurve addCurve;
	[HideInInspector]
	public AnimationCurve expCurve;
	[HideInInspector]
	public float exponentBase;
	[HideInInspector]
	public float addBase;
	[HideInInspector]
	public CombinationType combinationType;
	[HideInInspector]
	public float weight;

	public float execute(float height, float distance){
		return combine(height, preprocess(distance));
	}

	float preprocess(float distance)
	{
		switch (preprocessType){
		case PreprocessType.Curve:
			distance = preprocessCurve (distance);
			break;
		case PreprocessType.Exponent:
			distance = preprocessExponent (distance);
			break;
		case PreprocessType.None:
		default:
			break;
		}
		return distance;
	}

	float combine(float height, float distance){
		switch (combinationType){
		case CombinationType.Add:
		default:
			height = distance * weight + height * (1 - weight);
			break;
		case CombinationType.Exponent:
			height = Mathf.Pow (height, distance);
			break;
		}
		return height;
	}

	float preprocessCurve(float distance){
		switch (combinationType) {
		case CombinationType.Add:
		default:
			distance = addCurve.Evaluate (distance);
			break;
		case CombinationType.Exponent:
			distance = expCurve.Evaluate (distance);
			break;
		}
		return distance;
	}

	float preprocessExponent(float distance){
		switch (combinationType){
		case CombinationType.Add:
		default:
			distance = Mathf.Pow (addBase, -distance);
			break;
		case CombinationType.Exponent:
			distance = Mathf.Pow (exponentBase, distance + 1);
			break;
		}
		return distance;
	}
}
