using UnityEngine;
using System.Collections;

public class MoveGrid : MonoBehaviour {

	public GUIText text;

	public float speed;

	//private float goToTimer;
	//private float goToDuration;
	//private Vector3 startPos;
	//private Vector3 goToPos;

	void Start() {
		//GoTo (new Vector3 (10f, 0f, 10f));
	}

	// Update is called once per frame
	void Update () {
//		if (goToTimer < goToDuration) {
//			Vector3 temp = rigidbody.position;
			//temp.x = Mathf.Lerp(startPos.x, goToPos.x, goToTimer/goToDuration);
//			temp.y = Mathf.Lerp(startPos.y, goToPos.y, goToTimer/goToDuration);
//			temp.z = Mathf.Lerp(startPos.z, goToPos.z, goToTimer/goToDuration);
//			rigidbody.position = temp;
//			goToTimer += Time.deltaTime;
//		}
//		Vector3 gridPos = GetGridPos ();
//		text.text = "(" + rigidbody.position.x + ", "  + rigidbody.position.y + ", "  + rigidbody.position.z + ")";	
	}

	public void GoTo(Vector3 gridPos) {
	//	startPos = rigidbody.position;
	//	goToTimer = 0f;

	//	goToPos = gridPos;
	//	goToDuration = (startPos - goToPos).magnitude / speed;
	}

	public Vector3 GetGridPos() {
		return new Vector3(
				Mathf.RoundToInt(rigidbody.position.x),
				Mathf.RoundToInt(rigidbody.position.y),
				Mathf.RoundToInt(rigidbody.position.z));
	}
}
