using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatmapCreator : MonoBehaviour {

	public Texture2D[] textures;

	public NoiseCreator[] noises;
	public Area[] areas;

	[Range(0,1)]
	public float[] terrainCuts = {0.1f, 0.2f};
	[Range(0,1)]
	float waterLevel = 0.1f;

	private TerrainData terrainData;
	private float[,] heights;

	private float[,,] splatmap;
	private Transform water;
	private int numOfTextures;

	void initiate(){
		terrainData = GetComponent<Terrain> ().terrainData;
		terrainData.alphamapResolution = terrainData.heightmapResolution;
		heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
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
						splatmap [y, x, areas[i].getTextureNumber(y,x)] = 1;
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
			if (GetComponent<HeightCalc> ().resetRandom) {
				Random.state = GetComponent<HeightCalc> ().randomStartState;
			}
			if (GetComponent<HeightCalc> ().createTextures) {
				noise.createTexture ();
			}
		}
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
