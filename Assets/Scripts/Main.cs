using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public enum state
	{
		idle, //....includes the time which waits for tapping to start playing
		running, //....playing time
		gameover, //....includes the time which camera is set for viewing the completed stack
		waitting //....to temporarily solve timing problems during mouse click. When ui is done it would be removed (if it becomes unnecessary)
	}

	public GameObject _tile;
	List<GameObject> stack = new List<GameObject>();
	List<Color32> spectrum = new List<Color32>(){
		new Color32(0, 255, 33, 255)	,
		new Color32(167, 255, 0, 255)	,
		new Color32(230, 255, 0, 255)	,
		new Color32(255, 237, 0, 255)	,
		new Color32(255, 206, 0, 255)	,
		new Color32(255, 185, 0, 255)	,
		new Color32(255, 142, 0, 255)	,
		new Color32(255, 111, 0, 255)	,
		new Color32(255, 58, 0, 255)	,
		new Color32(255, 0, 0, 255)		,
		new Color32(255, 0, 121, 255)	,
		new Color32(255, 0, 164, 255)	,
		new Color32(241, 0, 255, 255)	,
		new Color32(209, 0, 255, 255)	,
		new Color32(178, 0, 255, 255)	};
	int modifier;
	int colorIndex;
	bool startMove;
	float lenghtOFMove;
	float speed;
	float camOrtSize_Final;
	state gameState;

	// Use this for initialization
	void Start () {

		gameState = state.running;
		modifier = 1;
		colorIndex = 0;
		startMove = false;

		stack.Add (GameObject.FindWithTag ("Bottom_tile"));
		stack [0].GetComponent<Renderer> ().material.color = spectrum [0];
		CreateNewTile ();
	}
	
	// Update is called once per frame
	void Update () {

		if (gameState == state.gameover) {

			Camera cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();

			if (cam.orthographicSize >= stack.Count) {
				gameState = state.waitting;
				return;
			}

			float increment = 0.5f;
			if (cam.orthographicSize >= camOrtSize_Final)
				gameState = state.idle;
			else
				cam.orthographicSize = cam.orthographicSize + increment;
			return;

		} else if (gameState == state.idle) {

			if (Input.GetMouseButtonUp (0))
				Application.LoadLevel ("Game");
			return;

		} else if (gameState == state.waitting) {

			if (Input.GetMouseButtonUp (0))
				gameState = state.idle;
			return;

		}

		//ELSE.....it means the game is running

		speed = 4 * Time.deltaTime;

		if (!startMove && (Input.GetMouseButtonDown (0) || Input.GetKeyDown("space"))) {
			startMove = true;
		}
		if (startMove) {
//			stack.ForEach (MoveTileDown);
			GameObject.FindWithTag("MainCamera").transform.Translate(0, speed, 0, Space.World);
			lenghtOFMove = lenghtOFMove + speed;
			if (lenghtOFMove >= stack[0].transform.localScale.y) {
				if (stack.Count % 2 == 0)
					lenghtOFMove = 0;
				else
					lenghtOFMove = speed;
				startMove = false;

				TextMesh scoreMesh = GameObject.Find("Score").GetComponent<TextMesh> ();
				scoreMesh.text = (stack.Count - 1).ToString();

				CreateNewTile ();
			}
		}
	}
	/*
	void MoveTileDown(GameObject obj){
		obj.transform.Translate (0, -0.2f, 0);
	}
	*/	
	void CreateNewTile(){

		GameObject previousTile = stack[stack.Count - 1];

		GameObject activeTile;
		TheTile tTile;

		activeTile = Instantiate (_tile);
		stack.Add (activeTile);
		tTile = activeTile.GetComponent <TheTile>();

		if(stack.Count > 2)
			activeTile.transform.localScale = previousTile.transform.localScale;
	
		activeTile.transform.position = new Vector3 (previousTile.transform.position.x, 
													 activeTile.transform.position.y + (stack.Count - 2)*activeTile.transform.localScale.y, 
													 previousTile.transform.position.z);

		if (stack.Count % 2 == 0) {
			tTile.move_axis = TheTile.movement_axis.x_axis;
		} else {
			tTile.move_axis = TheTile.movement_axis.z_axis;
		}

		colorIndex += modifier;
		if (colorIndex == spectrum.Count || colorIndex == -1) {
			modifier = -modifier;
			colorIndex += 2*modifier;
		}
		//SET COLORS
		Renderer tileRend;
		tileRend = activeTile.GetComponent<Renderer>();
		tileRend.material.color = spectrum[colorIndex];
	}

	public void GameOver(){
		gameState = state.gameover;
		Camera cam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		camOrtSize_Final = cam.orthographicSize + stack.Count*0.5f; //TODO: final size will be improved
	}
}
