using UnityEngine;
using System.Collections;

public class Rifle : Weapon {

	public Rigidbody bullet;
	public GameObject muzzleTransform;

	[Range(0.0f, 1.0f) ]
	public float addParentVelocity = 0f;

	[Range(0.0f, 1.0f) ]
	public float period = 0.1f;

	[SerializeField]
	[Range(0.0f, 100.0f) ]
	private float leavingVelocity = 10f;

	private Animator anim;

	void Start() {
		anim = gameObject.GetComponent<Animator> ();
	}

	void Update() {
		UpdateCoolDown(Time.deltaTime);
		this.coolDownTimePeriod = period;

	}

	override public void Attack() {

		Vector3 parentV = new Vector3 ();
		Rigidbody parentRB = transform.parent.gameObject.GetComponentInParent<Rigidbody>();
		if (parentRB != null) {
			parentV = parentRB.velocity;
		}

		if (IsCoolDownCold ()) {

			anim.SetTrigger("Fire");

			gameObject.GetComponent<AudioSource>().Play();

			Rigidbody bulletInstance = Instantiate (bullet, muzzleTransform.transform.position, muzzleTransform.transform.rotation) as Rigidbody;
			bulletInstance.velocity = parentV * addParentVelocity + this.transform.forward * leavingVelocity;

			SetCoolDownWarm();
		}
	}



}
