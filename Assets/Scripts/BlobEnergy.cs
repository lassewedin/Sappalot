using UnityEngine;
using System.Collections;

public class BlobEnergy : MonoBehaviour {

    public GameObject InkStainParticles;


    public int startHP = 2;
    public int hp { get; private set; }

    private new Rigidbody rigidbody;

    private void Start() {
        hp = startHP;
    }

    void OnTriggerEnter(Collider collider) {
        if (rigidbody == null) {
            rigidbody = transform.GetComponent<Rigidbody>();
        }

        if (collider.tag == "KillZone") {

        }
    }

    void OnTriggerStay(Collider collider) {
        if (rigidbody == null) {
            rigidbody = transform.GetComponent<Rigidbody>();
        }

        if (collider.tag == "HurtZone") {

        }
    }

    public void Hit(int damage, Vector3 projectileVelocity) {
        SplashBlood(damage, projectileVelocity);
        hp -= damage;
        if (hp <= 0) {
            Kill();
        }
    }

    private void SplashBlood(int damage, Vector3 projectileVelocity) {
        GameObject bloodInstance = Instantiate(InkStainParticles, transform.position, Quaternion.LookRotation(projectileVelocity, Vector3.up)) as GameObject;
        bloodInstance.GetComponent<ParticleSystem>().emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, (short)damage) });
        if (projectileVelocity != Vector3.zero) {
            bloodInstance.GetComponent<ParticleSystem>().startSpeed = projectileVelocity.magnitude * 0.5f;
        }
    }

    private void Kill() {
        SplashBlood(5, Vector3.zero);
        Destroy(gameObject);
    }
}
