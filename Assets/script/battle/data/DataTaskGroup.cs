using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataTaskGroup {

	private Dictionary<int, DataTask> _tasksMap;



	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_tasksMap = new Dictionary<int, DataTask> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataTask data = new DataTask();
			data.Load(subNode);

			_tasksMap.Add (data.id, data);
		}	
	}

	public DataTask GetTask(int id)
	{
		return _tasksMap [id];
	}

}
