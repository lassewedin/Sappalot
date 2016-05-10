using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerHurt : MonoBehaviour {

	public int startHP = 100;
	public int hurtZoneDamage = 1;
	public float hurtZoneForce = 10f;

	public int hp { get; private set;} 
	public HpText hpText;

	private new Rigidbody rigidbody;

	void Start () {
		hp = startHP;
	}

	void OnTriggerEnter (Collider collider) 
	{
		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		if (collider.tag == "HurtZone") {
			rigidbody.AddForce(new Vector3(0f, -hurtZoneForce, 0f), ForceMode.Impulse);
			hp -= hurtZoneDamage;
			UpdateHp();
		}

		if (collider.tag == "KillZone") {
			hp = 0;
			UpdateHp();
		}

	}

	private void UpdateHp() {
		hpText.setHp(Math.Max(hp, 0));
		if (hp <= 0) {
			Debug.Log("Game Over");
		}
	}
}
