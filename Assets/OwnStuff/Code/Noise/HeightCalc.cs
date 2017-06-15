using UnityEngine;
using System.Collections;

public enum NoiseType {Perlin, PerlinUnity};

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(IslandCreator))]
[RequireComponent(typeof(SplatmapCreator))]
public class HeightCalc : MonoBehaviour {

	public bool randomSeed = false;
	public string seed = "Seed";
	public bool resetRandom = true;
	public bool createTextures = true;

	public bool generateIslands = true;
	public bool updateSplatmap;

	public NoiseCreator heightNoise = new NoiseCreator ();

	private float[,] heights;
	private Random.State RandomStartState;

	private TerrainData terrainData;
	private SplatmapCreator splatmapCreator;
	private IslandCreator islandCreator;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		splatmapCreator = GetComponent<SplatmapCreator> ();
		islandCreator = GetComponent<IslandCreator> ();

		int randomInit;
		if (randomSeed) {
			randomInit = System.DateTime.Now.GetHashCode();
			seed = System.DateTime.Now.ToString ();
		} else {
			randomInit = seed.GetHashCode ();
		}
		Random.InitState (randomInit);
		heightNoise.setRandom ();
	}
		
	public void CalcAll() {
		initiate ();

		double y = 0.0F;
		while (y < terrainData.heightmapHeight) {
			double x = 0.0F;
			while (x < terrainData.heightmapWidth) {
				double xCoord = x / terrainData.heightmapWidth;
				double yCoord = y / terrainData.heightmapHeight;
				double sample = heightNoise.OctaveNoise(xCoord, yCoord, 0);
				heights[(int)y, (int)x] = (float)sample;
				x++;
			}
			y++;
		}

		terrainData.SetHeights (0, 0, heights);
		if (createTextures) {
			heightNoise.createTexture (terrainData.heightmapHeight, terrainData.heightmapWidth);
		}
		if (generateIslands) {
			islandCreator.combineWithDisks ();
		}
		if (updateSplatmap) {
			splatmapCreator.CreateSplatmap ();
		}
	}


}