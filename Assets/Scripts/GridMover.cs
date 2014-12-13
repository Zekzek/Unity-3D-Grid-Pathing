using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMover : MonoBehaviour
{
	public Movement movementSpeed;

	private List<GridTerrainConnection> moveList;

	private GridTerrain standingOn;
	private GridTerrain target;

	public GUIText debug;

	// Use this for initialization
	void Start ()
	{
		movementSpeed = gameObject.GetComponent("Movement") as Movement;
		//movementSpeed = Movement.BasicMover(gameObject);
		moveList = new List<GridTerrainConnection> ();
	}
	
	void FixedUpdate() {
		if (moveList.Count > 0) {
			if (CloseTo (moveList [0].GetPhysicsPathDestination ())) {
				moveList [0].MarkPhysicsStepComplete ();
				if (moveList [0].IsPhysicsPathEmpty ())
					moveList.RemoveAt (0);
			}
			else {
				Vector3 direction = moveList [0].GetPhysicsPathDestination () - rigidbody.position;
				direction.Normalize ();
				rigidbody.velocity = direction * moveList [0].GetTravelSpeed (this) / 3;
			}
		} else {
			rigidbody.velocity = Vector3.zero;
		}
	}
	
	bool CloseTo(Vector3 pos) {
		return (rigidbody.position - pos).sqrMagnitude < 0.01f;
	}

	// Update is called once per frame
	void Update ()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			
			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				Debug.Log( "clicked: " + hit.transform.gameObject.name );
				//if (hit.transform.gameObject)
				GridTerrain terrain = hit.transform.gameObject.GetComponent<GridTerrain>() as GridTerrain;
				if (terrain != null) {
					target = terrain;
					standingOn = GetStandingOn();
					moveList = PathToTerrain(target);
				}
			}
		}
	}

	public List<GridTerrainConnection> PathToTerrain(GridTerrain pos) {
		List<GridTerrainConnection> connected = standingOn.Connections;
		PathResult bestPath = new PathResult();
		bestPath.score = float.MaxValue;
		for (int i = 0; i < connected.Count; i++) {

			if (connected[i].HasMirror()) {
				connected[i].DebugSpeed();
				PathResult path = PathBetween (connected[i], pos, new List<GridTerrain>(), 0);
				if (path.score < bestPath.score) {
					bestPath = path;
				}
			}
		}
		return bestPath.path;
	}

	PathResult PathBetween(GridTerrainConnection from, GridTerrain to, List<GridTerrain> visited, int count) {
		count++;
		if (from.Mirror.OwnerTerrain == to) {
			PathResult path = new PathResult();
			path.path.Add (from.Mirror);
			path.score = 1 / from.GetTravelSpeed(this);
			return path;
		} 
		else {
			visited.Add (from.OwnerTerrain);
			List<GridTerrainConnection> connected = from.Mirror.OwnerTerrain.Connections;
			PathResult bestPath = new PathResult();
			bestPath.score = float.MaxValue;
			for (int i = 0; i < connected.Count; i++) {
				if (connected[i].HasMirror() && !visited.Contains(connected[i].Mirror.OwnerTerrain) && count < 20) {
					PathResult path = PathBetween (connected[i], to, visited, count);
					if (path.score < bestPath.score) {
						bestPath = path;
					}
				}
			}
			if (bestPath != null) {
				bestPath.path.Insert(0, from.Mirror);
				bestPath.score += 1 / from.GetTravelSpeed(this);
			}
			return bestPath;
		}
	}

	private class PathResult {
		public float score;
		public List<GridTerrainConnection> path = new List<GridTerrainConnection>();
	}

	public void TeleportToGrid (Vector3 pos) {
		rigidbody.position = ConvertToGrid (pos);
	}

	public static Vector3 ConvertToGrid (Vector3 pos) {
		return new Vector3(
			Mathf.RoundToInt(pos.x),
			Mathf.RoundToInt(pos.y),
			Mathf.RoundToInt(pos.z));
	}

	public GridTerrain GetStandingOn() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit))
			return hit.transform.gameObject.GetComponent<GridTerrain> () as GridTerrain;
		else
			return null;
	}
}