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

		public bool normalize = false;
		public bool equalize = false;

		public Texture2D texture;

		public AnimationCurve curve;

		private int accuracy = 128;
		private float minValue;
		private float maxValue;
		private int count;
		private int[] histogram;

		private float[,] field;

		public float[,] getField(){
			return field;
		}

		private int size;

		public void create(int size){
			this.size = size;
			if (random) {
				xOffset = Random.Range (0.0f, 1000.0f);
				yOffset = Random.Range (0.0f, 1000.0f);
			}
		}

		public float[,] createField(){
			field = new float[size,size];
			for(int y = 0; y < size; y++) {
				for(int x = 0; x < size; x++) {
					float sample = OctaveNoise(x,y, 0);
					field [x, y] = sample;
				}
			}
			if (normalize) {
				normalizeField ();
			}
			if (equalize) {
				equalizeField ();
			}
			createHistogramCurve ();
			createTexture ();
			return field;
		}

		void createTexture(){
			texture = new Texture2D (size, size, TextureFormat.RGB24, false);
			for(int y = 0; y < size; y++) {
				for(int x = 0; x < size; x++) {
					float sample = field [x, y];
					texture.SetPixel(y, x, Color.white * (1 - sample));
				}
			}
			texture.Apply ();
		}

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




		void createHistogramCurve(){
			createHistogram ();
			curve = new AnimationCurve ();
			for (int i = 0; i < accuracy; i++) {
				curve.AddKey (((float)i) / accuracy, histogram [i]);
			}
		}

		void createHistogram(){
			minValue = 2;
			maxValue = -1;
			count = 0;
			histogram = new int[accuracy];
			for(int y = 0; y < size; y++) {
				for(int x = 0; x < size; x++) {
					float height = field [x, y];
					count++;
					if (height > maxValue)
						maxValue = height;
					if (height < minValue)
						minValue = height;
					int i = Mathf.Clamp ((int)(height * accuracy), 0, accuracy - 1);
					histogram [i]++;
				}
			}
		}


		void normalizeField(){
			createHistogram ();
			for(int y = 0; y < size; y++) {
				for(int x = 0; x < size; x++) {
					float height = field [x, y];
					height = (height - minValue) * (1 / (maxValue - minValue));
					field [x, y] = height;
				}
			}
		}

		void equalizeField(){
			createHistogram ();
			int[] kumHistogram = new int[accuracy];
			int add = 0;
			for (int i = 0; i < accuracy; i++) {
				add += histogram [i];
				kumHistogram [i] = add;
			}
			float[] normKumHist = new float[accuracy];
			for (int i = 0; i < accuracy; i++) {
				normKumHist [i] = kumHistogram [i] / (float)kumHistogram [accuracy - 1];
			}
			for(int y = 0; y < size; y++) {
				for(int x = 0; x < size; x++) {
					float height = field [x, y];
					int i = Mathf.Clamp ((int)(height * accuracy), 0, accuracy - 1);
					field [x, y] = normKumHist [i];
				}
			}
		}


	}
}