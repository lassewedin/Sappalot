using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{

	public GameObject player; //reads only transform position
	public GameObject cursorWorldPosition; //reads only transform position

	[Range(0.0f, 10f)]
	public float cameraTilt = 1f; //degrres per unit

	[Range(0.0f, 50f)]
	public float cameraTiltMax = 30f;

	[Range(5f, 100f)]
	public float cameraAltitude = 10f;

	[Range(0f, 10f)]
	public float audioListenerAltitude = 10f;

	[Range(0.0f, 0.8f)]
	public float cameraCloseness = 0.5f;
	[Range(0.0f, 0.3f)]
	public float cameraSmoothness = 0.05f;

	private bool follow = true;

	void FixedUpdate ()	{
		if (Input.GetButtonDown ("Fire3")) { //MMB
			follow = !follow;

		}
	}

	void LateUpdate() {
		transform.position = new Vector3 (transform.position.x, cameraAltitude ,transform.position.z);
		gameObject.transform.GetChild (0).position = new Vector3 (1f, 1f, 1f);

		if(	follow	)
			TrackPlayer();
	}
	
	void TrackPlayer ()	{
		Vector3 playerPosition = player.transform.position;

		Vector3 playerToCursor = cursorWorldPosition.GetComponent<CursorTracker>().worldLocation - playerPosition;

		Vector3 cameraGoalPosition = playerPosition + playerToCursor * cameraCloseness;

		Vector3 newCameraPosition;
		if(cameraSmoothness != 0.0f )
			newCameraPosition = transform.position + (cameraGoalPosition - transform.position) * Time.deltaTime / cameraSmoothness;
		else
			newCameraPosition = cameraGoalPosition;

		transform.position = new Vector3 (newCameraPosition.x, transform.position.y, newCameraPosition.z);

		transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		transform.Rotate (90f - Mathf.Clamp (playerToCursor.z * cameraTilt, -cameraTiltMax, cameraTiltMax), 0f, 0f);
		transform.Rotate (0f, Mathf.Clamp(playerToCursor.x * cameraTilt,-cameraTiltMax, cameraTiltMax), 0f);


	}
}
