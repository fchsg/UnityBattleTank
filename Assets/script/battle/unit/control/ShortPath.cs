using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShortPath {

	private Unit _unit;
	private MapGrid _mapGrid;

	private bool _hasPath = false;
	public bool hasPath
	{
		get { return _hasPath; }
	}

	private Vector3 _targetTileCenter;
	public Vector3 targetTileCenter
	{
		get { return _targetTileCenter; }
	}

	private MapAStar.PATH _path = null;
	public MapAStar.PATH path
	{
		get { return _path; }
	}

	private List<COORD> _lastPathLineTiles = null;
	

	public ShortPath(Unit unit, MapGrid mapGrid)
	{
		_unit = unit;
		_mapGrid = mapGrid;
	}



	// Update is called once per frame
	public void Update () {

		if (_hasPath) {
			COORD currentCoord = MapGrid.GetTileCoord (_unit.transform.position.x, _unit.transform.position.z);
			COORD targetCoord = MapGrid.GetTileCoord (_targetTileCenter.x, _targetTileCenter.z);
			if(currentCoord.IsEqual(targetCoord))
			{
				_hasPath = false;
			}

			/*
			foreach(MapAStar.NODE node in _path.nodes)
			{
				if(tileCoord.x == node.x && tileCoord.z == node.z)
				{
					_hasPath = false;
					break;
				}
			}
			*/

		}

	}

	private void SetPathLineTiles(List<COORD> coords)
	{
//		if (_unit.team != DataConfig.TEAM.ENEMY) {
//			return;
//		}

		if (_lastPathLineTiles != null) {
			foreach(COORD c in _lastPathLineTiles)
			{
				_mapGrid.ClearHotField(c.x, c.z);
			}
			_lastPathLineTiles = null;
		}
		
		_lastPathLineTiles = coords;
		if (_lastPathLineTiles != null) {
			foreach(COORD c in _lastPathLineTiles)
			{
				_mapGrid.AddHotField(c.x, c.z);
			}
		}
	}
	
	public void SetPath(MapAStar.PATH path)
	{

		if (path != null && path.success && path.nodes.Count > 0) {
			_hasPath = true;
			this._path = path;

			_mapGrid.AddPath (path);

			List<COORD> coords = CalcTargetTileCenter();
			SetPathLineTiles(coords);

		} else {
			_hasPath = false;
			this._path = null;

			SetPathLineTiles(null);
		}
	}

	private List<COORD> CalcTargetTileCenter()
	{
		List<COORD> coords = null;

		int index = _path.nodes.Count - 1;
		while (index > 0) {
			MapAStar.NODE n = _path.nodes [index];
			Vector2 nc = MapGrid.GetTileCenter(n.x, n.z);

			coords = CollectTilesInLine(
				_unit.transform.position.x, _unit.transform.position.z, nc.x, nc.y);

			bool blocked = false;
			for(int i = 1; i < coords.Count; ++i)
			{
				COORD coord = coords[i];
				if (_mapGrid.IsBlock (coord.x, coord.z)) {
					blocked = true;
					break;
				}
			}

			if(!blocked)
			{
//				SetPathLineTiles(coords);
				break;
			}
			--index;
		}

		if (index == 0) {
			coords = new List<COORD>();
			coords.Add(new COORD(_path.nodes [index].x, _path.nodes [index].z));
//			SetPathLineTiles(coords);
		}

		MapAStar.NODE nodeShortPath = _path.nodes [index];
		Vector2 centerShortPath = MapGrid.GetTileCenter (nodeShortPath.x, nodeShortPath.z);
		_targetTileCenter = new Vector3 (centerShortPath.x, 0, centerShortPath.y);

		return coords;
	}

	private List<COORD> CollectTilesInLine(float x0, float z0, float x1, float z1)
	{
		List<COORD> coords = new List<COORD> ();
		coords.Add(MapGrid.GetTileCoord (x0, z0));

		if (x0 == x1 && z0 == z1) {
			return coords;
		}

		float EDGE = 0.001f;

		x0 /= MapGrid.GRID_SIZE;
		z0 /= MapGrid.GRID_SIZE;
		x1 /= MapGrid.GRID_SIZE;
		z1 /= MapGrid.GRID_SIZE;

		float dx = x1 - x0;
		float dz = z1 - z0;

		float dist = Mathf.Sqrt (dx * dx + dz * dz);

		float kx = dist / Mathf.Abs (dx);
		float kz = dist / Mathf.Abs (dz);
		
		float currentX = x0;
		float currentZ = z0;
		
		while (true) {
			float moveX;
			float moveZ;
			
			if (dx > 0) {
				if(currentX == (int)currentX)
				{
					moveX = 1;
				}
				else
				{
					moveX = Mathf.Ceil (currentX) + EDGE - currentX;
				}
			}
			else if(dx < 0)
			{
				if(currentX == (int)currentX)
				{
					moveX = -1;
				}
				else
				{
					moveX = (Mathf.Floor(currentX) - EDGE) - currentX;
				}
			}
			else {
				moveX = 0;
			}
			
			if (dz > 0) {
				if(currentZ == (int)currentZ)
				{
					moveZ = 1;
				}
				else
				{
					moveZ = Mathf.Ceil (currentZ) + EDGE - currentZ;
				}
			}
			else if(dz < 0)
			{
				if(currentZ == (int)currentZ)
				{
					moveZ = -1;
				}
				else
				{
					moveZ = (Mathf.Floor(currentZ) - EDGE) - currentZ;
				}
			}
			else {
				moveZ = 0;
			}
			
			if(moveX != 0 && moveZ != 0)
			{
				float changeDistFromX = Mathf.Abs (moveX) * kx;
				float changeDistFromZ = Mathf.Abs (moveZ) * kz;
				if (changeDistFromX <= changeDistFromZ) {
					moveZ = moveZ * changeDistFromX / changeDistFromZ;
					dist -= changeDistFromX;
				} else {
					moveX = moveX * changeDistFromZ / changeDistFromX;
					dist -= changeDistFromZ;
				}
			}
			else if(moveX != 0)
			{
				float changeDistFromX = Mathf.Abs (moveX) * kx;
				dist -= changeDistFromX;
			}
			else if(moveZ != 0)
			{
				float changeDistFromZ = Mathf.Abs (moveZ) * kz;
				dist -= changeDistFromZ;
			}
			
			if (dist < 0) {
				break;
			}

			currentX += moveX;
			currentZ += moveZ;
			COORD tile = MapGrid.GetTileCoord (currentX * MapGrid.GRID_SIZE, currentZ * MapGrid.GRID_SIZE);
			coords.Add(tile);

		}
		
		return coords;
	}
	

}
