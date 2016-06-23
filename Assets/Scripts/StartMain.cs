using UnityEngine;
using System.Collections;

public class StartMain : MonoBehaviour {

	GameObject title;
	GameObject tabToStart;
	// Use this for initialization

	float speed;
	float deceleration;
	bool sliding = false;

	void Start () {
		speed = 0.3f;
		deceleration = 0.005f;
		title = GameObject.Find ("Title");
		tabToStart = GameObject.Find("TapToStart");
		StartCoroutine ("SlideIn");

		GameObject tile = GameObject.Find ("Bottom tile");
		Color32 colour = new Color32 (0, 255, 33, 255);	//...Same color with the first tile in the game
		tile.GetComponent<Renderer> ().material.color = colour;

	}

	IEnumerator SlideIn() {
		sliding = true;
		while (title.transform.localPosition.x > 0) {
			title.transform.Translate (-speed, 0, 0);
			tabToStart.transform.Translate (speed, 0, 0);
			speed -= deceleration;
			yield return new WaitForSeconds (0.01f);
		}
		sliding = false;
	}

	IEnumerator SlideOut(){
		sliding = true;
		while (title.transform.localPosition.x > -8) {
			title.transform.Translate (speed, 0, 0);
			tabToStart.transform.Translate (-speed, 0, 0);
			speed -= deceleration;
			yield return new WaitForSeconds (0.01f);
		}
		Application.LoadLevel ("Game");
	}

	// Update is called once per frame
	void Update () {

		float speed = 1;

		//if (!slide && title.transform.localPosition.x > speed/2) {
		//	title.transform.Translate (-speed,0,0);
		//	tabToStart.transform.Translate (speed,0,0);
		//}

		if (Input.GetMouseButtonUp (0) && !sliding)
			StartCoroutine ("SlideOut");

		//if (slide && title.transform.localPosition.x < 8) {
		//	title.transform.Translate (speed, 0, 0);
		//	tabToStart.transform.Translate (-speed, 0, 0);
		//} else if (slide)
			
	}
}
