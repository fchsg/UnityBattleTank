using UnityEngine;

[System.Serializable]

public class Bezier : System.Object	
{	
	public Vector3[] points;

	public Bezier( Vector3[] points )		
	{		
		this.points = points;
	}
	
	// 0.0 >= t <= 1.0	
//	public Vector3 GetPointAtTime( float t )		
//	{		
//		Vector3 p = p0 * (1 - t) * (1 - t) * (1 - t)+
//					3 * p1 * t * (1 - t) * (1 - t) +
//					3 * p2 * t * t * (1 - t) +
//					p3 * t * t * t;
//		return p;	
//	}


	public Vector3 GetPoint( float t )		
	{		
		Vector3 p = new Vector3 ();

		int n = points.Length;

		float _t = 1 - t;

		float k1 = n - 1;
		float k2 = 0;

		for(int i = 0; i < n; ++i)
		{
			int g = (i == 0 || i == n - 1)? 1: (n - 1);

			p += g * points[i] * Mathf.Pow(_t, k1) * Mathf.Pow(t, k2);
			--k1;
			++k2;
		}

		return p;	
	}

}