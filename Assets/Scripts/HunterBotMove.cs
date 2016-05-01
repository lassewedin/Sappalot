using UnityEngine;
using System.Collections.Generic;

public class HunterBotMove : MonoBehaviour {

	public GameObject sphere;

	private LevelManager levelManager;
	private GameObject player;

	private float health = 1f;

	[Range(0.0f, 20.0f) ]
	public float speed = 1;

	[Range(0, 10.0f) ]
	public int tailLength = 5;

	[Range(0, 1f) ]
	public float gravity = 0.7f;

	public Vector3Int currentTile;
	public Vector3Int goalTile;
	public float timeToGoal = 0f;
	public float timeElapsed = 0;

	//private Vector3 velocity;

	private List<Vector3Int> tail = new List<Vector3Int>();
	private List<GameObject> tailGraphics = new List<GameObject>();

	// Use this for initialization
	void Start () {
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		if (levelManager == null)
			throw new UnityException ("Couldn't find LevelManager.");
		player = GameObject.Find("Player");
		if (player == null)
			throw new UnityException ("Couldn't find Player.");

		//adjust position
		updateCurrentTile ();
		centerOnTile (currentTile);
		findNewGoalTile2 ();
		headTowardsNewGoal ();
		updateTail ();
		UpdateHeading ();
		
	}

	private void updateCurrentTile() {
		currentTile = levelManager.toTilePosition (this.transform.position);
	}

	private void centerOnTile(Vector3Int tile) {
		this.transform.position = levelManager.getTileCenter (tile);
	}

	private bool isDiagonal (int x, int z) {
		return Mathf.Abs (x) + Mathf.Abs (z) == 2;
	}





	private void findNewGoalTile2(){

		Vector3Int playerTile = levelManager.toTilePosition (player.transform.position);

		RecordHolder options = new RecordHolder ();

		if (currentTile == playerTile) {
			goalTile = currentTile;
			return;
		}
		for (int z = -1; z <= 1; z++) {
			for (int x = -1; x <= 1; x++) {
				Vector3Int adjacentTile =  currentTile + new Vector3Int(x, 0, z);

				float baseValue = 0f;

				if(
					(adjacentTile == currentTile + new Vector3Int(-1, 0, 0) &&
			        levelManager.getTile(adjacentTile) == null ) //west

					|| (adjacentTile == currentTile + new Vector3Int(1, 0, 0) &&
			        levelManager.getTile(adjacentTile) == null ) //east
					
			        || (adjacentTile == currentTile + new Vector3Int(0, 0, -1) &&
			        levelManager.getTile(adjacentTile) == null ) //south
					
			        || (adjacentTile == currentTile + new Vector3Int(0, 0, 1) &&
			        levelManager.getTile(adjacentTile) == null ) //north

					|| (adjacentTile == currentTile + new Vector3Int(-1, 0, -1) &&
			        levelManager.getTile(adjacentTile ) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(1, 0, 0)) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(0, 0, 1)) == null) //south west

					|| (adjacentTile == currentTile + new Vector3Int(1, 0, -1) &&
			        levelManager.getTile(adjacentTile ) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(-1, 0, 0)) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(0, 0, 1)) == null)  //south east

					|| (adjacentTile == currentTile + new Vector3Int(-1, 0, 1) &&
			        levelManager.getTile(adjacentTile ) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(1, 0, 0)) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(0, 0, -1)) == null)  //north west

					|| (adjacentTile == currentTile + new Vector3Int(1, 0, 1) &&
			        levelManager.getTile(adjacentTile ) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(-1, 0, 0)) == null &&
			        levelManager.getTile(adjacentTile + new Vector3Int(0, 0, -1)) == null) ) { //north east

					//find out if adjacent tile is in tail
					int i = 0;
					bool inTail = false;
					foreach(Vector3Int tailSegment in tail ) { //first to last in List ==> newest (4) to oldest(0)
						i++;
						if( adjacentTile == tailSegment ) {
							inTail = true;
							break;
						}
					}
					if(! inTail ) {
						i = 0;
					}


					options.addRecord(adjacentTile, baseValue + LevelManager.boardWidth + LevelManager.boardHeight - manhattanD(adjacentTile, playerTile), i );
				}
			}
		}
		goalTile = options.pickOldestTailRecord(gravity).tile;
		//goalTile = options.pickRandomRecord().tile;
		//goalTile = options.pickHighestRecord().tile;
	}


	private int manhattanD(Vector3Int v0, Vector3Int v1) {
		return Mathf.Abs (v0.x - v1.x) + Mathf.Abs (v0.z - v1.z);
	}



	private void headTowardsNewGoal() {
		if (currentTile != goalTile) {
			Vector3 currentTileCenter = levelManager.getTileCenter (currentTile);
			Vector3 goalTileCenter = levelManager.getTileCenter (goalTile);



			float distance = (goalTileCenter - currentTileCenter).magnitude;

			timeToGoal = distance / speed;
			GetComponent<Rigidbody>().velocity = (goalTileCenter - currentTileCenter).normalized * speed;
		} else {
			GetComponent<Rigidbody>().velocity = new Vector3();
			timeToGoal = 1f;
		}
	}

	private void updateTail() {
		tail.Add (currentTile); // add last
		GameObject debugMarker = Instantiate (sphere, levelManager.getTileCenter(currentTile), Quaternion.identity ) as GameObject;
		tailGraphics.Add (debugMarker);
		while (tail.Count > tailLength) {
			tail.RemoveAt (0); // remove first (added first, that is oldest)
			GameObject toBeRemoved = tailGraphics[0];
			tailGraphics.RemoveAt (0);
			Destroy (toBeRemoved);
		}
		//print ("tail length: " + tail.Count);
	}

	// Update is called once per frame
	void Update () {


	}

	void FixedUpdate() {
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= timeToGoal) {
			updateCurrentTile(); //should be goal tile, but will work with any tile
			centerOnTile (currentTile); //should be close
			findNewGoalTile2 ();
			headTowardsNewGoal ();
			updateTail ();
			UpdateHeading ();
			timeElapsed = 0f;
		}


	}

	private float angle = 0f;

	private void UpdateHeading ()
	{

		if( GetComponent<Rigidbody>().velocity.magnitude > 0f) {
			angle = FindAngle (GetComponent<Rigidbody>().velocity);
		}
		transform.rotation = Quaternion.Euler (90f, 90f-angle,  0f);


	}
	
	private float FindAngle (Vector3 direction)
	{
		if (direction.x == 0f) {
			if (direction.z > 0f)
				return 90f;
			return 270f;
		}
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

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "bullet") {
			Destroy (this.gameObject);
			Destroy (collider.gameObject);
		}
	}
}
