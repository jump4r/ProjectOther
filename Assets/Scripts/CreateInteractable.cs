using UnityEngine;
using System.Collections;

public class CreateInteractable : MonoBehaviour {
	public Vector3[] objSpawnPos;
	public string[] objPrefabNames;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateObjects() {
		foreach (Vector3 v in objSpawnPos) {
			Debug.Log ("Creating the objects");
			int c = Random.Range(0, objPrefabNames.Length);
			PhotonNetwork.Instantiate(objPrefabNames[c], v, Quaternion.identity, 0);
		}
	}
}
