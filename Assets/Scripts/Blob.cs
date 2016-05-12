using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour {

    public int damage = 10;
	public float jumpForce = 20f;
	public float jumpCooldown = 1f;

	private Rigidbody rigidbody;
	private float jumpCooldownLeft = 0f; 

	private void OnTriggerStay(Collider collider) {

		if (collider.tag == "Player") {
			collider.GetComponent<PlayerEnergy>().Hit(damage, this.GetComponent<Rigidbody>().velocity);
		}

		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		if (collider.tag == "Block") {
			if (jumpCooldownLeft <= 0f) {
			
				rigidbody.AddForce(new Vector3(Random.Range(-jumpForce * 0.5f, jumpForce * 0.5f), jumpForce + Random.Range(0f, jumpForce * 0.5f), 0f), ForceMode.Impulse);
				jumpCooldownLeft = jumpCooldown;
			} else {
				jumpCooldownLeft -= Time.fixedDeltaTime;
			}
		}
    }

    private void OnTriggerEnter(Collider collider) {

    }

	private GameObject GetClosestPlayer() {
		return null;
	}

	private GameObject playerFire;
	private GameObject playerIce;
	private void FindPlayers() {
		playerFire = GameObject.Find("Player Fire");
		playerIce = GameObject.Find("Player Ice");
	}
}
