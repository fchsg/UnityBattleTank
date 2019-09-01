using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Model_Formation {

	public enum RESULT
	{
		SUCCESS,
		LACK,  		 // 可用战车不足
		SAME,        // 上阵战车相同
		MAX,		 // 达到上阵上限
		UNIT_TYPE_NONE,      // 没有上阵车辆
		SAME_HERO,             // 军官已上阵
	}
	
	private const int TEAM_COUNT = 3;    
	private const int POSITION_COUNT = 6;

	private int _currentSelectTeamId = 0; // 当前选择阵型teamId, 0表示未选择阵型 

	private Model_UnitGroup[][] _formations;  // teamId [0,2], posId [0,5]

	private Model_UnitGroup[][] _lastFormations;  // 记录上一次阵型状态


	public Model_Formation()
	{
		_formations = new Model_UnitGroup[TEAM_COUNT][];
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			_formations[i] = new Model_UnitGroup[POSITION_COUNT];
			for(int j = 0; j < POSITION_COUNT; ++j)
			{
				_formations[i][j] = new Model_UnitGroup(i, j);
			}
		}
	}

	public void Parse(List<SlgPB.UnitGroup> slgUnitGroups)
	{
		foreach(SlgPB.UnitGroup slgUnitGroup in slgUnitGroups)
		{
			int teamId = slgUnitGroup.teamId;
			int posId = slgUnitGroup.posId;

			_formations[teamId-1][posId-1].Parse(slgUnitGroup);

//			Trace.trace ("server formation " + " teamId " + teamId + " posId " + posId + " unitId " + 
//				slgUnitGroup.unitId + " unitNum " + slgUnitGroup.num + " heroId " + slgUnitGroup.heroId, Trace.CHANNEL.IO);					
		}
	}

	public void CreateUnitGroupsResquest(List<SlgPB.UnitGroup> requestUnitGroups)
	{
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			for(int j = 0; j < POSITION_COUNT; ++j)
			{
				Model_UnitGroup model_UnitGroup = _formations[i][j];

				SlgPB.UnitGroup SlgPB_UnitGroup = model_UnitGroup.ConvertSlgPBUnitGroup();
				requestUnitGroups.Add(SlgPB_UnitGroup);

//				Trace.trace ("client formation " + " teamId " +model_UnitGroup.teamId + " posId " + model_UnitGroup.posId + " unitId " + 
//									model_UnitGroup.unitId + " heroId " + model_UnitGroup.heroId, Trace.CHANNEL.IO);		
			}
		}
	}

	//Unit-------------------------------------------------------------------

	// 上阵
	public RESULT Add(Model_UnitGroup model_UnitGroup) 
	{
		RESULT result = RESULT.SUCCESS;

		int teamId = model_UnitGroup.teamId;
		int posId = model_UnitGroup.posId;
		int unitId = model_UnitGroup.unitId;

		if (!IsEnoughUnitNum (model_UnitGroup))
		{
			result = RESULT.LACK;		// 可战斗unit不足
		} 
		else if (IsTeamContaninUnit (teamId, unitId)) 
		{
			result = RESULT.SAME;		// 队伍中已经包含该类型Unit
		} 
		else if (IsReachMaxBattleNum (model_UnitGroup))
		{
			result = RESULT.MAX;     // 达到最大战斗数
		}
		else 
		{
			int addNum = _formations[teamId-1][posId-1].Add (model_UnitGroup);
			ChangeUserUnitNum(unitId, -addNum);
		}

		return result;
	}

	// 上阵替换
	public RESULT AddExchange(Model_UnitGroup source, Model_UnitGroup target)
	{
		RESULT result = RESULT.SUCCESS;

		int teamId = target.teamId;
		int posId = target.posId;
		int sourceUnitId = source.unitId;
		int targetUnitId = target.unitId;

		if (targetUnitId == sourceUnitId) 
		{
			if (!IsEnoughUnitNum (source)) 
			{
				result = RESULT.LACK;		
			}
			else if (IsReachMaxBattleNum (target))
			{
				result = RESULT.MAX;
			}
			else
			{
				int addNum = _formations[teamId-1][posId-1].Add (source);	
				ChangeUserUnitNum(sourceUnitId, -addNum);
			}
		}
		else
		{
			if (!IsEnoughUnitNum (source)) 
			{
				result = RESULT.LACK;		// 可战斗unit不足
			}
			else if (IsTeamContaninUnit (teamId, sourceUnitId)) 
			{
				result = RESULT.SAME;		// 队伍中已经包含该类型Unit
			}
			else 
			{
				int addNum = _formations[teamId-1][posId-1].AddExchage (source);	
				ChangeUserUnitNum(sourceUnitId, -addNum);
				ChangeUserUnitNum(targetUnitId, target.num);
			}
		}

		return result;
	}

	// 下阵
	public RESULT Remove(Model_UnitGroup model_UnitGroup)
	{
		RESULT result = RESULT.SUCCESS;

		int teamId = model_UnitGroup.teamId;
		int posId = model_UnitGroup.posId;
		int unitId = model_UnitGroup.unitId;
		int num = model_UnitGroup.num;

		_formations [teamId-1] [posId-1].Remove (model_UnitGroup);
		ChangeUserUnitNum(unitId, num);

		return result;
	}

	// 交换
	public RESULT Exchange(Model_UnitGroup model_UnitGroup1, Model_UnitGroup model_UnitGroup2) 
	{
		RESULT result = RESULT.SUCCESS;

		int teamId1 = model_UnitGroup1.teamId;
		int posId1 = model_UnitGroup1.posId;

		int teamId2 = model_UnitGroup2.teamId;
		int posId2 = model_UnitGroup2.posId;

		Model_UnitGroup unitGroup1 = _formations [teamId1-1] [posId1-1];
		Model_UnitGroup unitGroup2 = _formations [teamId2-1] [posId2-1];

		unitGroup1.Exchange(unitGroup2);

		return result;
	}
	
	public  Model_UnitGroup GetUnitGroup(int teamId, int posId) // teamId 1-3  posId 1-6
	{
		if (teamId < 1 || teamId > 3 || posId < 1 || posId > 6) {
			return null;
		}
		return _formations [teamId-1] [posId-1];
	}

	public  Model_UnitGroup[] GetUnitTeam(int teamId) // teamId 1-3 
	{
		if (teamId < 1 || teamId > 3) {
			return null;
		}
		return _formations [teamId-1];
	}

	// 战斗损失unit 同步战斗中损失到阵型
	public void SyncBattleDamageUnit()
	{
		BattleGame game = BattleGame.instance;
		if (game != null)
		{
			int teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId();
			List<Unit> untis = game.unitGroup.GetPlayerDeadUnits ();

			foreach (Unit unit in untis)
			{
				int posId = unit.slotIndex + 1;
				int deadCount = unit.unit.GetDeadCount();

				Model_UnitGroup model_UnitGroup = GetUnitGroup(teamId, posId);
				if(model_UnitGroup != null)
				{
//					deadCount = Mathf.Min(deadCount, model_UnitGroup.maxNum); 
					model_UnitGroup.num -= deadCount;
					Assert.assert (model_UnitGroup.num >= 0);
				}
			}
		}

	}

	// team 是否包含Unit
	public bool IsTeamContaninUnit(int teamId, int unitId)
	{
		if (teamId < 1 || teamId > 3) {
			return false;
		}

		Model_UnitGroup[] teamUnits = _formations [teamId-1];
		for (int i = 0; i < teamUnits.Length; ++i) {
			Model_UnitGroup unit = teamUnits[i];
			if(!unit.isEmpty && unit.unitId == unitId)
			{
				return true;
			}
		}
		return false;
	}

	public int GetPosId(int teamId, int unitId)
	{
		Model_UnitGroup[] teamUnits = _formations [teamId-1];
		for (int i = 0; i < teamUnits.Length; ++i) {
			Model_UnitGroup unit = teamUnits[i];
			if(!unit.isEmpty && unit.unitId == unitId)
			{
				return unit.posId;
			}
		}
		return 0;
	}

	public Model_UnitGroup GetUnitGroupByUnitId(int teamId, int unitId)
	{
		Model_UnitGroup[] teamUnits = _formations [teamId-1];
		for (int i = 0; i < teamUnits.Length; ++i) {
			Model_UnitGroup unit = teamUnits[i];
			if(!unit.isEmpty && unit.unitId == unitId)
			{
				return unit;
			}
		}
		return null;
	}

	private bool IsEnoughUnitNum(Model_UnitGroup model_UnitGroup)
	{
		int unitId = model_UnitGroup.unitId;
		Model_User user = InstancePlayer.instance.model_User;

		Model_Unit model_Unit; 
		user.unlockUnits.TryGetValue(unitId, out model_Unit);

		if (model_Unit != null && model_Unit.num > 0) {
			return true;
		}
		return false;
	}

	private bool IsReachMaxBattleNum(Model_UnitGroup model_UnitGroup)
	{
		Model_UnitGroup unit = _formations [model_UnitGroup.teamId-1][model_UnitGroup.posId-1];

		if (unit.num >= unit.maxNum) {
			return true;
		}
		return false;
	}

	private void ChangeUserUnitNum(int unitId, int num)
	{
		Model_User user = InstancePlayer.instance.model_User;

		Model_Unit model_Unit; 
		user.unlockUnits.TryGetValue(unitId, out model_Unit);
		
		if (model_Unit != null) 
		{
			model_Unit.num += num;
		}
	}

	private void RecordLastFormations()
	{
		ClearFormations (_lastFormations);
		_lastFormations = ArrayHelper.Clone<Model_UnitGroup>(_formations);
	}

	private void BackLastFormations()
	{
		ClearFormations (_formations);
		_formations = ArrayHelper.Clone<Model_UnitGroup>(_lastFormations);
	}

	private void ClearFormations(Model_UnitGroup[][] formations)
	{
		if (formations != null) 
		{
			for (int i = 0; i < TEAM_COUNT; ++i) 
			{
				for(int j = 0; j < POSITION_COUNT; ++j)
				{
					formations[i][j] = null;
				}
				formations[i] = null;
			}
			formations = null;
		}
	}

	// 设置上阵队伍teamId
	public void SetSelectTeamId(int teamId)
	{
		if (teamId > 0 && teamId <= 3) 
		{
			_currentSelectTeamId = teamId;
			DataPersistent.teamId = teamId;
		}
	}
	public int GetSelectTeamId()
	{
		return _currentSelectTeamId;
//		return DataPersistent.teamId;
	}

	// Team 战力
	public int CalcPower()
	{
		int power = 0;

		int teamId = GetSelectTeamId ();
		Model_UnitGroup[] unitGroups = GetUnitTeam (teamId);

		if (unitGroups != null) {
			foreach(Model_UnitGroup unitGroup in unitGroups)
			{
				power += unitGroup.CalcPower ();
			}
		}

		return power;
	}

	// Team 补兵
	public RESULT TeamReplenish(int teamId)
	{
		int unitTypeCount = GetTeamUnitTypeCount ();
		if (unitTypeCount <= 0)
		{
			return RESULT.UNIT_TYPE_NONE;
		}

		Model_UnitGroup[] team = GetUnitTeam (teamId);
		if (team != null) 
		{
			foreach (Model_UnitGroup unit in team) 
			{
				int num = unit.Replenish();
				ChangeUserUnitNum(unit.unitId, -num);
			}
		}
		return RESULT.SUCCESS;
	}

	// 获取当前队伍上阵类型数量
	public int GetTeamUnitTypeCount()
	{
		int typeCount = 0;

		if (_currentSelectTeamId == 0) 
		{
			return typeCount;		
		}
		Model_UnitGroup[] unitGroups = GetUnitTeam (_currentSelectTeamId);

		for (int i = 0; i < unitGroups.Length; ++i) 
		{
			if(unitGroups[i] != null && !unitGroups[i].isEmpty)
			{
				++typeCount;
			}
		}
		return typeCount;	
	}

	// Unit 是否上阵
	public bool IsUnitOnDuty(int unitId)
	{
		bool onDuty = false;
		for (int i = 1; i <= TEAM_COUNT; ++i) 
		{
			onDuty = IsTeamContaninUnit(i, unitId);
			if(onDuty)
			{
				return onDuty;
			}
		}
		return onDuty;
	}

	// 当前队伍可用于战斗unit总数
	public int GetCurrentTeamUnitCount()
	{
		int memberCount = 0;

		if (_currentSelectTeamId == 0) 
		{
			return memberCount;		
		}
		Model_UnitGroup[] unitGroups = GetUnitTeam (_currentSelectTeamId);

		for (int i = 0; i < unitGroups.Length; ++i) 
		{
			if(unitGroups[i] != null && !unitGroups[i].isEmpty)
			{
				memberCount += unitGroups[i].num;
			}
		}
		return memberCount;	
	}

	// 生成战斗中player上阵数据
	public void CreatePlayerArmy()
	{
		Model_UnitGroup[] unitGroups = GetUnitTeam (_currentSelectTeamId);
		if (unitGroups != null) 
		{
			InstancePlayer.instance.playerArmy = new InstancePlayerArmy ();
			InstancePlayer.instance.playerArmy.memberCount = 6; 
			int memberCount = InstancePlayer.instance.playerArmy.memberCount;
			
			InstancePlayer.instance.playerArmy.unitId = new int[memberCount];
			InstancePlayer.instance.playerArmy.unitCount = new int[memberCount];
			InstancePlayer.instance.playerArmy.heroId = new int[memberCount];
			
			for (int i = 0; i < memberCount; ++i) 
			{
				if(unitGroups[i] != null && !unitGroups[i].isEmpty)
				{
					InstancePlayer.instance.playerArmy.unitId [i] = unitGroups[i].unitId;
					InstancePlayer.instance.playerArmy.unitCount [i] = unitGroups[i].num;
					InstancePlayer.instance.playerArmy.heroId [i] = unitGroups [i].heroId;
				}
			}
		}
	}

	//军官----------------------------------------------------------

	// 上阵
	public RESULT AddHero(Model_UnitGroup model_UnitGroup) 
	{
		RESULT result = RESULT.SUCCESS;

		int teamId = model_UnitGroup.teamId;
		int posId = model_UnitGroup.posId;
		int heroId = model_UnitGroup.heroId;

		if (IsTeamContaninHero (teamId, heroId)) 
		{
			result = RESULT.SAME_HERO;		// 队伍中已经包含该类型Hero
		} 
		else 
		{
			Model_UnitGroup formation_Unit = GetUnitGroup (teamId, posId);
			formation_Unit.AddHero (heroId);
		}

		return result;
	}

	 // 交换
	public RESULT ExchangeHero(Model_UnitGroup model_UnitGroup1, Model_UnitGroup model_UnitGroup2) 
	{
		RESULT result = RESULT.SUCCESS;

		int teamId1 = model_UnitGroup1.teamId;
		int posId1 = model_UnitGroup1.posId;

		int teamId2 = model_UnitGroup2.teamId;
		int posId2 = model_UnitGroup2.posId;

		Model_UnitGroup formation_Unit1 = GetUnitGroup (teamId1, posId1);
		Model_UnitGroup formation_Unit2 = GetUnitGroup (teamId2, posId2);

		formation_Unit1.ExchangeHero (formation_Unit2);

		return result;
	}

	// 下阵
	public RESULT RemoveHero(Model_UnitGroup model_UnitGroup)
	{
		RESULT result = RESULT.SUCCESS;

		int teamId = model_UnitGroup.teamId;
		int posId = model_UnitGroup.posId;

		Model_UnitGroup formation_Unit = GetUnitGroup (teamId, posId);
		if (formation_Unit != null) 
		{
			formation_Unit.RemoveHero ();
		}

		return result;
	}

	// team 是否包含Hero
	public bool IsTeamContaninHero(int teamId, int heroId)
	{
		Model_UnitGroup[] team = GetUnitTeam(teamId);
		if (team != null)
		{
			foreach (Model_UnitGroup unit_Group in team) 
			{
				if(unit_Group.heroId == heroId)
				{
					return true;
				}
			}
		}
		return false;
	}


	public int[] GetHeroesId(int teamId)
	{
		Model_UnitGroup[] unitGroup = GetUnitTeam (teamId);

		int[] heroesId = new int[unitGroup.Length];
		for (int i = 0; i < unitGroup.Length; ++i) {
			heroesId [i] = unitGroup [i].heroId;
		}

		return heroesId;
	}

	public int[] GetSelectTeamHeroesId()
	{
		int teamId = GetSelectTeamId ();
		return GetHeroesId (teamId);
	}


	public void CreateLocalData()
	{
		_currentSelectTeamId = 1;
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			if(i < 1)  // 创建team1 数据
			{
				for(int j = 0; j < POSITION_COUNT; ++j)
				{
					if(j < 4)
					{
						_formations[i][j].unitId = j + 1;
						_formations[i][j].num = 5;
						_formations[i][j].isEmpty = false;
						//_formations [i] [j].heroId = j + 1;
					}
				}
			}
		}
	}
}
