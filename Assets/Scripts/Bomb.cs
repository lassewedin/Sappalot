using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public GameObject explosion;
    public Arm arm;

    //private PlayerEnergy energyPlayerFire;
	//private PlayerEnergy energyPlayerIce;

	public int damage = 30;
    public float fuseTime = 3f;
    private bool wasArmed = false;
    private float fuseTimeLeft;

	void Start() {
		//energyPlayerFire = GameObject.Find("Player Fire").GetComponent<PlayerEnergy>();
		//energyPlayerIce = GameObject.Find("Player Ice").GetComponent<PlayerEnergy>();

        fuseTimeLeft = fuseTime;
    }

    private void Update() {
        fuseTimeLeft -= Time.deltaTime;
        if (fuseTimeLeft < 0f) {
            Explode();
        }
    }

	void OnTriggerEnter (Collider collider) {

        if (collider.tag == "BombSafeZone") {
           	Debug.Log("disarming bomb");
           	Destroy(gameObject);
        }


            //if (arm) {
            //    wasArmed = true;
            // }

            //if (collider.tag == "DetonateZone" && arm) {
            //	//Explode();
            //} else if (collider.tag == "DisarmZone") {
            //	Debug.Log("disarming bomb");
            //	Destroy(gameObject);
            //}


        }

        void OnTriggerExit(Collider collider) {
        if (collider.tag == "ArmZone") {
            Explode();
        }
    }

	private void Explode() {
		//energyPlayerFire.Hit(damage, (energyPlayerFire.gameObject.transform.position - transform.position).normalized * 40f);
		//energyPlayerIce.Hit(damage, (energyPlayerIce.gameObject.transform.position - transform.position).normalized * 40f);

		GameObject explosionInstance = Instantiate (explosion, transform.position, Quaternion.identity ) as GameObject;
		Destroy(gameObject);
	}
}
