using UnityEngine;
using System.Collections;

public class HeightCalcCopy : MonoBehaviour {
	public float xOffset;
	public float yOffset;

	public float scale = 20;

	[Range(1, 8)]
	public int octaves = 1;

	[Range(1f, 4f)]
	public float lacunarity = 2f;

	[Range(0f, 1f)]
	public float persistence = 0.5f;

	[Range(0,10)]
	public float exponent = 1;

	[Range(0,1)]
	public float[] terrainCuts = {0.1f, 0.2f};

	private TerrainData terrainData;
	private float[,] heights;

	private float[,,] splatmap;

	private NoiseCopy NoiseObject;

	void Start() {
		
	}

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		NoiseObject = new NoiseCopy(persistence, lacunarity, octaves);
	}
		
	public void CalcHeights() {
		initiate ();
		NoiseObject.change (persistence, lacunarity, octaves);
		double y = 0.0F;
		while (y < terrainData.heightmapHeight) {
			double x = 0.0F;
			while (x < terrainData.heightmapWidth) {
				double xCoord = xOffset + x / terrainData.heightmapWidth;
				double yCoord = yOffset + y / terrainData.heightmapHeight;
				double sample = NoiseObject.OctavePerlin(xCoord * scale, yCoord * scale, 0);
				sample = Mathf.Pow ((float)sample, exponent);
				heights[(int)y, (int)x] = (float)sample;
				x++;
			}
			y++;
		}
		terrainData.SetHeights (0, 0, heights);
		CreateSplatmap ();
	}

	void CreateSplatmap()
	{
		terrainData.alphamapResolution = terrainData.heightmapResolution;
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
}