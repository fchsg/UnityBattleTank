using UnityEngine;
using System.Collections;

public class MovingControl {

	public class RESULT
	{
		public Vector3 destination;
		public float distance;
		public float distanceLeft;
		public bool arrived = false;
		public bool blocked = false;
	}

//	private bool _running = true;


	public static Vector3 CalcDestinationWithOrientation(Vector3 a, Vector3 b, float speed, Vector3 orientation,
	                                                      out float moveDist, out float dist)
	{
		Vector3 v = b - a;
		dist = v.magnitude;

		float moveMax = TimeHelper.deltaTime * speed;
		moveDist = Mathf.Min (moveMax, dist);
		
		VectorHelper.ResizeVector (ref orientation, moveDist);
		Vector3 destination = a + orientation;

		return destination;
	}


	public static RESULT MoveTo(Vector3 a, Vector3 b, float speed, Vector3 orientation, float arriveThresdhold)
	{
		float moveDist;
		float dist;
		Vector3 destination = CalcDestinationWithOrientation (a, b, speed, orientation, out moveDist, out dist);

		RESULT r = new RESULT ();
		r.distance = moveDist;
		r.distanceLeft = dist - moveDist;
		r.destination = destination;
		r.arrived = (r.distanceLeft <= arriveThresdhold);
		return r;
		
	}

	public static RESULT MoveTo(Vector3 a, Vector3 b, float speed, float arriveThresdhold)
	{
		Vector3 orientation = b - a;
		return MoveTo (a, b, speed, orientation, arriveThresdhold);

	}
	
}
