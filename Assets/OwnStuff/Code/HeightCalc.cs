using UnityEngine;
using System.Collections;

public enum NoiseType {Perlin, PerlinUnity};

public class HeightCalc : MonoBehaviour {

	public bool useSeed = true;
	public string seed = "Seed";

	public float xOffset;
	public float yOffset;

	public NoiseType noiseType = NoiseType.Perlin;
	public float scale = 20;

	[Range(1, 8)]
	public int octaves = 1;

	[Range(1f, 4f)]
	public float lacunarity = 2f;

	[Range(0f, 1f)]
	public float persistence = 0.5f;

	[Range(0,10)]
	public float exponent = 1;

	private TerrainData terrainData;
	private float[,] heights;

	private SplatmapCreator splatmapCreator;
	private Noise noiseHandler;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		splatmapCreator = GetComponent<SplatmapCreator> ();
		Random.InitState (seed.GetHashCode());
		if (useSeed) {
			xOffset = Random.Range (0.0f, 1000.0f);
			yOffset = Random.Range (0.0f, 1000.0f);
		}
//		xOffset = Mathf.Abs (xOffset);
//		yOffset = Mathf.Abs (yOffset);
		noiseHandler = new Noise(persistence, lacunarity, octaves, scale, noiseType);
	}
		
	public void CalcHeights() {
		initiate ();

		double y = 0.0F;
		while (y < terrainData.heightmapHeight) {
			double x = 0.0F;
			while (x < terrainData.heightmapWidth) {
				double xCoord = xOffset + x / terrainData.heightmapWidth;
				double yCoord = yOffset + y / terrainData.heightmapHeight;
				double sample = noiseHandler.OctaveNoise(xCoord, yCoord, 0);
				sample = Mathf.Pow ((float)sample, exponent);
				heights[(int)y, (int)x] = (float)sample;
				x++;
			}
			y++;
		}

		terrainData.SetHeights (0, 0, heights);
		splatmapCreator.CreateSplatmap ();
	}
}