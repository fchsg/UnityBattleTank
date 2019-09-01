using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_quickBuy : PBConnect<QuickBuyRequest, QuickBuyResponse> {

	public PBConnect_quickBuy() :
		base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "quickBuy.php"; // 购买cash
	}
	
	override protected void DisposeContent(QuickBuyResponse content)
	{
		Model_User model_user = InstancePlayer.instance.model_User;
		model_user.UpdateUserBasic (content.response.user);
	}
	
	override protected int ValidateContent(QuickBuyResponse content)
	{
		return ValidateApiResponse(content.response);
	}
}
