using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_buyShopItem : PBConnect<BuyShopItemRequest, BuyShopItemResponses> {

	public enum SHOP_TYPE
	{
		UNKNOWN = 0,
		RESULT_SHOP = 1, //战绩商店
		MALL = 2, //商城
		MYSTERY_SHOP = 3, //神秘商店
		ARMY_SHOP = 4, //军团商店
		OTHER_SHOP = 5, //其他商店
	}

	public static List<SlgPB.PrizeItem> dropPrizeItems = new List<PrizeItem> ();


	public PBConnect_buyShopItem() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "buyShopItem.php";
	}

	public override void Send (BuyShopItemRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		dropPrizeItems.Clear ();
	}

	override protected void DisposeContent(BuyShopItemResponses content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);

		dropPrizeItems.Add (content.prizeItem);

	}




	override protected int ValidateContent(BuyShopItemResponses content)
	{
		return ValidateApiResponse(content.response);
	}


	public enum RESULT
	{
		OK,
		SOLD,
	}

	public static RESULT Check(int index)
	{
		ShopItem item = InstancePlayer.instance.model_User.model_shop.GetItem (index);
		if (item.isSold > 0) {
			return RESULT.SOLD;
		}

		return RESULT.OK;
	}

	private static PBConnect_buyShopItem.DelegateConnectCallback _callback;

	public static RESULT Buy(int index, PBConnect_buyShopItem.DelegateConnectCallback callback)
	{
		RESULT r = Check (index);
		if (r != RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;

		BuyShopItemRequest request = new BuyShopItemRequest ();
		request.api = (new Model_ApiRequest ()).api;

		ShopItem item = InstancePlayer.instance.model_User.model_shop.GetItem (index);
		request.posId = item.posId;

		request.shopId = (int)InstancePlayer.instance.model_User.model_shop.shopType;

		(new PBConnect_buyShopItem ()).Send (request, OnBuy);

		return r;

	}

	private static void OnBuy(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (dropPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_buyShopItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_buyShopItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_buyShopItem.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

}
