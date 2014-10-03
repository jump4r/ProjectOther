using UnityEngine;
using System.Collections;

public class PlayerList : MonoBehaviour {

	public GameObject[] players;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Update Active players in the game. 
	[RPC]
	public void UpdatePlayers() {
		players = GameObject.FindGameObjectsWithTag ("Player");
		Debug.Log (players.Length + " players");
	}
}
