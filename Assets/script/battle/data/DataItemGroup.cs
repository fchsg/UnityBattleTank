using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataItemGroup {

	private Dictionary<int, DataItem> _itemMap;
	
	public DataItem GetItem(int id)
	{
		return _itemMap [id];
	}
	
	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);
		
		LitJson.JSONNode json = LitJson.JSON.Parse (content);
		
		_itemMap = new Dictionary<int, DataItem> ();
		
		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataItem data = new DataItem();
			data.Load(subNode);

			_itemMap.Add(data.id, data);
		}	
	}

	// ========================
	// helper

	public static DataDropGroup.DROP_TYPE CheckDropType(int itemId)
	{
		DataItem item = DataManager.instance.dataItemGroup.GetItem (itemId);
		return DataDropGroup.CheckDropType (item.dropGroup);
	}


	public static bool IsUseable(int itemId)
	{
		DataItem item = DataManager.instance.dataItemGroup.GetItem (itemId);
		if (item.category == DataConfig.ITEM_CATEGORY.CONSUME) {
			return true;
		}

		return false;
	}

}
