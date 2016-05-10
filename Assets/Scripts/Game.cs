using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public new Camera camera;
	public float cameraStartSpeed = 1f;
	public float cameraAcceleration = 0.1f;

	private float cameraSpeed;

	public Text centerMessage;

	void Start () {
		cameraSpeed = cameraStartSpeed;

		StartGame();
	}

	void FixedUpdate() {
		if (Input.GetKey(KeyCode.R)) {
			Reset();
		}

		cameraSpeed += cameraAcceleration * Time.fixedDeltaTime;
		camera.transform.position += Vector3.down * Time.fixedDeltaTime * cameraSpeed; 

	}

	public void GameOver() {
		centerMessage.text = "Game Over";
		Reset();
	}

	public void StartGame() {
		centerMessage.text = "Get Ready!";
	}

	public void Reset() {
		SceneManager.LoadScene("MainScene");
	}
}
