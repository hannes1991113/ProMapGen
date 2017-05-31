using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatmapCreator : MonoBehaviour {
	[Range(0,1)]
	public float[] terrainCuts = {0.1f, 0.2f};
	[Range(0,1)]
	public float waterLevel = 0.1f;

	private TerrainData terrainData;
	private float[,] heights;

	private float[,,] splatmap;
	private Transform water;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
	}

	public void CreateSplatmap()
	{
		initiate ();

		terrainData.alphamapResolution = terrainData.heightmapResolution;
		splatmap = new float[terrainData.alphamapHeight, terrainData.alphamapWidth, terrainCuts.Length];
		for(int y = 0; y < terrainData.alphamapHeight; y++){
			for(int x = 0; x < terrainData.alphamapWidth; x++){
				for (int i = 0; i < terrainCuts.Length; i++) {
					if (heights[y,x] <= terrainCuts [i]) {
						splatmap [y, x, i] = 1;
						break;
					}
				}
			}
		}
		terrainData.SetAlphamaps (0, 0, splatmap);
		setWaterLevel ();
	}

	public void setWaterLevel(){
		water = transform.GetChild (0);
		float scaleX = terrainData.heightmapScale.x;
		float scaleY = terrainData.heightmapScale.y;
		float scaleZ = terrainData.heightmapScale.z;
		Debug.Log("ScaleX = " + scaleX + " ScaleY = " + scaleY + " ScaleZ = " + scaleZ);
		water.localPosition = new Vector3 (scaleX * 0.5f, waterLevel * scaleY, scaleZ * 0.5f);
	}

}
