using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
public class MapAnalyser : MonoBehaviour {

	public int count;
	public float maxValue;
	public float minValue;
	[Range(5,100)]
	public int accuracy = 10;
	public AnimationCurve curve;

	private TerrainData terrainData;
	private float[,] heights;
	private int[] values;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		minValue = 2;
		maxValue = -1;
		count = 0;
		values = new int[accuracy];
	}

	public void analyseMap()
	{
		initiate ();
		for(int y = 0; y < terrainData.heightmapHeight; y++) {
			for(int x = 0; x < terrainData.heightmapWidth; x++) {
				float height = heights [y, x];
				count++;
				checkMaxMin(height);
				addToCurve (height);
			}
		}
		createCurve ();
	}

	private void createCurve(){
		curve = new AnimationCurve ();
		for (int i = 0; i < accuracy; i++) {
			curve.AddKey (((float)i) / accuracy, values [i]);
		}
	}

	private void checkMaxMin(float height){
		if (height > maxValue)
			maxValue = height;
		if (height < minValue)
			minValue = height;
	}

	private void addToCurve(float height){
		for (int i = 1; i <= accuracy; i++) {
			if (height < ((float)i) / accuracy) {
				values [i - 1]++;
				break;
			}
		}
	}
}
}
