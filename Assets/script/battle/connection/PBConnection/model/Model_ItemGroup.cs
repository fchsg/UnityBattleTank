using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class Model_ItemGroup {

	public Dictionary<int, Item> itemsGroup = new Dictionary<int, Item>();

	public void PushItem(Item item)
	{
		itemsGroup [item.itemId] = item;
	}

	public void SetItems(List<Item> items)
	{
		itemsGroup.Clear ();
		foreach (Item item in items) {
			PushItem(item);
		}
	}

	public void AddItemCount(int id, int count)
	{
		if (count != 0) {
			Item item = QueryItem(id);
			item.num += count;
			Assert.assert (item.num >= 0);
			itemsGroup [id] = item;
		}
	}

	public Item QueryItem(int id)
	{
		Item item;
		itemsGroup.TryGetValue (id, out item);
		if (item == null) {
			item = new Item ();
			item.itemId = id;
			item.num = 0;
			itemsGroup [id] = item;
		}

		return item;
	}

	public int GetItemCount(int id)
	{
		Item item = QueryItem (id);
		return item.num;
	}

	public int UseItem(int id, int count)
	{
		Item item = QueryItem (id);
		item.num -= count;
		Assert.assert (item.num >= 0);
		return item.num;
	}
	
	public static DataItem GetItemData(int id)
	{
		return DataManager.instance.dataItemGroup.GetItem (id);
	}

}
