using UnityEngine;
using System.Collections;

public class Bomb : Interactable {

	public BoxCollider blocker; //used to keep player from entering the tile where the bomb is placed
	public SphereCollider surface; //udet to bounce block against walls when the bomb has been thrown (is moving)
	public BoxCollider exitTrigger; //used to change the the plantedBlockCollider.layer as the player is leaving the bomb
	public GameObject bombExplosion;



	public AudioClip fuseClip;

	[Range(0.0f, 10.0f) ]
	public float fuseTime = 3f;

	[Range(0.0f, 0.5f) ]
	public float propagationDelay = 0.1f;

	private bool explode = false;

	private LevelManager levelManager;

	private bool triggered = false;

	private int early = 0;

	public bool flying = false;

	void Awake() {
		isPortable = true;
		isWalkable = false;
	}

	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		if (levelManager == null)
			throw new UnityException ("Couldn't find LevelManager.");

		exitTrigger.enabled = true;
		blocker.enabled = false;
		surface.enabled = false;
		early = 0;
		triggered = false;
		GetComponent<Rigidbody>().isKinematic = true; //object will not move
		flying = false;
		this.gameObject.layer = LayerMask.NameToLayer ("Default");

		StartCoroutine(BombDetonation());
		//Explode ();
	}

	void Update () {
		if (early++ > 5 && ! triggered) {
			blocker.enabled = true;
			triggered = true;
		}
	}

	IEnumerator BombDetonation()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = fuseClip; //Range is tested and ok!!
		audioSource.Play ();

		yield return new WaitForSeconds(fuseTime);

		Explode ();
	}

	//YEY!! =D
	IEnumerator waitAndExplode()
	{
		yield return new WaitForSeconds(propagationDelay);
		Instantiate (bombExplosion, transform.position, Quaternion.identity);
		Destroy (this.gameObject);

		Vector3Int tilePosition = levelManager.toTilePosition (this.transform.position);
		levelManager.ExplodeAtTile ( tilePosition );
		
	}
	


	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "player") {
			triggered = true;
		}
	}

	//void OnTriggerStay (Collider collider)  {
	//	if (collider.tag == "player") {
	//		print ("Stay: " + count++);
	//	}
	//}

	void OnTriggerExit (Collider collider)  {
		if (collider.tag == "player") {
			blocker.enabled = true;
		}
	}

	override public void Explode() {
		if (!explode) { //to prevent infinite recursion 
			explode = true;
			StartCoroutine(waitAndExplode());
		}
	}

	override public void Burn() {

	}

	void FixedUpdate() {
		float bombPlantVelocity = 0.5f; // 1
		if(	flying && GetComponent<Rigidbody>().velocity.magnitude < bombPlantVelocity ) {
			this.OnPlant();
		}
	}

	override public void OnPlant() {
		exitTrigger.enabled = true;
		blocker.enabled = false;
		surface.enabled = false;
		early = 0;
		triggered = false;
		GetComponent<Rigidbody>().isKinematic = true; //object will not move
		flying = false;
		this.gameObject.layer = LayerMask.NameToLayer ("Default");

		Vector3Int plantTile = levelManager.toTilePosition( transform.position );
		transform.position = levelManager.getTileCenter (plantTile) + new Vector3(0f, 0.3f, 0f);
		levelManager.setObjAtTile(this.gameObject, plantTile );
	}

	override public void OnPickup() {
		print ("I am getting picked up " + this );
		exitTrigger.enabled = false;
		blocker.enabled = false;
		surface.enabled = false;
		GetComponent<Rigidbody>().isKinematic = true; //object will not move
		flying = false;
		this.gameObject.layer = LayerMask.NameToLayer ("Default");
	}

	override public void OnThrow( Vector3 velocity) {
		print ("I am getting thrown " + this  + " with v = " + velocity);

		exitTrigger.enabled = false;
		blocker.enabled = false;
		surface.enabled = true;
		GetComponent<Rigidbody>().isKinematic = false;
		flying = true;
		this.gameObject.layer = LayerMask.NameToLayer ("bombFlying");

		GetComponent<Rigidbody>().velocity = velocity;
	}



}
