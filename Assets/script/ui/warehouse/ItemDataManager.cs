using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class ItemDataManager {
	 
	public class ItemData
	{
		public int id;
		public DataConfig.ITEM_CATEGORY type;
		public int num;
		public int quality;
		public string iconName;
		public string iconBgName;
		public Color nameColor;
		public DataItem itemData;
	}

	//data 
	List<ItemData> allItemList = new List<ItemData>();
	List<ItemData> normalItemList = new List<ItemData>();
	List<ItemData> consumeItemList = new List<ItemData>();
	public ItemDataManager()
	{
		Init();
	}

	void Init()
	{
		allItemList.Clear();
		normalItemList.Clear();
		consumeItemList.Clear();
		Model_ItemGroup model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
		Dictionary<int, Item> itemsGroup = model_itemGroup.itemsGroup;
		foreach(KeyValuePair<int,Item > kvp in itemsGroup)
		{
			DataItem dataItem = Model_ItemGroup.GetItemData(kvp.Value.itemId);
			ItemData item = InitItemData(dataItem);
			if(item.num != 0)
			{
				if(item.type == DataConfig.ITEM_CATEGORY.NORMAL)
				{
					normalItemList.Add(item);
				}
				else
				{
					consumeItemList.Add(item);
				}
				allItemList.Add(item);
			}

		}

	}

	public List<ItemData> GetAllItemData()
	{
		Init();
		return allItemList;
	}

	public List<ItemData> GetNormalItemData()
	{
		Init();
		return normalItemList;
	}

	public List<ItemData> GetConsumeItemData()
	{
		Init();
		return consumeItemList;
	}
	public ItemData getItemDataByID(int itemId)
	{
		return InitItemData(DataManager.instance.dataItemGroup.GetItem(itemId));
	}
	public ItemData InitItemData(DataItem dataItem)
	{
		if(dataItem != null)
		{ 
			Model_ItemGroup model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
			Item item_ = model_itemGroup.QueryItem(dataItem.id);
			ItemData item = new ItemData();

			item.id = dataItem.id;
			item.num = item_.num;
			item.iconName = UICommon.UNIT_SMALL_ICON_PATH + dataItem.id;
			item.iconBgName = UICommon.UNIT_ICON_BG + dataItem.quality;
			switch((int)dataItem.quality)
			{
			case 0:
				item.nameColor = UICommon.UNIT_NAME_COLOR_0;
				break;
			case 1:
				item.nameColor = UICommon.UNIT_NAME_COLOR_1;
				break;
			case 2:
				item.nameColor = UICommon.UNIT_NAME_COLOR_2;
				break;
			case 3:
				item.nameColor = UICommon.UNIT_NAME_COLOR_3;
				break;
			case 4:
				item.nameColor = UICommon.UNIT_NAME_COLOR_4;
				break;
			case 5:
				item.nameColor = UICommon.UNIT_NAME_COLOR_5;
				break;
			default :
				item.nameColor = UICommon.UNIT_NAME_COLOR_0;
				break;
			}
			item.itemData = dataItem;
			item.type = dataItem.category;

			return item;
		}
		return null;
	}

}
