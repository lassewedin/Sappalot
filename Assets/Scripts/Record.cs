using UnityEngine;
using System;
using System.Collections.Generic;

public class Record  : IComparable<Record>{
	public Vector3Int tile;
	public float value;
	public int tailIndex = 0;



	// Default comparer for Part type. 
	public int CompareTo(Record comparePart)
	{
		// A null value means that this object is greater. 
		if (comparePart == null) {
			return 1;
		}
		else if (this.value > comparePart.value) {
			return 1;
		}
		else if( this.value < comparePart.value) {
			return -1;
		}
		return 0;
	}



	/*private float floor;
	private float ceiling;

	public float Floor {
		get {
			return this.floor;
		}
		set {
			this.floor = value;
		}
	}

	public float Ceiling {
		get {
			return this.floor + this.value;
		}
	}*/

}
