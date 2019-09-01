using UnityEngine;
using System.Collections;

public class DataPersistent{


	public static string STORAGE_VERSION = "1.00.00";

	private static int _userId = -1;
	private static string keyPrefix
	{
		get
		{
			Assert.assert (_userId >= 0);
			return "[id]" + _userId + "_";
		}
	}

	public static void Init(int userId)
	{
		Assert.assert (userId >= 0);
		Assert.assert (_userId < 0);
		_userId = userId;

		if (version != STORAGE_VERSION) {
			string _name = name;
			string _pass = password;

			PlayerPrefs.DeleteAll ();

			name = _name;
			password = _pass;

			version = STORAGE_VERSION;
		}
	}

	private static string version
	{
		set{ PlayerPrefs.SetString ("version", value); }
		get{ return PlayerPrefs.GetString ("version", ""); }
	}

	public static string name
	{
		set{ PlayerPrefs.SetString ("name", value); }
		get{ return PlayerPrefs.GetString ("name", ""); }
	}

	public static string password
	{
		set{ PlayerPrefs.SetString ("password", value); }
		get{ return PlayerPrefs.GetString ("password", ""); }
	}

	public static int teamId
	{
		set { PlayerPrefs.SetInt(keyPrefix + "teamId", value); }
		get{ return PlayerPrefs.GetInt (keyPrefix + "teamId", 0); }
	}

	/*
	public static int gotNotificationId
	{
		set
		{
			Trace.trace ("save notifications id = " + value, Trace.CHANNEL.IO);
			PlayerPrefs.SetInt("gotNotificationId", value);
		}
		get
		{
			if (DEBUG_NOTIFICATION_ID >= 0) {
				return DEBUG_NOTIFICATION_ID;
			}
			return PlayerPrefs.GetInt ("gotNotificationId", 0);
		}

	}
	*/

}
