using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public GameObject explosion;
    public Arm arm;

    private PlayerEnergy energyPlayerFire;
	private PlayerEnergy energyPlayerIce;

	public int damage = 30;
    public float fuseTime = 3f;
    private bool wasArmed = false;
    private float fuseTimeLeft;
	private bool fuseBurning = false;


	void Awake() {

	}

	void Start() {
        fuseTimeLeft = fuseTime;
    }

    private void Update() {
		if (fuseBurning) {
			fuseTimeLeft -= Time.deltaTime;
			if (fuseTimeLeft < 0f) {
				Explode();
			}
		}
    }

	void OnTriggerEnter (Collider collider) {

        if (collider.tag == "BombSafeZone") {
           	Debug.Log("disarming bomb");
           	Destroy(gameObject);
		} else if (collider.tag == "ArmZone") {
			fuseBurning = true;
		}
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "ArmZone") {
            Explode();
        }
    }

	private void Explode() {
		GameObject playerFire = GameObject.Find("Player Fire");
		if (playerFire != null) {
			energyPlayerFire = playerFire.GetComponent<PlayerEnergy>();
			energyPlayerFire.Hit(damage, (energyPlayerFire.gameObject.transform.position - transform.position).normalized * 40f);
		}

		GameObject playerIce = GameObject.Find("Player Ice");
		if (playerIce != null) {
			energyPlayerIce = playerIce.GetComponent<PlayerEnergy>();
			energyPlayerIce.Hit(damage, (energyPlayerIce.gameObject.transform.position - transform.position).normalized * 40f);
		}

		GameObject explosionInstance = Instantiate (explosion, transform.position, Quaternion.identity ) as GameObject;
		Destroy(gameObject);
	}
}
