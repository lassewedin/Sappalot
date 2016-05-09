using UnityEngine;
using System.Collections;

public class SetAudioListenerPosition : MonoBehaviour {

    public float distanceZ = 10f;
    public Transform audioListenerTransform;

	void Update () {
        audioListenerTransform.position = new Vector3(transform.position.x, transform.position.y, -distanceZ);
    }
}
