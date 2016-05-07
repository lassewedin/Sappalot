using UnityEngine;
using System.Collections;

public class OffLevelOnTrasher : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Trasher") {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
