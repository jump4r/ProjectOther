using UnityEngine;
using System.Collections;

public class PlayerSoundInput : MonoBehaviour {

	private float soundDelay = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Pressing 'f1' plays a sound.
		if (Input.GetKeyDown (KeyCode.CapsLock) && soundDelay < 0f) {
			Debug.Log ("Instantiate New Sound Object");
			PhotonNetwork.Instantiate ("PlayerSound", gameObject.transform.position, Quaternion.identity, 0);
			soundDelay = 0.5f;
		}

		if (soundDelay >= 0f) {
			soundDelay -= Time.deltaTime;
		}

	}
}
