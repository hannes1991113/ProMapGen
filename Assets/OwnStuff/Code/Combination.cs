﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour {
	public enum CombinationType {
		Add,
		Multiply,
		Exponent,
	};

	public int maxDistance = 10;
	public CombinationType combinationType = CombinationType.Add;

	public float exp = 1;
	public float mult = 1;
	public float add = 0;

	[Range(0,1)]
	public float weight;
	public AnimationCurve hillCurve;

	private TerrainData terrainData;
	private float[,] heightMap;
	private float[,] distanceMap;
	private float[,] ppDistanceMap;
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
		ppDistanceMap = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
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
					ppDistanceMap [x, y] = preprocessDistance (distance);;
				}
			}
		}
	}

	void combineMaps(){
		newHeightMap = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		for (int x = 0; x < terrainData.heightmapWidth; x++) {
			for (int y=0; y < terrainData.heightmapHeight; y++) {
				newHeightMap[x,y] = combineValues (heightMap[x,y], ppDistanceMap[x,y], distanceMap[x,y]);
			}
		}
	}

	float combineValues(float height, float distance, float realDistance){
		float returnValue = 0;
		switch (combinationType){
		case CombinationType.Multiply:
			returnValue = height * distance;
			break;
		case CombinationType.Exponent:
			returnValue = Mathf.Pow (height, distance);
			break;
		case CombinationType.Add:
		default:
			returnValue = distance * weight + height * (1 - weight);
			break;
		}
		return returnValue;
	}

	void fillDistanceMap(){
		for (int x = 0; x < terrainData.heightmapWidth; x++) {
			for (int y=0; y < terrainData.heightmapHeight; y++) {
				ppDistanceMap [x, y] = preprocessDistance (1.414f);
				distanceMap [x, y] = 1.414f;
			}
		}
	}

	float preprocessDistance(float distance)
	{
		float ppDistance;
		ppDistance = hillCurve.Evaluate (distance);
		return ppDistance;
	}

	float calculateDistance(float px, float py, float qx, float qy){
		float distance = Vector2.Distance(new Vector2(px,py), new Vector2(qx, qy));
		distance = distance / maxDistance;
		return distance;
	}
}
