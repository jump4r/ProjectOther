using UnityEngine;
using System.Collections;

public class Interactable : Photon.MonoBehaviour {

	private float pushForce = 800f;

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		// Debug.Log ("This Photon Owner Id is: " + photonView.ownerId);
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {

		}

		else {
			//transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			//transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player") {
			Vector3 dir = (this.gameObject.transform.position - col.gameObject.transform.position).normalized * pushForce;
			// rigidbody.AddExplosionForce (pushForce, dir, 10f);
			rigidbody.AddForce (dir);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			Debug.Log ("Sending Position of Object Over the network");
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else {
			realPosition = (Vector3)stream.ReceiveNext();
			Debug.Log ("Recieving Position from the network: realPosition = " + realPosition);
			realRotation = (Quaternion)stream.ReceiveNext();
		}

	}
}
