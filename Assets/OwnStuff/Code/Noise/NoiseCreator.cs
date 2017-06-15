using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseCreator {

	public string name;
	public bool random = true;
	public float xOffset = 0;
	public float yOffset = 0;

	public NoiseType noiseType = NoiseType.Perlin;
	[Range(1,100)]
	public double scale = 20;
	[Range(1, 8)]
	public int octaves = 1;
	[Range(0f, 1f)]
	public double persistence = 0.5f;
	[Range(1f, 4f)]
	public double lacunarity = 2;
	[Range(0,10)]
	public float exponent = 1;


	public Texture2D texture;

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

	public float OctaveNoise(double x, double y, double z) {
		x += xOffset;
		y += yOffset;
		double total = 0;
		double frequency = scale;
		double amplitude = 1;
		double maxValue = 0;			// Used for normalizing result to 0.0 - 1.0
		for(int i=0;i<octaves;i++) {
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
			total += value * amplitude;
			maxValue += amplitude;
			amplitude *= persistence;
			frequency *= lacunarity;
		}
		double returnValue = total / maxValue;
		return (float)Mathf.Pow ((float)returnValue, exponent);
	}

	public void createTexture(int sizeX, int sizeY){
		texture = new Texture2D (sizeX, sizeY, TextureFormat.RGB24, false);
		for(int y = 0; y < sizeY; y++) {
			for(int x = 0; x < sizeX; x++) {
				double xCoord = x / (double)sizeX;
				double yCoord = y / (double)sizeY;
				double sample = OctaveNoise(xCoord, yCoord, 0);
				texture.SetPixel((int)x, (int)y, Color.white * (float) sample);
			}
		}
		texture.Apply ();
	}

	public float[,] createField(int sizeX, int sizeY){
		float[,] field = new float[sizeX,sizeY];
		for(int y = 0; y < sizeY; y++) {
			for(int x = 0; x < sizeX; x++) {
				double xCoord = x / (double)sizeX;
				double yCoord = y / (double)sizeY;
				double sample = OctaveNoise(xCoord, yCoord, 0);
				field [x, y] = (float)sample;
			}
		}
		return field;
	}

	public void setRandom(){
		if (random) {
			xOffset = Random.Range (0.0f, 1000.0f);
			yOffset = Random.Range (0.0f, 1000.0f);
		}
	}
}