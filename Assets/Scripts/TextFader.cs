using UnityEngine;
using System.Collections;

public class TextFader : MonoBehaviour {

	public float fadeSpeed = 1.5f;
	public bool changeScene;

	private float timer = 0f;
	private TextMesh text;

	private bool startFadeIn = false;
	private bool startFadeOut = false;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (startFadeIn) {
			StartFade ();
			Debug.Log ("1s, starting Fade In");
		}

		else if (startFadeOut) {
			EndFade ();
			Debug.Log("7s, starting Fade Out");
		}

		if (timer > 1f && timer < 7f && !startFadeIn) {
			Debug.Log("Setting StartFadeIn to true");
			startFadeIn = true;
		}

		if (timer > 7f) {
			startFadeOut = true;
		}

		timer += Time.deltaTime;
	}

	void FadeTextIn() {
		text.color = Color.Lerp (text.color, Color.white, fadeSpeed * Time.deltaTime);
	}

	void FadeTextOut() {
		text.color = Color.Lerp (text.color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	void StartFade() {
		FadeTextIn ();
		if (text.color.a > 0.90f) {
			text.color = Color.white;
			startFadeIn = false;
			Debug.Log ("The Color is " + text.color.a + "please set me to false");
		}
	}

	void EndFade() {
		FadeTextOut ();
		if (text.color.a < 0.15f) {
			text.color = Color.clear;
			startFadeOut = false;
			if (changeScene) {
				Application.LoadLevel (1);
			}
		}
	}
}
