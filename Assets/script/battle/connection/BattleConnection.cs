using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class BattleConnection {

	protected static BattleConnection _instance;
	public static BattleConnection instance
	{
		get
		{		
			if ( _instance == null)
			{
				_instance = new BattleConnection();
			}
			return _instance;
		}
	}

	public bool IsStartOfOffline()
	{
		bool isStartOffline = GameOffine.START_OF_OFFLINE;
		bool isLogin = InstancePlayer.instance.model_User.isLogin;

		if (isStartOffline || !isLogin)
			return true;
		return false;
	}

	// =====================================
	// formation

	public void SetFormation()
	{
		// 判定当前阵型是否有可战斗Unit
		int battleUnitCount = InstancePlayer.instance.model_User.model_Formation.GetCurrentTeamUnitCount ();

		if (battleUnitCount <= 0) 
		{
			//TODO 弹出提示 
			string msg = "当前阵型没有可上阵Unit"; 
			Trace.trace(msg, Trace.CHANNEL.UI);
		} 
		else
		{
			bool isOffline = IsStartOfOffline ();
			
			if (isOffline) 
			{
				SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE);
			}
			else
			{
				PBConnect_setUnitGroup.RESULT result = PBConnect_setUnitGroup.SetFormation (OnSetFormation);
				switch (result) 
				{
				case PBConnect_setUnitGroup.RESULT.OK:
					UIHelper.LoadingPanelIsOpen(true);
					break;
				}
			}
		}
	}
	private void OnSetFormation(bool success, System.Object content)
	{
		if (success)
		{
			Trace.trace("SetFormation Success", Trace.CHANNEL.UI);
			UIHelper.LoadingPanelIsOpen(false);

			// 设置成功后 关闭阵型页面
			UIController.instance.DeletePanel (UICommon.UI_PANEL_FORMATION);
		} 
		else 
		{
			Trace.trace("SetFormation Failed", Trace.CHANNEL.UI);
			UIHelper.LoadingPanelIsOpen(false);
		}
	}

	// =====================================
	// battle

	public void StartFight(int missionMagicId)
	{
		InstancePlayer.instance.missionMagicId = missionMagicId;

		bool isOffline = IsStartOfOffline ();
		if (isOffline)
		{
			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE);
		}
		else
		{
			int unitCount = InstancePlayer.instance.model_User.model_Formation.GetCurrentTeamUnitCount();
			if(unitCount <= 0)
			{
				UIController.instance.CreatePanel (UICommon.UI_PANEL_FORMATION);
			}
			else
			{
				PBConnect_startFight.RESULT result = PBConnect_startFight.StartFight (OnStartFight);
				switch (result) 
				{
				case PBConnect_startFight.RESULT.OK:
					UIHelper.LoadingPanelIsOpen (true);
					break;
				case PBConnect_startFight.RESULT.LACK_ENERGY:
					Trace.trace ("lack energy", Trace.CHANNEL.UI);
					break;
				}
			}
		}	
	}

	private void OnStartFight(bool success, System.Object content)
	{
		if (success)
		{
			SceneHelper.SwitchScene(AppConfig.SCENE_NAME_BATTLE);
		} 
		else 
		{
			Trace.trace("StartFight failed", Trace.CHANNEL.UI);
			UIHelper.LoadingPanelIsOpen(false);
		}
	}
	
	private bool _isBattleWin = false;
	public void EndFight()
	{
		// 同步战斗中损失到阵型
		InstancePlayer.instance.model_User.model_Formation.SyncBattleDamageUnit ();

		bool isOffline = IsStartOfOffline ();
		if (isOffline)
		{
			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE_OFFLINE);
		}
		else
		{
			_isBattleWin = InstancePlayer.instance.battle.IsPlayerWin ();

			Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (InstancePlayer.instance.battle.mission.magicId);
			int battleStarMask = InstancePlayer.instance.battleEvaluation.starMask;// _isBattleWin ? 7 : 0;
			battleStarMask |= modelMission.starMask;

			List<SlgPB.UnitGroup> deadUnits = PBConnect_finishBattle.GetPlayerDeadUnits ();
			int fightId = InstancePlayer.instance.currentFightId;

			PBConnect_finishBattle.FinishBattle (fightId, deadUnits, _isBattleWin, battleStarMask, OnEndFight);


			/*
			FinishBattleRequest request = new FinishBattleRequest ();
			request.api = new Model_ApiRequest ().api;
			
			request.fightId = InstancePlayer.instance.currentFightId;
			ListHelper.Push (request.unitGroup, PBConnect_finishBattle.GetPlayerDeadUnits ());

			if(InstancePlayer.instance.battle.IsPlayerWin ())
			{
				request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.WIN;
				request.star = 7;
				_isBattleWin = true;
			}
			else
			{
				request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.LOSS;
				request.star = 0;
				_isBattleWin = false;
			}

			(new PBConnect_finishBattle ()).Send (request, OnEndFight);
			*/

		}
	}
	private void OnEndFight(bool success, System.Object content)
	{
		if (success)
		{
			UIHelper.LoadingPanelIsOpen(false);
			UIController.instance.CreatePanel (UICommon.UI_PANEL_BATTLE_SETTLEMENT, _isBattleWin);
		} 
		else 
		{
			Trace.trace("OnEndFight failed", Trace.CHANNEL.UI);
		}
	}

	// =====================================
	// clear battle
	public void SrartClearBattle(int magicId, int count, PBConnect_multiFight.DelegateConnectCallback callback, GameObject parent)
	{
		PBConnect_multiFight.RESULT result = PBConnect_multiFight.MultiFight (magicId, count, callback);
		switch (result) 
		{
		case PBConnect_multiFight.RESULT.LACK_TIMES:
			{
				string msg = "剩余挑战次数不足";
				UIHelper.ShowTextPromptPanel (parent, msg);
			}
			break;
		case PBConnect_multiFight.RESULT.LACK_ENERGY:
			{
				string msg = "剩余体力不足";
				UIHelper.ShowTextPromptPanel (parent, msg);
			}
			break;
		case PBConnect_multiFight.RESULT.LACK_STAR:
			{
				string msg = "需要三星级通关才可进行扫荡";
				UIHelper.ShowTextPromptPanel (parent, msg);
			}
			break;
		}
	}

	public PBConnect_multiFight.RESULT CheckClearBattle(GameObject parent, int magicId)
	{
		PBConnect_multiFight.RESULT r = PBConnect_multiFight.CheckMultyFight (magicId, 1);
		if (r != PBConnect_multiFight.RESULT.OK)
		{
			switch (r) 
			{
			case PBConnect_multiFight.RESULT.LACK_TIMES:
				{
					string msg = "剩余挑战次数不足";
					UIHelper.ShowTextPromptPanel (parent, msg);
				}
				break;
			case PBConnect_multiFight.RESULT.LACK_ENERGY:
				{
					string msg = "剩余体力不足";
					UIHelper.ShowTextPromptPanel (parent, msg);
				}
				break;
			case PBConnect_multiFight.RESULT.LACK_STAR:
				{
					string msg = "需要三星级通关才可进行扫荡";
					UIHelper.ShowTextPromptPanel (parent, msg);
				}
				break;
			}
		}
		return r;
	}
		
	// =====================================
	// pvp battle


	public void StartPvpFight(SlgPB.PVPUser opponent)
	{
		InstancePlayer.instance.pvpUser = opponent;

		bool isOffline = IsStartOfOffline ();
		if (isOffline)
		{
//			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE);
		}
		else
		{
			int unitCount = InstancePlayer.instance.model_User.model_Formation.GetCurrentTeamUnitCount();
			if(unitCount <= 0)
			{
				UIController.instance.CreatePanel (UICommon.UI_PANEL_FORMATION);
			}
			else
			{
//				UIHelper.LoadingPanelIsOpen(true);

				PBConnect_startPVPLadderFight.RESULT r =  PBConnect_startPVPLadderFight.StartFight (OnStartPvpFight);
				switch(r)
				{
					
				case PBConnect_startPVPLadderFight.RESULT.OK:
					break;
				case PBConnect_startPVPLadderFight.RESULT.LACK_CHALLENGE:
					Trace.trace("剩余次数使用完毕！LACK_CHALLENGE",Trace.CHANNEL.UI);
					break;
				case PBConnect_startPVPLadderFight.RESULT.LACK_RANK:
					Trace.trace("LACK_RANK",Trace.CHANNEL.UI);
					break;
				default:
//					UIHelper.LoadingPanelIsOpen(false);
				break;
				}

			}
		}	
	}

	private void OnStartPvpFight(bool success, System.Object content)
	{
		if (success)
		{
			SceneHelper.SwitchScene(AppConfig.SCENE_NAME_BATTLE);
		} 
		else 
		{
			Trace.trace("StartPvpFight failed", Trace.CHANNEL.UI);
			UIHelper.LoadingPanelIsOpen(false);
		}
	}


	public void EndPvpFight()
	{
		// 同步战斗中损失到阵型
		InstancePlayer.instance.model_User.model_Formation.SyncBattleDamageUnit ();

		bool isOffline = IsStartOfOffline ();
		if (isOffline)
		{
//			SceneHelper.SwitchScene (AppConfig.SCENE_NAME_BATTLE_OFFLINE);
		}
		else
		{
			bool win = InstancePlayer.instance.battle.IsPlayerWin ();
			_isBattleWin = win;
			List<SlgPB.UnitGroup> deadUnits = PBConnect_finishBattle.GetPlayerDeadUnits ();

			PBConnect_finishPVPLadder.FinishPvpLadder (
				InstancePlayer.instance.currentPvpFightId,
				(win ? (int)PBConnect_finishBattle.FIGHT_RESULT.WIN : (int)PBConnect_finishBattle.FIGHT_RESULT.LOSS),
				deadUnits,
				OnEndPvpFight
			);

/*
			FinishBattleRequest request = new FinishBattleRequest ();
			request.api = new Model_ApiRequest ().api;

			request.fightId = InstancePlayer.instance.currentFightId;
			ListHelper.Push (request.unitGroup, PBConnect_finishBattle.GetPlayerDeadUnits ());

			if(InstancePlayer.instance.battle.IsPlayerWin ())
			{
				request.fightResult = 1;
			}
			else
			{
				request.fightResult = 0;
			}

			request.star = 1;
			(new PBConnect_finishBattle ()).Send (request, OnEndFight);
*/
		}
	}
	private void OnEndPvpFight(bool success, System.Object content)
	{
		if (success)
		{
			UIHelper.LoadingPanelIsOpen(false);
			UIController.instance.CreatePanel (UICommon.UI_PANEL_BATTLE_SETTLEMENT,_isBattleWin);
		} 
		else 
		{
			Trace.trace("OnEndPvpFight failed", Trace.CHANNEL.UI);
		}
	}


	// ===========================
	// sychronize battle progress

//	private PBConnect_finishBattle.DelegateConnectCallback _cbSendBattleProgress = null;

	public void SendBattleProgress()
	{
		bool isOffline = IsStartOfOffline ();
		if (isOffline) {
		} else {

			List<SlgPB.UnitGroup> deadUnits = PBConnect_finishBattle.GetPlayerDeadUnits ();

			if (InstancePlayer.instance.pvpUser == null) {
				PBConnect_finishBattle.SychronizeBattleProgress (InstancePlayer.instance.currentFightId, deadUnits, OnSendBattleProgress);
			} else {
				PBConnect_finishPVPLadder.SychronizeBattleProgress (InstancePlayer.instance.currentPvpFightId, deadUnits, OnSendBattleProgress);
			}

//			FinishBattleRequest request = new FinishBattleRequest ();
//			request.api = new Model_ApiRequest ().api;
//
//			ListHelper.Push (request.unitGroup, PBConnect_finishBattle.GetPlayerDeadUnits ());
//
//			request.fightResult = -1;
//			request.star = 0;
//
//			(new PBConnect_finishBattle ()).Send (request, OnSendBattleProgress);
//			_cbSendBattleProgress = cbSendBattleProgress;
		}

	}
	private void OnSendBattleProgress(bool success, System.Object content)
	{
		if (success)
		{
//			UIHelper.LoadingPanelIsOpen(false);
//			UIController.instance.CreatePanel (UICommon.UI_PANEL_BATTLE_SETTLEMENT);
		} 
		else 
		{
			Trace.trace("OnSendBattleProgress failed", Trace.CHANNEL.FIGHTING);

			BattleGame.instance.PauseGame ();

			//TODO, stop game and popup ui at here

		}

//		if (_cbSendBattleProgress != null) {
//			_cbSendBattleProgress (success, content);
//		}

	}


}
