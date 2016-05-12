using UnityEngine;
using System.Collections;

public class Blob : MonoBehaviour {

    public int damage = 10;
    private Rigidbody rigidbody;

    private void OnTriggerStay(Collider collider) {

        if (collider.tag == "Player") {
            collider.GetComponent<PlayerEnergy>().Hit(damage, this.GetComponent<Rigidbody>().velocity);
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (rigidbody == null) {
            rigidbody = transform.GetComponent<Rigidbody>();
        }

        if (collider.tag == "Block") {
            rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 5f, 0f), ForceMode.Impulse);
        }
    }
}
