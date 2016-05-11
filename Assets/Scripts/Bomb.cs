using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public GameObject explosion;
	private PlayerEnergy energyPlayerFire;
	private PlayerEnergy energyPlayerIce;

	public int damage = 30;

	void Start() {
		energyPlayerFire = GameObject.Find("Player Fire").GetComponent<PlayerEnergy>();
		energyPlayerIce = GameObject.Find("Player Ice").GetComponent<PlayerEnergy>();
	}

	void OnTriggerEnter (Collider collider) {
		
		if (collider.tag == "DetonateZone") {
			Explode();
		} else if (collider.tag == "DisarmZone") {
			Debug.Log("disarming bomb");
			Destroy(gameObject);
		}
	}

	private void Explode() {
		energyPlayerFire.Hit(damage, (energyPlayerFire.gameObject.transform.position - transform.position).normalized * 20f);
		energyPlayerIce.Hit(damage, (energyPlayerIce.gameObject.transform.position - transform.position).normalized * 20f);

		GameObject explosionInstance = Instantiate (explosion, transform.position, Quaternion.identity ) as GameObject;
		Destroy(gameObject);
	}
}
