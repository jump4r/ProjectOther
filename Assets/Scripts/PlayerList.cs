using UnityEngine;
using System.Collections;

public class PlayerList : Photon.MonoBehaviour {

	public GameObject[] players;
	public GameObject myPlayer;

	public AudioClip footstep;
	
	private float timer = 1f;

	private bool interactableInstatiated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (players.Length > 1) {
			Debug.DrawLine (players[0].transform.position, players[1].transform.position, Color.green, 1f);
			timer = 1f;

			// If both players are in the game, instatiate the Interactable Objects
			// We only want the objects instantiated once, really I should just have the master do it
			 if (PhotonNetwork.player.isMasterClient && !interactableInstatiated) {
				// Instatiate
				Debug.Log ("Instantiating Interactable Objects");
				GameObject.FindGameObjectWithTag("Scripts").GetComponent<CreateInteractable>().CreateObjects();
				interactableInstatiated = true; 
			} 
		}
		timer -= Time.deltaTime;
	}

	// Update Active players in the game. 
	[RPC]
	public void UpdatePlayers() {
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	// Play Footstep over the network so that other players can hear it.
	[RPC]
	public void PlayNetworkStep(Vector3 pos) {
		AudioSource.PlayClipAtPoint (footstep, pos);
		//Debug.Log ("Other player footstep at " + pos);
	}

	// Change the Level of fog in the world based off of players position.
	public void ChangeFog() {
		Debug.Log ("Calling ChangeFog from PlayerList");
		if (players.Length < 2)
			return;

		float normalizeDensity = 2000f;
		float density = Vector3.Distance(players[0].transform.position, players[1].transform.position) / normalizeDensity;
		// MAX Fog Density 0.08u, Minimum 0.0u
		if (density > 0.04f) {
			RenderSettings.fog = true;
			RenderSettings.fogDensity = 0.04f;
		}
		else if (density > 0.01) {
			RenderSettings.fog = true;
			RenderSettings.fogDensity = density;
		}
		else {
			RenderSettings.fog = false;
			//Debug.Log ("Disable Fog");
		}
	}

	public void ChangeLighting() {
		if (players.Length < 2)
			return;

		float normalizeColor = 300f;
		float c = Vector3.Distance (players[0].transform.position, players[1].transform.position) / normalizeColor;
		
		// Max RGB color val set at 0.45f (scale 0 - 1)
		if (c > 0.45f)
			c = 0.45f;
		Color color = new Color (0.45f - c, 0.45f - c, 0.45f - c, 1);
		RenderSettings.ambientLight = color;
		//Debug.Log ("Changing Light to " + color);
	}

	public void PlayFootstep() {
		AudioSource.PlayClipAtPoint (footstep, myPlayer.transform.position);
	}
}
