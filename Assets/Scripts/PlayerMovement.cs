using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed = 0.2f;
	private float input = 0.5f;

	// Update is called once per frame
	void Update ()
	{

		transform.position =  new Vector2(transform.position.x, Mathf.Lerp(5,-5, Input.GetAxis("Vertical")));

		Debug.Log(Input.GetAxis("Vertical"));


	}
}
