using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataMap {

	public int width;
	public int height;

	public TILE[] tiles;

	public COORD startPosition = null;
	public COORD extraPositionA = null;
	public COORD extraPositionB = null;
	public COORD extraPositionC = null;
	public COORD extraPositionD = null;
	public List<COORD> towerPositions = null;

	public enum TILE
	{
		EMPTY,
		OBSTACLE,
		START,
		A,
		B,
		C,
		D,
		TOWER,
	}

	public COORD GetExtraPosition(int index)
	{
		switch(index)
		{
		case 0: return extraPositionA;
		case 1: return extraPositionB;
		case 2: return extraPositionC;
		case 3: return extraPositionD;
		}

		Assert.assert (false);
		return null;
	}

	public TILE ParseTile(string s)
	{
		if (s.ToLower() == "x")
			return TILE.OBSTACLE;
		if (s.ToLower() == "s")
			return TILE.START;
		if (s.ToLower() == "a")
			return TILE.A;
		if (s.ToLower() == "b")
			return TILE.B;
		if (s.ToLower() == "c")
			return TILE.C;
		if (s.ToLower() == "d")
			return TILE.D;
		if (s.ToLower() == "t")
			return TILE.TOWER;

		return TILE.EMPTY;
	}

	public void Load(LitJson.JSONNode json)
	{
		int child = 0;
		int tileIndex = 0;

		foreach (LitJson.JSONNode node in json.Childs) {
			if(child == 0)
			{
				width = JsonReader.Int (node, "width");
				height = JsonReader.Int (node, "height");
				
				tiles = new TILE[width * height];
				towerPositions = new List<COORD> ();
			}
			else
			{
				int y = child - 1;
				string line = node["line"];
				
				for(int x = 0; x < width; ++x)
				{
					string c = line.Substring(x, 1);
					TILE t = ParseTile(c);
					tiles[tileIndex++] = t;
					
					if(t == TILE.START)
					{
						Assert.assert(startPosition == null);
						startPosition = new COORD(x, y);
					}
					
					if(t == TILE.A)
					{
						Assert.assert(extraPositionA == null);
						extraPositionA = new COORD(x, y);
					}
					if(t == TILE.B)
					{
						Assert.assert(extraPositionB == null);
						extraPositionB = new COORD(x, y);
					}
					if(t == TILE.C)
					{
						Assert.assert(extraPositionC == null);
						extraPositionC = new COORD(x, y);
					}
					if(t == TILE.D)
					{
						Assert.assert(extraPositionD == null);
						extraPositionD = new COORD(x, y);
					}
					
					if(t == TILE.TOWER)
					{
						towerPositions.Add(new COORD(x, y));
					}
					
				}
			}

			++child;
		}

		ValidatePositions ();
	}

	private void ValidatePositions()
	{
		COORD[] positions = new COORD[5];
		positions [0] = startPosition;
		positions [1] = extraPositionA;
		positions [2] = extraPositionB;
		positions [3] = extraPositionC;
		positions [4] = extraPositionD;

		for(int i = 0; i < positions.Length; ++i)
		{
			for(int j = i + 1; j < positions.Length; ++j)
			{
				if(positions[i] != null && positions[j] != null)
				{
					int dx = positions[i].x - positions[j].x;
					int dz = positions[i].z - positions[j].z;
					Assert.assert(Mathf.Abs(dx) >= 3 || Mathf.Abs(dz) >= 3);
				}
			}
		}
	}

	public TILE GetTile(int x, int z)
	{
		Assert.assert (x >= 0 && x < width);
		Assert.assert (z >= 0 && z < height);

		int index = x + z * width;
		return tiles[index];
	}

}
