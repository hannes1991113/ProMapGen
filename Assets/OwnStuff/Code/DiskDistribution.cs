using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiskDistribution : MonoBehaviour {

	/// fot algorithm details please take a look at PoissonDiskGenerator.cs.
	[Header("Distribution Settings")]
	public float minDistance = 5.0f;	// minimum distance between samples.
	public int k = 30;					// darting time. Higher number get better result but slower.
	public int sampleCount = 0;			// number of the samples.
	private List<Vector2> result;		// the result of sample list.

	[Header("Noise Settings")]

	[Header("Combination with Heightmap")]
	public int range = 0;

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

	public void Generate(){
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
		newHeights = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		Generate ();
		for(int i = 0; i < sampleCount; ++i){
			int x = (int)result [i].x;
			int y = (int)result [i].y;
			newHeights[x, y] = heights[x, y];
		}
		heights = newHeights;
		terrainData.SetHeights (0, 0, heights);
	}

	void combineWithDisk(int middleX, int middleY){
		int x = Mathf.Clamp(middleX - range, 0, terrainData.heightmapWidth);
		for (; x < middleX + range || x < terrainData.heightmapWidth; x++) {

		}
	}
}