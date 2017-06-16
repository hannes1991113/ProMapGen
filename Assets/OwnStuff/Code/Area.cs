using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area {
	public Texture2D defaultTexture;
	public string name;

	[Range(0,1)]
	public float cut = 1;

	public SubArea[] subAreas;

	private int textureNumber;

	public int TextureNumber {
		set{
			textureNumber = value;
		}
	}

	public int getTextureNumber (int x, int y){
		foreach(SubArea subArea in subAreas){
			int num = subArea.getTextureNumber (x, y);
			if (num > -1) {
				return num;
			}
		}
		return textureNumber;
	}

}
