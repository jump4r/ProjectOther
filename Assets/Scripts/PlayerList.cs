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
		Debug.Log (players.Length + " players");
	}


}
