using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Terrain/Setting timer
	float timer = 3f;
	float footstepReset = 1f;

	// Storing Vectors to determine distance between the two players.
	Vector3 vStore1;
	Vector3 vStore2;

	// Change Lighting script
	GameObject scripts;
	GameObject tp;
	PlayerList pl;
	// Use this for initialization
	void Start () {
		scripts = GameObject.FindGameObjectWithTag("Scripts");
		// Temp Player for testing purposes
		tp = GameObject.Find ("TempPlayer");
		vStore1 = new Vector3(0f, 0f, 0f);
		vStore2 = new Vector3(0f, 0f, 0f);

		pl = scripts.GetComponent<PlayerList>();
	}
	
	// Update is called once per frame
	void Update () {

		// Quit Game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		// Update the other player's position in the world.
		if( photonView.isMine ) {
			// Manage Footsteps. This is pretty sloppy but it'll work for now.
			if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && footstepReset < 0) {
				pl.GetComponent<PhotonView>().RPC ("PlayNetworkStep", PhotonTargets.All, transform.position);
				//pl.PlayFootstep ();
				footstepReset = .5f;
			}
		}
		else {
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
			//Debug.Log (transform.position);
		}
		

		//Debug.Log ("The Distance Between players is " + Vector3.Distance (vStore1, vStore2));
		// if (Vector3.Distance (vStore1, vStore2) > 50f) {
		if (timer < 0f) {
			timer = .25f;
			ChangeLighting nl = scripts.GetComponent<ChangeLighting> ();

			// Use temp player if in Offline mode
			if (PhotonNetwork.playerList.Length < 2) {
				//Debug.Log ("We Only have " + PhotonNetwork.playerList.Length + " player");
				//Debug.Log ("Distance Between Temp and Player is " + Vector3.Distance (tp.transform.position, transform.position));
				if (nl != null) {
					// Change Fog to Approriate Location
					nl.GetComponent<PhotonView> ().RPC ("ChangeFog", PhotonTargets.All, Vector3.Distance (tp.transform.position, transform.position));
					// New Color for Ambient Light
					//Color c = new Color(RenderSettings.ambientLight.r - .05f, RenderSettings.ambientLight.g - .05f, RenderSettings.ambientLight.b - .05f, 1);
					nl.GetComponent<PhotonView> ().RPC ("ChangeLight", PhotonTargets.All, Vector3.Distance (tp.transform.position, transform.position));
				} 
			}

			else if (nl != null) {
				Debug.Log ("We have " + PhotonNetwork.playerList.Length + " players");
				Debug.DrawLine (vStore1, vStore2, Color.red, 1f);
				// Change Fog based off of Approriate Location
				//GameObject.FindGameObjectWithTag("Scripts").GetComponent<ChangeLighting>().IndependantChangeFog(Vector3.Distance (vStore1, vStore2));
				//nl.GetComponent<PhotonView> ().RPC ("ChangeFog", PhotonTargets.Others, Vector3.Distance(vStore1, vStore2));
				pl.ChangeFog ();
				// New Color for Ambient Light
				//Color c = new Color(RenderSettings.ambientLight.r - .05f, RenderSettings.ambientLight.g - .05f, RenderSettings.ambientLight.b - .05f, 1);
				//nl.GetComponent<PhotonView> ().RPC ("ChangeLight", PhotonTargets.All, c);
				pl.ChangeLighting ();
			} 
		}


		footstepReset -= Time.deltaTime;
		timer -= Time.deltaTime;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			// This is OUR player. We need to send our actual position to the network.
			stream.SendNext(transform.position);
			//Debug.Log("My location is " + transform.position);
			vStore1 = transform.FindChild ("OVRCameraController").transform.position;

			stream.SendNext(transform.rotation);
		}
		else {
			// This is someone else's player. We need to receive their position (as of a few
			// millisecond ago, and update our version of that player.

			realPosition = (Vector3)stream.ReceiveNext();
			vStore2 = transform.FindChild ("OVRCameraController").transform.position;

			realRotation = (Quaternion)stream.ReceiveNext();
		}

	}
}
