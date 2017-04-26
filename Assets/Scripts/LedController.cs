using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arduino;
using UnityEngine.Rendering;

public class LedController
{
	private ArduinoPort port;

	private Animations currentAnimation;

	private Color32 waveAnimationColor;
	private int step;

	public enum Animations
	{
		FullColorWave
	}

	public LedController(ArduinoPort port)
	{
		this.port = port;
		waveAnimationColor = Color.red;
	}

	public void SetCurrentAnimation(Animations anim)
	{
		currentAnimation = anim;
		step = 0;

		if (currentAnimation == Animations.FullColorWave)
		{
			waveAnimationColor = Color.red;
		}
	}

	public void Update()
	{
		switch (currentAnimation)
		{
			case Animations.FullColorWave:
#region change color
				switch (step)
				{
					case 0:
						if (waveAnimationColor.r > Time.deltaTime*50)
						{
							waveAnimationColor.r -= (byte) (Time.deltaTime*50);
							waveAnimationColor.g += (byte) (Time.deltaTime*50);

						}
						else
						{
							waveAnimationColor.r = 0;
							waveAnimationColor.g = 255;
							step++;
						}
						break;
					case 1:
						if (waveAnimationColor.g > Time.deltaTime * 50)
						{
							waveAnimationColor.g -= (byte)(Time.deltaTime * 50);
							waveAnimationColor.b += (byte)(Time.deltaTime * 50);

						}
						else
						{
							waveAnimationColor.g = 0;
							waveAnimationColor.b = 255;
							step++;
						}
					break;
					case 2:
						if (waveAnimationColor.b > Time.deltaTime * 50)
						{
							waveAnimationColor.b -= (byte)(Time.deltaTime * 50);
							waveAnimationColor.r += (byte)(Time.deltaTime * 50);

						}
						else
						{
							waveAnimationColor.b = 0;
							waveAnimationColor.r = 255;
							step = 0;
						}
					break;

				}
#endregion
				port.LEDFillColor(waveAnimationColor);
				port.LEDShow();
				break;
		}
	}
	

}
