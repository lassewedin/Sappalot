using UnityEngine;
using System.Collections;
using System;


//any object on the board
public abstract class Interactable : MonoBehaviour {

	protected bool isPortable = false;
	protected bool isWalkable = false; //can walk into

	public DynamicStateEnum dynamicState = DynamicStateEnum.noStateYet;
	public enum DynamicStateEnum {
		noStateYet,
		planted,
		carried,
		flying
	}


	abstract public void Explode ();
		
	abstract public void Burn();

	abstract public void OnPickup();

	abstract public void OnThrow( Vector3 velocity);

	abstract public void OnPlant();

	public DynamicStateEnum State {
		get{
			return dynamicState;
		}
	}

	public bool IsPortable {
		get{
			return isPortable;
		}
	}

	public bool IsWalkable {
		get{
			return isWalkable;
		}
	}



	//public abstract bool canPlantAt (Vector3 position);
	//public abstract bool plantAt (Vector3 position);

	//public abstract bool canPickupAt(Vector3 position);
	//public abstract void pickUpAt(Vector3 position);




}

