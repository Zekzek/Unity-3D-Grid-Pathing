using UnityEngine;
using System.Collections;

public class MoveToFollow : MonoBehaviour {

	public Rigidbody toFollow;

	private Move move;
	private float deadzone = 0.5f;

	void Start() {
		move = gameObject.GetComponent("Move") as Move;
		if (move == null)
			Debug.LogError ("Failed to load 'Move' in 'MoveToFollow'");
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 desiredMove = new Vector2(
			(toFollow.position.x - rigidbody.position.x)/10,
			(toFollow.position.z - rigidbody.position.z)/10);


		if(desiredMove.magnitude < deadzone)
			desiredMove = Vector2.zero;
		else
			desiredMove = desiredMove.normalized * (desiredMove.magnitude - deadzone);

		move.MoveTo (desiredMove.x, desiredMove.y);
	}
}
