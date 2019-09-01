using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataStarRateGroup {

	private Dictionary<int, DataStarRate> _starRateMap;



	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_starRateMap = new Dictionary<int, DataStarRate> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataStarRate data = new DataStarRate();
			data.Load(subNode);

			_starRateMap.Add (data.id, data);
		}	
	}

	public DataStarRate GetStarRate(int id)
	{
		return _starRateMap [id];
	}



}
