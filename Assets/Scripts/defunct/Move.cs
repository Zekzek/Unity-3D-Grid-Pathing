using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour {

	public float maxSpeed;
	public float movePower;
	public float jumpPower;
	
	private float sqrMaxSpeed;
	private float jumpDelay = 0.1f;
	private float lastJump;
	private float distToGround;

	private List<Vector3> goToList;
	private Vector3 lastGoTo;
	private float sqrCloseDist = 0.2f;

	void Start() {
		distToGround = collider.bounds.extents.y;
		sqrMaxSpeed = maxSpeed * maxSpeed;

		goToList = new List<Vector3> ();
	}

	void FixedUpdate() {
		if (goToList.Count > 0) {
			MoveInDirection (goToList[0] - rigidbody.position);
			if (CloseTo (goToList[0])) {
				lastGoTo = goToList[0];
				goToList.RemoveAt(0);
			}
		}
		else
			FineTuneTo (lastGoTo);
	}

	bool CloseTo(Vector3 other) {
		return (other - rigidbody.position).sqrMagnitude < sqrCloseDist;
	}

	public void MoveInDirection(Vector3 moveDirection) {
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize ();
		float maxSpeedMod = (sqrMaxSpeed - rigidbody.velocity.sqrMagnitude) / sqrMaxSpeed;
		rigidbody.AddForce(movePower * moveDirection * maxSpeedMod);
		Debug.Log ("MoveInDirection(" + moveDirection.x + ", " + moveDirection.y + ", " + moveDirection.z + ")");
	}

	public void FineTuneTo(Vector3 moveDirection) {
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize ();
		rigidbody.AddForce(movePower/2 * moveDirection);
		Debug.Log ("FineTuneTo(" + moveDirection.x + ", " + moveDirection.y + ", " + moveDirection.z + ")");
	}

	public void MoveTo(float horizontal, float vertical) {
		MoveInDirection (new Vector3 (horizontal, 0, vertical));
	}

	public void MoveTo(List<Vector3> posList) {
		goToList = posList;
	}

	public void stop() {
		rigidbody.AddForce (-rigidbody.velocity);
	}

	public void Jump() {
		if (Time.time - lastJump > jumpDelay && IsGrounded ()) {
			rigidbody.AddForce(Vector3.up * jumpPower);
			lastJump = Time.time;
		}
	}
	
	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	bool IsJump() {

		return rigidbody.velocity.sqrMagnitude > 0 &&
			Physics.Raycast(transform.position, rigidbody.velocity, rigidbody.velocity.magnitude/2);
	}
}
