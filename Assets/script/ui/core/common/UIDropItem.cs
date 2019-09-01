using UnityEngine;
using System.Collections;

public class UIDropItem {

	public int id;
	public int type;
	public int count;

	public string icon;
	public string name;

	public UIDropItem(SlgPB.PrizeItem prizeItem)
	{
		id = prizeItem.itemId;
		type = prizeItem.type;
		count = prizeItem.num;
	}

}
