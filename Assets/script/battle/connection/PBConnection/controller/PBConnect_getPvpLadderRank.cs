using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_getPvpLadderRank : PBConnect<CommonRequest, GetPVPLadderRankResponse> {

	public PBConnect_getPvpLadderRank() : base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "getPVPLadderRank.php";
	}

	override protected void DisposeContent(GetPVPLadderRankResponse content)
	{
		InstancePlayer.instance.model_User.model_pvpUser.SetPvpRankUsers (content.pvpUser);
	}

	override protected int ValidateContent(GetPVPLadderRankResponse content)
	{
		return ValidateApiResponse(content.response);
	}



}
