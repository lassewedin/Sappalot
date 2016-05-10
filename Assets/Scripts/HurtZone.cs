using UnityEngine;
using System.Collections;

public class HurtZone : MonoBehaviour {

	public new Camera camera;
	public float distanceY = 1f;

	void FixedUpdate () {
		transform.position = new Vector3(transform.position.x, camera.transform.position.y + camera.orthographicSize + distanceY, transform.position.z);	
	}
}
