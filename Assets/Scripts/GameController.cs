using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Transform FloorTile;
	public Transform RampTile;
	public Transform BlockTile;

	private static int MAX_X = 100;
	private static int MAX_Y = 20;
	private static int MAX_Z = 100;

	private enum Facing {left, right, forward, back};

	private GridTerrain[,,] terrainMap = new GridTerrain[MAX_X, MAX_Y, MAX_Z];

	void Start () {
		AddBlockTile (-1, 0, 0);
		AddBlockTile (-1, 0, 1);
		AddRampTile (-1, 0, 2, Facing.back);
		AddFloorTile (-1, 0, 3);

		AddFloorTile (0, 0, 0);
		AddFloorTile (0, 0, 1);
		AddFloorTile (0, 0, 2);
		AddFloorTile (0, 0, 3);

		AddFloorTile (1, 0, 1);
		AddRampTile (1, 0, 3, Facing.right);

		AddBlockTile (2, 0, 0);
		AddFloorTile (2, 0, 1);
		AddFloorTile (2, 1, 1);
		AddBlockTile (2, 0, 2);
		AddBlockTile (2, 0, 3);

		AddRampTile (3, 0, 0, Facing.left);
		AddFloorTile (3, 0, 1);

		AddFloorTile (4, 0, 0);
		AddFloorTile (4, 0, 1);
	}

	void AddFloorTile(int x, int y, int z) {
		int xIndex = x + 50;
		int yIndex = y + 10;
		int zIndex = z + 50;

		Transform tile = (Transform) Instantiate(FloorTile, new Vector3(x, y, z), Quaternion.identity);
		tile.position = new Vector3 (x, y - 0.5f, z);

		GridTerrain script = tile.GetComponent<GridTerrain>() as GridTerrain;
		terrainMap [xIndex, yIndex, zIndex] = script;
		script.AddConnectionPoint (new Vector3 (x - 0.5f, y - 0.5f, z), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x + 0.5f, y - 0.5f, z), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z - 0.5f), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z + 0.5f), new string[] {"walk"});

		List<GridTerrain> near = GetAdjacentTerrain(xIndex, yIndex, zIndex);
		for (int i = 0; i < near.Count; i++)
			script.TryConnect(near[i]);		
	}

	void AddRampTile(int x, int y, int z, Facing direction) {
		int xIndex = x + 50;
		int yIndex = y + 10;
		int zIndex = z + 50;
		
		Transform tile = (Transform) Instantiate(RampTile, new Vector3(x, y, z), Quaternion.identity);
		tile.position = new Vector3 (x, y, z);

		GridTerrain script = tile.GetComponent<GridTerrain>() as GridTerrain;
		terrainMap [xIndex, yIndex, zIndex] = script;
		if (direction == Facing.right) {
			tile.RotateAround(tile.position, Vector3.forward, 45);
			script.AddConnectionPoint (new Vector3 (x - 0.5f, y - 0.5f, z), new string[] {"ramp"});
			script.AddConnectionPoint (new Vector3 (x + 0.5f, y + 0.5f, z), new string[] {"ramp"});
		}
		if (direction == Facing.left) {
			tile.RotateAround(tile.position, Vector3.forward, -45);
			script.AddConnectionPoint (new Vector3 (x + 0.5f, y - 0.5f, z), new string[] {"ramp"});
			script.AddConnectionPoint (new Vector3 (x - 0.5f, y + 0.5f, z), new string[] {"ramp"});
		}
		if (direction == Facing.forward) {
			tile.RotateAround(tile.position, Vector3.forward, 45);
			tile.RotateAround(tile.position, Vector3.up, -90);
			script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z - 0.5f), new string[] {"ramp"});
			script.AddConnectionPoint (new Vector3 (x, y + 0.5f, z + 0.5f), new string[] {"ramp"});
		}
		if (direction == Facing.back) {
			tile.RotateAround(tile.position, Vector3.forward, 45);
			tile.RotateAround(tile.position, Vector3.up, 90);
			script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z + 0.5f), new string[] {"ramp"});
			script.AddConnectionPoint (new Vector3 (x, y + 0.5f, z - 0.5f), new string[] {"ramp"});
		}
		List<GridTerrain> near = GetAdjacentTerrain(xIndex, yIndex, zIndex);
		for (int i = 0; i < near.Count; i++)
			script.TryConnect(near[i]);		
	}

	void AddBlockTile(int x, int y, int z) {
		int xIndex = x + 50;
		int yIndex = y + 10;
		int zIndex = z + 50;
		
		Transform tile = (Transform) Instantiate(BlockTile, new Vector3(x, y, z), Quaternion.identity);
		tile.position = new Vector3 (x, y, z);
		
		GridTerrain script = tile.GetComponent<GridTerrain>() as GridTerrain;
		terrainMap [xIndex, yIndex, zIndex] = script;
		script.AddConnectionPoint (new Vector3 (x - 0.5f, y + 0.5f, z), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x + 0.5f, y + 0.5f, z), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x, y + 0.5f, z - 0.5f), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x, y + 0.5f, z + 0.5f), new string[] {"walk"});
		script.AddConnectionPoint (new Vector3 (x - 0.5f, y - 0.5f, z), new string[] {"scale"});
		script.AddConnectionPoint (new Vector3 (x + 0.5f, y - 0.5f, z), new string[] {"scale"});
		script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z - 0.5f), new string[] {"scale"});
		script.AddConnectionPoint (new Vector3 (x, y - 0.5f, z + 0.5f), new string[] {"scale"});

		List<GridTerrain> near = GetAdjacentTerrain(xIndex, yIndex, zIndex);
		for (int i = 0; i < near.Count; i++)
			script.TryConnect(near[i]);
	}
	
	List<GridTerrain> GetAdjacentTerrain(int xIndex, int yIndex, int zIndex) {
		List<GridTerrain> near = new List<GridTerrain> ();
		if (yIndex > 0) {
			if (xIndex < MAX_X && terrainMap [xIndex + 1, yIndex - 1, zIndex] != null)
				near.Add (terrainMap [xIndex + 1, yIndex - 1, zIndex]);
			if (xIndex > 0 && terrainMap [xIndex - 1, yIndex - 1, zIndex] != null)
				near.Add (terrainMap [xIndex - 1, yIndex - 1, zIndex]);
			if (zIndex < MAX_Z && terrainMap [xIndex, yIndex - 1, zIndex + 1] != null)
				near.Add (terrainMap [xIndex, yIndex - 1, zIndex + 1]);
			if (zIndex > 0 && terrainMap [xIndex, yIndex - 1, zIndex - 1] != null)
				near.Add (terrainMap [xIndex, yIndex - 1, zIndex - 1]);
		}

		if (xIndex < MAX_X && terrainMap [xIndex + 1, yIndex, zIndex] != null)
			near.Add (terrainMap [xIndex + 1, yIndex, zIndex]);
		if (xIndex > 0 && terrainMap [xIndex - 1, yIndex, zIndex] != null)
			near.Add (terrainMap [xIndex - 1, yIndex, zIndex]);
		if (zIndex < MAX_Z && terrainMap [xIndex, yIndex, zIndex + 1] != null)
			near.Add (terrainMap [xIndex, yIndex, zIndex + 1]);
		if (zIndex > 0 && terrainMap [xIndex, yIndex, zIndex - 1] != null)
			near.Add (terrainMap [xIndex, yIndex, zIndex - 1]);

		if (yIndex < MAX_Y) {
			if (xIndex < MAX_X && terrainMap [xIndex + 1, yIndex + 1, zIndex] != null)
				near.Add (terrainMap [xIndex + 1, yIndex + 1, zIndex]);
			if (xIndex > 0 && terrainMap [xIndex - 1, yIndex + 1, zIndex] != null)
				near.Add (terrainMap [xIndex - 1, yIndex + 1, zIndex]);
			if (zIndex < MAX_Z && terrainMap [xIndex, yIndex + 1, zIndex + 1] != null)
				near.Add (terrainMap [xIndex, yIndex + 1, zIndex + 1]);
			if (zIndex > 0 && terrainMap [xIndex, yIndex + 1, zIndex - 1] != null)
				near.Add (terrainMap [xIndex, yIndex + 1, zIndex - 1]);
		}
		return near;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
