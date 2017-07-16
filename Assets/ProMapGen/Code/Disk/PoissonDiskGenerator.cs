using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProMapGen{
	public class PoissonDiskGenerator {

		// min distance between two points.
		public static float minDist = 5.0f;			
		public static int k = 30;
		// size of the field
		public static float sampleRange = 256.0f;

		// grid to save points
		private static bool[,] grid;
		private static float m_CeiledSampleRange;
		private static float gridCellSize = 0.0f;
		private static int gridLength = 0;

		private static bool IsInputsValid(){
			return (minDist > 0.0f) && (k > 0) && (sampleRange > minDist);
		}

		public static List<Vector2> Generate (){

			if (!IsInputsValid()) {
				Debug.LogWarning("Invalid inputs.");
				return null;
			}

			// Init.

			gridCellSize = minDist / Mathf.Sqrt(2.0f);
			int activePointCount = 0;

			// Create grid.
			gridLength = Mathf.CeilToInt (sampleRange / gridCellSize);
			m_CeiledSampleRange = gridLength * gridCellSize;
			grid = new bool[gridLength,gridLength];

			// Create processing list
			float[] activePointListX = new float[ gridLength * gridLength ];
			float[] activePointListY = new float[ gridLength * gridLength ];

			// randomly add first point
			activePointListX [0] = Random.Range (0.0f, m_CeiledSampleRange);
			activePointListY [0] = Random.Range (0.0f, m_CeiledSampleRange);
			grid [_PositionToGridIndex (activePointListX [0]), _PositionToGridIndex (activePointListY [0])] = true;

			// throw darts
			float dartX = 0.0f, dartY = 0.0f;
			float dartRadians = 0.0f;
			float dartDist = 0.0f;
			int gridX = 0, gridY = 0;

			// for each point in active list 
			for (int proc = 0; proc <= activePointCount; ++proc) {
				// throw darts to get samples
				for(int dart = 0; dart < k; ++dart ){
					// randomly chose a dart in the ring area
					dartRadians = Random.Range(0,Mathf.PI + Mathf.PI);
					dartDist = Random.Range(minDist, minDist + minDist);
					dartX = activePointListX [proc] + dartDist * Mathf.Cos(dartRadians);
					dartY = activePointListY [proc] + dartDist * Mathf.Sin(dartRadians);
					gridX = _PositionToGridIndex(dartX);
					gridY = _PositionToGridIndex(dartY);

					// find out if there is samples near this dart
					bool hasSamples = false;
					for(int x = -1; x <= 1; ++x){
						for(int y = -1; y <= 1; ++y){
							hasSamples |= grid[_WrapIndex(gridX + x), _WrapIndex(gridY + y)];
						}
					}

					if( hasSamples ){
						// there is a sample inside the minimum distance circle, abandon
						continue;
					}
					else{
						// no sample around, add this dart sample into processing list
						++activePointCount;
						grid[gridX,gridY] = true;
						activePointListX[activePointCount] = _WrapRepeatFloat(dartX);
						activePointListY[activePointCount] = _WrapRepeatFloat(dartY);
					}
				}
			}

			List<Vector2> resultList = new List<Vector2>();

			for (int i = 0; i <= activePointCount; ++i) {
				if(activePointListX[i] <= sampleRange && activePointListY[i] <= sampleRange){
					resultList.Add(new Vector2(activePointListX[i], activePointListY[i]));
				}
			}
			return resultList;
		}

		// Given a float, return the grid index in any dimenssion 
		static private int _PositionToGridIndex(float f){
			return Mathf.FloorToInt (_WrapRepeatFloat(f) / gridCellSize);
		}

		// wrap float into generate range
		static private float _WrapRepeatFloat(float f){
			return f - (Mathf.FloorToInt (f / m_CeiledSampleRange) * m_CeiledSampleRange);
		}

		// wrap grid index into grid length
		static private int _WrapIndex(int index){
			return  index < 0 ? (index % gridLength + gridLength) : (index % gridLength);
		}
	}
}