using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Terrain/Setting timer
	float timer = 3f;

	// Storing Vectors to determine distance between the two players.
	Vector3 vStore1 = new Vector3(0f, 0f, 0f);
	Vector3 vStore2 = new Vector3(0f, 0f, 0f);

	// Change Lighting script
	GameObject cl;
	GameObject tp;
	// Use this for initialization
	void Start () {
		cl = GameObject.FindGameObjectWithTag("Scripts");
		// Temp Player for testing purposes
		tp = GameObject.Find ("TempPlayer");
	}
	
	// Update is called once per frame
	void Update () {
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
			ChangeLighting nl = cl.GetComponent<ChangeLighting> ();

			// Use temp player if in Offline mode
			if (PhotonNetwork.playerList.Length < 2) {
				Debug.Log ("We Only have " + PhotonNetwork.playerList.Length + " player");
				Debug.Log ("Distance Between Temp and Player is " + Vector3.Distance (tp.transform.position, transform.position));
				if (nl != null) {
					// Change Fog to Approriate Location
					nl.GetComponent<PhotonView> ().RPC ("ChangeFog", PhotonTargets.All, Vector3.Distance (tp.transform.position, transform.position));
					// New Color for Ambient Light
					//Color c = new Color(RenderSettings.ambientLight.r - .05f, RenderSettings.ambientLight.g - .05f, RenderSettings.ambientLight.b - .05f, 1);
					nl.GetComponent<PhotonView> ().RPC ("ChangeLight", PhotonTargets.All, Vector3.Distance (tp.transform.position, transform.position));
				} 
			}

			else if (nl != null) {
				// Change Fog to Approriate Location
				nl.GetComponent<PhotonView> ().RPC ("ThirdFog", PhotonTargets.All, RenderSettings.fogDensity - .02f);
				// New Color for Ambient Light
				Color c = new Color(RenderSettings.ambientLight.r - .05f, RenderSettings.ambientLight.g - .05f, RenderSettings.ambientLight.b - .05f, 1);
				nl.GetComponent<PhotonView> ().RPC ("ChangeLight", PhotonTargets.All, c);
			} 
		}
		timer -= Time.deltaTime;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			// This is OUR player. We need to send our actual position to the network.

			stream.SendNext(transform.position);
			//Debug.Log("My location is " + transform.position);
			vStore1 = transform.position;
			stream.SendNext(transform.rotation);
		}
		else {
			// This is someone else's player. We need to receive their position (as of a few
			// millisecond ago, and update our version of that player.

			realPosition = (Vector3)stream.ReceiveNext();
			//Debug.Log ("Other Players location is " + transform.position);
			vStore2 = transform.position;
			realRotation = (Quaternion)stream.ReceiveNext();
		}

	}
}
