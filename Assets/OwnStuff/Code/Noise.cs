using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {

	private int repeat;
	private double persistence;
	private int octaves;
	private double lacunarity;
	private double scale;

	public Noise(double persistence, double lacunarity, int octaves, double scale, int r = -1){
		change (persistence, lacunarity, octaves, scale);
	}

	public void change(double persistence, double lacunarity, int octaves, double scale, int r = -1){
		this.persistence = persistence;
		this.octaves = octaves;
		this.lacunarity = lacunarity;
		this.scale = scale;
		repeat = r;
	}

	public double OctaveNoise(double x, double y, double z) {
		double total = 0;
		double frequency = scale;
		double amplitude = 1;
		double maxValue = 0;			// Used for normalizing result to 0.0 - 1.0
		for(int i=0;i<octaves;i++) {
			total += PerlinNoise.perlin(x * frequency, y * frequency, z * frequency, repeat) * amplitude;

			maxValue += amplitude;
			amplitude *= persistence;
			frequency *= lacunarity;
		}

		return total/maxValue;
	}
}

