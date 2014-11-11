using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	public float fadeTime = 2.5f;
	public GameObject cubeCurtain; // Jesus, this is a really hacky way to get around this. But i'm not sure how to fade the entire screent to black.
	private bool fade = false;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fade) {
			Color c = cubeCurtain.renderer.material.color;
			float newAlpha = c.a + 0.005f;
			c.a = newAlpha;
			cubeCurtain.renderer.material.color = c;
		}
	}

	public void FadeScreen() {
		cubeCurtain.GetComponent<MeshRenderer> ().enabled = true;
		fade = true;
		Invoke ("ChangeLevel", fadeTime); // Literally just discovered this function and it is great.
	}

	void ChangeLevel() {
		Application.LoadLevel (2);
	}
}