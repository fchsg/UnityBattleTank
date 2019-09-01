using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLadderGroup {

	private List<DataLadder> _laddersMap;
//	private Dictionary<int, DataLadder> _laddersMap; //rank, ladder


	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_laddersMap = new List<DataLadder> ();
//		_laddersMap = new Dictionary<int, DataLadder> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataLadder data = new DataLadder();
			data.Load(subNode);

//			_laddersMap.Add(data.rank, data);
			_laddersMap.Add(data);
		}

		_laddersMap.Sort ();
	}


	public DataLadder GetLadder(int rank)
	{
		foreach (DataLadder ladder in _laddersMap) {
			if (ladder.rank >= rank) {
				return ladder;
			}
		}

		return null;

	}

}
