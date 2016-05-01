using UnityEngine;
using System.Collections;

public class ChangeCursor : MonoBehaviour {

	public Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		CursorMode cursorMode = CursorMode.Auto;
		Vector2 hotSpot = new Vector2(cursorTexture.width * 0.5f, cursorTexture.height * 0.5f);
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	void OnMouseEnter() {

	}
}
