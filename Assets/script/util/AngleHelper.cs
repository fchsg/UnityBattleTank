using UnityEngine;
using System.Collections;

public class AngleHelper {

	public static float LimitAngleIn0_360(float angle)
	{
		return ((angle % 360) + 360) % 360;
	}

	public static float LimitAngleInNPI_PI(float angle)
	{
		angle = LimitAngleIn0_360 (angle);
		if(angle > 180)
		{
			angle -= 360;
		}
		return angle;
	}

	public static float AngleToRadius(float angle)
	{
		return angle / 180 * Mathf.PI;
	}

}
