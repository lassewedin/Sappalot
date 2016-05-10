using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerHurt : MonoBehaviour {

	public int startHP = 100;
	public int hurtZoneDamage = 1;
	public int hp { get; private set;} 
	public HpText hpText;

	// Use this for initialization
	void Start () {
		hp = startHP;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider collider) 
	{
		if (collider.tag == "HurtZone") {
			Debug.Log("Player Hurt");
			hp -= hurtZoneDamage;
			UpdateHp();
		}

		if (collider.tag == "KillZone") {
			Debug.Log("Player Killed");
			hp -= 10000000;
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
