using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProMapGen{
	public class MapAnalyser : MonoBehaviour {

		public int count;
		public float maxValue;
		public float minValue;
		[Range(5,128)]
		public int accuracy = 128;
		public AnimationCurve curve;

		private TerrainData terrainData;
		private float[,] heights;
			private int[] histogram;

		void initiate(){
			terrainData = GetComponent<Terrain> ().terrainData;
			heights = terrainData.GetHeights (0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
			minValue = 2;
			maxValue = -1;
			count = 0;
			histogram = new int[accuracy];
		}

		public void createHistogram()
		{
			initiate ();
			for(int y = 0; y < terrainData.heightmapHeight; y++) {
				for(int x = 0; x < terrainData.heightmapWidth; x++) {
					float height = heights [y, x];
					count++;
					checkMaxMin(height);
					addToCurve (height);
				}
			}
			createCurve ();
		}

		private void createCurve(){
			curve = new AnimationCurve ();
			for (int i = 0; i < accuracy; i++) {
				curve.AddKey (((float)i) / accuracy, histogram [i]);
			}
		}

		private void checkMaxMin(float height){
			if (height > maxValue)
				maxValue = height;
			if (height < minValue)
				minValue = height;
		}

		private void addToCurve(float height){
			int i = Mathf.Clamp ((int)(height * accuracy), 0, accuracy - 1);
			histogram [i]++;
		}
	}
}
