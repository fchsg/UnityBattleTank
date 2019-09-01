using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_drawProduction : PBConnect<DrawProductionRequest, DrawProductionResponse> {
	
	public PBConnect_drawProduction() :
		base(false)
	{
	}
	
	override protected string GetUrl()
	{
		return URL_BASE + "drawProduction.php";
	}
	
	override protected void DisposeContent(DrawProductionResponse content)
	{
		InstancePlayer.instance.model_User.UpdateUserBasic (content.response.user);
	}
	
	
	override protected int ValidateContent(DrawProductionResponse content)
	{
		return ValidateApiResponse(content.response);
	}
}
