using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
	private MovementSpeed[] movementSpeeds;

	// Use this for initialization
	void Start ()
	{
		movementSpeeds = gameObject.GetComponents<MovementSpeed> ();
	}

	public static Movement Generate(GameObject gameObject, string[] moveTypes) {
		for (int i = 0; i < moveTypes.Length; i++) {
			MovementSpeed speed = gameObject.AddComponent ("MovementSpeed") as MovementSpeed;
			speed.movementType = moveTypes[i];
			speed.movementSpeed = 1;
		}
		Movement movement = gameObject.AddComponent ("Movement") as Movement;
		return movement;
	}

	public static Movement GenerateConnection(GameObject gameObject, string[] moveTypes) {
		GameObject connection = new GameObject ();

		for (int i = 0; i < moveTypes.Length; i++) {
			MovementSpeed speed = connection.AddComponent ("MovementSpeed") as MovementSpeed;
			speed.movementType = moveTypes[i];
			speed.movementSpeed = 1;
		}
		connection.transform.parent = gameObject.transform;
		Movement movement = connection.AddComponent ("Movement") as Movement;
		return movement;
	}

	
	public float GetMovementSpeed(string movementType) {
		if (movementSpeeds != null)
			for (int i = 0; i < movementSpeeds.Length; i++)
				if (movementSpeeds [i].movementType == movementType)
					return movementSpeeds [i].movementSpeed;
		return 0;
	}

	public float GetBestSpeed(Movement mover) {
		Speed bestSpeed = new Speed ("none", 0);
		for (int i = 0; i < movementSpeeds.Length; i++) {
			string movementType = movementSpeeds[i].movementType;
			float speed = GetMovementSpeed(movementType) * mover.GetMovementSpeed(movementType);
			if (speed > bestSpeed.speed)
				bestSpeed.speed = speed;
		}
		return bestSpeed.speed;
	}

	public class Speed {
		public float speed;
		public string movementType;

		public Speed(string movementType, float speed) {
			this.movementType = movementType;
			this.speed = speed;
		}
	}
}