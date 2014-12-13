using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridTerrain : MonoBehaviour
{
	public float topOfBlock;
	public List<GridTerrainConnection> Connections { get; set; }

	public Vector3 GetPos() {
		Vector3 position = rigidbody.position;
		position.y += topOfBlock + 0.5f;
		return position;
	}

	public void AddConnectionPoint(Vector3 point, string[] moveTypes) {
		GameObject connectionObject = new GameObject ();
		connectionObject.transform.parent = transform;
		Movement movement = Movement.Generate(connectionObject, moveTypes);

		if (Connections == null)
			Connections = new List<GridTerrainConnection> ();
		GridTerrainConnection connection 
			= gameObject.AddComponent("GridTerrainConnection") as GridTerrainConnection;
		connection.MoveTypes = movement;
		connection.Position = point;
		connection.OwnerTerrain = this;

		Connections.Add (connection);
	}

	public void TryConnect(GridTerrain other) {
		if (other == null)
			return;
		else {
			bool connected = false;
			for (int i = 0; i < other.Connections.Count; i++)
				for (int j = 0; j < Connections.Count; j++)
					if(other.Connections[i].TryLink(Connections[j]))
						connected = true;
			if (connected) {
				for (int i = 0; i < Connections.Count; i++)
					for (int j = i + 1; j < Connections.Count; j++)
						Connections[i].RemoveImpossibleLink(Connections[j]);
				for (int i = 0; i < other.Connections.Count; i++)
					for (int j = i + 1; j < other.Connections.Count; j++)
						other.Connections[i].RemoveImpossibleLink(other.Connections[j]);
			}
		}
	}
	
	public override string ToString() {
		return base.ToString() + "@ (" + rigidbody.position.x + ", " 
			+ rigidbody.position.y + ", " + rigidbody.position.z + ")";
	}
}