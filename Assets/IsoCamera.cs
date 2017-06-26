using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour {

	public float speed = 10;

	private GameObject terrain;

	private TerrainCollider terrainCollider;

	private TerrainData terraindata;

	private ProMapGen.SplatmapCreator splatmapCreator;

	private Camera myCam;

	// Use this for initialization
	void Start () {
		terrain = GameObject.Find ("Terrain");
		terrainCollider = terrain.GetComponent<TerrainCollider> ();
		terraindata = terrain.GetComponent<Terrain> ().terrainData;
		splatmapCreator = terrain.GetComponent<ProMapGen.SplatmapCreator> ();
		myCam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * Input.GetAxis ("Vertical") * speed, Space.World);
		transform.Translate (Vector3.right * Input.GetAxis ("Horizontal") * speed, Space.World);

		if(Input.GetMouseButtonDown(0)){
			checkCollission();
		}
	}
	
	void checkCollission(){
		Ray ray = myCam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if (terrainCollider.Raycast (ray, out hitInfo, 9999)) {
			Vector3 hitPosition = hitInfo.point;
			hitPosition = terrain.transform.worldToLocalMatrix.MultiplyPoint (hitPosition);
			hitPosition.x = (hitPosition.x / terraindata.size.x) * terraindata.heightmapResolution;
			hitPosition.z = (hitPosition.z / terraindata.size.z) * terraindata.heightmapResolution;
			splatmapCreator.getTileName ((int)hitPosition.z, (int)hitPosition.x);
		}
	}
}
