using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLeaderGroup {

	private Dictionary<int, DataLeader> _leadersMap; //level, leader

	public DataLeader GetLeader(int level)
	{
		return _leadersMap [level];
	}

	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_leadersMap = new Dictionary<int, DataLeader> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataLeader data = new DataLeader();
			data.Load(subNode);

			_leadersMap.Add(data.level, data);
		}	
	}

	public int GetLevel(int honor)
	{
		int n = _leadersMap.Count;
		for(int i = 2; i <= n; ++i)
		{
			DataLeader leader = _leadersMap [i];
			if (honor < leader.honor) {
				return i - 1;
			}
		}
		return n;
	}

}
