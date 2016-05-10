using UnityEngine;
using System.Collections;

public class FollowCameraY : MonoBehaviour {

	public new Camera camera;
	public float distanceY = 1f;
	public bool top = true; 

	void FixedUpdate () {
		if (top) {
			transform.position = new Vector3(transform.position.x, camera.transform.position.y + camera.orthographicSize + distanceY, transform.position.z);
		} else {
			transform.position = new Vector3(transform.position.x, camera.transform.position.y - camera.orthographicSize + distanceY, transform.position.z);
		}
	}
}
