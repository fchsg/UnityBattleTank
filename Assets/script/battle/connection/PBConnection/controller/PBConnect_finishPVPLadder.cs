using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_finishPVPLadder : PBConnect<FinishBattleRequest, GetPVPLadderResponse> {

	private FinishBattleRequest _request;

	public PBConnect_finishPVPLadder() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "finishPVPLadder.php";
	}

	public override void Send (FinishBattleRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;
	}

	override protected void DisposeContent(GetPVPLadderResponse content)
	{
		if (_request.fightResult == (int)PBConnect_finishBattle.FIGHT_RESULT.WIN) {
			InstancePlayer.instance.model_User.model_pvpUser.Parser (content);

		}
	}

	override protected int ValidateContent(GetPVPLadderResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public static void FinishPvpLadder(int fightId, int isWin, List<SlgPB.UnitGroup> deadUnits, PBConnect_finishPVPLadder.DelegateConnectCallback callback)
	{
		FinishBattleRequest request = new FinishBattleRequest ();
		request.api = new Model_ApiRequest ().api;

		request.fightId = fightId;
		request.fightResult = isWin;
		request.star = 0;
		ListHelper.Push (request.unitGroup, deadUnits);

		(new PBConnect_finishPVPLadder ()).Send (request, callback);
	}




	// ========================
	// sychronize battle progress

	//	private static PBConnect_finishBattle.DelegateConnectCallback sychronizeCallback;

	public static void SychronizeBattleProgress(int currentFightId, List<SlgPB.UnitGroup> deadUnits, PBConnect_finishPVPLadder.DelegateConnectCallback callback)
	{
		Assert.assert (currentFightId > 0);

		FinishBattleRequest request = new FinishBattleRequest ();
		request.api = new Model_ApiRequest ().api;

		request.fightId = currentFightId;

		ListHelper.Push (request.unitGroup, deadUnits);

		request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.SYCHRONIZE_PROGRESS;
		request.star = 0;

		(new PBConnect_finishPVPLadder ()).Send (request, callback);
		//		sychronizeCallback = callback;

	}

}
