using UnityEngine;
using System.Collections;

public class LifePickup : MonoBehaviour {

	public int healHp = 5;

	void OnTriggerEnter (Collider collider) {

		if (collider.tag == "Player") {
			collider.gameObject.GetComponent<PlayerEnergy>().Heal(healHp);
			Destroy(transform.parent.gameObject);
		}
	}
}
