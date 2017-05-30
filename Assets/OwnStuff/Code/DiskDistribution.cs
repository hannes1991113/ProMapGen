using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DiskDistribution : MonoBehaviour {
	public enum CombinationType {
		NormalValue,
		Multiply,
		Exponent,
		Add,
	};

	/// fot algorithm details please take a look at PoissonDiskGenerator.cs.
	[Header("Distribution Settings")]
	public float minDistance = 5.0f;	// minimum distance between samples.
	public int k = 30;					// darting time. Higher number get better result but slower.
	public int sampleCount = 0;			// number of the samples.
	private List<Vector2> result;		// the result of sample list.

	[Header("Noise Settings")]

	[Header("Combination with Heightmap")]
	public int maxDistance = 10;
	public CombinationType combinationType = CombinationType.NormalValue;

	public float exp = 1;
	public float mult = 1;
	public float add = 0;

	[Range(0,1)]
	public float weight;
	public AnimationCurve hillCurve;

	private TerrainData terrainData;
	private float[,] heights;
	private float[,] newHeights;

	public void showPointsOnly(){
		Generate ();
		for(int i = 0; i < sampleCount; ++i){
			newHeights[(int)result[i].x, (int)result[i].y] = 1.0f;
		}
		terrainData.SetHeights (0, 0, newHeights);
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		newHeights = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		PoissonDiskGenerator.minDist = minDistance;
		PoissonDiskGenerator.k = k;
		PoissonDiskGenerator.sampleRange = terrainData.heightmapHeight;
		Random.InitState (GetComponent<HeightCalc> ().seed.GetHashCode());
	}

	void Generate(){
		initiate ();
		result = PoissonDiskGenerator.Generate();
		sampleCount = PoissonDiskGenerator.sampleCount;
	}

	public void combineWithDisks(){
		Generate ();
		influenceAll ();
		for(int i = 0; i < sampleCount; ++i){
			int x = (int)result [i].x;
			int y = (int)result [i].y;
			combineWithDisk (x, y);
		}
		terrainData.SetHeights (0, 0, newHeights);
	}

	void combineWithDisk(int middleX, int middleY){
		int x = Mathf.Clamp(middleX - maxDistance, 0, terrainData.heightmapWidth);
		int xBorder = Mathf.Clamp ((middleX + maxDistance), 0, terrainData.heightmapWidth);
		for (; x < xBorder; x++) {
			int y = Mathf.Clamp(middleY - maxDistance, 0, terrainData.heightmapWidth);
			int yBorder = Mathf.Clamp ((middleY + maxDistance), 0, terrainData.heightmapWidth);
			for (; y < yBorder; y++) {
				float distance = calculateDistance (x, y, middleX, middleY);
				combineDistanceAndNoise (distance, x, y);
			}
		}
	}

	float calculateDistance(float px, float py, float qx, float qy){
		float distance = Vector2.Distance(new Vector2(px,py), new Vector2(qx, qy));
		distance = distance / maxDistance;
		return distance;
	}

	void combineDistanceAndNoise(float distance, int x, int y){
		if (distance > 1)
			return;
		switch (combinationType){
		case CombinationType.NormalValue:
		default:
			newHeights [x, y] = heights [x, y];
			break;
		case CombinationType.Multiply:
			newHeights [x, y] = heights [x, y] * (1 - (Mathf.Pow ((distance), exp) * mult));
			break;
		case CombinationType.Exponent:
			newHeights [x, y] = Mathf.Pow (heights [x, y], Mathf.Pow (distance, exp)*mult + add);
			break;
		case CombinationType.Add:
			newHeights [x, y] = hillCurve.Evaluate (distance) * weight + heights [x, y] * (1 - weight);
			break;
		}
	}

	void influenceAll(){
		
		for (int x = 0; x < terrainData.heightmapWidth; x++) {
			for (int y=0; y < terrainData.heightmapHeight; y++) {
				combineDistanceAndNoise (1, x, y);
			}
		}
	}
}