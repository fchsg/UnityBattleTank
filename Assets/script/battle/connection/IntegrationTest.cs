using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class IntegrationTest : MonoBehaviour {

	private const string LOGIN_USERNAME = "ding.ning.30";


	static void _INFO(string msg)
	{
		Trace.trace ("Pass:\t" + msg, Trace.CHANNEL.INTEGRATION);
	}
	static void _WARNING(string msg)
	{
		Trace.trace ("<<WARNING>>\tFail:\t" + msg, Trace.CHANNEL.INTEGRATION);
	}
	
	
	// Use this for initialization
	void Start () {

		DataManager.instance.InitData ();

		Init();
	}

	void Update ()
	{
		if (_dynamicFileDownload != null) {
			if(_dynamicFileDownload.state == DynamicFileDownload.STATE.COMPLETE)
			{
				_dynamicFileDownload = null;
				_INFO("Success download dynamic cfg");
				Login();
			}
			else if(_dynamicFileDownload.state == DynamicFileDownload.STATE.ERROR)
			{
				_dynamicFileDownload = null;
				_WARNING("Fail download dynamic cfg");
				Login();
			}
		}
	}

	void Init()
	{
		InitRequest request = new InitRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_init ()).Send (request, OnInit);
	}
	void OnInit(bool success, System.Object content)
	{
		DownloadDynamicCfg(content as InitResponse);
	}

	private DynamicFileDownload _dynamicFileDownload = null;
	void DownloadDynamicCfg(InitResponse content)
	{
		_dynamicFileDownload = new DynamicFileDownload (content.configUrl, content);
		_dynamicFileDownload.Download ();
	}


	void Login()
	{
		LoginRequest request = new LoginRequest ();
		request.userName = LOGIN_USERNAME;	
		request.api = new Model_ApiRequest ().api;
		
		(new PBConnect_login ()).Send (request, OnLogin);
	}
	void OnLogin(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnLogin");

			DrawProduction();

			UpgradeBuilding();

			AddUnit();

			SetUnitGroup();

			QuickBuy();

			StartFight();

			TestLadder ();

		} else {
			_WARNING("OnLogin");
		}
	}

	void DrawProduction()
	{
		if (InstancePlayer.instance.model_User.buildings.Count > 0)
		{
			DrawProductionRequest request = new DrawProductionRequest ();

			Model_Building building	= InstancePlayer.instance.model_User.buildings[Model_Building.Building_Type.OilFactory];
			if(building != null)
			{
				Model_Production production = building.model_Production;
				if(production != null)
				{
					request.resourceType = production.resourceType; //要收取的资源类型	
					request.api = new Model_ApiRequest ().api;			
					(new PBConnect_drawProduction ()).Send (request, OnDrawProduction);
					return;
				}
			}
		}
		_WARNING("no production DrawProduction");
	}
	void OnDrawProduction(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnDrawProduction");
		} else {
			_WARNING("OnDrawProduction");
		}
	}

	void UpgradeBuilding() // 建筑升级
	{
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();


		Model_Building building = InstancePlayer.instance.model_User.buildings [Model_Building.Building_Type.ControlCenter];
		if (building != null) {

			request.buildingId = building.id;

			//request.byCash = 1;//是否只消耗cash来立即升级，1：是
			//request.buyQueue = 1; //是否消耗cash忽视建筑升级队列限制
			//request.buyCd = 1; //是否消耗cash忽略剩余CD时间直接完成

			Model_User user = InstancePlayer.instance.model_User;
			int isUpgrade = ConnectionValidateHelper.IsEnoughCashImmediateUpgradeBuinding(user, building.id);
			if(isUpgrade == 0)
			{
				// success
			}

			request.api = new Model_ApiRequest ().api;	
			(new PBConnect_upgradeBuilding()).Send(request, OnUpgradeBuilding);
			return;
		}

		_WARNING("no building OnUpgradeBuilding");

	}
	void OnUpgradeBuilding(bool success, System.Object content)
	{
		if (success) {
			//FinishUpgradeBuilding();  
			_INFO("OnUpgradeBuilding");
		} else {
			_WARNING("OnUpgradeBuilding");
		}
	}
	
	void FinishUpgradeBuilding() // 建筑升级CD结束时调用
	{
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();
		
		if (InstancePlayer.instance.model_User.buildings.Count > 0) {
			Model_Building building = InstancePlayer.instance.model_User.buildings [Model_Building.Building_Type.FoodFactory];
			if (building != null) {

				request.buildingId = building.id;
				request.api = new Model_ApiRequest ().api;	

				(new PBConnect_FinishUpgradeBuilding()).Send(request, OnFinishUpgradeBuilding);

				return;
			}
		}
		_WARNING("no building OnFinishUpgradeBuilding");
		
	}
	void OnFinishUpgradeBuilding(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnFinishUpgradeBuilding");
		} else {
			_WARNING("OnFinishUpgradeBuilding");
		}
	}
	
	void AddUnit()  // 正常生产
	{
		ProcessUnitRequest request = new ProcessUnitRequest ();

		request.api = new Model_ApiRequest ().api;
		request.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0]; 
		request.num = 1;

		request.buyCd = 1;  // 加速

		(new PBConnect_addUnit ()).Send (request, OnAddUnit);

		InstancePlayer.instance.model_User.model_Queue.AddUnitProduceQueue();
	}
	void OnAddUnit(bool success, System.Object content)
	{
		if (success) {

			FinishUnitNormal();

			RepairUnit();
			
			DismissUnit();
			
			FinishUnitNoCD();

			UpgradeUnitPart();

			UnlockUnit();

			ComposeHero ();

			UseItem ();

			_INFO("OnAddUnit");
		} else {
			_WARNING("OnAddUnit");
		}
	}


	void ComposeHero()
	{
		int heroId = 1;

		PBConnect_composeHero.RESULT r = PBConnect_composeHero.CheckResult (heroId);
		if (r == PBConnect_composeHero.RESULT.OK) {
			ComposeHeroRequest request = new ComposeHeroRequest ();
			request.api = new Model_ApiRequest ().api;
			request.heroId = heroId;

			(new PBConnect_composeHero ()).Send (request, OnComposeHero);
		} else {
			EvolveHero ();
		}
	}
	void OnComposeHero(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnComposeHero");

			EvolveHero ();
		} else {
			_WARNING("OnComposeHero");
		}
	}


	void EvolveHero()
	{
		int heroId = 1;

		PBConnect_evolveHero.RESULT r = PBConnect_evolveHero.CheckResult (heroId);
		if (r == PBConnect_evolveHero.RESULT.OK) {
			EvolveHeroRequest request = new EvolveHeroRequest ();
			request.api = new Model_ApiRequest ().api;
			request.heroId = 1;

			(new PBConnect_evolveHero ()).Send (request, OnEvolveHero);
		} else {
		}
	}
	void OnEvolveHero(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnEvolveHero");
		} else {
			_WARNING("OnEvolveHero");
		}
	}


	void UnlockUnit()
	{
		ComposeUnitRequest request = new ComposeUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = DataManager.instance.dataUnitsGroup.GetUnitUnlockWithItem (200001).id;

		(new PBConnect_composeUnit ()).Send (request, OnUnlockUnit);
	}
	void OnUnlockUnit(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnUnlockUnit");
		} else {
			_WARNING("OnUnlockUnit");
		}
	}


	void UpgradeUnitPart()
	{
		UpgradeUnitPartRequest request = new UpgradeUnitPartRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0];
		request.partIndex = 0;
		
		(new PBConnect_upgradeUnitPart ()).Send (request, OnUpgradeUnitPart);
		
	}
	void OnUpgradeUnitPart(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnUpgradeUnitPart");
		} else {
			_WARNING("OnUpgradeUnitPart");
		}
	}

	void FinishUnitNoCD() // 免CD生产Unit
	{
		ProcessUnitRequest request = new ProcessUnitRequest ();
		
		request.api = new Model_ApiRequest ().api;
		request.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0]; 
		request.num = 1;

		// Clear CD Need Cash
		Model_User user = InstancePlayer.instance.model_User;
		Model_Unit model_Unit; 
		user.unlockUnits.TryGetValue(1, out model_Unit);
		if (model_Unit != null) {
			float needCash = user.model_InitialConfig.GetClearUnitCDCash (model_Unit.produceLeftTime);
			Trace.trace("ClearUnitCDNeedCash: " + needCash, Trace.CHANNEL.INTEGRATION);
		}
		
		(new PBConnect_finishAddUnit ()).Send (request, OnFinishUnitNoCD);
	}
	void OnFinishUnitNoCD(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnFinishUnitNoCD");
		} else {
			_WARNING("OnFinishUnitNoCD");
		}
	}

	void RepairUnit() // 正常维修
	{
		RepairUnitRequest request = new RepairUnitRequest ();
		request.api = new Model_ApiRequest ().api;

		SlgPB.Unit unit = new SlgPB.Unit ();
		unit.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0]; 
		unit.num = 1;
		request.units.Add (unit);

		request.buyCd = 0;
	
		(new PBConnect_repairUnit ()).Send (request, OnRepairUnit);

		InstancePlayer.instance.model_User.model_Queue.AddUnitRepairQueue();
	}
	void OnRepairUnit(bool success, System.Object content)
	{
		if (success) {

			FinishRepairUnit();

			_INFO("OnRepairUnit");
		} else {
			_WARNING("OnRepairUnit");
		}
	}

	void FinishRepairUnit() //免CD修理Unit
	{
		RepairUnitRequest request = new RepairUnitRequest ();
		request.api = new Model_ApiRequest ().api;

		request.buyCd = 1;
		(new PBConnect_finishRepairUnit ()).Send (request, OnFinishRepairUnit);
	}
	void OnFinishRepairUnit(bool success, System.Object content)
	{
		if (success) {		
			_INFO("OnFinishRepairUnit");
		} else {
			_WARNING("OnFinishRepairUnit");
		}
	}
	
	void DismissUnit()  //解散Unit
	{
		ProcessUnitRequest request = new ProcessUnitRequest ();
		
		request.api = new Model_ApiRequest ().api;
		request.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0]; 
		request.num = 1;
		
		(new PBConnect_dismissUnit ()).Send (request, OnDismissUnit);
	}
	
	void OnDismissUnit(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnDismissUnit");
		} else {
			_WARNING("OnDismissUnit");
		}
	}

	void FinishUnitNormal() // 生产,维修 unit CD结束时调用
	{ 
		ProcessUnitRequest request = new ProcessUnitRequest ();
		
		request.api = new Model_ApiRequest ().api;
		request.unitId = InstancePlayer.instance.model_User.unlockUnitsId [0]; 
		request.num = 1;
		
		(new PBConnect_FinishUnit ()).Send (request, OnFinishUnitNormal);
	}
	void OnFinishUnitNormal(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnFinishUnitNormal");
		} else {
			_WARNING("OnFinishUnitNormal");
		}
	}


	void SetUnitGroup() //设置阵型
	{
		InstancePlayer.instance.model_User.model_Formation.SetSelectTeamId (1);

		//
		SetUnitGroupRequest request = new SetUnitGroupRequest ();

		Model_Formation model_Formation = InstancePlayer.instance.model_User.model_Formation;

		Model_Unit model_Unit; 
		InstancePlayer.instance.model_User.unlockUnits.TryGetValue(1, out model_Unit);
		Model_UnitGroup model_UnitGroup = new Model_UnitGroup();
		if (model_Unit != null)
		{
			model_UnitGroup.unitId = model_Unit.unitId;
			model_UnitGroup.num = model_Unit.num;
			model_UnitGroup.teamId = 1;
			model_UnitGroup.posId = 1;
		}
	    model_Formation.Add(model_UnitGroup);

        model_Formation.CreateUnitGroupsResquest (request.unitGroup);

		request.api = new Model_ApiRequest ().api;
		(new PBConnect_setUnitGroup ()).Send (request, OnSetUnitGroup);

	}
	void OnSetUnitGroup(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnSetUnitGroup");
		} else {
			_WARNING("OnSetUnitGroup");
		}
	}

	void QuickBuy() // 购买cash
	{
		QuickBuyRequest request = new QuickBuyRequest ();
		request.api = new Model_ApiRequest ().api;

		int [] types = new []{
			(int)Model_Production.Production_Type.Food,
			(int)Model_Production.Production_Type.Metal,
			(int)Model_Production.Production_Type.Oil,
			(int)Model_Production.Production_Type.Rare,

		};

		for (int i = 0; i < 4; i++) 
		{
			Production product = new Production ();
			product.resourceType = types[i];
			product.num = 5000;

			request.buy.Add(product);
		}

		// UI 显示需要金币
		int needCash = Model_Helper.GetResourcesNeedCash (5000, 5000, 5000, 5000);

		(new PBConnect_quickBuy ()).Send (request, OnQuickBuy);
	}
	void OnQuickBuy(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnQuickBuy");
		} else {
			_WARNING("OnQuickBuy");
		}
	}



	void StartFight()
	{
		StartFightRequest request = new StartFightRequest ();
		request.api = new Model_ApiRequest ().api;

		request.missionId = DataManager.instance.dataMissionGroup.GetAllMissions () [0].magicId;
		request.teamId = 1;

		(new PBConnect_startFight ()).Send (request, OnStartFight);

	}
	void OnStartFight(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnStartFight");

			SychronizeBattleProgress1 ();
			
		} else {
			_WARNING("OnStartFight");
		}
	}


	void SychronizeBattleProgress1()
	{
		int fightId = InstancePlayer.instance.currentFightId;
		List<SlgPB.UnitGroup> deadUnits = PBConnect_finishBattle.GetPlayerDeadUnits ();

		PBConnect_finishBattle.SychronizeBattleProgress (fightId, deadUnits, OnSychronizeBattleProgress1);
	}
	void OnSychronizeBattleProgress1(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnSychronizeBattleProgress1");

			SychronizeBattleProgress2 ();

		} else {
			_WARNING("OnSychronizeBattleProgress1");
		}
	}


	void SychronizeBattleProgress2()
	{
		int fightId = InstancePlayer.instance.currentFightId;
		List<SlgPB.UnitGroup> deadUnits = PBConnect_finishBattle.GetPlayerDeadUnits ();

		PBConnect_finishBattle.SychronizeBattleProgress (fightId, deadUnits, OnSychronizeBattleProgress2);
	}
	void OnSychronizeBattleProgress2(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnSychronizeBattleProgress2");

			EndFight ();

		} else {
			_WARNING("OnSychronizeBattleProgress2");
		}
	}

	void EndFight()
	{
		FinishBattleRequest request = new FinishBattleRequest ();
		request.api = new Model_ApiRequest ().api;

		request.fightId = InstancePlayer.instance.currentFightId;
		ListHelper.Push (request.unitGroup, PBConnect_finishBattle.GetPlayerDeadUnits ());
		request.fightResult = (int)PBConnect_finishBattle.FIGHT_RESULT.WIN;
		request.star = 1;

		(new PBConnect_finishBattle ()).Send (request, OnEndFight);
	}
	void OnEndFight(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnEndFight");

			SyncUser();
		} else {
			_WARNING("OnEndFight");
		}
	}



	void SyncUser()
	{
		SyncRequest request = new SyncRequest ();
		request.api = new Model_ApiRequest ().api;

		PBConnect_sync.FillSyncRequest (InstancePlayer.instance.battleGotPrizeItems, request);

		(new PBConnect_sync ()).Send (request, OnSyncUser);
	}
	void OnSyncUser(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnSyncUser");
		} else {
			_WARNING("OnSyncUser");
		}
	}



	void UseItem()
	{
		PBConnect_useItem.USE_ITEM_RESULT r = PBConnect_useItem.UseItem (OnUseItem, 100001, 2, 0);
		if (r != PBConnect_useItem.USE_ITEM_RESULT.OK) {
			_WARNING("UseItem");

			UseItem_exp ();
		}
	}
	void OnUseItem(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnUseItem");

			UseItem_exp ();
		} else {
			_WARNING("OnUseItem");
		}
	}


	void UseItem_exp()
	{
		int heroId = 1;
		PBConnect_useItem.USE_ITEM_RESULT r = PBConnect_useItem.UseItem (OnUseItem_exp, 100006, 1, heroId);
		if (r != PBConnect_useItem.USE_ITEM_RESULT.OK) {
			_WARNING("UseItem_exp");
		}
	}
	void OnUseItem_exp(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnUseItem_exp");
		} else {
			_WARNING("OnUseItem_exp");
		}
	}


	// ===========================================
	// pvp

	void TestLadder()
	{
		GetLadderRank ();
	}

	void GetLadderRank()
	{
		CommonRequest request = new CommonRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_getPvpLadderRank ()).Send (request, OnGetLadderRank);

	}
	void OnGetLadderRank(bool success, System.Object content)
	{
		if (success) {
			GetLadder ();

			_INFO("OnGetLadderRank");
		} else {
			_WARNING("OnGetLadderRank");
		}
	}

	void GetLadder()
	{
		CommonRequest request = new CommonRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_getPvpLadder ()).Send (request, OnGetLadder);
	}
	void OnGetLadder(bool success, System.Object content)
	{
		if (success) {
			RefreshLadder ();

			_INFO("OnGetLadder");
		} else {
			_WARNING("OnGetLadder");
		}
	}


	void RefreshLadder()
	{
		PBConnect_refreshLadder.RESULT r = PBConnect_refreshLadder.RefreshLadder (OnRefreshLadder);
		if (r != PBConnect_refreshLadder.RESULT.OK) {
			_WARNING("OnRefreshLadder");
		}

	}
	void OnRefreshLadder(bool success, System.Object content)
	{
		if (success) {
			_INFO("OnRefreshLadder");
		} else {
			_WARNING("OnRefreshLadder");
		}
	}


}
