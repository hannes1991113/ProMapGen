using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area {
	public string name;
	public Texture2D defaultTexture;

	[Range(0,1)]
	public float cut = 1;

	public SubArea[] subAreas;

	private int textureNumber;

	public int TextureNumber {
		set{
			textureNumber = value;
		}
	}

	public Tile getTile (int x, int y){
		Tile tile = new Tile ();
		tile.areaName = name;
		tile.textureNumber = textureNumber;
		foreach(SubArea subArea in subAreas){
			int num = subArea.getTextureNumber (x, y);
			if (num > -1) {
				tile.textureNumber = num;
				tile.subAreaName = subArea.name;
				break;
			}
		}
		return tile;
	}

}
