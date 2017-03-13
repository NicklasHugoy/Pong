using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arduino;

public class GameManager : MonoBehaviour
{

	public static ArduinoPort port;

	void Awake()
	{
		port = new ArduinoPort();
	}

}
