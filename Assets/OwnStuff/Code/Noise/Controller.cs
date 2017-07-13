using UnityEngine;
using System.Collections;

namespace ProMapGen{
	public enum NoiseType {Perlin, PerlinUnity};

	[RequireComponent(typeof(Terrain))]
	[RequireComponent(typeof(IslandCreator))]
	[RequireComponent(typeof(IslandDistribution))]
	[RequireComponent(typeof(SplatmapCreator))]
	[RequireComponent(typeof(MapAnalyser))]
	public class Controller : MonoBehaviour {

		public bool randomSeed = false;
		public int seed = 0;
		public bool debugResetRandom = true;

		public bool generateIslands = true;
		public bool updateSplatmap = true;

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

			heights = heightNoise.createField ();

			terrainData.SetHeights (0, 0, heights);
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
}