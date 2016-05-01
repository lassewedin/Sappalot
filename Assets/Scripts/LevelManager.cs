using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

	//public GameObject empty;
	public GameObject floor;
	public GameObject bomb;
	public GameObject blockNormal;
	public GameObject blockTough;

	public GameObject hunterBot;
	//[HideInInspector]
	public const int boardWidth = 19; // in tiles
	//[HideInInspector]
	public const int boardHeight = 19; // in tiles
	private const float tileSide = 1.0f; // in units

	public float bugPeriod = 200f;
	private float bugCooldown = 0f;

	private Vector3 southWestCorner = new Vector3 (0f, 0f, 0f);
	private float westBorder; // xMin
	private float eastBorder; // xMax
	private float southBorder; // zMin
	private float northBorder; // zMax


	private Transform boardHolder;

	private GameObject[,] board = new GameObject[boardWidth, boardHeight]; //GameObjects here should have a script component, extending Interactable

	public enum TileType {
		empty,
		bomb,
		blockRed,
		blockGreen,
		blockBlue,
		blockYellow,
		blockCommon,
		blockTough,
	}

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		print ("Generating board: " + this.name);
		westBorder = southWestCorner.x;
		eastBorder = southWestCorner.x + tileSide * boardWidth;
		southBorder = southWestCorner.z;
		northBorder = southWestCorner.z + tileSide * boardHeight;

		generateBoard ();

	}

	public int bugs = 0;

	void Update () {
		bugCooldown -= Time.deltaTime;
		if ( false && bugCooldown < 0f) {
			bugs++;
			bugCooldown = bugPeriod;

			instantiateObjAtTile (LevelManager.TileType.empty, new Vector3Int (7, 0, 7));
			GameObject bug = Instantiate (hunterBot, new Vector3 (7.5f, hunterBot.transform.position.y, 7.5f), hunterBot.transform.rotation) as GameObject;
		}
	}

	public void ExplodeAtTile( Vector3Int position ) {
		ExplodeAtTile (position, 1);
	}
	public void ExplodeAtTile( Vector3Int position, int size ) {
		for( int zz = -size; zz <= size; zz++ ) {
			for( int xx = -size; xx <= size; xx++ ) {
				if(!(xx==0 && zz==0) && this.getTile( new Vector3Int( position.x+xx, 0, position.z+zz) ) != null ) {
					this.getTile ( position + new Vector3Int(xx, 0, zz) ).GetComponent<Interactable>().Explode();
				}
			}
		}
	}

	public GameObject instantiateObjAtTile(TileType type, Vector3Int position ) {
		GameObject toDestroy = null;
		GameObject instance = instantiateObjAtTile( type, position, out toDestroy);
		Destroy ( toDestroy );
		return instance;
	}
	//Instantiate a gameObject in tile(x,z).
	//Removes (not destroy) anything allready on tile(x,z)
	public GameObject instantiateObjAtTile(TileType type, Vector3Int position, out GameObject removedObject) {

		switch (type) {
			case TileType.empty:
				return instantiateObjAtTile(null, position, out removedObject);

			case TileType.bomb:
				return instantiateObjAtTile(bomb, position, out removedObject);

			case TileType.blockRed:
				return instantiateObjAtTile(blockNormal, position, out removedObject);

			case TileType.blockTough:
				return instantiateObjAtTile(blockTough, position, out removedObject);

			default:
				return instantiateObjAtTile(null, position, out removedObject);
		}

	}


	private GameObject instantiateObjAtTile(GameObject prefab, Vector3Int position) {
		GameObject toDestroy = null;
		GameObject instance = instantiateObjAtTile( prefab, position, out toDestroy);
		Destroy ( toDestroy );
		return instance;
	}
	private GameObject instantiateObjAtTile(GameObject prefab, Vector3Int position, out GameObject removedObject) {
		if(prefab != null) {
			Vector3 instancePosition = southWestCorner + new Vector3 ((position.x + 0.5f) * tileSide, prefab.transform.position.y, (position.z + 0.5f) * tileSide);
			GameObject instance = Instantiate (prefab, instancePosition, prefab.transform.rotation) as GameObject;
			setObjAtTile (instance, position, out removedObject);
			return instance;
		}
		setObjAtTile (null, position, out removedObject);
		return null;

	}


	public void setObjAtTile(GameObject gameObject, Vector3Int position) {
		GameObject toDestroy = null;
		setObjAtTile (gameObject, position, out toDestroy );
		Destroy ( toDestroy );
	}
	//Place gameObject in tile(x,z).
	//Removes (and leaves) ant tile allready on tile(x,z)
	public void setObjAtTile(GameObject gameObject, Vector3Int position, out GameObject removedObject ) {
		removedObject = null;


		//remove potential present object
		if (board [position.x, position.z] != null) {

			GameObject removed = board [position.x, position.z];
			removed.transform.SetParent (null);
			removedObject = removed;
		}

		//set new object
		if( gameObject != null )
			gameObject.transform.SetParent (boardHolder);
		board [position.x, position.z] = gameObject;

	}


	//Return gameObject in tile(x,z)
	public GameObject getTile( Vector3Int position ) {
		return board[position.x, position.z];
	}

	private void generateBoard() {
		boardHolder = new GameObject ("Board").transform;
		
		print ("name: " + boardHolder.name);
		boardHolder.SetParent (this.GetComponent<Transform>());
		
		for(int z = 0; z < boardHeight; z++ ) {
			for(int x = 0; x < boardWidth; x++ ) {
				//create a floor tile, will not be stored in array
				Vector3 position = southWestCorner + new Vector3 ((x + 0.5f) * tileSide, floor.transform.position.y, (z + 0.5f) * tileSide);
				GameObject instance = Instantiate (floor, position, floor.transform.rotation) as GameObject;
				instance.transform.SetParent (boardHolder);
				//instantiateObjAtTile(TileType.empty, x, z );

				if( x == 0 || x == boardWidth-1 || z == 0 || z == boardHeight-1 || ( x == 2 && z % 2 == 0 ) || ( x == boardWidth-3 && z % 2 == 0 ) || ( z == 2 && x % 2 == 0 ) || ( z == boardHeight-3 && x % 2 == 0 ))
					instantiateObjAtTile(TileType.blockTough, new Vector3Int(x, 0, z) );
				else {
					if( Random.value > 0.75f )
						instantiateObjAtTile(TileType.blockRed, new Vector3Int(x, 0, z) );
					else
						instantiateObjAtTile(TileType.empty, new Vector3Int(x, 0, z) );
				}
			}
		}
		//player
		instantiateObjAtTile(LevelManager.TileType.empty, new Vector3Int(5, 0, 5));

		//bug
		//instantiateObjAtTile(LevelManager.TileType.bomb, new Vector3Int(7, 0, 7));
		instantiateObjAtTile(LevelManager.TileType.empty, new Vector3Int(7, 0, 7));
		GameObject bug = Instantiate (hunterBot, new Vector3(7.5f, hunterBot.transform.position.y, 7.5f), hunterBot.transform.rotation) as GameObject;

	}

	public bool isInsideBoard( Vector3 worldPosition ) {
		if (worldPosition.x < westBorder || worldPosition.x > eastBorder || worldPosition.z < southBorder || worldPosition.z > northBorder) {
			return false;
		}
		return true;
	}

	public Vector3Int toTilePosition(Vector3 worldPosition ) {
		if (isInsideBoard(worldPosition)) {
			int x = (int) Mathf.Floor(worldPosition.x / tileSide);
			int z = (int) Mathf.Floor(worldPosition.z / tileSide);
			return new Vector3Int(x, 0, z);
		}
		print ("Outside!!!!: world" + worldPosition); 
		return new Vector3Int(-1, 0, -1);
	}

	//returns west, east, south, north
	public Vector4 getTileBorders( Vector3Int position ) {
		return new Vector4 (
			westBorder + position.x * tileSide,
			westBorder + (position.x + 1) * tileSide,
			southBorder + position.z * tileSide,
			southBorder + (position.z + 1) * tileSide );
	}

	public Vector3 getTileCenter( Vector3Int position ) {
		return new Vector3 (southWestCorner.x + (position.x + 0.5f) * tileSide, southWestCorner.y, southWestCorner.z + (position.z + 0.5f) * tileSide);
	}

	public Vector3 toTileSpacePosition( Vector3 worldPosition ) {
		Vector3Int tilePosition = toTilePosition (worldPosition);
		Vector4 tileBorders = getTileBorders (tilePosition);

		float xTile = (worldPosition.x - tileBorders.x) / tileSide;
		float zTile = (worldPosition.z - tileBorders.z) / tileSide;
	
		return new Vector3 (xTile, 0f, zTile);
	}

	public float TileSide {
		get {
			return tileSide;
		}
	}


}

