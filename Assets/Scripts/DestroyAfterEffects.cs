using UnityEngine;
using System.Collections;

public class DestroyAfterEffects : MonoBehaviour {

	void Update () {
		ParticleSystem particle = gameObject.GetComponent<ParticleSystem> ();
		AudioSource audio = gameObject.GetComponent<AudioSource> ();

		//OK!
		//if (audio != null &&  !audio.isPlaying) {
		//	print ("destroy");
		//	Destroy (gameObject);
		//}

		//if (particle != null &&  !particle.IsAlive()) {
		//	print ("destroy");
			//Destroy (gameObject);
		//}

		if (
			(particle != null && audio == null && !particle.IsAlive ()) || 
			(audio != null && !audio.isPlaying) ||
			(particle != null && !particle.IsAlive () && audio != null && !audio.isPlaying)) {

			Destroy (gameObject);
		}
	}

}
