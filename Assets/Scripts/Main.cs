using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

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

	// Use this for initialization
	void Start () {

		modifier = 1;
		colorIndex = 0;
		startMove = false;

		stack.Add (GameObject.FindWithTag ("Bottom_tile"));
		stack [0].GetComponent<Renderer> ().material.color = spectrum [0];
		CreateNewTile ();
	}
	
	// Update is called once per frame
	void Update () {
		
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
				CreateNewTile ();
			}
		}
	}

	void MoveTileDown(GameObject obj){
		obj.transform.Translate (0, -0.2f, 0);
	}
		
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

}
