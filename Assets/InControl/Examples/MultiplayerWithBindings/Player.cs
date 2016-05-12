using System;
using UnityEngine;
using InControl;


namespace MultiplayerWithBindingsExample
{
	// This is just a simple "player" script that rotates and colors a cube
	// based on input read from the actions field.
	//
	// See comments in PlayerManager.cs for more details.
	//
	public class Player : MonoBehaviour	{

		public bool invertedControls = false;
		public KeyCode shootLeftKey;
		public KeyCode shootRightKey;

		public float maxSpeedX = 1f;
		public float maxSpeedY = 1f;
		public float forceMagnitude = 10f;
		public float forceAngle = 20f;
		public float gunAngle = 20f;
		public float gunScatterAngle = 5f;
		public float waitTimeBeforeFireWapon0 = 0f;
		public float waitTimeBeforeFireWapon1 = 1f;
		public Transform hands;

		private new Rigidbody rigidbody;
		private float timeUntilFireWeapon0;
		private float timeUntilFireWeapon1;

		public float hoverForce = 20f;

		public Weapon[] weapons;

		private Game game;

		private void Awake() {
			game = GameObject.Find("Game").GetComponent<Game>();
		}

		void FixedUpdate() {
			if (rigidbody == null) {
				rigidbody = transform.GetComponent<Rigidbody>();
			}

			Vector3 leftForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f + forceAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f + forceAngle)) * forceMagnitude, 0f);
			Vector3 rightForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f - forceAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f - forceAngle)) * forceMagnitude, 0f);
			Vector3 centerForceVector = new Vector3(0f, forceMagnitude, 0f);

			bool shootLeft = invertedControls ? Input.GetKey(shootRightKey) : Input.GetKey(shootLeftKey);
			bool shootRight = invertedControls ? Input.GetKey(shootLeftKey) : Input.GetKey(shootRightKey);

			if (game.isUsingJoypad) {
				if (actions == null) {
					// If no controller exists for this cube, just make it translucent.
//				cachedRenderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
				} else {
					if (actions.Rotate.X < 0f) {
						hands.transform.rotation = Quaternion.Euler(0f, 0f, -gunAngle);
						rigidbody.AddForce(rightForceVector, ForceMode.Impulse);
						Attack();
					} else if (actions.Rotate.X > 0f) {
						rigidbody.AddForce(leftForceVector, ForceMode.Impulse);
						hands.transform.rotation = Quaternion.Euler(0f, 0f, gunAngle);
						Attack();
					} else if (actions.Rotate.Y < 0f) {
						rigidbody.AddForce(centerForceVector, ForceMode.Impulse);
						hands.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
						Attack();
					} else if (actions.Rotate.X == 0f && actions.Rotate.Y >= 0f) {
						timeUntilFireWeapon0 = waitTimeBeforeFireWapon0;
						timeUntilFireWeapon1 = waitTimeBeforeFireWapon1;
						hands.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
					}
				}
			} else {

				// OLD WITH KEYBOARD
				if (shootLeft && !shootRight) {
					hands.transform.rotation = Quaternion.Euler(0f, 0f, -gunAngle);
					rigidbody.AddForce(rightForceVector, ForceMode.Impulse);
					Attack();
				}
				else if (shootRight && !shootLeft) {
					rigidbody.AddForce(leftForceVector, ForceMode.Impulse);
					hands.transform.rotation = Quaternion.Euler(0f, 0f, gunAngle);
					Attack();
				}
				else if (shootLeft && shootRight) {
					rigidbody.AddForce(centerForceVector, ForceMode.Impulse);
					hands.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
					Attack();
				}
				else if (!shootLeft && !shootRight) {
					timeUntilFireWeapon0 = waitTimeBeforeFireWapon0;
					timeUntilFireWeapon1 = waitTimeBeforeFireWapon1;
					hands.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				}
			}

		}

		private void Attack() {
			if (timeUntilFireWeapon0 <= 0f) {
				weapons[0].Attack();
			}
			else {
				timeUntilFireWeapon0 -= Time.deltaTime;
			}

			if (timeUntilFireWeapon1 <= 0f) {
				weapons[1].Attack();
			}
			else {
				timeUntilFireWeapon1 -= Time.deltaTime;
			}
		}

		void OnTriggerStay(Collider collider) {
			if (rigidbody == null) {
				rigidbody = transform.GetComponent<Rigidbody>();
			}

			if (collider.tag == "HoverZone") {
				rigidbody.AddForce(Vector3.up * hoverForce, ForceMode.Acceleration);
			}

		}

		//-----------------------------------------------------------

		public PlayerActions actions { get; set; }

		Renderer cachedRenderer;


		void OnDisable()
		{
			if (actions != null)
			{
				actions.Destroy();
			}
		}


//		void Start()
//		{
//			cachedRenderer = GetComponent<Renderer>();
//		}
//
//
//		void Update()
//		{
//			if (Actions == null)
//			{
//				// If no controller exists for this cube, just make it translucent.
//				cachedRenderer.material.color = new Color( 1.0f, 1.0f, 1.0f, 0.2f );
//			}
//			else
//			{
//				// Set object material color.
//				cachedRenderer.material.color = GetColorFromInput();
//
//				// Rotate target object.
//				transform.Rotate( Vector3.down, 500.0f * Time.deltaTime * Actions.Rotate.X, Space.World );
//				transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * Actions.Rotate.Y, Space.World );
//			}
//		}
//
//
//		Color GetColorFromInput()
//		{
//			if (Actions.Green)
//			{
//				return Color.green;
//			}
//
//			if (Actions.Red)
//			{
//				return Color.red;
//			}
//
//			if (Actions.Blue)
//			{
//				return Color.blue;
//			}
//
//			if (Actions.Yellow)
//			{
//				return Color.yellow;
//			}
//
//			return Color.white;
//		}
	}
}

