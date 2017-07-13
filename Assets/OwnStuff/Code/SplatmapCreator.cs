﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
	public class SplatmapCreator : MonoBehaviour {
		public NoiseCreator[] noises;
		public Area[] areas;

		public Vector2 position;

		[Range(0,1)]
		float waterLevel = 0.1f;

		private TerrainData terrainData;
		private float[,] heights;

		private float[,,] splatmap;
		private Transform water;
		private int numOfTextures;

		private Tile[,] tileMap;

		void Start(){
			CreateSplatmap ();
		}

		void initiate(){
			terrainData = GetComponent<Terrain> ().terrainData;
			terrainData.alphamapResolution = terrainData.heightmapResolution;
			heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
			tileMap = new Tile[terrainData.heightmapWidth, terrainData.heightmapHeight];
			processAndCountTextures ();
			splatmap = new float[terrainData.alphamapHeight, terrainData.alphamapWidth, numOfTextures];
			createNoiseCreators ();
		}

		public void CreateSplatmap()
		{
			initiate ();
			for(int y = 0; y < terrainData.alphamapHeight; y++){
				for(int x = 0; x < terrainData.alphamapWidth; x++){
					for (int i = 0; i < areas.Length; i++) {
						if (heights[y,x] <= areas [i].cut) {
							Tile newTile = areas[i].getTile(y,x);
							tileMap [y, x] = newTile;
							splatmap [y, x, newTile.textureNumber] = 1;
							break;
						}
					}
				}
			}
			terrainData.SetAlphamaps (0, 0, splatmap);
		}

		void processAndCountTextures(){
			List<SplatPrototype> list = new List<SplatPrototype> ();
			numOfTextures = 0;
			for (int i = 0; i < areas.Length; i++) {
				Area curArea = areas [i];
				SplatPrototype newSP = new SplatPrototype ();
				newSP.texture = curArea.defaultTexture;
				list.Add (newSP);
				curArea.TextureNumber = numOfTextures;
				numOfTextures++;
				for (int j = 0; j < curArea.subAreas.Length; j++) {
					SubArea curSubArea = curArea.subAreas [j];
					newSP = new SplatPrototype ();
					newSP.texture = curSubArea.texture;
					list.Add (newSP);
					curSubArea.TextureNumber = numOfTextures;
					curSubArea.takeNoise (noises);
					numOfTextures++;
				}
			}
			SplatPrototype[] array = list.ToArray ();
			terrainData.splatPrototypes = array;
		}

		void createNoiseCreators(){
			foreach (NoiseCreator noise in noises) {
				noise.create (terrainData.heightmapResolution);
				if (GetComponent<Controller> ().debugResetRandom) {
					Random.state = GetComponent<Controller> ().randomStartState;
				}
			}
		}
		
		public void getTileName(int x, int y){
			Tile posTile = tileMap [x, y];
			Debug.Log (posTile.areaName + " and " + posTile.subAreaName + " at " + x + " " + y);
		}
		
		public void getTileName(){
			getTileName ((int)position.x, (int)position.y);
		}

		//Shit
		public void setWaterLevel(){
			water = transform.GetChild (0);
			float scaleX = terrainData.heightmapScale.x;
			float scaleY = terrainData.heightmapScale.y;
			float scaleZ = terrainData.heightmapScale.z;
			Debug.Log("ScaleX = " + scaleX + " ScaleY = " + scaleY + " ScaleZ = " + scaleZ);
			water.localPosition = new Vector3 (water.localPosition.x , waterLevel * scaleY, water.localPosition.z);
		}
		
	}
}