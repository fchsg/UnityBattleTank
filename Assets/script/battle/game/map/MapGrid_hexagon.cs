using UnityEngine;
using System.Collections;

public class MapGrid_hexagon {

	public class COORD
	{
		public int x;
		public int z;

		public COORD(int x = 0, int z = 0)
		{
			this.x = x;
			this.z = z;
		}

		public bool IsEqual(COORD c)
		{
			return x == c.x && z == c.z;
		}

	}

	private int[] _grids;
	private int[] _gridsPathLeftTick;

	public MapGrid_hexagon()
	{
		_grids = new int[MAP_GRID_COUNT_X * MAP_GRID_COUNT_Z];
		_gridsPathLeftTick = new int[MAP_GRID_COUNT_X * MAP_GRID_COUNT_Z];

		if (AppConfig.SHOW_TILE) {
			InitShowTile();

			Test();
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


	public bool IsInsideMap(int tileX, int tileZ)
	{
		if (tileX >= 0 && tileX < MAP_GRID_COUNT_X &&
		    tileZ >= 0 && tileZ < MAP_GRID_COUNT_Z) {
			return true;
		} else {
			return false;
		}
		
	}
	
	public int GetGridIndex(int tileX, int tileZ)
	{
		Assert.assert (tileX >= 0 && tileX < MAP_GRID_COUNT_X);
		Assert.assert (tileZ >= 0 && tileZ < MAP_GRID_COUNT_Z);
		
		int index = tileX + tileZ * MAP_GRID_COUNT_X;
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

	public bool IsLineBlocked(int x0, int z0, int x1, int z1)
	{
		return false;

		/*
		Vector2 s = GetTileCenter (x0, z0);
		Vector2 e = GetTileCenter (x1, z1);
		Vector2 v = e - s;
		float l = v.magnitude;

		Vector2 b = new Vector2 (v.y, -v.x);
		VectorHelper.ResizeVector(ref b, HEXAGON_A);

		float step = HEXAGON_A;
		Vector2 n = VectorHelper.ResizeVector (v, step);

		Vector2 p = e;
		while (l > 0) {
			COORD coord = GetTileCoord(p.x, p.y);
			if(IsBlock(coord.x, coord.z))
			{
				return true;
			}

			Vector2 p1 = p + b;
			COORD coord1 = GetTileCoord(p1.x, p1.y);
			if(IsBlock(coord1.x, coord1.z))
			{
				return true;
			}

			Vector2 p2 = p - b;
			COORD coord2 = GetTileCoord(p2.x, p2.y);
			if(IsBlock(coord2.x, coord2.z))
			{
				return true;
			}

			p -= n;
			l -= step;
		}

		return false;
		*/

	}


	// ============================================================
	// display

	private GameObject[] _tileObjects;
	
	public void InitShowTile()
	{
		GameObject root = new GameObject ();
		root.name = "HEXAGON_ROOT";

		_tileObjects = new GameObject[MAP_GRID_COUNT_X * MAP_GRID_COUNT_Z];

		string asset = AppConfig.FOLDER_PROFAB + "tile/hexagon";
		Object profab = Resources.Load(asset);

		for (int x = 0; x < MAP_GRID_COUNT_X; ++x) {
			for (int z = 0; z < MAP_GRID_COUNT_Z; ++z) {
				GameObject tile = GameObject.Instantiate(profab) as GameObject;
				tile.transform.parent = root.transform;
				
				tile.transform.localScale = new Vector3(1.732f * HEXAGON_L, 0.1f, 1.732f * HEXAGON_L);
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

			if(_gridsPathLeftTick[i] > 0)
			{
				--_gridsPathLeftTick[i];
			}

			float r = 0.1f + _grids[i] * 0.3f;
			float g = 0.1f + 0.9f * _gridsPathLeftTick[i] / SHOW_PATH_TICK_MAX;
			float b = 0.1f;
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
			_gridsPathLeftTick[index] = Mathf.Max(_gridsPathLeftTick[index], SHOW_PATH_TICK_MAX);
		}
	}

	// ============================================================
	// algorithm

	/*
	 * ---A---------L------
	 * |     /             \
	 * |    /               \
	 * B   L                 L
	 * |  /                   \
	 * | /                     \
	 * |/                       \
	 */

	public const int MAP_GRID_COUNT_X = 30;
	public const int MAP_GRID_COUNT_Z = 10;
	
	public const float HEXAGON_L = 5;
	public const float HEXAGON_A = HEXAGON_L * 0.5f; //sin30;
	public const float HEXAGON_B = HEXAGON_L * 0.8660254f; //cos30
	public const float HEXAGON_SIZE = HEXAGON_L + 2 * HEXAGON_A;

	public static Vector2 GetTileCenter(int tileX, int tileZ) {
		float x = tileX * (HEXAGON_L + HEXAGON_A);
		float z = (2 * tileZ + (tileX & 1)) * HEXAGON_B;
		return new Vector2(x, z);
	}

	public static COORD GetTileCoord(float x, float z)
	{
//		x = 165f;
//		z = 38.97114f;

		/// tile dist in x = L + A
		/// tile dist in z = 2 * b
		/// 

		int baseX = (int)Mathf.Floor(x / (HEXAGON_L + HEXAGON_A));
		int baseZ = (int)Mathf.Floor(z / (2 * HEXAGON_B));

		int x0 = baseX;
		int z0 = baseZ;
		int x1 = baseX + 1;
		int z1 = baseZ + 1;

		float backupD2 = int.MaxValue;
		int backupX = baseX;
		int backupZ = baseZ;

		for(int tx = x0; tx <= x1; ++tx)
		{
			for(int tz = z0; tz <= z1; ++tz)
			{
				Vector2 center = GetTileCenter(tx, tz);

				float dx = Mathf.Abs(x - center.x);
				float dz = Mathf.Abs(z - center.y);

				if(dx <= HEXAGON_L / 2 + HEXAGON_A &&
				   dz <= HEXAGON_B)
				{
					float c = HEXAGON_L / 2 + HEXAGON_A - dx;
					float m = c * 1.732050808f; //Mathf.Sqrt(3);
					if(dz <= m)
					{
						return new COORD(tx, tz);
					}
				}

				float d2 = dx * dx + dz * dz;
				if(d2 < backupD2)
				{
					backupD2 = d2;
					backupX = tx;
					backupZ = tz;
				}

			}
		}

		return new COORD (backupX, backupZ);
	}

	public static float GetMapWidth()
	{
		Vector2 pos = GetTileCenter (MAP_GRID_COUNT_X - 1, MAP_GRID_COUNT_Z - 1);
		return pos.x;
	}

	public static float GetMapHeight()
	{
		Vector2 pos = GetTileCenter (MAP_GRID_COUNT_X - 1, MAP_GRID_COUNT_Z - 1);
		return pos.y;
	}

}




