using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		ParticleSystem ps = gameObject.GetComponent<ParticleSystem> ();
		if (ps != null && !ps.IsAlive ())
			Destroy (gameObject);
	}
}
