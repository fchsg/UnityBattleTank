using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_refreshLadder : PBConnect<CommonRequest, RefreshPVPLadderResponse> {

	public PBConnect_refreshLadder() : base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "refreshPVPLadder.php";
	}

	override protected void DisposeContent(RefreshPVPLadderResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);
		InstancePlayer.instance.model_User.model_pvpUser.Parser (content);
	}

	override protected int ValidateContent(RefreshPVPLadderResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public enum RESULT
	{
		OK,
		NEED_CASH,
	}

	public static RESULT RefreshLadder(PBConnect_refreshLadder.DelegateConnectCallback callback)
	{
		int cost = InstancePlayer.instance.model_User.model_pvpUser.refreshCost;
		if (cost > InstancePlayer.instance.model_User.model_Resource.cash) {
			return RESULT.NEED_CASH;
		}


		CommonRequest request = new CommonRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_refreshLadder ()).Send (request, callback);


		return RESULT.OK;
	}


}
