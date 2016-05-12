using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerEnergy : MonoBehaviour {

	public Game game;
	public MeshRenderer bodyMeshRenderer;
	public MeshRenderer headMeshRenderer;

	public GameObject BloodStainParticles;
	public Material playerMaterial;
	private Material playerMaterialOriginal;
	public Material playerMaterialHurt;

	public int startHP = 100;
    public int maxHP = 100;
	public int hurtZoneDamage = 1;
	public float hurtZoneForce = 10f;

	public int hp { get; private set;} 
	private HpText hpText;

	private new Rigidbody rigidbody;
	private float hitCoolDown = 0f;

	public enum Type {
		red,
		blue,

	}
	public Type type = Type.red;

	void Start () {
		hp = startHP;
		UpdateHp();
		game = GameObject.Find("Game").GetComponent<Game>();
	}

	private void Update() {
		if (hitCoolDown > 0f) {
			hitCoolDown -= Time.deltaTime;

			bodyMeshRenderer.material = playerMaterialHurt;
			headMeshRenderer.material = playerMaterialHurt;

		} else {
			bodyMeshRenderer.material = playerMaterial;
			headMeshRenderer.material = playerMaterial;
		}
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
		hitCoolDown = 0.1f;

		hp -= damage;
		UpdateHp();
	}

	private void SplashBlood(int damage, Vector3 projectileVelocity) {
		GameObject bloodInstance = Instantiate(BloodStainParticles, transform.position + new Vector3(0f, 0f, -2f), Quaternion.LookRotation(projectileVelocity, Vector3.up)) as GameObject;
		bloodInstance.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0f, (short)damage)});
		if (projectileVelocity != Vector3.zero) {
			bloodInstance.GetComponent<ParticleSystem>().startSpeed = projectileVelocity.magnitude * 0.9f;
		}
	}

	private void UpdateHp() {
		if (type == Type.red) {
			GameObject.Find("Text Player Fire").GetComponent<HpText>().setHp(Math.Max(hp, 0));
		} else {
			GameObject.Find("Text Player Ice").GetComponent<HpText>().setHp(Math.Max(hp, 0));
		}

		if (hp <= 0) {
			Kill();
			game.GameOver(2f);
		}
	}

	private void Kill() {
		SplashBlood(100, Vector3.zero);
		gameObject.SetActive(false);
	} 
}
