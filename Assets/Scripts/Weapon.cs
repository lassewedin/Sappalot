using UnityEngine;
using System.Collections;
using System;


public abstract class Weapon : MonoBehaviour {


	protected float coolDownTimePeriod = 0.1f;
	protected float coolDownTimeLeft = 0f;

	protected void UpdateCoolDown(float deltaTime ) {
		coolDownTimeLeft -= deltaTime;
	}

	protected bool IsCoolDownCold() {
		return coolDownTimeLeft < 0f;
	}

	protected void SetCoolDownWarm() {
		coolDownTimeLeft = coolDownTimePeriod;
	}

	protected float CoolDown {
		get{return coolDownTimeLeft;}
	}

	abstract public void Attack ();


}

