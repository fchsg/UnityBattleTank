using UnityEngine;
using System.Collections;

public class DataProduct {

	//public int type;
	//public int id;
	public int level;
	public int resourceType;
	public float produceSpeed;
	public float capacity;
	
	public void Load(LitJson.JSONNode json)
	{
		//type = int.Parse (json ["Type"]);
		//id = int.Parse (json ["ID"]);
		level = int.Parse (json ["Level"]);
		resourceType = int.Parse (json ["ResourceType"]);
		produceSpeed = float.Parse (json ["ProduceSpeed"]);
		capacity = float.Parse (json ["Capacity"]);
	}

}
