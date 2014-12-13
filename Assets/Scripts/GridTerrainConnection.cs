using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridTerrainConnection : MonoBehaviour
{
	public Movement MoveTypes { get; set; } 
	public Vector3 Position { get; set; }
	public GridTerrain OwnerTerrain { get; set; }
	public GridTerrainConnection Mirror { get; set; } 

	private List<Vector3> physicsPath;

	void Start() {
		physicsPath = new List<Vector3> ();
	}

	public bool TryLink(GridTerrainConnection other) {
		if (CompatibleWith (other)) {
			Debug.Log("Linking " + OwnerTerrain.ToString() + " and " + other.OwnerTerrain.ToString());
			Mirror = other;
			other.Mirror = this;
			DebugSpeed ();
			return true;
		}
		return false;
	}

	public void RemoveImpossibleLink(GridTerrainConnection other) {
		if (Mirror == null || other.Mirror == null)
			return;
		if (Position.x == other.Position.x &&
				Position.z == other.Position.z) {
			if (other.Position.y > Position.y) {
				Debug.Log("removing " + this);
				Mirror.Mirror = null;
				Mirror = null;
			}
			else {
				Debug.Log("removing " + other);
				other.Mirror.Mirror = null;
				other.Mirror = null;
			}
		} 
	}

	private bool CompatibleWith(GridTerrainConnection other) {
		return (other.Position - Position).sqrMagnitude < 0.1f;
	}

	public bool HasMirror() {
		return Mirror != null;
	}

	public Vector3 GetPhysicsPathDestination() {
		if (physicsPath.Count == 0) {
			if (OwnerTerrain.transform.name == "BlockTile(Clone)" 
			    	|| Mirror.OwnerTerrain.transform.name == "BlockTile(Clone)") {
				Vector3 blockMidPos = Mirror.OwnerTerrain.GetPos();
				if(blockMidPos.y < OwnerTerrain.GetPos().y) {
					blockMidPos.y += 1.0f;
					physicsPath.Add (blockMidPos);
				}
			}
			physicsPath.Add (GetTerrainPos());
		}
		return physicsPath [0];
	}

	public bool IsPhysicsPathEmpty() {
		return physicsPath.Count == 0;
	}

	public void MarkPhysicsStepComplete() {
		physicsPath.RemoveAt(0);
	}

	public Vector3 GetTerrainPos() {
		Vector3 terrainPos = OwnerTerrain.GetPos ();
		return terrainPos;
	}

	public void DebugSpeed() {
		string msg = "scale: " + MoveTypes.GetMovementSpeed ("scale")
				+ "  walk: " + MoveTypes.GetMovementSpeed ("walk")
				+ "  ramp: " + MoveTypes.GetMovementSpeed ("ramp");
		if (Mirror != null)
        	msg += "\t\tMirror ~ "
	        		+ "scale: " + Mirror.MoveTypes.GetMovementSpeed ("scale")
					+ "  walk: " + Mirror.MoveTypes.GetMovementSpeed ("walk")
					+ "  ramp: " + Mirror.MoveTypes.GetMovementSpeed ("ramp");
		Debug.Log (msg);
	}

	public float GetTravelSpeed(GridMover mover) {
		DebugSpeed ();
		float speed = MoveTypes.GetBestSpeed (mover.movementSpeed);
		float otherSpeed = Mirror.MoveTypes.GetBestSpeed (mover.movementSpeed);
		if (speed < otherSpeed)
			return speed;
		else
			return otherSpeed;
	}
}