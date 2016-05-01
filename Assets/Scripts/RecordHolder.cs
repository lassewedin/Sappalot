using UnityEngine;
using System.Collections.Generic;

public class RecordHolder : MonoBehaviour {

	private List<Record> recordList = new List<Record> ();

	private float pointer = 0f;


	public void addRecord(Vector3Int tile, float value, int tailIndex ) {
		Record record = new Record ();
		record.tile = tile;
		record.value = Mathf.Max(0f, value);
		record.tailIndex = tailIndex;
		addRecord (record);
	}



	private void addRecord( Record record ) {
		//record.Floor = pointer;
		pointer += record.value;
		recordList.Add(record);

	}

	public Record pickNonTailRecord() {
		for (int t = 0; t < 100; t++) {
			int index = Random.Range (0, recordList.Count);
			if( recordList[index].tailIndex == 0 ) {
				return recordList[index];
			}
		}
		//return default
		return recordList[Random.Range (0, recordList.Count)];
	}

	public Record pickOldestTailRecord( float gravity ) {
		List<Record> nonTailRecord = new List<Record> ();

		int lowestFound = 666666666;
		Record lowestRecord = null; // closest to player
		foreach(Record r in recordList) {
			if( r.tailIndex < lowestFound ) {
				lowestFound = r.tailIndex;
				lowestRecord = r;
			}
			if( r.tailIndex == 0 ) {
				nonTailRecord.Add(r);
			}
		}
		//if there are records without any tail
		if (lowestFound == 0 && recordList.Count > 0 ) {
			nonTailRecord.Sort ();
			if( Random.Range (0, 1f) > 1f - gravity )
				lowestRecord = nonTailRecord[nonTailRecord.Count - 1];
			else
				lowestRecord = nonTailRecord[Random.Range (0, nonTailRecord.Count)];
		}
		return lowestRecord;
	}

	//based on value
	public Record pickRandomRecord() {
		float diceValue = Random.Range (0, pointer);
		float testValue = 0f;
		foreach (Record record in recordList) {
			testValue += record.value; 
			if( diceValue < testValue )	
				return record;
		}

		return null;
	}

	public Record pickHighestRecord() {
		recordList.Sort();
		//print ("records: " + recordList.Count );
		return recordList[recordList.Count - 1]; 
	}
}


