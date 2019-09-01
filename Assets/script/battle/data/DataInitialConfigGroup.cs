using UnityEngine;
using System.Collections;

public class DataInitialConfigGroup {

	private DataInitialConfig _dataInitialConfig;

	public DataInitialConfig GetDataInitialConfig()
	{
		return _dataInitialConfig;
	}

	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		foreach (LitJson.JSONNode subNode in json.Childs) 
		{
			_dataInitialConfig = new DataInitialConfig();
			_dataInitialConfig.Load(subNode);
		}
	}


}
