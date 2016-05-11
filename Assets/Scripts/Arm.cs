using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {

	public bool armed { get; private set;}

	void Start() {
		armed = false;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "ArmZone") {
			Debug.Log("Arming: " + gameObject);
			armed = true;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.tag == "ArmZone") {
			Debug.Log("Disarming: " + gameObject);
			armed = false;
		}
	}

}
