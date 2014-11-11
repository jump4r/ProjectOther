using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject standbyCamera;
	public bool offlineMode = false;
	public GameObject[] players;

	// Update The List of Players
	PlayerList pl;
	SpawnSpot[] spawnSpots;

	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		pl = GameObject.FindGameObjectWithTag ("Scripts").GetComponent<PlayerList> ();
		Connect ();
	}

	void Connect() {
		if(offlineMode) {
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		}

		else 
			PhotonNetwork.ConnectUsingSettings( "TheOtherv001" );
	}
	 
	void OnGUI() {
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
	}

	void OnJoinedLobby() {
		//Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		//Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom( null );
	}

	void OnJoinedRoom() {
		//Debug.Log ("OnJoinedRoom");

		SpawnMyPlayer();
	}

	void SpawnMyPlayer() {
		if(spawnSpots == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}

		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);

		standbyCamera.SetActive(false);

		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("MouseLook")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("CharacterMotor")).enabled = true;
		myPlayerGO.GetComponent<PlayerSoundInput> ().enabled = true;
		myPlayerGO.GetComponent<ScreenFade> ().enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("OVRPlayerController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("OVRGamepadController")).enabled = true;

		// Activate Oculus Character Contorller
		//myPlayerGO.GetComponent<OVRPlayerController> ().enabled = true;

		// Change this to Oculus Camera
		//myPlayerGO.transform.FindChild("OVRCameraController").gameObject.SetActive(true);
		GameObject cc = myPlayerGO.transform.FindChild ("OVRCameraController").gameObject;
		myPlayerGO.transform.FindChild("OVRCameraController").gameObject.transform.FindChild ("CameraLeft").GetComponent<Camera>().enabled = true;
		myPlayerGO.transform.FindChild("OVRCameraController").gameObject.transform.FindChild ("CameraRight").GetComponent<Camera>().enabled = true;
		myPlayerGO.transform.FindChild("OVRCameraController").GetComponent<AudioListener> ().enabled = true;


		// Add player to players Array.
		// Send RPC Call to ChangeLighting which adds players to an array?
		//ChangeLighting cl = GameObject.FindGameObjectWithTag ("Scripts").GetComponent<ChangeLighting> ();
		//cl.GetComponent<PhotonView>().RPC ("UpdatePlayers", PhotonTargets.All);
		pl.GetComponent<PhotonView> ().RPC ("UpdatePlayers", PhotonTargets.AllBuffered);
		pl.myPlayer = myPlayerGO;

		// Check to see if this is the master client
		if (!PhotonNetwork.player.isMasterClient) { 
			Debug.Log("This player is NOT the master client");
		}
	}
}
