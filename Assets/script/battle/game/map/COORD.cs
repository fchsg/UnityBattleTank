using UnityEngine;
using System.Collections;

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

	public Vector3 ToVector3()
	{
		return new Vector3 (x, 0, z);
	}
	
}
