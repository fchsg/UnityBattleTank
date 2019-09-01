using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_useItem : PBConnect<UseItemRequest, UseItemResponse> {

	private UseItemRequest _request;

	public static List<SlgPB.PrizeItem> dropPrizeItems = new List<PrizeItem> ();

	public PBConnect_useItem() : base(false)
	{
	}

	public override void Send (UseItemRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
		dropPrizeItems.Clear ();
	}


	override protected string GetUrl()
	{
		return URL_BASE + "UseItem.php"; // 生产
	}

	override protected void DisposeContent(UseItemResponse content)
	{
		dropPrizeItems = content.prizeItems;


		DataItem item = DataManager.instance.dataItemGroup.GetItem (_request.itemId);
		DataDropGroup.DROP_TYPE dropType = DataDropGroup.CheckDropType (item.dropGroup);
		if (dropType == DataDropGroup.DROP_TYPE.EXP) {

			SlgPB.PrizeItem prizeItem = dropPrizeItems [0];
			Assert.assert (prizeItem.type == (int)DataConfig.DATA_TYPE.Exp);

			int exp = prizeItem.num;
			int targetHeroId = _request.target;
			Model_HeroGroup modelHeroGroup = InstancePlayer.instance.model_User.model_heroGroup;
			modelHeroGroup.AddExp (targetHeroId, exp);
		}

		InstancePlayer.instance.model_User.model_itemGroup.AddItemCount (_request.itemId, -_request.num);

	}

	override protected int ValidateContent(UseItemResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	// ======================================
	// helper

	public enum USE_ITEM_RESULT
	{
		OK,
		NOT_USEABLE,
		LACK_ITEM,
		LACK_TARGET,
	}

	public static USE_ITEM_RESULT CheckUseItem(int itemId, int num, int targetId)
	{
		if (!DataItemGroup.IsUseable (itemId)) {
			return USE_ITEM_RESULT.NOT_USEABLE;
		}

		DataItem item = DataManager.instance.dataItemGroup.GetItem (itemId);
		DataDropGroup.DROP_TYPE dropType = DataDropGroup.CheckDropType (item.dropGroup);
		if (dropType == DataDropGroup.DROP_TYPE.EXP) {
			if (targetId <= 0) {
				return USE_ITEM_RESULT.LACK_TARGET;
			}
		}

		int itemCount = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount (itemId);
		if (itemCount < num) {
			return USE_ITEM_RESULT.LACK_ITEM;
		}

		return USE_ITEM_RESULT.OK;
	}

	private static PBConnect_useItem.DelegateConnectCallback _callback = null;

	public static USE_ITEM_RESULT UseItem(PBConnect_useItem.DelegateConnectCallback callback, int itemId, int num, int targetId = 0)
	{

		USE_ITEM_RESULT r = CheckUseItem (itemId, num, targetId);
		if (r != USE_ITEM_RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;

		UseItemRequest request = new UseItemRequest ();
		request.api = new Model_ApiRequest ().api;

		request.itemId = itemId;
		request.target = targetId;
		request.num = num;

		(new PBConnect_useItem ()).Send (request, OnUseItem);
		return r;
	}

	private static void OnUseItem(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (dropPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_useItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_useItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_useItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

}
