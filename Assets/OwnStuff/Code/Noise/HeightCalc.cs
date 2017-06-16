using UnityEngine;
using System.Collections;

public enum NoiseType {Perlin, PerlinUnity};

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(IslandCreator))]
[RequireComponent(typeof(SplatmapCreator))]
public class HeightCalc : MonoBehaviour {

	public bool randomSeed = false;
	public int seed = 0;
	public bool resetRandom = true;
	public bool createTextures = true;

	public bool generateIslands = true;
	public bool updateSplatmap;

	public NoiseCreator heightNoise = new NoiseCreator ();

	private float[,] heights;
	[HideInInspector]
	public Random.State randomStartState;

	private TerrainData terrainData;
	private SplatmapCreator splatmapCreator;
	private IslandCreator islandCreator;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		splatmapCreator = GetComponent<SplatmapCreator> ();
		islandCreator = GetComponent<IslandCreator> ();

		if (randomSeed) {
			seed = System.DateTime.Now.GetHashCode();
		}
		Random.InitState (seed);
		randomStartState = Random.state;
		heightNoise.create (terrainData.heightmapResolution);
	}
		
	public void CalcAll() {
		initiate ();

		double y = 0.0F;
		while (y < terrainData.heightmapHeight) {
			double x = 0.0F;
			while (x < terrainData.heightmapWidth) {
				double sample = heightNoise.OctaveNoise(x, y, 0);
				heights[(int)y, (int)x] = (float)sample;
				x++;
			}
			y++;
		}

		terrainData.SetHeights (0, 0, heights);
		if (createTextures) {
			heightNoise.createTexture ();
		}
		if (generateIslands) {
			islandCreator.combineWithDisks ();
		}
		if (updateSplatmap) {
			splatmapCreator.CreateSplatmap ();
		}
	}

	public float[,] getHeights(){
		if (heights != null) {
			return heights;
		} else {
			return GetComponent<Terrain> ().terrainData.
				GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		}
	}

}