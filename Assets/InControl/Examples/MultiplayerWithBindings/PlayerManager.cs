using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;


namespace MultiplayerWithBindingsExample
{
	// This example iterates on the basic multiplayer example by using action sets with
	// bindings to support both joystick and keyboard players. It would be a good idea
	// to understand the basic multiplayer example first before looking a this one.
	//
	public class PlayerManager : MonoBehaviour
	{
		
		//public GameObject playerPrefab;
		public GameObject playerFirePrefab;
		public GameObject playerIcePrefab;

        public Transform spawnFire;
        public Transform spawnIce;
 
		const int maxPlayers = 2;
		List<Player> players = new List<Player>( maxPlayers );

		private Game game;

        List<Vector3> playerPositions = new List<Vector3>() {
			//new Vector3( -3, -27, 2 ),
			//new Vector3( 3, -27, 2 ),
			//new Vector3( -1, -1, -10 ),
			//new Vector3( 1, -1, -10 ),
		};

		PlayerActions keyboardListener;
		PlayerActions joystickListener;


		private void Awake() {
			game = GameObject.Find("Game").GetComponent<Game>();
            playerPositions.Add(spawnFire.transform.position);
            playerPositions.Add(spawnIce.transform.position);
        }

		void OnEnable()
		{
			InputManager.OnDeviceDetached += OnDeviceDetached;
			keyboardListener = PlayerActions.CreateWithKeyboardBindings();
			joystickListener = PlayerActions.CreateWithJoystickBindings();
		}


		void OnDisable()
		{
			InputManager.OnDeviceDetached -= OnDeviceDetached;
			joystickListener.Destroy();
			keyboardListener.Destroy();
		}

		bool hasSpawnedKeyboardPlayers = false;
		void Update()
		{
			if (JoinButtonWasPressedOnListener( joystickListener ))
			{
				var inputDevice = InputManager.ActiveDevice;

				if (ThereIsNoPlayerUsingJoystick( inputDevice ))
				{
					CreatePlayer( inputDevice);
				}
			}

			if(!game.isUsingJoypad && !hasSpawnedKeyboardPlayers) {
				CreatePlayer( null );
				CreatePlayer( null );
				hasSpawnedKeyboardPlayers = true;
			}

//			if (JoinButtonWasPressedOnListener( keyboardListener ))
//			{
//				if (ThereIsNoPlayerUsingKeyboard())
//				{
//					CreatePlayer( null );
//				}
//			}
		}


		bool JoinButtonWasPressedOnListener( PlayerActions actions )
		{
			return actions.Green.WasPressed || actions.Red.WasPressed || actions.Blue.WasPressed || actions.Yellow.WasPressed;
		}


		Player FindPlayerUsingJoystick( InputDevice inputDevice )
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
				if (player.actions.Device == inputDevice)
				{
					return player;
				}
			}

			return null;
		}


		bool ThereIsNoPlayerUsingJoystick( InputDevice inputDevice )
		{
			return FindPlayerUsingJoystick( inputDevice ) == null;
		}


		Player FindPlayerUsingKeyboard()
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
				if (player.actions == keyboardListener)
				{
					return player;
				}
			}

			return null;
		}


		bool ThereIsNoPlayerUsingKeyboard()
		{
			return FindPlayerUsingKeyboard() == null;
		}


		void OnDeviceDetached( InputDevice inputDevice )
		{
			var player = FindPlayerUsingJoystick( inputDevice );
			if (player != null)
			{
				RemovePlayer( player );
			}
		}


		Player CreatePlayer( InputDevice inputDevice ) // -1 = dont care, 0 = red, 1 = blue
		{
			if (players.Count < maxPlayers)
			{
				// Pop a position off the list. We'll add it back if the player is removed.
				var playerPosition = playerPositions[0];
				playerPositions.RemoveAt( 0 );

				var gameObject = (GameObject) Instantiate( players.Count == 0 ? playerFirePrefab : playerIcePrefab, playerPosition, Quaternion.identity );
				gameObject.name = players.Count == 0 ? "Player Fire" : "Player Ice";
				var player = gameObject.GetComponent<Player>();

				if (inputDevice == null)
				{
					// We could create a new instance, but might as well reuse the one we have
					// and it lets us easily find the keyboard player.
					player.actions = keyboardListener;
				}
				else
				{
					// Create a new instance and specifically set it to listen to the
					// given input device (joystick).
					var actions = PlayerActions.CreateWithJoystickBindings();
					actions.Device = inputDevice;

					player.actions = actions;
				}

				players.Add( player );

				if (players.Count == 2) {
					Time.timeScale = 1f;
				}

				return player;
			}



			return null;
		}


		void RemovePlayer( Player player )
		{
			playerPositions.Insert( 0, player.transform.position );
			players.Remove( player );
			player.actions = null;
			Destroy( player.gameObject );
		}


		void OnGUI()
		{
//			const float h = 22.0f;
//			var y = 10.0f;
//
//			GUI.Label( new Rect( 10, y, 300, y + h ), "Active players: " + players.Count + "/" + maxPlayers );
//			y += h;
//
//			if (players.Count < maxPlayers)
//			{
//				GUI.Label( new Rect( 10, y, 300, y + h ), "Press a button or a/s/d/f key to join!" );
//				y += h;
//			}
		}
	}
}