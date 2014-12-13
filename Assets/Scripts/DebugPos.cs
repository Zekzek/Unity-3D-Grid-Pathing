using UnityEngine;
using System.Collections;

public class DebugPos : MonoBehaviour {

	public GUIText text;

	// Update is called once per frame
	void Update () {
		text.text = "(" + rigidbody.position.x + ", " + rigidbody.position.y + ", " + rigidbody.position.z + ")\n"
						+ "(" + GetGridPos ().x + ", " + GetGridPos ().y + ", " + GetGridPos ().z + ")\n"
						+ "(" + rigidbody.velocity.x + ", " + rigidbody.velocity.y + ", " + rigidbody.velocity.z + ")";
	}

	public Vector3 GetGridPos() {
		return new Vector3(
			Mathf.RoundToInt(rigidbody.position.x),
			Mathf.RoundToInt(rigidbody.position.y),
			Mathf.RoundToInt(rigidbody.position.z));
	}
}
