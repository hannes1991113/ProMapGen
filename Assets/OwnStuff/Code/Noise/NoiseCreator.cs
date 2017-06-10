using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseCreator {

	private int repeat;
	private double persistence;
	private int octaves;
	private double lacunarity;
	private double scale;
	private NoiseType noiseType;

	public NoiseCreator(double persistence, double lacunarity, int octaves, double scale, NoiseType noiseType, int r = -1){
		this.persistence = persistence;
		this.octaves = octaves;
		this.lacunarity = lacunarity;
		this.scale = scale;
		this.noiseType = noiseType;
		repeat = r;
	}

	// To get a single octave out of the Noisemap
	// octaves starting with 0
	// Not in use yet, but maybe later
	public double getOctave(double x, double y, double z, int octave){
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
			value = PerlinNoise.perlin (x2, y2, z * frequency, repeat);
			break;
		}
		return value;
	}

	public double OctaveNoise(double x, double y, double z) {
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
				value = PerlinNoise.perlin (x2, y2, z * frequency, repeat);
				break;
			}
			total += value * amplitude;
			maxValue += amplitude;
			amplitude *= persistence;
			frequency *= lacunarity;
		}
		double returnValue = total / maxValue;
		return returnValue;
	}
}