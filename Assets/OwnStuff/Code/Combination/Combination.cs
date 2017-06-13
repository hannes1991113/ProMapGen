using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public PreprocessType preprocessType = PreprocessType.None;
	[HideInInspector]
	public AnimationCurve addCurve = new AnimationCurve();
	[HideInInspector]
	public AnimationCurve expCurve = new AnimationCurve();
	[HideInInspector]
	public float exponentBase = 2.0f;
	[HideInInspector]
	public float addBase = 2.0f;
	[HideInInspector]
	public CombinationType combinationType = CombinationType.Add;
	[HideInInspector]
	public AnimationCurve weightCurve = new AnimationCurve(new Keyframe(0,1)) ;

	public float execute(float height, float distance){
		return combine(height, preprocess(distance), preprocessWeight(distance));
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

	float combine(float height, float distance, float weight){
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

	float preprocessWeight(float distance){
		distance = weightCurve.Evaluate(distance);
		return distance;
	}
}
