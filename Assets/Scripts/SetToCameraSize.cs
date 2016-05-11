﻿using UnityEngine;
using System.Collections;

public class SetToCameraSize : MonoBehaviour {

	public new Camera camera;

	private BoxCollider boxCollider;

	private void Start() {
		boxCollider = GetComponent<BoxCollider>();
	}

	private void Update() {
		float aspect = (float)Screen.width / (float)Screen.height;

		float height = camera.orthographicSize * 2f;
		float width = aspect * height;
		transform.localScale = new Vector3(width, height, transform.localScale.z);

		transform.position = new Vector3(transform.position.x, camera.transform.position.y, transform.position.z);

	}
}