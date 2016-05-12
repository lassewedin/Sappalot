using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpText : MonoBehaviour {

	public void setHp(int hp) {
		GetComponent<Text>().text = "HP: " + hp;
	}

	public void Update() {
		
	}
}
