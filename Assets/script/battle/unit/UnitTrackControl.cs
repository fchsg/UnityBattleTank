using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitTrackControl : MonoBehaviour {

	private int tick = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{
		if ((tick++ & 0x03) == 0) {
			ApplyStripe();
		}
	}


	// =================================================================
	// create mesh
	
	private static GameObject _trackPrimitive = null;
	private static Mesh _primitiveMesh;
	private static int _verticesLengthPerSegment;
	private static int _trianglesLengthPerSegment;
	private static Vector3[] _segmentVertices;
	private static Vector3[] _segmentNormals;
	private static Vector2[] _segmentUVs;
	private static int[] _segmentTriangles;
	private static Color[] _segmentColors;
	private static Vector3[] _stripeVertices;
	private static Vector3[] _stripeNormals;
	private static Vector2[] _stripeUVs;
	private static int[] _stripeTriangles;
	private static Color[] _stripeColors;
	private static int _stripeCapacity = 0;
	
	private static List<int> _freeTileIndexList = new List<int>();
	
	private static void CreateTrackMesh()
	{
		if (_trackPrimitive == null) {
			_trackPrimitive = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Tank_Rut_Primitive");
			MonoBehaviour.DontDestroyOnLoad(_trackPrimitive);

			MeshFilter meshFilter = _trackPrimitive.GetComponent<MeshFilter> ();
			_primitiveMesh = meshFilter.mesh;
			
			_verticesLengthPerSegment = _primitiveMesh.vertices.Length;
			_trianglesLengthPerSegment = _primitiveMesh.triangles.Length;
			
			_segmentVertices = new Vector3[_verticesLengthPerSegment];
			for(int i = 0; i < _verticesLengthPerSegment; ++i)
			{
				Vector3 p = _primitiveMesh.vertices[i];
				p = _trackPrimitive.transform.TransformVector(p);
				_segmentVertices[i] = p;
			}
			//update list
			_primitiveMesh.vertices = _segmentVertices;
			
			_segmentNormals = new Vector3[_verticesLengthPerSegment];
			for(int i = 0; i < _verticesLengthPerSegment; ++i)
			{
				Vector3 p = _primitiveMesh.normals[i];
				p = _trackPrimitive.transform.TransformDirection(p);
				_segmentNormals[i] = p;
			}
			//update list
			_primitiveMesh.normals = _segmentNormals;
			
			_segmentUVs = _primitiveMesh.uv;
			_segmentTriangles = _primitiveMesh.triangles;
			
			Assert.assert(_primitiveMesh.colors.Length == 0);
			_segmentColors = new Color[_verticesLengthPerSegment];
			for(int i = 0; i < _verticesLengthPerSegment; ++i)
			{
				_segmentColors[i] = new Color(1, 1, 1, 1);
			}
			//update list
			_primitiveMesh.colors = _segmentColors;
			
			//
			_stripeVertices = _primitiveMesh.vertices;
			_stripeNormals = _primitiveMesh.normals;
			_stripeUVs = _primitiveMesh.uv;
			_stripeTriangles = _primitiveMesh.triangles;
			_stripeColors = _primitiveMesh.colors;
			
			//
			RenderHelper.ResetLocalTransform(_trackPrimitive);

			_stripeCapacity = 1;
			ReturnTrackTile(0);
		}
		
	}
	
	public static void ClearTrackTiles()
	{
		_freeTileIndexList.Clear ();
		for(int i = 0; i < _stripeCapacity; ++i)
		{
			ReturnTrackTile(i);
		}
	}
	
	public static int QueryTrackTile()
	{
		CreateTrackMesh ();

		if (_freeTileIndexList.Count > 0) {
			int tileIndex = _freeTileIndexList [0];
			_freeTileIndexList.RemoveAt (0);
			
			SetTileAlpha (tileIndex, 1);
			ShowTile(tileIndex);
			return tileIndex;
		} else {
			int tileIndex = ExtendTrackTile() - 1;
			return tileIndex;
		}
	}


	private static int ExtendTrackTile()
	{
		_stripeVertices = ArrayHelper.Concat<Vector3> (_stripeVertices, _segmentVertices);
		_stripeNormals = ArrayHelper.Concat<Vector3> (_stripeNormals, _segmentNormals);
		_stripeUVs = ArrayHelper.Concat<Vector2> (_stripeUVs, _segmentUVs);
		
		int triangleBase = _stripeCapacity * _verticesLengthPerSegment;
		int[] _triangles = new int[_trianglesLengthPerSegment];
		for (int i = 0; i < _trianglesLengthPerSegment; ++i) {
			_triangles[i] = _segmentTriangles[i] + triangleBase;
		}
		_stripeTriangles = ArrayHelper.Concat<int> (_stripeTriangles, _triangles);
		
		_stripeColors = ArrayHelper.Concat<Color> (_stripeColors, _segmentColors);

		SetDirty (DIRTY_MASK_ALL);
		/*
		_primitiveMesh.vertices = _stripeVertices;
		_primitiveMesh.normals = _stripeNormals;
		_primitiveMesh.uv = _stripeUVs;
		_primitiveMesh.triangles = _stripeTriangles;
		_primitiveMesh.colors = _stripeColors;
		*/

		++_stripeCapacity;
		return _stripeCapacity;
	}
	
	public static void ReturnTrackTile(int tileIndex)
	{
		_freeTileIndexList.Add (tileIndex);
		SetTileAlpha (tileIndex, 0);
		HideTile (tileIndex);
	}



	public static void SetTileAlpha(int tileIndex, float alpha)
	{
		Assert.assert (tileIndex < _stripeCapacity);
		//		return;
		
		int position = tileIndex * _verticesLengthPerSegment;
		for(int i = 0; i < _verticesLengthPerSegment; ++i)
		{
			_stripeColors[position++].a = alpha;
		}

		SetDirty (DIRTY_MASK_COLOR);
		/*
		_primitiveMesh.colors = _stripeColors;
		*/
	}
	
	public static void HideTile(int tileIndex)
	{
		int position = tileIndex * _trianglesLengthPerSegment;
		for(int i = 0; i < _trianglesLengthPerSegment; ++i)
		{
			_stripeTriangles[position++] = 0;
		}

		SetDirty (DIRTY_MASK_TRIANGLE);
		/*
		_primitiveMesh.triangles = _stripeTriangles;
		*/
	}
	
	public static void ShowTile(int tileIndex)
	{
		int triangleBase = tileIndex * _verticesLengthPerSegment;
		int position = tileIndex * _trianglesLengthPerSegment;
		for(int i = 0; i < _trianglesLengthPerSegment; ++i)
		{
			_stripeTriangles[position++] = _segmentTriangles[i] + triangleBase;
		}

		SetDirty (DIRTY_MASK_TRIANGLE);
		/*
		_primitiveMesh.triangles = _stripeTriangles;
		*/
	}
	
	public static void SetTrackTile(int tileIndex, Vector3 position, float rotationY)
	{
		Assert.assert (tileIndex < _stripeCapacity);
		
//		_trackPrimitive.transform.Rotate(new Vector3(0, rotationY, 0));
//		_trackPrimitive.transform.position = position;

		Vector3 rotation = new Vector3 (0, rotationY, 0);
		
		int verticesPosition = tileIndex * _verticesLengthPerSegment;
		for (int i = 0; i < _verticesLengthPerSegment; ++i) {
			Vector3 v = _segmentVertices[i];
			v = GeometryHelper.TransformPoint(v, rotation, position);
			_stripeVertices[verticesPosition++] = v;
		}

		SetDirty (DIRTY_MASK_VERTEX);
		/*
		_primitiveMesh.vertices = _stripeVertices;
		*/
		
//		RenderHelper.ResetLocalTransform(_trackPrimitive);
	}
	
	// ==================================================
	// dirty

	private static int _dirtyMask = 0;
	private const int DIRTY_MASK_VERTEX = 1 << 0;
	private const int DIRTY_MASK_NORMAL = 1 << 1;
	private const int DIRTY_MASK_UV = 1 << 2;
	private const int DIRTY_MASK_TRIANGLE = 1 << 3;
	private const int DIRTY_MASK_COLOR = 1 << 4;
	private const int DIRTY_MASK_ALL = 0xffff;

	private static void ApplyStripe()
	{
		if (_dirtyMask != 0) {

			if((_dirtyMask & DIRTY_MASK_VERTEX) != 0)
			{
				_primitiveMesh.vertices = _stripeVertices;
			}
			if((_dirtyMask & DIRTY_MASK_NORMAL) != 0)
			{
				_primitiveMesh.normals = _stripeNormals;
			}
			if((_dirtyMask & DIRTY_MASK_UV) != 0)
			{
				_primitiveMesh.uv = _stripeUVs;
			}
			if((_dirtyMask & DIRTY_MASK_TRIANGLE) != 0)
			{
				_primitiveMesh.triangles = _stripeTriangles;
			}
			if((_dirtyMask & DIRTY_MASK_COLOR) != 0)
			{
				_primitiveMesh.colors = _stripeColors;
			}

			_dirtyMask = 0;
			
		}
	}
	
	private static void SetDirty(int mask)
	{
		_dirtyMask |= mask;
	}


}
