using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

	public CursorTracker cursorTracker;
	public LevelManager levelManager;
	//public GameObject tileMarker;

	public GameObject bomb;

	[SerializeField]
	[Range(0.0f, 10.0f) ]
	private float moveSpeed = 1.0f;

	[SerializeField]
	[Range(0.0f, 10.0f) ]
	private float moveSpeedPickup = 1.0f;

	[SerializeField]
	[Range(0.0f, 2f) ]
	private float pickupTime = 1f;

	[SerializeField]
	[Range(0.0f, 10.0f) ]
	private float throwForce = 1.0f;


	[SerializeField]
	private bool shootWhileCarying = false;

	public Weapon[] weapons;

	public GameObject tileMarker;

	private bool readyToGrab = false;
	private GameObject pickupObject = null;


	private bool canPickup = false;
	private float timeTillPickup;
	private Vector3Int pickupAim;



	void Update ()
	{
		// Rotation
		UpdateHeading ();


		if (Input.GetButton ("Fire1")) { //LMB

			if(pickupObject == null || shootWhileCarying) {
				foreach(Weapon weapon in weapons ) {
					weapon.Attack();
				}
			}
		}

		if (Input.GetButtonDown ("Fire2")) { //RMB
			if (!canPickup) {
				readyToGrab = false;
				//plant bomb at tile
				Vector3Int floorPos = levelManager.toTilePosition (this.transform.position);
				if (levelManager.getTile (floorPos) == null) {
					Vector3 tileCenter = levelManager.getTileCenter (floorPos);
					levelManager.instantiateObjAtTile (LevelManager.TileType.bomb, floorPos);
				}
			}
		}

		if (Input.GetButton ("Fire2")) { //RMB

			if (pickupObject == null ) {
				if( readyToGrab ) {
					if (canPickup) {

						timeTillPickup -= Time.deltaTime;
						if (timeTillPickup <= 0f) {
							levelManager.instantiateObjAtTile (LevelManager.TileType.empty, pickupAim, out pickupObject);
							if( pickupObject!= null ) {
								readyToGrab = false;
								print ("pickedUp " + pickupObject.name);
								pickupObject.GetComponent<Interactable>().OnPickup();
								pickupObject.transform.SetParent(this.transform);
								pickupObject.transform.localPosition = new Vector3(0f,1f, 0f);
							}
							else {
								throw new UnityException("Picked uo object 'null', should not be possible");
							}
						}

					} else {
						print ("(Can not pickup)");
					}
				}
				else {
					print ("not ready to grab");
				}
			} else {
				print ("Carying object");
			}
		}

		if (Input.GetButtonUp ("Fire2")) { //RMB
			readyToGrab = true;


			if( pickupObject != null ) {
				print ("Throwing object!!");

				pickupObject.transform.localPosition = new Vector3(0f, 0f, 0f); //throw from chest
				pickupObject.transform.parent = null; //free
				float startSpeed = (cursorTracker.worldLocation - this.gameObject.transform.position).magnitude * throwForce;
				pickupObject.GetComponent<Interactable>().OnThrow( this.transform.forward * startSpeed );



				pickupObject = null;
			}
			else {
				print("Ready to pickup new object again");
			}

		}
	}

	private void updatePickupTile() {
		canPickup = false;

		float xInput = Input.GetAxisRaw ("Horizontal");
		float zInput = Input.GetAxisRaw ("Vertical");

		Vector3Int playerPos = levelManager.toTilePosition( this.transform.position );
		Vector3Int pickupTestPos = playerPos + new Vector3Int ((int)xInput, 0, (int)zInput);

		if( levelManager.getTile(pickupTestPos) != null && Mathf.Abs(xInput) + Mathf.Abs(zInput) < 1.5f ) {
			Interactable interactable = levelManager.getTile(pickupTestPos).GetComponent<Interactable>();
			if( interactable.IsPortable ) {
				pickupAim = pickupTestPos;
				canPickup = true;
			}
		}


		if (canPickup) {
			tileMarker.GetComponent<MeshRenderer> ().enabled = true;
			tileMarker.transform.position = levelManager.getTileCenter(pickupAim) + new Vector3(0f, 0.5f, 0f);
		} else {
			print ("resetting pickup time");
			timeTillPickup = pickupTime;
			tileMarker.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	private void setWeaponActive(int weaponIndex, bool active ) {
		weapons [weaponIndex].gameObject.SetActive (active);
	}

	private void UpdateHeading ()
	{
		Vector3 heading = (cursorTracker.worldLocation - this.gameObject.transform.position).normalized;
		float angle = FindAngle (heading);

		transform.localRotation = Quaternion.Euler (0f, -angle+90f,  0f);
	}
	
	private float FindAngle (Vector3 direction)
	{
		float angle = Mathf.Rad2Deg * Mathf.Atan (Mathf.Abs (direction.z) / Mathf.Abs (direction.x));
		
		if (direction.x > 0f) {
			if (direction.z < 0f)
				return 360f - angle;
			return angle;
		}
		if (direction.z > 0f) 
			return 180f - angle;
		return 180f + angle;
		
	}


	void FixedUpdate () {
		updatePickupTile ();
		updateVelocity ();

	}

	/**
	 * Lots of effort was made to polish this bug bomber control.
	 * Keep it for later!
	 * 
	 */
	private void updateVelocity() {
		//Movement
		float speed = pickupObject == null ? moveSpeed : moveSpeedPickup;


		float xInput = Input.GetAxisRaw ("Horizontal");
		float zInput = Input.GetAxisRaw ("Vertical");
		
		GetComponent<Rigidbody>().velocity = Vector3.Normalize(new Vector3 (xInput, 0f, zInput));
		GetComponent<Rigidbody>().velocity *= speed; 

		float slideDistanceFromBlock = 0.05f;
		float noSlideDistanceFromBlockCenter = 0.05f;
		
		//tile space
		Vector3 tileSpace = levelManager.toTileSpacePosition (this.gameObject.transform.position);
		
		//tiles
		Vector3Int tilePosition = levelManager.toTilePosition (this.gameObject.transform.position);

		//Dont add velocity if object aheat is big and can be picked up (read is a bomb)
		Vector3Int tileAhead = tilePosition + new Vector3Int ((int)xInput, 0, (int)zInput);
		if ( ! ( canPickup && pickupAim.x == tileAhead.x && pickupAim.z == tileAhead.z) ) {


			//Add velocity to help player navigate obstacles
			Vector3Int t = tilePosition + new Vector3Int (-1, 0, 1);
			bool nwFloor = levelManager.getTile (tilePosition + new Vector3Int (-1, 0, 1)	) == null; 
			bool nnFloor = levelManager.getTile (tilePosition + new Vector3Int (0, 0, 1)	) == null;
			bool neFloor = levelManager.getTile (tilePosition + new Vector3Int (1, 0, 1)	) == null;
			
			bool wwFloor = levelManager.getTile (tilePosition + new Vector3Int (-1, 0, 0)	) == null;
			bool ccFloor = levelManager.getTile (tilePosition + new Vector3Int (0, 0, 0)	) == null;
			bool eeFloor = levelManager.getTile (tilePosition + new Vector3Int (1, 0, 0)	) == null;
			
			bool swFloor = levelManager.getTile (tilePosition + new Vector3Int (-1, 0, -1)	) == null;
			bool ssFloor = levelManager.getTile (tilePosition + new Vector3Int (0, 0, -1)	) == null;
			bool seFloor = levelManager.getTile (tilePosition + new Vector3Int (1, 0, -1)	) == null;
			
			if (zInput > 0.5f){ // pressing north ***
				if( tileSpace.z > 0.5 - slideDistanceFromBlock ) { // north
					
					// FB
					// FO
					if(xInput < 0.5f && !nnFloor && nwFloor && wwFloor && tileSpace.x < 0.5f - noSlideDistanceFromBlockCenter ) { //blocked n, free way to w
						GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0f, GetComponent<Rigidbody>().velocity.z);
					}
					
					// BF
					// OF
					if( xInput > -0.5f && !nnFloor && neFloor && eeFloor && tileSpace.x > 0.5f + noSlideDistanceFromBlockCenter ) { //blocked n, free way to e
						GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, GetComponent<Rigidbody>().velocity.z);
					}
					
					//BF   FB
					// O   O 
					if( nnFloor && ( ( !nwFloor && tileSpace.x < 0.5f && xInput > -0.5f) || ( !neFloor && tileSpace.x > 0.5f && xInput < 0.5f) ) ) { //blocked ne, free way n OR 
						GetComponent<Rigidbody>().velocity = new Vector3(speed * (0.5f - tileSpace.x) * 4f, 0f, GetComponent<Rigidbody>().velocity.z);
					}
				}
			}
			
			if (zInput < -0.5f){ // pressing south
				if( tileSpace.z < 0.5 + slideDistanceFromBlock ) { // north
					
					if( xInput < 0.5f && !ssFloor && swFloor && wwFloor && tileSpace.x < 0.5f - noSlideDistanceFromBlockCenter ) { //blocked n, free way to w
						GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0f, GetComponent<Rigidbody>().velocity.z);
					}
					
					if( xInput > -0.5f && !ssFloor && seFloor && eeFloor && tileSpace.x > 0.5f + noSlideDistanceFromBlockCenter ) { //blocked n, free way to e
						GetComponent<Rigidbody>().velocity = new Vector3(speed, 0f, GetComponent<Rigidbody>().velocity.z);
					}
					
					if( ssFloor && ( ( !swFloor && tileSpace.x < 0.5f && xInput > -0.5f) || ( !seFloor && tileSpace.x > 0.5f && xInput < 0.5f) ) ) { //blocked ne, free way n OR 
						GetComponent<Rigidbody>().velocity = new Vector3(speed * (0.5f - tileSpace.x) * 4f, 0f, GetComponent<Rigidbody>().velocity.z);
					}
				}
			}
			
			if (xInput > 0.5f){ // pressing east
				if( tileSpace.x > 0.5f - slideDistanceFromBlock ) {
					
					if(zInput < 0.5f && !eeFloor && seFloor && ssFloor && tileSpace.z < 0.5f - noSlideDistanceFromBlockCenter ) { //blocked n, free way to w
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, -speed);
					}
					
					if( zInput > -0.5f && !eeFloor && neFloor && nnFloor && tileSpace.z > 0.5f + noSlideDistanceFromBlockCenter ) { //blocked n, free way to e
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, speed);
					}
					
					if( eeFloor && ( ( !seFloor && tileSpace.z < 0.5f && zInput > -0.5f) || ( !neFloor && tileSpace.z > 0.5f && zInput < 0.5f) ) ) { //blocked ne, free way n OR 
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, speed * (0.5f - tileSpace.z) * 4f );
					}
				}
			}
			
			
			if (xInput < -0.5f){ // pressing west
				if( tileSpace.x < 0.5f + slideDistanceFromBlock ) {
					
					if( zInput < 0.5f && !wwFloor && swFloor && ssFloor && tileSpace.z < 0.5f - noSlideDistanceFromBlockCenter ) { //blocked n, free way to w
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, -speed);
					}
					
					if( zInput > -0.5f && !wwFloor && nwFloor && nnFloor && tileSpace.z > 0.5f + noSlideDistanceFromBlockCenter ) { //blocked n, free way to e
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, speed);
					}
					
					if( wwFloor && ( ( !swFloor && tileSpace.z < 0.5f && zInput > -0.5f) || ( !nwFloor && tileSpace.z > 0.5f && zInput < 0.5f) ) ) { //blocked ne, free way n OR 
						GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, speed * (0.5f - tileSpace.z) * 4f );
					}
				}
			}
		}
		GetComponent<Rigidbody>().velocity = new Vector3 (Mathf.Clamp(GetComponent<Rigidbody>().velocity.x, -speed, speed ), 0f, Mathf.Clamp(GetComponent<Rigidbody>().velocity.z, -speed, speed ));
	
	}


	//private void correctNorth( float intersect ) {
	//	rigidbody.transform.position = new Vector3 (rigidbody.transform.position.x, rigidbody.transform.position.y, rigidbody.transform.position.z - intersect * levelManager.TileSide);
	//}


	private void setVelocit(Vector3 velocity) {
		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + velocity;
	}

	private void playerSide( out bool west, out bool south ) {
		Vector3 position = levelManager.toTileSpacePosition (this.gameObject.transform.position);
		west = position.x < 0.5f;
		south = position.z < 0.5f;
	} 


	private void playerIntersection( out float west, out float east, out float south, out float north ) {
		Vector3 position = levelManager.toTileSpacePosition (this.gameObject.transform.position);
		float radius = 0.5f / levelManager.TileSide;
		
		west = 0;
		east = 0;
		south = 0;
		north = 0;
		
		if (position.x < radius)
			west = radius - position.x;
		if (position.x > 1 - radius)
			east = position.x - 1 + radius;
		if (position.z < radius)
			south = radius - position.z;
		if (position.z > 1 - radius)
			north = position.z - 1 + radius;
	} 
	

}
