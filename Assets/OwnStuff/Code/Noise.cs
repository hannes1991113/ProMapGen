using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {

	private int repeat;
	private double persistence;
	private int octaves;
	private double lacunarity;
	private double scale;
	private NoiseType noiseType;

	public Noise(double persistence, double lacunarity, int octaves, double scale, NoiseType noiseType, int r = -1){
		this.persistence = persistence;
		this.octaves = octaves;
		this.lacunarity = lacunarity;
		this.scale = scale;
		this.noiseType = noiseType;
		repeat = r;
	}

	public double OctaveNoise(double x, double y, double z) {
		double total = 0;
		double frequency = scale;
		double amplitude = 1;
		double maxValue = 0;			// Used for normalizing result to 0.0 - 1.0
		for(int i=0;i<octaves;i++) {
			
			switch (noiseType) {
			case NoiseType.Perlin:
				total += PerlinNoise.perlin (x * frequency, y * frequency, z * frequency, repeat) * amplitude;
				break;
			case NoiseType.PerlinUnity:
				total += Mathf.PerlinNoise ((float)(x * frequency), (float)(y * frequency));
				break;
			default:
				break;
			}

			maxValue += amplitude;
			amplitude *= persistence;
			frequency *= lacunarity;
		}

		return total/maxValue;
	}
}

