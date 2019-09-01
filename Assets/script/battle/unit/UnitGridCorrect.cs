using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGridCorrect {

	private Unit _unit;
	private MapGrid _mapGrid;

	private bool _isHidding = false;
	private List<COORD> _tiles = new List<COORD> ();

	private const bool USER_SQUARE = false;

	public UnitGridCorrect(Unit unit, MapGrid mapGrid)
	{
		_unit = unit;
		_mapGrid = mapGrid;
	}

	public void Update()
	{
		Assert.assert (!_isHidding);

		ClearSlefGrid ();
		AddSelfToGrid ();
	}

	public void ClearSlefGrid()
	{
		int n = _tiles.Count;
		for (int i = 0; i < n; ++i) {
			COORD c = _tiles[i];
			_mapGrid.Clear(c.x, c.z);
		}

		_tiles.Clear ();
	}

	public void AddSelfToGrid()
	{
		COORD c = MapGrid.GetTileCoord (_unit.transform.position.x, _unit.transform.position.z);
		if(_mapGrid.IsInsideMap(c.x, c.z))
		{
			_tiles.Add (c);
			_mapGrid.Add(c.x, c.z);
		}

		if (USER_SQUARE) {
			float r = _unit.unit.dataUnit.GetCollisionRadius();
			
			COORD c1 = MapGrid.GetTileCoord (_unit.transform.position.x - r, _unit.transform.position.z);
			if(_mapGrid.IsInsideMap(c1.x, c1.z))
			{
				_tiles.Add (c1);
				_mapGrid.Add(c1.x, c1.z);
			}
			
			COORD c2 = MapGrid.GetTileCoord (_unit.transform.position.x + r, _unit.transform.position.z);
			if(_mapGrid.IsInsideMap(c2.x, c2.z))
			{
				_tiles.Add (c2);
				_mapGrid.Add(c2.x, c2.z);
			}
			
			COORD c3 = MapGrid.GetTileCoord (_unit.transform.position.x, _unit.transform.position.z - r);
			if(_mapGrid.IsInsideMap(c3.x, c3.z))
			{
				_tiles.Add (c3);
				_mapGrid.Add(c3.x, c3.z);
			}
			
			COORD c4 = MapGrid.GetTileCoord (_unit.transform.position.x, _unit.transform.position.z + r);
			if(_mapGrid.IsInsideMap(c4.x, c4.z))
			{
				_tiles.Add (c4);
				_mapGrid.Add(c4.x, c4.z);
			}
		}
	}

	public void HideCurrentGrid()
	{
		Assert.assert (!_isHidding);
		_isHidding = true;

		int n = _tiles.Count;
		for (int i = 0; i < n; ++i) {
			COORD c = _tiles[i];
			_mapGrid.Clear(c.x, c.z);
		}
	}

	public void ShowCurrentGrid()
	{
		Assert.assert (_isHidding);
		_isHidding = false;

		int n = _tiles.Count;
		for (int i = 0; i < n; ++i) {
			COORD c = _tiles[i];
			_mapGrid.Add(c.x, c.z);
		}
	}
	
}
