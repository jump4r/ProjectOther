using UnityEngine;
using System.Collections;

public class PlayerSoundInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Pressing 'f1' plays a sound.
		if (Input.GetKeyDown (KeyCode.F1)) {
			Debug.Log ("Instantiate New Sound Object");
			PhotonNetwork.Instantiate ("PlayerSound", gameObject.transform.position, Quaternion.identity, 0);
		}
	}
}
