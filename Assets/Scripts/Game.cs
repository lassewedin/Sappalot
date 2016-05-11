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
		StartCoroutine(ShowGetReady());
	}

	void FixedUpdate() {
		if (Input.GetKey(KeyCode.R)) {
			Reset();
		}

		cameraSpeed += cameraAcceleration * Time.fixedDeltaTime;
		camera.transform.position += Vector3.down * Time.fixedDeltaTime * cameraSpeed; 

	}

	public void GameOver(float waitBefore) {
		StartCoroutine(WaitThenGameOver());
	}

	public void Reset() {
		SceneManager.LoadScene("MainScene");
	}


	private IEnumerator ShowGetReady() {
		centerMessage.text = "Get Ready!";
		yield return new WaitForSeconds(1f);
		centerMessage.text = string.Empty;

	}

	private IEnumerator WaitThenGameOver() {
		yield return new WaitForSeconds(2f);
		StartCoroutine(ShowGameOverThenRestart());
	}

	private IEnumerator ShowGameOverThenRestart() {
		centerMessage.text = "Game Over";
		yield return new WaitForSeconds(3f);
		centerMessage.text = string.Empty;
		Reset();
	}
}
