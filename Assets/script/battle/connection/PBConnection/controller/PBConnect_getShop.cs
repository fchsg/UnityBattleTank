using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_getShop : PBConnect<GetShopRequest, GetShopResponse> {

	private GetShopRequest _request;

	public enum REQUEST_TYPE
	{
		GET = 0,
		REFRESH = 1,
	}

	public static int refreshCostType = -1;
	public static int refreshCostCount;

	public PBConnect_getShop() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "getShop.php";
	}

	public override void Send (GetShopRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}


	override protected void DisposeContent(GetShopResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);
		InstancePlayer.instance.model_User.model_shop.Update ((PBConnect_buyShopItem.SHOP_TYPE)_request.shopId, content.shopItems);

		refreshCostType = content.refreshPriceType;
		refreshCostCount = content.refreshPrice;
	}




	override protected int ValidateContent(GetShopResponse content)
	{
		return ValidateApiResponse(content.response);
	}

	public enum RESULT
	{
		OK,
		LACK_RESOURCE,
	}

	public static RESULT Check(PBConnect_buyShopItem.SHOP_TYPE shopId, REQUEST_TYPE requestType)
	{
		if (requestType == REQUEST_TYPE.REFRESH) {
			Assert.assert (refreshCostType >= 0);

			if (!Model_Helper.HasEnoughResource (refreshCostType, refreshCostCount)) {
				return RESULT.LACK_RESOURCE;
			}

		}

		return RESULT.OK;
	}

	public static RESULT GetShop(PBConnect_buyShopItem.SHOP_TYPE shopId, PBConnect_getShop.DelegateConnectCallback callback)
	{
		REQUEST_TYPE requestType = REQUEST_TYPE.GET;

		RESULT r = Check (shopId, requestType);
		if (r != RESULT.OK) {
			return r;
		}

		GetShopRequest request = new GetShopRequest ();
		request.api = (new Model_ApiRequest ()).api;

		request.shopId = (int)shopId;
		request.refresh = (int)requestType;

		(new PBConnect_getShop ()).Send (request, callback);

		return r;
	}



	public static RESULT RefreshShop(PBConnect_buyShopItem.SHOP_TYPE shopId, PBConnect_getShop.DelegateConnectCallback callback)
	{
		REQUEST_TYPE requestType = REQUEST_TYPE.REFRESH;


		RESULT r = Check (shopId, requestType);
		if (r != RESULT.OK) {
			return r;
		}

		GetShopRequest request = new GetShopRequest ();
		request.api = (new Model_ApiRequest ()).api;

		request.shopId = (int)shopId;
		request.refresh = (int)requestType;

		(new PBConnect_getShop ()).Send (request, callback);

		return r;
	}




}
