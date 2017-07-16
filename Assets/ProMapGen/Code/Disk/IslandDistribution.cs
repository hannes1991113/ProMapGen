using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProMapGen{

	public class IslandDistribution : MonoBehaviour {
		// minimum distance between samples
		public float minDistance = 5.0f;
		// number of tries, higher number get better result but slower
		public int k = 30;
		// manual distribution, if true ignore all members above
		public bool manualDistribution = false;
		// list of points
		public List<Vector2> result;

		private TerrainData terrainData;

		void initiate(){
			terrainData = GetComponent<Terrain> ().terrainData;
			PoissonDiskGenerator.minDist = minDistance;
			PoissonDiskGenerator.k = k;
			PoissonDiskGenerator.sampleRange = terrainData.heightmapHeight;
			Controller controller = GetComponent<Controller> ();
			if (controller.debugResetRandom) {
				Random.state = controller.randomStartState;
			}
		}

		public void showPointsOnly(){
			generateDisks ();
			float[,] newHeightMap =  new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
			for(int i = 0; i < result.Count; ++i){
				newHeightMap[(int)result[i].x, (int)result[i].y] = 1.0f;
			}
			terrainData.SetHeights (0, 0, newHeightMap);
		}

		public List<Vector2> generateDisks(){
			initiate ();
			if (!manualDistribution) {
				result = PoissonDiskGenerator.Generate ();
			}
			return result;
		}
	}
}