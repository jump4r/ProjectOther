using UnityEngine;
using System.Collections;

public class PlayerList : MonoBehaviour {

	public GameObject[] players;
	public float timer = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (players.Length > 1) {
			Debug.DrawLine (players[0].transform.position, players[1].transform.position, Color.green, 1f);
			timer = 1f;
		}
		timer -= Time.deltaTime;
	}

	// Update Active players in the game. 
	[RPC]
	public void UpdatePlayers() {
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	// Change the Level of fog in the world based off of players position.
	public void ChangeFog() {
		if (players.Length < 2)
			return;

		float normalizeDensity = 1000f;
		float density = Vector3.Distance(players[0].transform.position, players[1].transform.position) / normalizeDensity;
		// MAX Fog Density 0.08u, Minimum 0.0u
		if (density > 0.06f) {
			RenderSettings.fog = true;
			RenderSettings.fogDensity = 0.08f;
		}
		else if (density > 0.01) {
			RenderSettings.fog = true;
			RenderSettings.fogDensity = density;
		}
		else {
			RenderSettings.fog = false;
			Debug.Log ("Disable Fog");
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
		Debug.Log ("Changing Light to " + color);
	}
}
