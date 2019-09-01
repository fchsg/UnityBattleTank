using UnityEngine;
using System.Collections.Generic;

public class MapAStar {


	public class NODE
	{
		public int step;
		public int x;
		public int z;
		public float weight;
		public NODE previousNode = null;
	}
	
	public class PATH
	{
		public bool success = false;
		public COORD start;
		public COORD end;
		public List<NODE> nodes = new List<NODE>();
	}


	private MapGrid _mapGrid;

	private List<NODE> _candidates = new List<NODE> ();

	private byte[] _gridTicket;
	private byte _currentTicket = 0;


	public MapAStar(MapGrid mapGrid)
	{
		_mapGrid = mapGrid;

		_gridTicket = new byte[_mapGrid.mapGridWidth * _mapGrid.mapGridHeight];
		for(int i = 0 ; i < _gridTicket.Length; ++i)
		{
			_gridTicket[i] = _currentTicket;
		}
	}

	public PATH Calc(int x0, int z0, int x1, int z1)
	{
		PATH path = new PATH ();
		path.start = new COORD (x0, z0);
		path.end = new COORD (x1, z1);

		if (x0 == x1 && z0 == z1) {
			path.success = true;
			return path;
		}

		++_currentTicket;
		
		_candidates.Clear ();
		CollectCandidates (x0, z0, x1, z1, null);

		while (_candidates.Count > 0) {
			NODE n = _candidates[0];
			_candidates.RemoveAt(0);

			int dist = Mathf.Abs(n.x - x1) + Mathf.Abs(n.z - z1);
			if(dist <= 1)
			{
				while(n != null)
				{
					path.nodes.Insert(0, n);
					n = n.previousNode;
				}

				path.success = true;
				return path;
			}
			else
			{
				CollectCandidates (n.x, n.z, x1, z1, n);
			}
		}

		path.success = false;
		return path;
	}

	private void CollectCandidates(int tileX, int tileZ, int targetX, int targetZ, NODE previousNode)
	{
		int step = (previousNode == null)? 1: (previousNode.step + 1);

//		MapGrid_hexagon.COORD[] tileCoords = GetSurroundHexagonTileCoord (tileX, tileZ);
		COORD[] tileCoords = GetSurroundSquareTileCoord (tileX, tileZ);

		foreach (COORD c in tileCoords) {
			if(_mapGrid.IsBlock(c.x, c.z))
			{
				continue;
			}

			int index = _mapGrid.GetGridIndex(c.x, c.z);
			if(_gridTicket[index] == _currentTicket)
			{
				continue;
			}
			_gridTicket[index] = _currentTicket;

			int hot = _mapGrid.GetGridHotField(c.x, c.z);
			int dist = (Mathf.Abs(targetX - c.x) + Mathf.Abs(targetZ - c.z));
			float weight = step + dist + hot * 0.5f;

			NODE node = new NODE();
			node.step = step;
			node.x = c.x;
			node.z = c.z;
			node.weight = weight;
			node.previousNode = previousNode;
			InsertCandidate(node);

		}
	}
	
	private void InsertCandidate(NODE node)
	{
		int n = _candidates.Count;
		for (int i = 0; i < n; ++i) {
			if(node.weight <= _candidates[i].weight)
			{
				_candidates.Insert(i, node);
				return;
			}
		}

		_candidates.Add (node);
	}


	private static COORD[] SURROUND_TILE_OFFSET1 = new COORD[]{
		new COORD(-1, 0),
		new COORD(-1, 1),
		new COORD(1, 0),
		new COORD(1, 1),
		new COORD(0, -1),
		new COORD(0, 1),
	};
	
	private static COORD[] SURROUND_TILE_OFFSET2 = new COORD[]{
		new COORD(-1, -1),
		new COORD(-1, 0),
		new COORD(1, -1),
		new COORD(1, 0),
		new COORD(0, -1),
		new COORD(0, 1),
	};
	private COORD[] GetSurroundHexagonTileCoord(int tileX, int tileZ)
	{

		COORD[] offset = ((tileX & 1) != 0) ? SURROUND_TILE_OFFSET1 : SURROUND_TILE_OFFSET2;

		COORD[] coords = new COORD[6];
		for (int i = 0; i < 6; ++i) {
			int x = offset[i].x + tileX;
			int z = offset[i].z + tileZ;
			coords[i] = new COORD(x, z);
		}
		return coords;
	}


	private COORD[] GetSurroundSquareTileCoord(int tileX, int tileZ)
	{
		COORD[] coords = new COORD[4];
		coords [0] = new COORD (tileX - 1, tileZ);
		coords [1] = new COORD (tileX + 1, tileZ);
		coords [2] = new COORD (tileX, tileZ - 1);
		coords [3] = new COORD (tileX, tileZ + 1);
		return coords;
	}
	
	
}
