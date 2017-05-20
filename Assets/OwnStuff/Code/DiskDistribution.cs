using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DiskDistribution : MonoBehaviour {
	public enum CombinationType {
		NormalValue,
		Add,
		Multiply,
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

	public float a = 0.1f;
	public float b = 0.7f;
	public float c = 0.5f;

	private TerrainData terrainData;
	private float[,] heights;
	private float[,] newHeights;



	public void showPointsOnly(){
		Generate ();
		ClearHeights(0);
		for(int i = 0; i < sampleCount; ++i){
			heights[(int)result[i].x, (int)result[i].y] = 1.0f;
		}
		terrainData.SetHeights (0, 0, heights);
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		PoissonDiskGenerator.minDist = minDistance;
		PoissonDiskGenerator.k = k;
		PoissonDiskGenerator.sampleRange = terrainData.heightmapHeight;
	}

	void Generate(){
		initiate ();
		result = PoissonDiskGenerator.Generate();
		sampleCount = PoissonDiskGenerator.sampleCount;
	}

	void ClearHeights(float clearValue){
		for(int x = 0; x < terrainData.heightmapWidth; ++x){
			for(int y = 0; y < terrainData.heightmapHeight; ++y){
				heights [x, y] = clearValue;
			}
		}
	}

	public void combineWithDisks(){
		Generate ();
		newHeights = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		for(int i = 0; i < sampleCount; ++i){
			int x = (int)result [i].x;
			int y = (int)result [i].y;
			combineWithDisk (x, y);
			//newHeights[x, y] = heights[x, y];
		}
		heights = newHeights;
		terrainData.SetHeights (0, 0, heights);
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
		//float distance = Mathf.Sqrt((px - qx) * (px - qx) + (py - qy) * (py - qy));
		float distance = Vector2.Distance(new Vector2(px,py), new Vector2(qx, qy));
		distance = distance / maxDistance;
		//distance = Mathf.Clamp (distance, 0, 1);
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
		case CombinationType.Add:
			newHeights [x, y] = heights [x, y] + a - b * Mathf.Pow (distance, c);
			break;
		case CombinationType.Multiply:
			newHeights [x, y] = (heights [x, y] + a) * (1 - b * Mathf.Pow (distance, c));
			break;
		}


	}
}