using UnityEngine;
using System.Collections;

public class MapGrid {
	private DataMap _map;

	public int _mapGridWidth = 20;
	public int mapGridWidth
	{
		get { return _mapGridWidth; }
	}
	
	public int _mapGridHeight = 7;
	public int mapGridHeight
	{
		get { return _mapGridHeight; }
	}


	private int[] _grids;
	private int[] _gridsPathShowLeftTick;
	private int[] _gridsHotField;


	public MapGrid(DataMap map)
	{
		_map = map;
		_mapGridWidth = map.width;
		_mapGridHeight = map.height;

		_grids = new int[mapGridWidth * mapGridHeight];
		_gridsPathShowLeftTick = new int[mapGridWidth * mapGridHeight];
		_gridsHotField = new int[mapGridWidth * mapGridHeight];

		if (AppConfig.SHOW_TILE) {
			InitShowTile();
			
//			Test();
		}

		for (int x = 0; x < map.width; ++x) {
			for (int z = 0; z < map.height; ++z) {
				if(_map.GetTile(x, z) == DataMap.TILE.OBSTACLE)
				{
					Add(x, z);
				}
			}
		}

	}
	
	private void Test()
	{
		Add (7, 4);
		Add (7, 5);
		Add (7, 6);
		Add (7, 7);
		
		Add (21, 4);
		Add (21, 5);
		Add (21, 6);
		Add (21, 7);
		
	}
	

	public float GetMapWidth()
	{
		return mapGridWidth * GRID_SIZE;
	}
	
	public float GetMapHeight()
	{
		return mapGridHeight * GRID_SIZE;
	}

	public bool IsInsideMap(int tileX, int tileZ)
	{
		if (tileX >= 0 && tileX < mapGridWidth &&
		    tileZ >= 0 && tileZ < mapGridHeight) {
			return true;
		} else {
			return false;
		}
		
	}
	
	public int GetGridIndex(int tileX, int tileZ)
	{
		Assert.assert (tileX >= 0 && tileX < mapGridWidth);
		Assert.assert (tileZ >= 0 && tileZ < mapGridHeight);
		
		int index = tileX + tileZ * mapGridWidth;
		return index;
	}


	public int GetGrid(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		return _grids[index];
	}
	
	public bool IsBlock(int tileX, int tileZ)
	{
		if (IsInsideMap (tileX, tileZ)) {
			int g = GetGrid (tileX, tileZ);
			return (g != 0);
		} else {
			return true;
		}
		
	}
	
	public void Clear(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		_grids [index]--;
		Assert.assert (_grids [index] >= 0);
	}
	
	public void Add(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		_grids [index]++;
	}
	
	
	/*
	public static int GetGridX(float x)
	{
		return Mathf.FloorToInt (x / GRID_SIZE);
	}
	public static int GetGridZ(float z)
	{
		return Mathf.FloorToInt (z / GRID_SIZE);
	}
*/


	public void ClearHotField(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		_gridsHotField [index]--;
		Assert.assert (_gridsHotField [index] >= 0);
	}
	
	public void AddHotField(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		_gridsHotField [index]++;
	}
	
	public int GetGridHotField(int tileX, int tileZ)
	{
		int index = GetGridIndex (tileX, tileZ);
		return _gridsHotField[index];
	}
	

	// ============================================================
	// display
	
	private GameObject[] _tileObjects;
	
	public void InitShowTile()
	{
		GameObject root = new GameObject ();
		root.name = "HEXAGON_ROOT";
		
		_tileObjects = new GameObject[mapGridWidth * mapGridHeight];
		
		string asset = AppConfig.FOLDER_PROFAB + "tile/square";
		Object profab = Resources.Load(asset);
		
		for (int x = 0; x < mapGridWidth; ++x) {
			for (int z = 0; z < mapGridHeight; ++z) {
				GameObject tile = GameObject.Instantiate(profab) as GameObject;
				tile.transform.parent = root.transform;
				
				tile.transform.localScale = new Vector3(GRID_SIZE - 0.5f, 0.1f, GRID_SIZE - 0.5f);
				Vector2 tileCenter = GetTileCenter(x, z);
				
				if(AppConfig.DEBUGGING)
				{
					//validate tile coord <-> pos algorithm
					COORD tileCoord = GetTileCoord(tileCenter.x, tileCenter.y);
					if(tileCoord.x != x || tileCoord.z != z)
					{
						Assert.assert(false);
					}
				}
				
				tile.transform.position = new Vector3(tileCenter.x, 0, tileCenter.y);
				
				_tileObjects[GetGridIndex(x, z)] = tile;
			}
			
		}
	}
	
	public void UpdateShowTile()
	{
		if (!AppConfig.SHOW_TILE) {
			return;
		}
		
		int n = _grids.Length;
		for(int i = 0; i < n; ++i)
		{
			GameObject tile = _tileObjects[i];
			
			if(_gridsPathShowLeftTick[i] > 0)
			{
				--_gridsPathShowLeftTick[i];
			}
			
			float r = 0.1f + _grids[i] * 0.3f;
			float g = 0.1f + 0.9f * _gridsPathShowLeftTick[i] / SHOW_PATH_TICK_MAX;
			float b = 0.1f + (AppConfig.SHOW_HOTFIELD? _gridsHotField[i] * 0.1f: 0);
			Color color = new Color(r, g, b);
			RenderHelper.ChangeWholeModelColor(tile, color);
			
		}
		
		
	}
	
	private const int SHOW_PATH_TICK_MAX = 100;// * 100;
	
	public void AddPath(MapAStar.PATH path)
	{
		if (!AppConfig.SHOW_PATH) {
			return;
		}
		
		foreach (MapAStar.NODE node in path.nodes) {
			int index = GetGridIndex(node.x, node.z);
			_gridsPathShowLeftTick[index] = Mathf.Max(_gridsPathShowLeftTick[index], SHOW_PATH_TICK_MAX);
		}
	}
	
	// ============================================================
	// algorithm
	
	public const float GRID_SIZE = 10;
	
	public static Vector2 GetTileCenter(int tileX, int tileZ) {
		float x = (0.5f + tileX) * GRID_SIZE;
		float z = (0.5f + tileZ) * GRID_SIZE;
		return new Vector2(x, z);
	}
	
	public static COORD GetTileCoord(float x, float z)
	{
		int tx = (int)Mathf.Floor(x / GRID_SIZE);
		int tz = (int)Mathf.Floor(z / GRID_SIZE);
		return new COORD(tx, tz);
	}
	

	public static Vector2 GetTilePosition(float x, float z)
	{
		float tx = x / GRID_SIZE;
		float tz = z / GRID_SIZE;
		return new Vector2(tx, tz);
	}
	


}




