using UnityEngine;
using System.Collections;

public class MathHelper {

	public static float Clip(float n, float low, float high)
	{
		if (n < low)
			n = low;
		else if (n > high) {
			n = high;
		}
		return n;
	}

	public static void Swap(ref int a, ref int b)
	{
		int temp = a;
		a = b;
		b = temp;
	}

}
