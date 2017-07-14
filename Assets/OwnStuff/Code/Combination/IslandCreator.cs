using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
	public class IslandCreator : MonoBehaviour {
		public enum CombinationType {
			Add,
			ExponentDeprecated,
		}

		public enum PreprocessType {
			Curve,
			ExponentBase,
		}

		[Header("Preprocess Distance")]
		public PreprocessType preprocessType = PreprocessType.ExponentBase;
		public AnimationCurve curve = new AnimationCurve();
		public float exponentBase = 4;
		[Header("Combination")]
		public CombinationType combinationType = CombinationType.Add;
		public AnimationCurve weightCurve = new AnimationCurve();

		[Header("Size")]
		public float maxSize = 10;
		public float minSize = 5;

		private TerrainData terrainData;
		private float[,] heightMap;
		private float[,] distanceMap;
		private float[,] newHeightMap;

		private IslandDistribution islandDistribution;
		private List<Vector2> disks;

		void initiate(){
			islandDistribution = GetComponent<IslandDistribution> ();
			terrainData = GetComponent<Terrain> ().terrainData;
			heightMap = GetComponent<Controller> ().getHeights ();
		}

		public void combineWithDisks(){
			initiate ();
			disks = islandDistribution.generateDisks ();
			createDistanceMap ();
			combineMaps ();
			terrainData.SetHeights (0, 0, newHeightMap);
		}

		void createDistanceMap(){
			distanceMap = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
			fillDistanceMap ();
			for(int i = 0; i < disks.Count; ++i){
				int x = (int)disks [i].x;
				int y = (int)disks [i].y;
				calcDiskArea (x, y);
			}
		}

		void calcDiskArea(int middleX, int middleY){
			float maxDistance = Random.Range (minSize, maxSize);

			int x = Mathf.FloorToInt (middleX - maxDistance);
			x = Mathf.Clamp(x, 0, terrainData.heightmapWidth);

			int xBorder = Mathf.CeilToInt (middleX + maxDistance);
			xBorder = Mathf.Clamp (xBorder, 0, terrainData.heightmapWidth);

			for (; x < xBorder; x++) {
				int y = Mathf.FloorToInt (middleY - maxDistance);
				y = Mathf.Clamp(y, 0, terrainData.heightmapWidth);

				int yBorder = Mathf.CeilToInt (middleY + maxDistance);
				yBorder = Mathf.Clamp (yBorder, 0, terrainData.heightmapWidth);

				for (; y < yBorder; y++) {
					float distance = calculateDistance (x, y, middleX, middleY, maxDistance);
					if (distance < distanceMap [x, y]) {
						distanceMap [x, y] = distance;
					}
				}
			}
		}

		void combineMaps(){
			newHeightMap = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
			for (int x = 0; x < terrainData.heightmapWidth; x++) {
				for (int y=0; y < terrainData.heightmapHeight; y++) {
					newHeightMap[x,y] = combineValues (heightMap[x,y], distanceMap[x,y]);
				}
			}
		}

		void fillDistanceMap(){
			for (int x = 0; x < terrainData.heightmapWidth; x++) {
				for (int y=0; y < terrainData.heightmapHeight; y++) {
					distanceMap [x, y] = 1;
				}
			}
		}

		float calculateDistance(float px, float py, float qx, float qy, float maxDistance){
			float distance = Vector2.Distance(new Vector2(px,py), new Vector2(qx, qy));
			distance = distance / maxDistance;
			return distance;
		}





		float combineValues(float height, float distance){
			return combine(height, preprocess(distance), preprocessWeight(distance));
		}

		float preprocess(float distance)
		{
			switch (preprocessType){
			case PreprocessType.Curve:
			default:
				distance = curve.Evaluate (distance);
				break;
			case PreprocessType.ExponentBase:
				distance = Mathf.Pow (exponentBase, -distance);
				break;
			}
			return Mathf.Clamp(distance, 0, 1);
		}

		float preprocessWeight(float distance){
			distance = weightCurve.Evaluate(distance);
			return Mathf.Clamp(distance, 0, 1);
		}

		float combine(float height, float distance, float weight){
			switch (combinationType){
			case CombinationType.Add:
			default:
				height = distance * weight + height * (1 - weight);
				break;
			case CombinationType.ExponentDeprecated:
				height = Mathf.Pow (height, (1-distance));
				break;
			}
			return height;
		}


	}
}