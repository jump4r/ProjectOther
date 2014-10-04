using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Terrain/Setting timer
	float timer = 3f;

	// Storing Vectors to determine distance between the two players.
	Vector3 vStore1;
	Vector3 vStore2;

	// Change Lighting script
	GameObject scripts;
	GameObject tp;
	// Use this for initialization
	void Start () {
		scripts = GameObject.FindGameObjectWithTag("Scripts");
		// Temp Player for testing purposes
		tp = GameObject.Find ("TempPlayer");
		vStore1 = new Vector3(0f, 0f, 0f);
		vStore2 = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

		// Update the other player's position in the world.
		if( photonView.isMine ) {
			// Do nothing -- the character motor/input/etc... is moving us
		}
		else {
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
			//Debug.Log (transform.position);
		}
		

		//Debug.Log ("The Distance Between players is " + Vector3.Distance (vStore1, vStore2));
		// if (Vector3.Distance (vStore1, vStore2) > 50f) {
		if (timer < 0f) {
			timer = 1f;
			ChangeLighting nl = scripts.GetComponent<ChangeLighting> ();
			PlayerList pl = scripts.GetComponent<PlayerList>();

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
				Debug.Log("Our player is at position: " + vStore1);
				Debug.Log("Other player is at  " + vStore2);
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
			//Debug.Log ("Other Players location is " + transform.position);
			vStore2 = transform.FindChild ("OVRCameraController").transform.position;

			realRotation = (Quaternion)stream.ReceiveNext();
		}

	}
}
