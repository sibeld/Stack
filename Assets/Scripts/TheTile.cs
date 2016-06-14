using UnityEngine;
using System.Collections;

public class TheTile : MonoBehaviour {

	public enum movement_axis{
		x_axis,
		y_axis,
		z_axis,
		none
	}

	public GameObject lostTile;
	public movement_axis move_axis = movement_axis.none;
	//int tileLocation;
	float stepLength;
	float distance;
	float maxDistance;
	//int locationMax;
	//int locationMin;
	bool moveForward;

	// Use this for initialization
	void Start () {

		maxDistance = 5.0f;
		//locationMax = 60;
		//locationMin = 0 - locationMax;

		distance = maxDistance;
		if(move_axis == movement_axis.x_axis)
			transform.Translate (distance, 0, 0);
		else if(move_axis == movement_axis.z_axis)
			transform.Translate (0, 0, distance);

		//tileLocation = locationMax;
		moveForward = false;
	}
	
	// Update is called once per frame
	void Update () {

		stepLength = 6.0f * Time.deltaTime;
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown("space")){

			if (Mathf.Abs(distance) <= 0.2f)
				Congratulate ();
			else
				ScaleTile();

			move_axis = movement_axis.none;
			Destroy (this);
		}

		if(move_axis == movement_axis.x_axis)
			MoveX();
		else if (move_axis == movement_axis.z_axis)
			MoveZ();
	}

	void MoveX(){
		if (moveForward) {
			if (distance < maxDistance) {
				transform.Translate (stepLength, 0, 0);
				distance = distance + stepLength;
			} else {
				moveForward = false;
			}
		} else {
			if (distance > -maxDistance) {
				transform.Translate (-stepLength, 0, 0);
				distance = distance - stepLength;
			} else {
				moveForward = true;
			}
		}
	} 

	void MoveZ(){
		if (moveForward) {
			if (distance < maxDistance) {
				transform.Translate (0, 0, stepLength);
				distance = distance + stepLength;
			} else {
				moveForward = false;
			}
		} else {
			if (distance > -maxDistance) {
				transform.Translate (0, 0, -stepLength);
				distance = distance - stepLength;
			} else {
				moveForward = true;
			}
		}
	}

	void MoveY(){
		transform.Translate (0, -1, 0);
	}

	void ScaleTile(){

		float lostLenght = Mathf.Abs(distance);

		Renderer lostRend;
		Renderer tileRend;

		tileRend = GetComponent<Renderer> ();

		if (move_axis == movement_axis.x_axis) {

			if (transform.localScale.x < lostLenght) {
				gameObject.AddComponent<Rigidbody>();
				Application.LoadLevel ("Game");
				return;
			}

			//...........................................................LOST TILES
			//.....create
			GameObject _lostTile = Instantiate (lostTile);
			_lostTile.transform.localScale = new Vector3 (lostLenght, transform.localScale.y, transform.localScale.z);

			if (distance > 0) {
				_lostTile.transform.localPosition = new Vector3 (transform.localPosition.x + (transform.localScale.x - lostLenght) / 2, transform.localPosition.y, transform.localPosition.z);
				_lostTile.GetComponent<Rigidbody> ().AddRelativeTorque (0,0,-100000);
			} else {
				_lostTile.transform.localPosition = new Vector3 (transform.localPosition.x - (transform.localScale.x-lostLenght)/2, transform.localPosition.y, transform.localPosition.z);
				_lostTile.GetComponent<Rigidbody> ().AddRelativeTorque (0,0,100000);
			}
			//........set color
			lostRend = _lostTile.GetComponent<Renderer>();
			lostRend.material.SetColor ("_Color", tileRend.material.GetColor("_Color"));

			transform.localScale -= new Vector3 (lostLenght, 0, 0);
			if(distance > 0)
				transform.Translate ( -(lostLenght/2), 0, 0);
			else
				transform.Translate ( lostLenght/2, 0, 0);
		
		} else if (move_axis == movement_axis.z_axis) {

			if (transform.localScale.z < lostLenght) {
				gameObject.AddComponent<Rigidbody>();
				Application.LoadLevel ("Game");
				return;
			}
		
			//...........................................................LOST TILES
			//.....create
			GameObject _lostTile = Instantiate (lostTile);
			_lostTile.transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, lostLenght);

			if (distance > 0) {
				_lostTile.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + (transform.localScale.z - lostLenght) / 2);
				_lostTile.GetComponent<Rigidbody> ().AddRelativeTorque (100000,0,0);
			} else {
				_lostTile.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - (transform.localScale.z - lostLenght) / 2);
				_lostTile.GetComponent<Rigidbody> ().AddRelativeTorque (-100000,0,0);
			}
			//......set color
			lostRend = _lostTile.GetComponent<Renderer>();
			lostRend.material.SetColor ("_Color", tileRend.material.GetColor("_Color"));

			transform.localScale -= new Vector3 (0, 0, lostLenght);
			if(distance > 0)
				transform.Translate (0, 0, -(lostLenght/2));
			else
				transform.Translate (0, 0, lostLenght/2);
		}

	}

	void Congratulate(){
		if(move_axis == movement_axis.x_axis)
			gameObject.transform.Translate (-distance, 0, 0);
		if(move_axis == movement_axis.z_axis)
			gameObject.transform.Translate (0, 0, -distance);
		//make gloving and sound
	}
}
