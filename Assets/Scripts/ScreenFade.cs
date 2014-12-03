using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public float fadeTime = 100.5f;
	private float timer = 0f;
	public GameObject cubeCurtain; // Jesus, this is a really hacky way to get around this. But i'm not sure how to fade the entire screent to black.
	private bool fade = false;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fade) {
			timer += Time.deltaTime;
			Color c = cubeCurtain.renderer.material.color;
			float newAlpha = c.a + 0.12f * Time.deltaTime;
			c.a = newAlpha;
			cubeCurtain.renderer.material.color = c;
		}
	}

	public void FadeScreen() {
		Debug.Log ("Fade Screen Called");
		cubeCurtain.GetComponent<MeshRenderer> ().enabled = true;
		fade = true;
		Invoke ("ChangeLevel", 15f); // Literally just discovered this function and it is great.
	}

	void ChangeLevel() {
		Debug.Log ("Loaded level after " + timer + " seconds");
		Application.LoadLevel (2);
	}
}