using UnityEngine;
using System.Collections;

public class Assert {

	public static void assert(bool condition, string msg = "unknown")
	{
		if (AppConfig.DEBUGGING) {
			if (!condition) {
				throw new System.Exception(msg);
			}
		}
	}

}
