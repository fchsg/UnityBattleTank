using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataErrorCodeGroup {

	private Dictionary<int, DataErrorCode> _dataErrorCodes; 

	public DataErrorCode GetErrorCode(int errorCode)
	{
		DataErrorCode dataErrorCode;
		_dataErrorCodes.TryGetValue (errorCode, out dataErrorCode);
		if (dataErrorCode != null)
		{
			return dataErrorCode;
		}
		
		return null;
	}
	
	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		_dataErrorCodes = new Dictionary<int, DataErrorCode> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs)
		{
			DataErrorCode data = new DataErrorCode();
			data.Load(subNode);
			_dataErrorCodes.Add(data.errorCode, data);
		}
	}


}
