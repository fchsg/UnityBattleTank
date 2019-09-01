using UnityEngine;
using System.Collections;

public class Trace {

	public enum CHANNEL
	{
		NORMAL,
		FIGHTING,
		HTTP,
		UI,
		IO,
		INTEGRATION,
		DYNAMIC_DATACONFIG,
	}

	private static CHANNEL[] ACTIVE_CHANNEL = {
//		CHANNEL.NORMAL,
		CHANNEL.FIGHTING,
//		CHANNEL.HTTP,
		CHANNEL.UI,
		CHANNEL.IO,
		CHANNEL.INTEGRATION,
		CHANNEL.DYNAMIC_DATACONFIG,
	};

	public static void trace(string msg, CHANNEL channel = CHANNEL.NORMAL)
	{
		bool active = false;
		for (int i = 0; i < ACTIVE_CHANNEL.Length; ++i) {
			if(ACTIVE_CHANNEL[i] == channel)
			{
				active = true;
				break;
			}
		}

		if(active)
		{
			Debug.Log("[" + channel + "]\t" + msg);
		}
	}

}
