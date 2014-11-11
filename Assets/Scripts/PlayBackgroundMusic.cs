using UnityEngine;
using System.Collections;

public class PlayBackgroundMusic : Photon.MonoBehaviour {

	private PlayerList pl;

	private float timer = 0.5f;
	private const float timerReset = 0.5f;
	private const float volumeNormalizer = 200f;

	// Ending Song to play during the ending scene.
	public AudioClip endingSong;		// Ending song.
	private bool endingPlaying = false; // Is the ending playing.
	private bool endingCompleted = false; // Has the ending completed

	// Use this for initialization
	void Start () {
		pl = GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerList>();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0f) {
			timer = timerReset;
			if (!endingPlaying)
				UpdateMusicVolume();

			if (endingPlaying && !audio.isPlaying)
				endingCompleted = true;
		}

		// Fade and switch Scenes when the ending song completes
		if (endingCompleted) {
			foreach (GameObject player in pl.players) {
				if (player.GetComponent<ScreenFade>().enabled) {
					player.GetComponent<ScreenFade>().FadeScreen ();
				}
			}
		}
	}

	// Get the distance between the two players
	// ROFL this function is totally unneeded.
	float GetDistance() {
		pl = GameObject.FindGameObjectWithTag("Scripts").GetComponent<PlayerList>();
		//Debug.Log ("Number of players in the game (from the BGMusic Player " + pl.players.Length);
		if (pl.players.Length == 2) {
 			float distance = Vector3.Distance (pl.players[0].transform.position, pl.players[1].transform.position);
			return distance;
		}
	
		return 0f;
	}

	// Update the music volume based off of the position of the agents in the world.
	void UpdateMusicVolume() {
		float dist = GetDistance ();
		if (dist != 0f) {
			gameObject.audio.volume = 1f - (dist / volumeNormalizer);
			print ("Updated music volume based off of distance: " + dist);
		}

		// If the players get close enough, play the ending sequence
		if (dist < 15f && dist != 0 && !endingPlaying) {
			endingPlaying = true;
			PlayEndingSequence();
		}
	}

	void PlayEndingSequence() {
		Debug.Log ("Ending Sequence Activated, Screen Fade just for check");
		endingPlaying = true;
		audio.Stop ();
		audio.loop = false;
		audio.clip = endingSong;
		audio.volume = .5f;
		audio.Play ();
		// Test out the FadeScree
	}
}
