using UnityEngine;
using System.Collections;

public class MoveWithKeyboard : MonoBehaviour {

	private Move move;

	void Start() {
		move = gameObject.GetComponent("Move") as Move;
		if (move == null)
			Debug.LogError ("Failed to load 'Move' in 'MoveWithKeyboard'");
	}

	void FixedUpdate() {
		if (Input.GetButtonDown ("Jump"))
			move.Jump ();

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		move.MoveTo (horizontal, vertical);
	}
}