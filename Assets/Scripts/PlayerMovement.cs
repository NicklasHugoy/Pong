using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed = 0.2f;

	[SerializeField]
	private int pin;

	public bool isInverted;

	// Update is called once per frame
	void Update ()
	{
		if (isInverted)
		{
			transform.position = new Vector2(transform.position.x, Mathf.Lerp(-5, 5, GameManager.port.GetAnalogPin(pin) / 1023f));
		}
		else
		{
			transform.position = new Vector2(transform.position.x, Mathf.Lerp(5, -5, GameManager.port.GetAnalogPin(pin) / 1023f));
		}
		
	}
}
