using UnityEngine;
using System.Collections;

public class GeometryHelper {

	public static Vector3 ProjectPointToPlane(Vector3 p, float height, Vector3 orientation)
	{
		Assert.assert (orientation.y != 0);

		float dy = height - p.y;
		float k = dy / orientation.y;
		orientation *= k;
		return p + orientation;
	}

	private static Quaternion _RotateVector_q = new Quaternion();
	private static Matrix4x4 _RotateVector_m = new Matrix4x4();
	public static Vector3 RotateVector(Vector3 v, Vector3 eulerAngles)
	{
		_RotateVector_q.eulerAngles = eulerAngles;

		_RotateVector_m.SetTRS (Vector3.zero, _RotateVector_q, Vector3.one);
		v = _RotateVector_m.MultiplyVector (v);
		return v;

	}

	private static Vector3 _RotateVector_eulerAngles = new Vector3();
	public static Vector3 RotateVector(Vector3 v, float eulerAnglesX, float eulerAnglesY, float eulerAnglesZ)
	{
		_RotateVector_eulerAngles.Set (eulerAnglesX, eulerAnglesY, eulerAnglesZ);
		return RotateVector (v, _RotateVector_eulerAngles);
	}

	public static Vector3 TransformPoint(Vector3 v, Vector3 eulerAngles, Vector3 translate)
	{
		_RotateVector_q.eulerAngles = eulerAngles;
		
		_RotateVector_m.SetTRS (translate, _RotateVector_q, Vector3.one);
		v = _RotateVector_m.MultiplyPoint (v);
		return v;
	}

	public static Vector3 TransformVector(Vector3 v, Vector3 eulerAngles, Vector3 translate)
	{
		_RotateVector_q.eulerAngles = eulerAngles;
		
		_RotateVector_m.SetTRS (translate, _RotateVector_q, Vector3.one);
		v = _RotateVector_m.MultiplyVector (v);
		return v;
	}


	// =============================================================================
	//

	public static bool IsPointInsidePolygon(float x, float y, float[] vertices)
	{
		/*
		if (!IsPointInsidePolygonBounding (x, y, vertices)) {
			return false;
		}
		*/

		int crossLeft = 0;
		int oddLeft = 0;

		int n = vertices.Length / 2;
		for (int i = 0; i < n - 1; ++i) {
			float x1 = vertices[i * 2 + 0];
			float y1 = vertices[i * 2 + 1];
			float x2 = vertices[i * 2 + 2];
			float y2 = vertices[i * 2 + 3];

			if(y1 == y2 && y1 == y)
			{
				continue;
			}
			else if((y1 > y && y2 > y) || (y1 < y && y2 < y))
			{
				continue;
			}
			else if(x1 > x && x2 > x)
			{
				continue;
			}
			else
			{
				double dx = x1 - x2;
				double dy = y1 - y2;
				double k = (double)(y - y2) / dy;
				double crossX = k * dx + x2;

				if(crossX <= x)
				{
					crossLeft++;
				}

				if(y1 == y || y2 == y)
				{
					oddLeft++;
				}

			}


		}

		int cross = crossLeft - oddLeft / 2;
		bool inside = (cross & 1) != 0;
		return inside;
	}

	public static bool IsPointInsidePolygonBounding(float x, float y, float[] vertices)
	{
		float minX = int.MaxValue;
		float minY = int.MaxValue;
		float maxX = int.MinValue;
		float maxY = int.MinValue;

		int n = vertices.Length / 2;
		if (n == 0) {
			return false;
		}

		for (int i = 0; i < n; ++i) {
			float vx = vertices[i * 2 + 0];
			float vy = vertices[i * 2 + 1];

			minX = Mathf.Min(minX, vx);
			minY = Mathf.Min(minY, vy);
			maxX = Mathf.Max(maxX, vx);
			maxY = Mathf.Max(maxY, vy);

		}

		if (x < minX || x > maxX || y < minY || y > maxY) {
			return false;
		}

		return true;
	}



	// ======================================================
	// intersect

	public static bool CalcIntersectPoint(Vector2 s, Vector2 d, Vector2 p1, Vector2 p2,
	                                      out Vector2 intersectPoint, out float intersectDist)
	{
		if (!IsLineIntersect (s, d, p1, p2)) {
			intersectPoint = Vector2.zero;
			intersectDist = 0;
			return false;
		}

		float d1 = CalcPointDistToLine (s, p1, p2);
		float d2 = CalcPointDistToLine (d, p1, p2);
		float k = d1 / (d1 + d2);
		intersectPoint = Vector2.Lerp (s, d, k);
		intersectDist = d1;
		return true;
	}

	public static bool IsLineIntersect(Vector2 s, Vector2 d, Vector2 p1, Vector2 p2)
	{
		Vector2 line12 = p2 - p1;
		Vector2 line1s = s - p1;
		Vector2 line1d = d - p1;

		float c1 = VectorHelper.Cross (line12, line1s);
		float c2 = VectorHelper.Cross (line12, line1d);
		if (c1 * c2 > 0) {
			return false;
		}

		//
		Vector2 linesd = d - s;
		Vector2 lines1 = p1 - s;
		Vector2 lines2 = p2 - s;

		float c3 = VectorHelper.Cross (linesd, lines1);
		float c4 = VectorHelper.Cross (linesd, lines2);
		if (c3 * c4 > 0) {
			return false;
		}

		return true;
	}


	public static Vector2 CalcProjectionToLine(Vector2 p, Vector2 p1, Vector2 p2)
	{
		Vector2 line12 = p2 - p1;
		line12.Normalize ();

		Vector2 line1p = p - p1;
		float projectLength = Vector2.Dot (line1p, line12);
		line12 *= projectLength;
		line12 += p1;
		return line12;
	}

	public static float CalcPointDistToLine(Vector2 p, Vector2 p1, Vector2 p2)
	{
		Vector2 line12 = p2 - p1;
		Vector2 line12plumb = new Vector2(line12.y, -line12.x);
		line12plumb.Normalize ();

		Vector2 line1p = p - p1;
		float projectLength = Vector2.Dot (line1p, line12plumb);
		return Mathf.Abs (projectLength);
	}

}
