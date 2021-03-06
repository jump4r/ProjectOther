﻿using UnityEngine;
using System.Collections;

public class ChangeLighting : MonoBehaviour {

	public GameObject[] players;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// NON-RPC Change Fog effects when players move positions
	public void IndependantChangeFog (float rawDensity) {
		Debug.Log ("RawDensity: " + rawDensity);
		float density = rawDensity / 1000f;
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

	// Change Fog effects when the players move positions
	[RPC]
	public void ChangeFog (float rawDensity) {
		float density = rawDensity / 1000f;
		// MAX Fog Density 0.08u, Minimum 0.0u
		if (density > 0.03f) {
			RenderSettings.fog = true;
			RenderSettings.fogDensity = 0.03f;
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

	[RPC]
	// CHange Ambient Light to make scene lighter or darker.
	public void ChangeLight(float rawDist) {
		float c = rawDist / 500f;

		// Max RGB color val set at 0.45f (scale 0 - 1)
		if (c > 0.45f)
			c = 0.45f;
		Color color = new Color (0.45f - c, 0.45f - c, 0.45f - c, 1);
		RenderSettings.ambientLight = color;
	}

	//[RPC]
	public void UpdatePlayers() {
		if (PhotonNetwork.playerList.Length == 2) {
			players = GameObject.FindGameObjectsWithTag ("Player");
			Debug.Log (players.Length + " players");
		}
	}
}
