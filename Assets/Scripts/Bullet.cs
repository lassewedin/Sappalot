using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


	public GameObject ricochetParticle;

	public AudioClip[] audioClips;
	private bool inAir = true;

	public float temperature = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		if (!inAir && !gameObject.GetComponent<AudioSource> ().isPlaying) {
			Destroy (this.gameObject);	
		}
	}

	private void play() {
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.pitch = Random.Range (0.5f, 1.5f);
		audioSource.clip = audioClips [Random.Range (0, audioClips.Length)]; //Range is tested and ok!!
		audioSource.Play ();

	} 

	void OnTriggerEnter (Collider collider) 
	{
		if (collider.tag == "Block") {
			play ();
			//find exact collision
			float rayLength = 1.8f;
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(this.transform.position - this.transform.forward * 0.5f * rayLength,  this.transform.forward, out hitInfo, rayLength );
			//bool hit = Physics.Raycast(new Vector3(-1f, 0.5f, -1f),  new Vector3(1f, 0f, 0.5f), out hitInfo, 100f );
			Vector3 collisionPoint = this.transform.position; //default
			if(hit)
				collisionPoint = hitInfo.point;

			Vector3 box = collider.transform.position;

			Vector3 boxToImp = collisionPoint - box;
			Vector3 normal;

			if(Mathf.Abs ( boxToImp.x ) >  Mathf.Abs ( boxToImp.z ) ) {
				if( boxToImp.x < 0f ) 
					normal = -Vector3.right;
				else
					normal = Vector3.right;
			}
			else {
				if( boxToImp.z < 0f ) 
					normal = -Vector3.forward;
				else
					normal = Vector3.forward;
			}

            normal = Vector3.up;

			float ricochetDamp = 0.8f;

			GameObject ricochetInstance = Instantiate (ricochetParticle, collisionPoint, Quaternion.LookRotation(Vector3.Reflect (this.transform.forward, normal), Vector3.up) ) as GameObject;
			ricochetInstance.GetComponent<ParticleSystem>().startSpeed = this.GetComponent<Rigidbody>().velocity.magnitude * ricochetDamp;

			//keep game object because the audio source is still needed to play ricochet sound
			Destroy( gameObject.GetComponent<BoxCollider>() );
			Destroy( gameObject.GetComponent<Rigidbody>() );
			Destroy( gameObject.GetComponent<LineRenderer>() );
			Destroy( gameObject.GetComponent<Light>() );
			inAir = false;

            Block block = collider.GetComponent<Block>();
            if (block != null) {
				block.Hit(1, temperature);
            }
            
		}
	}
}
