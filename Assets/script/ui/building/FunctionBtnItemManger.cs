using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunctionBtnItemManger {

	public static Dictionary<string,FunctionBtnItem> dict;

	public static void Init()
	{
		dict = new Dictionary<string, FunctionBtnItem>(); 
	}


	public static void AddItem(FunctionBtnItem itemData)
	{
		if (dict == null) dict = new Dictionary<string, FunctionBtnItem> ();
		if(!dict.ContainsKey(itemData.btnItemId))
		{
			dict.Add(itemData.btnItemId, itemData);
		}else{
			dict[itemData.btnItemId] = itemData;
		}
	}

	public static FunctionBtnItem GetItem(string itemId)
	{
		if (dict == null || !dict.ContainsKey(itemId)) return null;
		return dict [itemId];
	}
	
	public static void RemoveItem(string itemId)
	{
		if(dict.ContainsKey(itemId))
		{
			dict.Remove(itemId);
		}
	}

	public static void RemoveAllItem()
	{
		if(dict != null)
		{
			dict.Clear();
		}
	}
}
