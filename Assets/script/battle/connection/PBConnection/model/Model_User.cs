using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class Model_User {

    public int  userId;//用户id
	public string userName;//用户名称
//	public int level;//当前等级
//	public int exp;//当前总经验值
	public int honor;//当前总荣誉值
	public int honorChanged = 0;
	public int honorLevel = 1;
	public int honorLevelChanged = 0;
//	public int honorLevel;//当前荣誉等级


//	public int levelChanged = 0;
//	public int honorLevelChanged = 0;


	// 体力
	public Model_Energy model_Energy = new Model_Energy();
	
	// 资源
	public Model_Resource model_Resource = new Model_Resource();

	// 队列
	public Model_Queue model_Queue = new Model_Queue();

	// 建筑列表
	public Dictionary<Model_Building.Building_Type, Model_Building> buildings = new Dictionary<Model_Building.Building_Type, Model_Building>();
	
	// 单位列表
	public Dictionary<int, Model_Unit> unlockUnits = new Dictionary<int, Model_Unit>();
	// 已解锁unit
	public List<int> unlockUnitsId = new List<int>();
	// 维修列表
	public Model_RepairUnit model_RepairUnit = new Model_RepairUnit();

	// 阵型
	public Model_Formation model_Formation = new Model_Formation();

	// 初始化信息
	public Model_InitialConfig model_InitialConfig = new Model_InitialConfig ();

	public Model_Level model_level = new Model_Level();

	public Model_ItemGroup model_itemGroup = new Model_ItemGroup();

	public Model_HeroGroup model_heroGroup = new Model_HeroGroup ();

	public Model_PvpUser model_pvpUser = new Model_PvpUser();

	public Model_NotificationGroup model_notificationGroup = new Model_NotificationGroup();

	public Model_shop model_shop = new Model_shop();

	public Model_Tasks model_task = new Model_Tasks();

	public CustomData customData = new CustomData();

	// 用户是否已经登录
	public bool isLogin = false; 

	public Model_User()
	{
	}

	public void UpdateHonor(int h, bool isLogin = false)
	{
		if (isLogin) {
			honorChanged = 0;
			honorLevelChanged = 0;
			honor = h;
			honorLevel = DataManager.instance.dataLeaderGroup.GetLevel (h);
		} else {
			int hLevel = DataManager.instance.dataLeaderGroup.GetLevel (h);

			honorChanged = h - honor;
			honorLevelChanged = hLevel - honorLevel;
			honor = h;
			honorLevel = hLevel;
		}
	}

	public int CalcPlayerUnitCapacity()
	{
		DataLeaderGroup leadershipGroup = DataManager.instance.dataLeaderGroup;
		DataLeader leader = leadershipGroup.GetLeader (honorLevel);
		return leader.Leadership;
		
	}

	public static int CalcPlayerUnitCapacity(int honorLevel)
	{
		DataLeaderGroup leadershipGroup = DataManager.instance.dataLeaderGroup;
		DataLeader leader = leadershipGroup.GetLeader (honorLevel);
		return leader.Leadership;

	}

	public void UpdateUserBasic(User user, bool isLogin = false)
	{
		/*
		if (level != 0) {
			levelChanged = user.level - level;
		}
		level = user.level;

		if (honorLevel != user.honorLevel) {
			honorLevelChanged = user.honorLevel - honorLevel;
		}
		honorLevel = user.honorLevel;

		exp = user.exp;
		*/
//		honor = user.honor;
		UpdateHonor (user.honor, isLogin);
		
		model_Energy.energy = user.energy;
		model_Energy.nextEnergyRecoverTimestamp =TimeHelper.GetCurrentRealTimestamp () + user.nextEnergy * 1000; //转化为毫秒

		model_Resource.UpdateUserResources (user, isLogin);
		
		model_Energy.ResumeRecoverEnergyTimer();

		if (isLogin) {
			customData.Parse (user.clientData);
		}
	}

	public void UpdateUserBuildings(User user, bool isLogin = false)
	{
		foreach (Building building in user.buildings) 
		{
			if(building != null)
			{
				Model_Building model_Building= buildings[(Model_Building.Building_Type)building.id];		
				model_Building.Parse(building, isLogin);
			}
		}
	}
		
	// login
	// ============================================================

	public void Login(User user)
	{
		isLogin = true;

		InitBuildings ();

		userId = user.userId;
		userName = user.userName;
		
		UpdateUserBasic (user, true);
		
		UpdateUserBuildings (user, true);

		model_Queue.UpdateQueue (user);

		LoadLocalStorageData ();
	}

	private void LoadLocalStorageData()
	{
		DataPersistent.Init(userId);
		model_Formation.SetSelectTeamId (DataPersistent.teamId);

		DataNotificationStorage.Init (userId);
		model_notificationGroup.Load ();

	}

	
	//drawProduction
	// ============================================================

	/*
	public void UpdateDrawProduction(User user)
	{
		model_Resource.UpdateUserResources (user);
	}
	*/
	
	// building
	// ============================================================

	private void InitBuildings()
	{
		buildings.Clear ();

		int[] allBuildingsType = DataManager.instance.dataBuildingGroup.GetAllBuildingsType ();
		foreach (int buildingId in allBuildingsType) 
		{
			Model_Building model_Building = new Model_Building();
			model_Building.isUnlocked = false;
			model_Building.buildingType = (Model_Building.Building_Type)buildingId;

			buildings.Add(model_Building.buildingType, model_Building);
		}
	}
	
	public void UpgradeBuilding(User user, List<Unlock> unlocks)
	{
		UpdateUserBasic (user);

		UpdateUserBuildings (user);

		foreach (Unlock unlock in unlocks) 
		{
			buildings[(Model_Building.Building_Type)unlock.buildingId].UnlockBuildingNextLevel();
		}
	}

	// unit
	// ============================================================

	public void UpdateUnit(SlgPB.Unit slgUnit)
	{
		Model_Unit model_Unit;
		int unitId = slgUnit.unitId;
		
		unlockUnits.TryGetValue(unitId, out model_Unit);
		if(model_Unit == null)
		{
			model_Unit = new Model_Unit();
			unlockUnits.Add(unitId, model_Unit);
		}
		
		model_Unit.Parse(slgUnit);		
		
		// 添加已解锁unitId
		if(!unlockUnitsId.Contains(unitId))
		{
			unlockUnitsId.Add(unitId);
		}

		//服务器下发Unit 数量
		//DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (slgUnit.unitId);
		//Trace.trace ("id," + slgUnit.unitId + ",name," + dataUnit.name + ",num," + slgUnit.num, Trace.CHANNEL.UI);
	}

	public bool HasUnit(int unitId)
	{
		if (unlockUnits.ContainsKey (unitId)) {
			return unlockUnits[unitId].num > 0;
		}
		return false;
	}

	public int GetUnitCount(int unitId)
	{
		if (HasUnit (unitId)) {
			return unlockUnits[unitId].num;
		}
		return 0;
	}

	// repair unit
	// ============================================================
	public void UpdateRepairUnits(List<SlgPB.Unit> units, int repairEndTime, int repairTotalTime, bool isLogin = false)
	{
		model_RepairUnit.Parse (units, repairEndTime, repairTotalTime, isLogin);
	}
		
	// 建筑升级 添加解锁unitId
	public void UpdateUnlockUnits(List<SlgPB.Unit> slgUnits)
	{
		if (slgUnits != null) {
			foreach (SlgPB.Unit slgUnit in slgUnits)
			{
				int unitId = slgUnit.unitId;
				if(!unlockUnitsId.Contains(unitId))
				{
					unlockUnitsId.Add(unitId);
				}
			}
		}
	}

	// 阵型 
	// ============================================================
	public void UpdateUnitGroup(List<SlgPB.UnitGroup> unitGroups) //只在登录时下发
	{
		if (unitGroups != null) 
		{
			model_Formation.Parse(unitGroups);
		}
	}

	// 阵型 
	// ============================================================

	public void UpdateLevel(List<SlgPB.Mission> missions)
	{
		model_level.Init (missions);
	}


	// items
	// ============================================================
	
	public void UpdateItems(List<SlgPB.Item> items)
	{
		model_itemGroup.SetItems (items);
	}


	// building
	// ============================================================
	
	public void UpdateHeroes(List<SlgPB.Hero> heroes)
	{
		model_heroGroup.SetHeroes (heroes);
	}

	
}
