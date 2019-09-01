using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_getPvpLadder : PBConnect<CommonRequest, GetPVPLadderResponse> {

	public PBConnect_getPvpLadder() : base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "getPVPLadder.php";
	}

	override protected void DisposeContent(GetPVPLadderResponse content)
	{
		InstancePlayer.instance.model_User.model_pvpUser.Parser (content);
	}

	override protected int ValidateContent(GetPVPLadderResponse content)
	{
		return ValidateApiResponse(content.response);
	}

}
