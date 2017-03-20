using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
	private float speed = 10;

	public Text Player1Text, Player2Text;
	private int player1Score, player2Score;
	private bool waitForServe;

	public GameObject leftPlayer, rightPlayer;

	
	void Start ()
	{
		transform.position = Vector2.zero;
		GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;

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
			float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);

			Vector2 dir = new Vector2(xDirection, y).normalized;

			GetComponent<Rigidbody2D>().velocity = dir * speed;
		}
	}

	private void CheckForGoal(Collision2D col)
	{
		if(col.gameObject.tag == "SideWall")
		{
			// Left Wall
			if (col.gameObject.name == "LeftWall")
			{
				player2Score++;
				Player2Text.text = player2Score.ToString();
			}

			// Right Wall
			if (col.gameObject.name == "RightWall")
			{
				player1Score++;
				Player1Text.text = player1Score.ToString();
			}

			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			waitForServe = true;
		}

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
		}
	}

	private void ReadyToServe(GameObject player, Vector2 direction)
	{
		transform.position = new Vector2(player.transform.position.x + direction.x, rightPlayer.transform.position.y);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GetComponent<Rigidbody2D>().velocity = direction * speed;
			waitForServe = false;
		}
	}
}
