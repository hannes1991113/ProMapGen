using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TextureCreator : MonoBehaviour {

	public int resolution = 256;

	public int seed = 0;

	public float frequency = 1;

	private Texture2D texture;

	[Range(1, 2)]
	public int dimensions = 2;

	public NoiseMethodType noiseMethodType = NoiseMethodType.Value;



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void FillTexture () {
		CreateTexture ();
		InitRandom ();

		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));


		NoiseMethod method = Noise.noiseMethods[(int)noiseMethodType][dimensions - 1];

		float stepSize = 1.0f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				texture.SetPixel(x, y, Color.white * method(point, frequency));
			}
		}
		texture.Apply();
	}

	void InitRandom(){
		Random.InitState (seed);
	}

	void CreateTexture() {
		texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
		texture.name = "Procedural Texture";
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Trilinear;
		texture.anisoLevel = 9;
		GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
	}

}
