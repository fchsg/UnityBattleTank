using UnityEngine;
using System.Collections;

public class JsonReader {

	public static float Float(LitJson.JSONNode json, string key)
	{
		string field = json [key];
		if (field == null || field == "") {
			return 0;
		}

		return float.Parse (field);
	}

	public static int Int(LitJson.JSONNode json, string key)
	{
		string field = json [key];
		if (field == null || field == "") {
			return 0;
		}
		
		return int.Parse (field);
	}
	
}
