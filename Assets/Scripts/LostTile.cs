using UnityEngine;
using System.Collections;

public class LostTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.y < -20)
			Destroy (gameObject);
	}
}
