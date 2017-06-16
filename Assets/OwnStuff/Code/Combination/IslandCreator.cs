using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCreator : MonoBehaviour {

	public float maxSize = 10;

	public float minSize = 5;

	public Combination[] combiner = {new Combination()};

	private TerrainData terrainData;
	private float[,] heightMap;
	private float[,] distanceMap;
	private float[,] newHeightMap;

	private DiskDistribution diskDistribution;
	private List<Vector2> disks;

	void initiate(){
		diskDistribution = GetComponent<DiskDistribution> ();
		terrainData = GetComponent<Terrain> ().terrainData;
		heightMap = GetComponent<HeightCalc> ().getHeights ();
	}

	public void combineWithDisks(){
		initiate ();
		disks = diskDistribution.generateDisks ();
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

	float combineValues(float height, float distance){
		for(int i = 0; i < combiner.Length; i++){
			height = combiner [i].execute (height, distance);
		}
		return height;
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
}
