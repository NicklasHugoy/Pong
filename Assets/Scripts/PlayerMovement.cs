using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed = 0.2f;

	[SerializeField]
	private int pin;

	// Update is called once per frame
	void Update ()
	{

		transform.position =  new Vector2(transform.position.x, Mathf.Lerp(5,-5, GameManager.port.GetAnalogPin(pin)/1023f));
	}
}
