using UnityEngine;
using System.Collections;

using SlgPB;

public class PBConnect_startPVPLadderFight : PBConnect<StartPVPLadderFightRequest, StartFightResponse> {

	public PBConnect_startPVPLadderFight() :
	base(false)
	{
	}

	override protected string GetUrl()
	{
		return URL_BASE + "startPVPLadderFight.php";
	}

	public override void Send (StartPVPLadderFightRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);


	}

	override protected void DisposeContent(StartFightResponse content)
	{
		InstancePlayer.instance.currentPvpFightId = content.fightId;

		InstancePlayer.instance.model_User.model_pvpUser.SubChallengeTimes (1);

	}

	override protected int ValidateContent(StartFightResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	// ===============================
	// helper

	public enum RESULT
	{
		OK,
		LACK_CHALLENGE,
		LACK_RANK,
	}

	public static RESULT CheckStartFight(SlgPB.PVPUser pvpUser)
	{
		if (InstancePlayer.instance.model_User.model_pvpUser.remainChallengeTimes <= 0) {
			return RESULT.LACK_CHALLENGE;
		}

		if (pvpUser.rank <= 3) {
			if (InstancePlayer.instance.model_User.model_pvpUser.selfPvpUser.rank > 100) {
				return RESULT.LACK_RANK;
			}
		}


		return RESULT.OK;
	}

	private static PBConnect_startPVPLadderFight.DelegateConnectCallback _fightCallback = null;

	public static RESULT StartFight(PBConnect_startPVPLadderFight.DelegateConnectCallback callback)
	{
		Assert.assert (_fightCallback == null);

		RESULT r = CheckStartFight (InstancePlayer.instance.pvpUser);
		if (r != RESULT.OK) {
			return r;
		}

		/*
		SlgPB.PVPUser selfUser = InstancePlayer.instance.model_User.model_pvpUser.selfPvpUser;
		int selfRank = selfUser.rank;

		if (selfRank >= opponent.rank) {
			return RESULT.RANK_ERROR;
		}
		*/

		_fightCallback = callback;

		SlgPB.PVPUser opponent = InstancePlayer.instance.pvpUser;

		StartPVPLadderFightRequest request = new StartPVPLadderFightRequest ();
		request.api = new Model_ApiRequest ().api;

		request.rank = opponent.rank;
		request.oppUserId = opponent.userId;
		request.teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId ();

		(new PBConnect_startPVPLadderFight ()).Send (request, OnStartFight);

		return r;
	}

	private static void OnStartFight(bool success, System.Object content)
	{
		if (content != null) {
			//refresh pvp user if rank changed
			StartFightResponse response = content as StartFightResponse;
			if (response.pvpUser != null) {
				InstancePlayer.instance.model_User.model_pvpUser.PushPvpUser (response.pvpUser);
			}
		}

		PBConnect_startPVPLadderFight.DelegateConnectCallback cb = _fightCallback;
		_fightCallback = null;
		cb (success, content);
	}

}



