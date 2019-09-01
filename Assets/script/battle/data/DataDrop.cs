using UnityEngine;
using System.Collections;

public class DataDrop {

	public int id;
//	public float ratio;

	public int itemType;
	public int[] itemIds;

	public int baseNum;
	/*
	public int itemId;
	public float itemRate;
	public int itemLevel;
	public int minNum;
	public int maxNum;
	public int baseNum;
	*/


	public void Load(LitJson.JSONNode json)
	{
		id = JsonReader.Int(json, "ID");
//		ratio = JsonReader.Float(json, "Ratio");

		itemType = JsonReader.Int(json, "ItemType");

		string str_itemsId = json["ItemID"];
		itemIds = StringHelper.ReadIntArrayFromString (str_itemsId);

		baseNum = JsonReader.Int(json, "BaseNum");
		/*
		itemId = JsonReader.Int(json, "ItemID");
		itemRate = JsonReader.Float(json, "ItemRate");
		itemLevel = JsonReader.Int(json, "ItemLevel");
		minNum = JsonReader.Int(json, "MinNum");
		maxNum = JsonReader.Int(json, "MaxNum");
		baseNum = JsonReader.Int(json, "BaseNum");
		*/

	}


}
