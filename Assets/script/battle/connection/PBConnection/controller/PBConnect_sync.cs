using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_sync : PBConnect<SyncRequest, SyncResponse> {

	public PBConnect_sync() :
		base(false)
	{
	}


	override public void Send(SyncRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);
		
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "Sync.php";
	}
	
	override protected void DisposeContent(SyncResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user, false);

		foreach (SlgPB.Unit unit in content.units) {
			InstancePlayer.instance.model_User.UpdateUnit(unit);
		}

		foreach (SlgPB.Item item in content.items) {
			InstancePlayer.instance.model_User.model_itemGroup.PushItem (item);
		}
	}
	

	override protected int ValidateContent(SyncResponse content)
	{
		return ValidateApiResponse(content.response);
	}
	

	// ===============================================================
	// helper

	public static void FillSyncRequest(List<SlgPB.PrizeItem> items, SyncRequest request)
	{
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Food);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Oil);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Metal);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Rare);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Cash);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Combat);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Energy);

		foreach (SlgPB.PrizeItem item in items) {
			if(item.type == (int)DataConfig.DATA_TYPE.Unit)
			{
				AddUnitToRequest (request, item.itemId);
//				request.unitIds.Add (item.itemId);
//				request.types.Add(item.type);
			}
			else if(item.type == (int)DataConfig.DATA_TYPE.Item)
			{
				AddItemToRequest (request, item.itemId);
//				request.items.Add (item.itemId);
//				request.types.Add(item.type);
			}
			else
			{
			}
		}

	}

	private static void AddItemToRequest(SyncRequest request, int itemId)
	{
		if (request.items.IndexOf (itemId) < 0) {
			request.items.Add (itemId);
		}
	}

	private static void AddUnitToRequest(SyncRequest request, int unitId)
	{
		if (request.unitIds.IndexOf (unitId) < 0) {
			request.unitIds.Add (unitId);
		}
	}

	private static void AddTypeToRequest(SyncRequest request, int type)
	{
		if (request.types.IndexOf (type) < 0) {
			request.types.Add(type);
		}
	}

	public static void FillSyncRequest(List<SlgPB.MultiPrizeItem> multiItems, SyncRequest request)
	{
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Food);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Oil);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Metal);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Rare);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Cash);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Combat);
		AddTypeToRequest (request, (int)DataConfig.DATA_TYPE.Energy);

		foreach (SlgPB.MultiPrizeItem prizeItems in multiItems) {
			FillSyncRequest (prizeItems.prizeItems, request);
		}

	}

}

