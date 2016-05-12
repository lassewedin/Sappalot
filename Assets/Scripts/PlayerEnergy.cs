using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerEnergy : MonoBehaviour {

	public Game game;
	public GameObject BloodStainParticles;
	public Material playerMaterial;
	public Color color;

	public int startHP = 100;
    public int maxHP = 100;
	public int hurtZoneDamage = 1;
	public float hurtZoneForce = 10f;

	public int hp { get; private set;} 
	public HpText hpText;

	private new Rigidbody rigidbody;
	private float hitCoolDown = 0f;

	void Start () {
		hp = startHP;
		UpdateHp();
	}

	private void Update() {
		playerMaterial.color = Color.Lerp(color, Color.white, hitCoolDown);
		hitCoolDown -= Time.deltaTime;
		hitCoolDown = Mathf.Max(0f, hitCoolDown);
	}

	void OnTriggerEnter (Collider collider) {
		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		if (collider.tag == "KillZone") {
			hp = 0;
			UpdateHp();
		}
	}

	void OnTriggerStay(Collider collider) {
		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		if (collider.tag == "HurtZone") {
			rigidbody.AddForce(new Vector3(0f, -hurtZoneForce, 0f), ForceMode.Impulse);
			Hit(hurtZoneDamage, Vector3.zero);
		}
	}

	public void Heal(int hp) {
		this.hp += hp;
        this.hp = Math.Min(this.hp, maxHP);
		UpdateHp();
	}

	public void Hit(int damage, Vector3 projectileVelocity) {
		SplashBlood(damage, projectileVelocity);
		hitCoolDown = 0.5f;

		hp -= damage;
		UpdateHp();
	}

	private void SplashBlood(int damage, Vector3 projectileVelocity) {
		GameObject bloodInstance = Instantiate(BloodStainParticles, transform.position, Quaternion.LookRotation(projectileVelocity, Vector3.up)) as GameObject;
		bloodInstance.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0f, (short)damage)});
		if (projectileVelocity != Vector3.zero) {
			bloodInstance.GetComponent<ParticleSystem>().startSpeed = projectileVelocity.magnitude * 0.9f;
		}
	}

	private void UpdateHp() {
		hpText.setHp(Math.Max(hp, 0));
		if (hp <= 0) {
			Kill();
			game.GameOver(2f);
		}
	}

	private void Kill() {
		SplashBlood(100, Vector3.zero);
        Destroy(gameObject);
	} 
}
