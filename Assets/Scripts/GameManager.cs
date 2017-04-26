using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arduino;
using UnityEngine.Events;

[Serializable]
public class ExposedEvent : UnityEvent
{
	
}

public class GameManager : MonoBehaviour
{

	public static ArduinoPort port;

	[SerializeField]
	public ExposedEvent button1;
	[SerializeField]
	public ExposedEvent button2;
	void Awake()
	{
		port = new ArduinoPort();

		for (int i = 2; i <= 16; i++)
		{
			port.PinMode(i, PinModes.OUPUT);
		}

		port.WriteSevenSeg(2, 0);
		port.WriteSevenSeg(9, 0);

		for (int i = 0; i < 9; i += 2)
		{
			port.LEDSetColor(i, Color.red);
			port.LEDSetColor(i + 1, Color.blue);
		}
		port.LEDShow();
	}

	void FixedUpdate()
	{

		//Button Input
		short buttonValue = port.GetAnalogPin(17);

		if (buttonValue > 390 && buttonValue < 452)
		{
			button1.Invoke();
			button2.Invoke();
		}
		else if(buttonValue > 670 && buttonValue < 730)
		{
			button1.Invoke();
		}
		else if(buttonValue > 490 && buttonValue < 550)
		{
			button2.Invoke();
		}
		Debug.Log(buttonValue);
	}

	void OnApplicationQuit()
	{
		port.Dispose();
	}

}
