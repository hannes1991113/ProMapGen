using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DiskDistribution : MonoBehaviour {
	public enum CombinationType {
		Add,
		Multiply,
		Exponent,
	};

	/// for algorithm details please take a look at PoissonDiskGenerator.cs.
	[Header("Distribution Settings")]
	public float minDistance = 5.0f;	// minimum distance between samples.
	public int k = 30;					// darting time. Higher number get better result but slower.
	public int sampleCount = 0;			// number of the samples.
	private List<Vector2> result;		// the result of sample list.

	[Header("Noise Settings")]

	private TerrainData terrainData;

	public void showPointsOnly(){
		generateDisks ();
		float[,] newHeightMap =  new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		for(int i = 0; i < sampleCount; ++i){
			newHeightMap[(int)result[i].x, (int)result[i].y] = 1.0f;
		}
		terrainData.SetHeights (0, 0, newHeightMap);
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		PoissonDiskGenerator.minDist = minDistance;
		PoissonDiskGenerator.k = k;
		PoissonDiskGenerator.sampleRange = terrainData.heightmapHeight;
		Random.InitState (GetComponent<HeightCalc> ().seed.GetHashCode());
	}

	public List<Vector2> generateDisks(){
		initiate ();
		result = PoissonDiskGenerator.Generate();
		sampleCount = PoissonDiskGenerator.sampleCount;
		return result;
	}
}