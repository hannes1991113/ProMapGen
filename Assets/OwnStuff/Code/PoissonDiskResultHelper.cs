using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoissonDiskResultHelper : MonoBehaviour {

	/// fot algorithm details please take a look at PoissonDiskGenerator.cs.
	public float minDistance = 5.0f;	// minimum distance between samples.
	public int k = 30;					// darting time. Higher number get better result but slower.
	//private float sampleRange = 256.0f;	// the edge length of the squre area for generation.
	public int sampleCount = 0;			// number of the samples.
	public List<Vector2> result;		// the result of sample list.
	private TerrainData terrainData;
	private float[,] heights;

	public void showPointsOnly(){
		Generate ();
		ClearHeights(0);
		sampleCount = PoissonDiskGenerator.sampleCount;
		for(int i = 0; i < sampleCount; ++i){
			heights[(int)result[i].x, (int)result[i].y] = 1.0f;
		}
		terrainData.SetHeights (0, 0, heights);
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
	}

	public void Generate(){
		initiate ();
		PoissonDiskGenerator.minDist = minDistance;
		PoissonDiskGenerator.k = k;
		PoissonDiskGenerator.sampleRange = terrainData.heightmapHeight;
		result = PoissonDiskGenerator.Generate();
	}

	void ClearHeights(float clearValue){
		for(int x = 0; x < terrainData.heightmapWidth; ++x){
			for(int y = 0; y < terrainData.heightmapHeight; ++y){
				heights [x, y] = clearValue;
			}
		}
	}
}