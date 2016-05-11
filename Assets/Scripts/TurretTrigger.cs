using UnityEngine;
using System.Collections;

public class TurretTrigger : MonoBehaviour {

	public Weapon[] weapons;

	void OnTriggerStay(Collider collider) {

		Debug.Log("On trigger");

		if (collider.tag == "Player") {
			weapons[0].Attack();
		}
	}

}
