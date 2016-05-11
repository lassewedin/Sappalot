using UnityEngine;
using System.Collections;

public class TurretTrigger : MonoBehaviour {

	public Weapon[] weapons;
	public Arm arm;

	void OnTriggerStay(Collider collider) {
		if (collider.tag == "Player" && arm.armed) {
			weapons[0].Attack();
		}
	}
}
