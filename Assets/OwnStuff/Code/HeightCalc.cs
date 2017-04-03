using UnityEngine;
using System.Collections;

public class HeightCalc : MonoBehaviour {
	public float xOffset;
	public float yOffset;

	public float scale1 = 1;
	[Range(0,1)]
	public float weight1 = 1;

	public float scale2 = 2;
	[Range(0,1)]
	public float weight2 = 0;

	public float scale3 = 4;
	[Range(0,1)]
	public float weight3 = 0;

	public float scale4 = 4;
	[Range(0,1)]
	public float weight4 = 0;

	public float scale5 = 4;
	[Range(0,1)]
	public float weight5 = 0;

	public float scale6 = 4;
	[Range(0,1)]
	public float weight6 = 0;

	[Range(0,10)]
	public float exponent = 1;

	[Range(0,1)]
	public float[] terrainCuts = {0.1f, 0.2f};

	private TerrainData terrainData;
	private float[,] heights;

	private float[,,] splatmap;

	void Start() {
		
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
	}
		
	public void CalcHeights() {
		if (terrainData == null || heights == null)
			initiate ();
		float y = 0.0F;
		while (y < terrainData.heightmapHeight) {
			float x = 0.0F;
			while (x < terrainData.heightmapWidth) {
				float xCoord = xOffset + x / terrainData.heightmapWidth;
				float yCoord = yOffset + y / terrainData.heightmapHeight;
				float sample = weight1 * Mathf.PerlinNoise(xCoord * scale1, yCoord * scale1);
				sample += weight2 * Mathf.PerlinNoise(xCoord * scale2, yCoord * scale2);
				sample += weight3 * Mathf.PerlinNoise(xCoord * scale3, yCoord * scale3);
				sample += weight4 * Mathf.PerlinNoise(xCoord * scale4, yCoord * scale4);
				sample += weight5 * Mathf.PerlinNoise(xCoord * scale5, yCoord * scale5);
				sample += weight6 * Mathf.PerlinNoise(xCoord * scale6, yCoord * scale6);
				sample /= (weight1 + weight2 + weight3 + weight4 + weight5 + weight6);
				sample = Mathf.Pow (sample, exponent);
				heights[(int)y, (int)x] = sample;
				x++;
			}
			y++;
		}
		CreateSplatmap ();
		terrainData.SetHeights (0, 0, heights);
	}

	void CreateSplatmap()
	{
		splatmap = new float[terrainData.alphamapHeight, terrainData.alphamapWidth, terrainCuts.Length];
		for(int y = 0; y < terrainData.alphamapHeight; y++){
			for(int x = 0; x < terrainData.alphamapWidth; x++){
				for (int i = 0; i < terrainCuts.Length; i++) {
					if (heights[y,x] < terrainCuts [i]) {
						splatmap [y, x, i] = 1;
						break;
					}
				}
			}
		}
		terrainData.SetAlphamaps (0, 0, splatmap);
	}

	void Update() {
		
	}
}