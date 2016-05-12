using UnityEngine;
using System.Collections;

public class SetToCameraSize : MonoBehaviour {

	public new Camera camera;

	private BoxCollider boxCollider;

	public float exagerate = 1f;

	private void Start() {
		boxCollider = GetComponent<BoxCollider>();
	}

	private void Update() {
		float aspect = (float)Screen.width / (float)Screen.height;

		float height = camera.orthographicSize * 2f * exagerate;
		float width = aspect * height * 4;
		transform.localScale = new Vector3(width, height, transform.localScale.z);

		transform.position = new Vector3(transform.position.x, camera.transform.position.y, transform.position.z);

	}
}
