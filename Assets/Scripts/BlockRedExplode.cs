using UnityEngine;
using System.Collections;

public class BlockRedExplode : Interactable {

	public GameObject blockDestroyed;

	private bool burn = false;
	private bool explode = false;

	void Awake() {
		isPortable = false;
		isWalkable = false;
	}

	override public void Explode() {
		if (!explode) {
			Instantiate (blockDestroyed, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
		
	override public void Burn() {
		if (!burn) { 
			burn = true;
		}
	}

	override public void OnPickup() {
		print ("I am getting picked up " + this );
	}

	override public void OnThrow( Vector3 velocity) {
		print ("I am getting thrown " + this );
	}

	override public void OnPlant() {
	}
}
