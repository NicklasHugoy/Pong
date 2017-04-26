using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
	private float speed = 10;
	private bool waitForServe;

	public Text Player1Text, Player2Text;

	private int player1Score, player2Score;
	private GameObject leftPlayer, rightPlayer;
	private bool button1, button2;


	void Start()
	{
		leftPlayer = GameObject.Find("LeftPlayer");
		rightPlayer = GameObject.Find("RightPlayer");

		transform.position = Vector2.zero;
		GetComponent<Rigidbody2D>().velocity = Vector2.right*speed;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		// Left player
		CheckPlayerCollision(col, "LeftPlayer", 1);

		// Right player
		CheckPlayerCollision(col, "RightPlayer", -1);

		if (col.gameObject.tag == "SideWall")
			CheckForGoal(col);
	}

	private void CheckPlayerCollision(Collision2D col, string player, int xDirection)
	{
		if (col.gameObject.name == player)
		{

			if (player == "LeftPlayer")
			{
				GameManager.port.LEDFillColor(Color.blue);
				GameManager.port.LEDShow();
			}else if (player == "RightPlayer")
			{
				GameManager.port.LEDFillColor(Color.red);
				GameManager.port.LEDShow();
			}

			speed += 0.2f;
			float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);

			Vector2 dir = new Vector2(xDirection, y).normalized;

			GetComponent<Rigidbody2D>().velocity = dir*speed;
		}
	}

	private void CheckForGoal(Collision2D col)
	{
		// Left Wall
		if (col.gameObject.name == "LeftWall")
		{
			GameManager.port.LEDFillColor(Color.blue);
			GameManager.port.LEDShow();

			player2Score++;
			Player2Text.text = player2Score.ToString();
			speed = 10 + player1Score + player2Score;
			GameManager.port.WriteSevenSeg(9, player2Score);
		}

		// Right Wall
		if (col.gameObject.name == "RightWall")
		{
			GameManager.port.LEDFillColor(Color.red);
			GameManager.port.LEDShow();

			player1Score++;
			Player1Text.text = player1Score.ToString();
			speed = 10 + player1Score + player2Score;
			GameManager.port.WriteSevenSeg(2, player1Score);
		}

		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		waitForServe = true;
	}

	float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
	{
		return (ballPos.y - racketPos.y)/racketHeight;
	}


	void Update()
	{
		if (waitForServe)
		{
			if (gameObject.transform.position.x > 0)
			{
				ReadyToServe(rightPlayer, Vector2.left);
			}
			else
			{
				ReadyToServe(leftPlayer, Vector2.right);
			}
			button1 = false;
			button2 = false;
		}
	}

	private void ReadyToServe(GameObject player, Vector2 direction)
	{
		transform.position = new Vector2(player.transform.position.x + direction.x, player.transform.position.y);

		if (player == leftPlayer && button1)
		{
			GetComponent<Rigidbody2D>().velocity = direction * speed;
			waitForServe = false;
		}

		if (player == rightPlayer && button2)
		{
			GetComponent<Rigidbody2D>().velocity = direction * speed;
			waitForServe = false;
		}
	}

	public void OnButton1()
	{
		button1 = true;
	}

	public void OnButton2()
	{
		button2 = true;
	}
}
