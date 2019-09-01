using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class PBConnect_finishBattle : PBConnect<FinishBattleRequest, FinishBattleResponse> {


	public enum FIGHT_RESULT
	{
		LOSS = 0,
		WIN = 1,
		SYCHRONIZE_PROGRESS = 2,
	}

	private FinishBattleRequest _request;

	/*
	public class HeroGotExp
	{
		public int heroId;
		public int exp;
		public int levelUpgraded;
	}

	public class PlayerGotHonor
	{
		public int honor = 0;
		public int levelUpgraded = 0;
	}
	*/


	public PBConnect_finishBattle() :
		base(false)
	{
	}

	override public void Send(FinishBattleRequest content, DelegateConnectCallback callback)
	{
		base.Send (content, callback);

		_request = content;

		InstancePlayer.instance.battleGotPrizeItems.Clear ();
		InstancePlayer.instance.battleHeroGotExp.Clear ();
	}

	override protected string GetUrl()
	{
		return URL_BASE + "FinishBattle.php";
	}
	
	override protected void DisposeContent(FinishBattleResponse content)
	{
		if (_request.fightResult == (int)PBConnect_finishBattle.FIGHT_RESULT.WIN) {
			InstancePlayer.instance.model_User.model_level.SetStar (InstancePlayer.instance.missionMagicId, _request.star);

			TryUnlockNextMission ();
			InstancePlayer.instance.battleGotPrizeItems = content.prizeItems;

			CalcWinResult ();
		}

	}

	private void CalcWinResult()
	{
		DataMission mission = DataManager.instance.dataMissionGroup.GetMission (InstancePlayer.instance.missionMagicId);
		int gotExp = mission.exp;
		int gotHonor = mission.honor;
		int currentHonor = InstancePlayer.instance.model_User.honor;

		InstancePlayer.instance.model_User.UpdateHonor (currentHonor + gotHonor);

		Model_HeroGroup heroGroup = InstancePlayer.instance.model_User.model_heroGroup;
		Model_Formation formation = InstancePlayer.instance.model_User.model_Formation;

		int[] HeroesId = formation.GetSelectTeamHeroesId ();

		foreach (int heroId in HeroesId) {
			if (heroId > 0) {
				Model_HeroGroup.ExpChangeResult r = heroGroup.AddExp (heroId, gotExp);
				InstancePlayer.instance.battleHeroGotExp.Add (r);
			}
		}

	}

	private void TryUnlockNextMission()
	{
		if (_request.fightResult == (int)FIGHT_RESULT.WIN) {
			int battleMissionId = InstancePlayer.instance.missionMagicId;
			InstancePlayer.instance.model_User.model_level.TryUnlockNextMission(battleMissionId);
		}

	}


	override protected int ValidateContent(FinishBattleResponse content)
	{
		return ValidateApiResponse(content.response);
	}


	public static List<SlgPB.UnitGroup> GetPlayerDeadUnits()
	{
		List<SlgPB.UnitGroup> deadUnits = new List<SlgPB.UnitGroup> ();

		BattleGame game = BattleGame.instance;
		if (game != null) {
			List<Unit> untis = game.unitGroup.GetPlayerDeadUnits ();
			
			foreach (Unit unit in untis) {
				SlgPB.UnitGroup unitGroup = new SlgPB.UnitGroup();
				deadUnits.Add(unitGroup);
				
				unitGroup.num = unit.unit.GetDeadCount();
				unitGroup.posId = unit.slotIndex + 1;
				unitGroup.unitId = unit.unit.unitId;
				unitGroup.teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId();
			}
		}


		return deadUnits;
	}


	public enum RESULT
	{
		OK,
	}

	public static RESULT Check()
	{
		return RESULT.OK;
	}

	private static PBConnect_finishBattle.DelegateConnectCallback _callback = null;

	public static RESULT FinishBattle(int fightId, List<SlgPB.UnitGroup> deadUnits, bool isBattleWin, int star, PBConnect_finishBattle.DelegateConnectCallback callback)
	{
		Assert.assert (fightId > 0);

		RESULT r = Check ();
		if (r != RESULT.OK) {
			return r;
		}

		Assert.assert (_callback == null);
		_callback = callback;

		FinishBattleRequest request = new FinishBattleRequest ();
		request.api = new Model_ApiRequest ().api;

		request.fightId = fightId;
		ListHelper.Push (request.unitGroup, deadUnits);

		if(isBattleWin)
		{
			request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.WIN;
		}
		else
		{
			request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.LOSS;
		}

		request.star = star;

		(new PBConnect_finishBattle ()).Send (request, OnEndFight);

		return r;
	}


	private static void OnEndFight(bool success, System.Object content)
	{
		if (success) {
			SyncRequest request = new SyncRequest ();
			request.api = new Model_ApiRequest ().api;

			PBConnect_sync.FillSyncRequest (InstancePlayer.instance.battleGotPrizeItems, request);

			(new PBConnect_sync ()).Send (request, OnSyncUser);

		} else {
			PBConnect_finishBattle.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}

	private static void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			PBConnect_finishBattle.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		} else {
			PBConnect_finishBattle.DelegateConnectCallback cb = _callback;
			_callback = null;
			cb (success, content);
		}
	}


	// ========================
	// sychronize battle progress

//	private static PBConnect_finishBattle.DelegateConnectCallback sychronizeCallback;

	public static void SychronizeBattleProgress(int currentFightId, List<SlgPB.UnitGroup> deadUnits, PBConnect_finishBattle.DelegateConnectCallback callback)
	{
		Assert.assert (currentFightId > 0);

		FinishBattleRequest request = new FinishBattleRequest ();
		request.api = new Model_ApiRequest ().api;

		request.fightId = currentFightId;

		ListHelper.Push (request.unitGroup, deadUnits);

		request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.SYCHRONIZE_PROGRESS;
		request.star = 0;

		(new PBConnect_finishBattle ()).Send (request, callback);
//		sychronizeCallback = callback;

	}
//	private static void OnSychronizeBattleProgress(bool success, System.Object content)
//	{
//		sychronizeCallback (success, content);
//	}

}
