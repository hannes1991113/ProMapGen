using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour {
	public enum CombinationType {
		Add,
		Exponent,
	};

	public enum PreprocessType {
		None,
		Exponent,
	}

	public int maxDistance = 10;
	public CombinationType combinationType = CombinationType.Add;
	[Range(0,1)]
	public float weight;
	public AnimationCurve addCurve;
	public AnimationCurve expCurve;


	private TerrainData terrainData;
	private float[,] heightMap;
	private float[,] distanceMap;
	private float[,] newHeightMap;

	private DiskDistribution diskDistribution;
	private List<Vector2> disks;

	void initiate(){
		diskDistribution = GetComponent<DiskDistribution> ();
		terrainData = GetComponent<Terrain> ().terrainData;
		heightMap = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
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
		int x = Mathf.Clamp(middleX - maxDistance, 0, terrainData.heightmapWidth);
		int xBorder = Mathf.Clamp ((middleX + maxDistance), 0, terrainData.heightmapWidth);
		for (; x < xBorder; x++) {
			int y = Mathf.Clamp(middleY - maxDistance, 0, terrainData.heightmapWidth);
			int yBorder = Mathf.Clamp ((middleY + maxDistance), 0, terrainData.heightmapWidth);
			for (; y < yBorder; y++) {
				float distance = calculateDistance (x, y, middleX, middleY);
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
		float newHeight = 0;
		distance = preprocessDistance(distance);
		switch (combinationType){
		case CombinationType.Add:
		default:
			newHeight = distance * weight + height * (1 - weight);
			break;
		case CombinationType.Exponent:
			newHeight = Mathf.Pow (height, distance);
			break;
		}
		return newHeight;
	}

	void fillDistanceMap(){
		for (int x = 0; x < terrainData.heightmapWidth; x++) {
			for (int y=0; y < terrainData.heightmapHeight; y++) {
				distanceMap [x, y] = 1.414f;
			}
		}
	}

	float preprocessDistance(float distance)
	{
		switch(combinationType){
		case CombinationType.Add:
		default:
			distance = addCurve.Evaluate (distance);
			break;
		case CombinationType.Exponent:
			distance = expCurve.Evaluate (distance);
			break;
		}
		return distance;
	}

	float calculateDistance(float px, float py, float qx, float qy){
		float distance = Vector2.Distance(new Vector2(px,py), new Vector2(qx, qy));
		distance = distance / maxDistance;
		return distance;
	}
}
