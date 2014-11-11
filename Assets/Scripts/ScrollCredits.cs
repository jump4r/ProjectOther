using UnityEngine;
using System.Collections;

public class ScrollCredits : MonoBehaviour {

	private float scrollBy = .05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, (transform.position.y + scrollBy), transform.position.z);
	}
}
