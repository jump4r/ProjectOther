using UnityEngine;
using System.Collections;

public class PlaySoundFromPlayer : MonoBehaviour {

	public AudioClip[] soundClips;
	private bool started = false;
	private bool finished = false;
	private float delayTimeToDestroy = 1f;

	// Use this for initialization
	void Start () {
		RandomPlay ();
	}
	
	// Update is called once per frame
	void Update () {
		if (started && !audio.isPlaying && delayTimeToDestroy < 0f) {
			// Destroy (gameObject);
			Debug.Log ("Destroy Audio Object");
			PhotonNetwork.Destroy (gameObject);
		}

		delayTimeToDestroy -= Time.deltaTime;
	}

	void RandomPlay() {
		int c = Random.Range(0, soundClips.Length);
		if (!audio.isPlaying) {
			audio.clip = soundClips[c];
			audio.Play();
			started = true;
			Debug.Log("Audio Clip " + c + " has played");
		}
	}
}
