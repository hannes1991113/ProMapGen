using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
[System.Serializable]
public class SubArea {
	public string name;
	public string noiseName;
	public Texture2D texture;

	[Range(0,1)]
	public float cut = 1;

	private int textureNumber;
	private NoiseCreator myNoise;

	public int TextureNumber {
		set{
			textureNumber = value;
		}
	}

	public int getTextureNumber(int x, int y){
		if (myNoise.OctaveNoise (x, y, 0) <= cut) {
			return textureNumber;
		} else {
			return -1;
		}
	}

	public void takeNoise(NoiseCreator[] noises){
		for (int i = 0; i < noises.Length; i++) {
			if (noises [i].name == noiseName) {
				myNoise = noises [i];
				break;
			}
		}
		if (myNoise == null) {
			Debug.Log ("Area named " + name + "did not find Noise named: " + noiseName);
		}
	}
}
}