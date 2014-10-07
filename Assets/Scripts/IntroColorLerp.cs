using UnityEngine;
using System.Collections;

public class IntroColorLerp : MonoBehaviour {

	public GameObject cameraLeft;
	public GameObject cameraRight;


	private bool lerpBackwards = false;

	// Hard set lightsettings 
	private float blackLight = 0.05f;	// Darker Light
	private  float whiteLight = 0.65f;	// Brighter Light

	// Timer for blur removal
	private float timer = 3f;

	// Lerp Speed
	private float lerpSpeed = .1f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// Adding Input Cause Lazy
		if (Input.anyKey) {
			Application.LoadLevel (1);
		}

		if (timer < 0) {
			//RemoveBlur();
		}

		if (lerpBackwards) {
			float c = whiteLight + Random.Range (0f, 0.05f);
			//Debug.Log ("Lerping Color to " + c);
			LerpColor(c);
			if (RenderSettings.ambientLight.r < 0.1f) {
				lerpBackwards = !lerpBackwards;
			}
		}
		
		else {
			float c = blackLight + Random.Range(0f, 0.05f);
			//Debug.Log ("Lerping Color to " + c);
			LerpColor(c);
			if (RenderSettings.ambientLight.r < 0.4f) {
				lerpBackwards = !lerpBackwards;
			}
		}
		timer -= Time.deltaTime;	
	}

	// Hurray for unneeded funcitons.
	void LerpColor(float color) {
		RenderSettings.ambientLight = Color.Lerp (RenderSettings.ambientLight, new Color(color, color, color), lerpSpeed * Time.deltaTime);
		//Debug.Log ("Lerping Color to " + color);
	}
}
