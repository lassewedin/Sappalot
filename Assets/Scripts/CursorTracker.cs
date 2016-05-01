using UnityEngine;
using System.Collections;


//
public class CursorTracker : MonoBehaviour {

	public new Camera camera;
	public float worldAltitude = 0.5f;
	public GameObject sphere;


	[HideInInspector]
	public Vector3 worldLocation = new Vector3();

	[HideInInspector]
	public bool pointingOnPlane = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Plane intersectionPlane = new Plane (new Vector3(0f, 1f, 0f), new Vector3(0f, worldAltitude, 0f));

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float rayDistance;
		if (intersectionPlane.Raycast (ray, out rayDistance)) {
			worldLocation = ray.GetPoint (rayDistance);
			pointingOnPlane = true;
		} else {
			worldLocation = new Vector3();
			pointingOnPlane = false;
		}

		sphere.transform.position = new Vector3(worldLocation.x, worldLocation.y, worldLocation.z);
	}

	void FixedUpdate() {



	}

	public void foo() {

	}
}
