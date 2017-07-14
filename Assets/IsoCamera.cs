using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCamera : MonoBehaviour {

	public float speed = 10;

	private Camera myCam;

	// Use this for initialization
	void Start () {
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
		if (Physics.Raycast (ray, out hitInfo)) {
			if (hitInfo.collider.gameObject.GetComponent<ProMapGen.Controller> ()) {
				Vector3 hitPosition = hitInfo.point;
				GameObject terrain = hitInfo.collider.gameObject;
				TerrainData terraindata = terrain.GetComponent<Terrain> ().terrainData;
				ProMapGen.SplatmapCreator splatmapCreator = terrain.GetComponent<ProMapGen.SplatmapCreator> ();

				hitPosition = terrain.transform.worldToLocalMatrix.MultiplyPoint (hitPosition);
				hitPosition.x = (hitPosition.x / terraindata.size.x) * terraindata.heightmapResolution;
				hitPosition.z = (hitPosition.z / terraindata.size.z) * terraindata.heightmapResolution;
				splatmapCreator.getTileName ((int)hitPosition.z, (int)hitPosition.x);
			}
		}
	}
}
