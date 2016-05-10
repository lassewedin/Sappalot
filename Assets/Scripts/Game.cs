using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (Input.GetKey(KeyCode.R)) {
			Debug.Log("Restarting...");
			SceneManager.LoadScene("MainScene");
		}
	}
}
