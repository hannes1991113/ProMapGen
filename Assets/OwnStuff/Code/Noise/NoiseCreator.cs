using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
[System.Serializable]
public class NoiseCreator {

	public string name;
	public bool random = true;
	public float xOffset = 0;
	public float yOffset = 0;

	public NoiseType noiseType = NoiseType.Perlin;
	[Range(1,100)]
	public float scale = 20;
	[Range(1, 8)]
	public int octaves = 1;
	[Range(0f, 1f)]
	public float persistence = 0.5f;
	[Range(1f, 4f)]
	public float lacunarity = 2;
	[Range(0,10)]
	public float exponent = 1;

	public Texture2D texture;

	private int size;

	public float OctaveNoise(int x, int y, int z) {
		float thisX = (x + xOffset) / size;
		float thisY = (y + yOffset) / size;
		float total = 0;
		float frequency = scale;
		float amplitude = 1;
		float maxValue = 0;			// Used for normalizing result to 0.0 - 1.0
		for(int i=0;i<octaves;i++) {
			float value;
			switch (noiseType) {
			case NoiseType.PerlinUnity:
				float x1 = thisX * frequency;
				float y1 = thisY * frequency;
				value = Mathf.PerlinNoise (x1, y1);
				break;
			case NoiseType.Perlin:
			default:
				float x2 = thisX * frequency;
				float y2 = thisY * frequency;
				value = (float)PerlinNoise.perlin (x2, y2, z * frequency);
				break;
			}
			if (value > 1 || value < 0) {
				Debug.LogWarning ("Wrong NoiseValue: " + value + " at Octave " + i + " with " + noiseType.ToString());
				break;
			}
			total += value * amplitude;
			maxValue += amplitude;
			amplitude *= persistence;
			frequency *= lacunarity;
		}
		float returnValue = total / maxValue;
		return Mathf.Pow (returnValue, exponent);
	}

	public void createTexture(){
		texture = new Texture2D (size, size, TextureFormat.RGB24, false);
		for(int y = 0; y < size; y++) {
			for(int x = 0; x < size; x++) {
				double sample = OctaveNoise(x, y, 0);
				texture.SetPixel((int)x, (int)y, Color.white * (float) sample);
			}
		}
		texture.Apply ();
	}

	public float[,] createField(){
		float[,] field = new float[size,size];
		for(int y = 0; y < size; y++) {
			for(int x = 0; x < size; x++) {
				float sample = OctaveNoise(x,y, 0);
				field [x, y] = sample;
			}
		}
		return field;
	}

	public void create(int size){
		this.size = size;
		if (random) {
			xOffset = Random.Range (0.0f, 1000.0f);
			yOffset = Random.Range (0.0f, 1000.0f);
		}
	}



	// To get a single octave out of the Noisemap
	// octaves starting with 0
	// Not in use yet, but maybe later
	// DEPRECATED
	public double getSingleOctave(double x, double y, double z, int octave){
		double frequency = scale;
		for(int i=0;i < octave; i++) {
			frequency *= lacunarity;
		}
		double value;
		switch (noiseType) {
		case NoiseType.PerlinUnity:
			float x1 = (float)(x * frequency);
			float y1 = (float)(y * frequency);
			value = Mathf.PerlinNoise (x1, y1);
			break;
		case NoiseType.Perlin:
		default:
			double x2 = x * frequency;
			double y2 = y * frequency;
			value = PerlinNoise.perlin (x2, y2, z * frequency);
			break;
		}
		return value;
	}
}
}