using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{

	public Text Player1Text, Player2Text;

	private float speed = 10;

	private int player1Score, player2Score;
	
	void Start ()
	{
		Serve();

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		// Left player
		if (col.gameObject.name == "LeftPlayer")
		{
			float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);

			Vector2 dir = new Vector2(1, y).normalized;

			GetComponent<Rigidbody2D>().velocity = dir * speed;

		}

		// Right player
		if (col.gameObject.name == "RightPlayer")
		{
			float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);

			Vector2 dir = new Vector2(-1, y).normalized;

			GetComponent<Rigidbody2D>().velocity = dir * speed;
		}

		if(col.gameObject.tag == "SideWall")
		CheckForGoal(col);

	}

	private void CheckForGoal(Collision2D col)
	{
		// Left Wall
		if (col.gameObject.name == "LeftWall")
		{
			player2Score++;
			Player2Text.text = player2Score.ToString();
			Serve();
		}

		// Right Wall
		if (col.gameObject.name == "RightWall")
		{
			player1Score++;
			Player1Text.text = player1Score.ToString();
			Serve();
		}
	}

	float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
	{
		return (ballPos.y - racketPos.y)/racketHeight;
	}

	void Serve()
	{
		transform.position = Vector2.zero;
		GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
	}

}
