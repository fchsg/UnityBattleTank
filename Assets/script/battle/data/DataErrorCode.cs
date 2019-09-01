using UnityEngine;
using System.Collections;

public class DataErrorCode {

	public int errorCode; 
	public int operation;  // 对应操作代码，0：重启客户端；1：弹出Message；2：不做任何处理
	public string message; // 弹出描述描述
	public string detial;

	public void Load(LitJson.JSONNode json)
	{
		errorCode = JsonReader.Int (json, "ErrorCode");
		operation = JsonReader.Int(json, "Operation");
		message = json["Message"];
		detial = json["Detail"];
	}
}
