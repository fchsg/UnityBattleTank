using UnityEngine;
using System.Collections;

public class VectorHelper {

	public static void ResizeVector(ref Vector3 v, float s)
	{
		v.Normalize();
		v *= s;
	}

	public static Vector3 ResizeVector(Vector3 v, float s)
	{
		v.Normalize();
		v *= s;
		return v;
	}
	
	public static void ResizeVector(ref Vector2 v, float s)
	{
		v.Normalize();
		v *= s;
	}
	
	public static Vector2 ResizeVector(Vector2 v, float s)
	{
		v.Normalize();
		v *= s;
		return v;
	}

	public static float Cross(Vector2 A, Vector2 B)
	{
		float c = A.x * B.y - A.y * B.x;
		return c;
	}
	

	
}
