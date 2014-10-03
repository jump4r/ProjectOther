using UnityEngine;
using System.Collections;

public class RandomAmbientSound : MonoBehaviour {

	public AudioClip[] ambientClips;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Play Clips randomly around the map;
		if (Random.Range (0, 1) < 0.4f) {
			RandomPlay ();
		}
	}

	void RandomPlay() {
		int c = Random.Range(0, ambientClips.Length);
		if (!audio.isPlaying) {
			audio.clip = ambientClips[c];
			audio.Play();
			//Debug.Log("Audio Clip " + c + " has played");
		}
	}
}
