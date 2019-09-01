using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataDropGroup {
	
	public class DropCollection
	{
		public List<DataDrop> drops = new List<DataDrop> ();

		public List<SlgPB.PrizeItem> ToPrizeItems()
		{
			List<SlgPB.PrizeItem> dropGroup = new List<SlgPB.PrizeItem> ();

			foreach (DataDrop drop in drops) 
			{
				int itemIdCount = drop.itemIds.Length;
				if (itemIdCount <= 0) 
				{
					SlgPB.PrizeItem item = new SlgPB.PrizeItem ();
					item.type = drop.itemType;
					item.num = drop.baseNum;
					dropGroup.Add (item);
				} 
				else
				{
					foreach (int itemId in drop.itemIds) 
					{
						SlgPB.PrizeItem item = new SlgPB.PrizeItem ();
						item.type = drop.itemType;
						item.itemId = itemId;
						item.num = drop.baseNum;

						dropGroup.Add (item);
					}
				}
			}

			return dropGroup;
		}
	}

	private Dictionary<int, DropCollection> _dropCollectionMap;


	public DropCollection GetDropCollection(int id)
	{
		return _dropCollectionMap [id];
	}

	public void Load(string name)
	{
		byte[] bin = DynamicFileControl.QueryFileContent (name);
		string content = StringHelper.ReadFromBytes (bin);

		LitJson.JSONNode json = LitJson.JSON.Parse (content);

		_dropCollectionMap = new Dictionary<int, DropCollection> ();

		foreach (LitJson.JSONNode subNode in json.Childs) {
			DataDrop data = new DataDrop();
			data.Load(subNode);

			if (!_dropCollectionMap.ContainsKey (data.id)) {
				_dropCollectionMap.Add (data.id, new DropCollection ());
			}
			DropCollection collection = _dropCollectionMap[data.id];

			collection.drops.Add(data);
		}	
	}




	// ========================
	// helper

	public enum DROP_TYPE
	{
		MIX,
		EXP,
	}
	public static DROP_TYPE CheckDropType(int dropId)
	{
		Assert.assert (dropId > 0);

		DataDropGroup dataDropGroup = DataManager.instance.dataDropGroup;

		DropCollection collection = dataDropGroup.GetDropCollection (dropId);
		if (collection.drops.Count > 1) {

			//validate
			foreach (DataDrop d in collection.drops) {
				Assert.assert (d.itemType != (int)DataConfig.DATA_TYPE.Exp);
			}

			return DROP_TYPE.MIX;
		}

		DataDrop drop = collection.drops [0];
		if (drop.itemType == (int)DataConfig.DATA_TYPE.Exp) {
			return DROP_TYPE.EXP;
		}

		return DROP_TYPE.MIX;
	}




	public List<int> CollectPossibleDropItemIds(int dropId)
	{
		List<int> itemIds = new List<int> ();

		DropCollection collection = GetDropCollection (dropId);
		foreach (DataDrop drop in collection.drops) {
			if (drop.itemType == (int)DataConfig.DATA_TYPE.Item) {
				ListHelper.Push (itemIds, drop.itemIds);
			}
		}

		return itemIds;
	}


	public class POSSIBLE_DROP
	{
		public int type;
		public int id;
	}

	public List<POSSIBLE_DROP> CollectPossibleDrops(int dropId)
	{
		List<POSSIBLE_DROP> possibleDrops = new List<POSSIBLE_DROP> ();

		DropCollection collection = GetDropCollection (dropId);
		foreach (DataDrop drop in collection.drops) {
			foreach (int id in drop.itemIds) {
				POSSIBLE_DROP possible = new POSSIBLE_DROP();
				possible.type = drop.itemType;
				possible.id = drop.id;

				possibleDrops.Add (possible);
			}
		}

		return possibleDrops;
	}


}
