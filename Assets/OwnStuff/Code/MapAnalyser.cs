using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnalyser : MonoBehaviour {

	public int count;
	public float maxValue;
	public float minValue;


	private TerrainData terrainData;
	private float[,] heights;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
	}

	public void analyseMap()
	{
		initiate ();
		minValue = 2;
		maxValue = -1;
		count = 0;
		for(int y = 0; y < terrainData.heightmapHeight; y++) {
			for(int x = 0; x < terrainData.heightmapWidth; x++) {
				float height = heights [y, x];

				if (height > maxValue)
					maxValue = height;
				if (height < minValue)
					minValue = height;

				count++;
			}
		}
	}
}
