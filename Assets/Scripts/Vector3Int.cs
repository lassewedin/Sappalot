using System.Collections;
using System;


public struct Vector3Int {

	public int x;
	public int y;
	public int z;

	public Vector3Int(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static Vector3Int operator +(Vector3Int first, Vector3Int second) {
		return new Vector3Int (first.x + second.x, first.y + second.y, first.z + second.z);
	}

	public static Vector3Int operator -(Vector3Int first, Vector3Int second) {
		return new Vector3Int (first.x - second.x, first.y - second.y, first.z - second.z);
	}

	public static bool operator ==(Vector3Int first, Vector3Int second) {
		return first.Equals(second);
	}

	public static bool operator !=(Vector3Int first, Vector3Int second) {
		return ! (first == second);
	}

	public override bool Equals(Object obj)
	{
		if (!(obj is Vector3Int)) return false;
		
		Vector3Int p = (Vector3Int) obj;
		return x == p.x && y == p.y && z == p.z;
	}
	
	public override int GetHashCode()
	{ 
		return ShiftAndWrap( ShiftAndWrap(x.GetHashCode(), 2) ^ y.GetHashCode(), 2) ^ z.GetHashCode();
	} 
	
	private int ShiftAndWrap(int value, int positions)
	{
		positions = positions & 0x1F;
		
		// Save the existing bit pattern, but interpret it as an unsigned integer. 
		uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
		// Preserve the bits to be discarded. 
		uint wrapped = number >> (32 - positions);
		// Shift and wrap the discarded bits. 
		return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
	}
}


