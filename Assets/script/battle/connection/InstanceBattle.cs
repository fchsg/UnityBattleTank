using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstanceBattle {

	public DataMission mission;
	public InstanceTeam myTeam;
	public int extraTeamCount;
	public InstanceTeam[] enemyTeams;
	public InstanceTeam[] friendTeams;
	public DataMap dataMap;

	public Dictionary<int, SlgPB.Hero> pbHeroesMap;
	public Dictionary<int, SlgPB.Unit> pbUnitsMap;

	private void InitPvpDataMap(SlgPB.PVPUser pvpUser)
	{
		pbHeroesMap = new Dictionary<int, SlgPB.Hero> ();
		foreach (SlgPB.Hero hero in pvpUser.heroes) {
			pbHeroesMap [hero.heroId] = hero;
		}

		pbUnitsMap = new Dictionary<int, SlgPB.Unit> ();
		foreach (SlgPB.Unit unit in pvpUser.units) {
			pbUnitsMap [unit.unitId] = unit;
		}


	}

	public void ImportFromPvp(SlgPB.PVPUser pvpUser)
	{
		InitPvpDataMap (pvpUser);

		mission = new DataMission ();
		mission.asset = DataMission.BK_NAMES [(int)RandomHelper.Range (0, DataMission.BK_NAMES.Length)];

		extraTeamCount = 1;
		enemyTeams = new InstanceTeam[extraTeamCount];
		friendTeams = new InstanceTeam[extraTeamCount];

		InstanceTeam instanceTeam = new InstanceTeam();
		instanceTeam.units = new InstanceUnitPvp[DataConfig.FORMATION_TOTAL_SLOT];

		int memberCount = pvpUser.unitGroups.Count;
		for (int i = 0; i < memberCount; ++i) {
			SlgPB.UnitGroup group = pvpUser.unitGroups [i];

			int unitId = group.unitId;
			int unitCount = group.num;
			int heroId = group.heroId;
			float powerScale = 1;

			if(unitId > 0 && unitCount > 0)
			{
				SlgPB.Hero pbHero = null;
				if (pbHeroesMap.ContainsKey (heroId)) {
					pbHero = pbHeroesMap [heroId];
				}

				SlgPB.Unit pbUnit = null;
				if (pbUnitsMap.ContainsKey (unitId)) {
					pbUnit = pbUnitsMap [unitId];
				}

				InstanceUnitPvp instanceUnitPvp = new InstanceUnitPvp(false, heroId, unitId, unitCount, powerScale, pbHero, pbUnit);
				instanceUnitPvp.Init();

				int slotId = group.posId - 1;
				instanceTeam.units[slotId] = instanceUnitPvp;
			}
		}

		enemyTeams [0] = instanceTeam;


	}

	public void ImportFromLevel(DataMission mission)
	{
		this.mission = mission;

		extraTeamCount = mission.teamsId.Length;
		enemyTeams = new InstanceTeam[extraTeamCount];
		friendTeams = new InstanceTeam[extraTeamCount];

		for (int teamIndex = 0; teamIndex < extraTeamCount; ++teamIndex) {
			int teamId = mission.teamsId[teamIndex];
			if(teamId != 0)
			{
				int memberCount = mission.GetMemberCount(teamIndex);
				int[] unitsId = mission.GetUnitsId(teamIndex);
				int[] unitsCount = mission.GetUnitsCount(teamIndex);
				
				InstanceTeam instanceTeam = new InstanceTeam();

				for (int i = 0; i < memberCount; ++i) {
					int unitId = unitsId[i];
					int unitCount = unitsCount[i];
					int heroId = 0;
					float powerScale = mission.powerScale;

					if(unitId > 0 && unitCount > 0)
					{
						InstanceUnit instanceUnit = new InstanceUnit(false, heroId, unitId, unitCount, powerScale);
						instanceTeam.units[i] = instanceUnit;
						instanceUnit.Init();
					}
				}

				if(teamId > 0)
				{
					enemyTeams[teamIndex] = instanceTeam;
				}
				else
				{
					friendTeams[teamIndex] = instanceTeam;
				}
			}
		}

	}

	public void ImportFromPlayer()
	{
		InstancePlayerArmy playerArmy = InstancePlayer.instance.playerArmy;

		InstanceTeam instanceTeam = new InstanceTeam();
		for (int i = 0; i < playerArmy.memberCount; ++i) {
			int unitId = playerArmy.unitId[i];
			int unitCount = playerArmy.unitCount[i];
			int heroId = playerArmy.heroId [i];
			float powerScale = 1;

			if(unitId > 0 && unitCount > 0)
			{
				InstanceUnit instanceUnit = new InstanceUnit(true, heroId, unitId, unitCount, powerScale);
				instanceTeam.units[i] = instanceUnit;
				instanceUnit.Init();
			}
		}
		
		myTeam = instanceTeam;

	}

	public void ImportMap()
	{
		dataMap = DataManager.instance.dataMapGroup.QueryMap (2);
	}

	/*
	public InstanceBattle Duplicate()
	{
		InstanceBattle cloneBattle = new InstanceBattle ();

		InstanceUnit[] myUnits = this.myTeam.units;
		InstanceUnit[] enemyUnits = this.enemyTeam.units;	

		cloneBattle.enemyTeam = new InstanceTeam();
		for (int i = 0; i < enemyUnits.Length; ++i) 
		{
			InstanceUnit enemyUnit = enemyUnits[i];
			if(enemyUnit != null)
			{
				int unitId = enemyUnit.unitId;
				int unitCount = enemyUnit.unitCount;
				float powerScale = enemyUnit.powerScale;

				InstanceUnit instanceUnit = new InstanceUnit(unitId, unitCount, powerScale);
				cloneBattle.enemyTeam.units[i] = instanceUnit;
				instanceUnit.Init();
			}
		}

		cloneBattle.myTeam = new InstanceTeam();
		for (int i = 0; i < myUnits.Length; ++i)
		{		
			InstanceUnit myUnit = myUnits[i];
			if(myUnit != null)
			{
				int unitId = myUnit.unitId;
				int unitCount = myUnit.unitCount;
				float powerScale = myUnit.powerScale;

				InstanceUnit instanceUnit = new InstanceUnit(unitId, unitCount, powerScale);
				cloneBattle.myTeam.units[i] = instanceUnit;
				instanceUnit.Init();
			}
		}

		return cloneBattle;
	}
	*/

	/*
	public InstanceTeam GetTeam(DataConfig.TEAM team)
	{
		InstanceTeam instanceTeam = null;
		
		if (team == DataConfig.TEAM.MY) {
			instanceTeam = myTeam;
		} else if (team == DataConfig.TEAM.ENEMY) {
			instanceTeam = enemyTeam;
		} else {
			Assert.assert(false);
		}

		return instanceTeam;
	}

	public InstanceTeam GetOppositeTeam(DataConfig.TEAM team)
	{
		InstanceTeam instanceOppositeTeam = null;
		
		if (team == DataConfig.TEAM.MY) {
			instanceOppositeTeam = enemyTeam;
		} else if (team == DataConfig.TEAM.ENEMY) {
			instanceOppositeTeam = myTeam;
		} else {
			Assert.assert(false);
		}
		
		return instanceOppositeTeam;
	}
	*/

	/*
	public InstanceUnit GetUnit(AttackQueue.MEMBER member)
	{
		InstanceUnit instanceUnit = null;

		if (member.team == DataConfig.TEAM.MY) {
			instanceUnit = myTeam.units[member.slotId];
		} else if (member.team == DataConfig.TEAM.ENEMY) {
			instanceUnit = enemyTeam.units[member.slotId];
		} else {
			Assert.assert(false);
		}

		return instanceUnit;
	}
	*/

	public float GetTeamTotalHP(DataConfig.TEAM team)
	{
		if (team == DataConfig.TEAM.MY) {
			return myTeam.GetInitialHP();
		}
		else if (team == DataConfig.TEAM.FRIEND) {
			float hp = 0;
			for(int i = 0; i < friendTeams.Length; ++i)
			{
				if(friendTeams[i] != null)
				{
					hp += friendTeams[i].GetInitialHP();
				}
			}
			return hp;
		}
		else {
			float hp = 0;
			for(int i = 0; i < enemyTeams.Length; ++i)
			{
				if(enemyTeams[i] != null)
				{
					hp += enemyTeams[i].GetInitialHP();
				}
			}
			return hp;
		}
	}

	public float GetTeamCurrentHP(DataConfig.TEAM team)
	{
		if (team == DataConfig.TEAM.MY) {
			return myTeam.GetCurrentHP();
		}
		else if (team == DataConfig.TEAM.FRIEND) {
			float hp = 0;
			for(int i = 0; i < friendTeams.Length; ++i)
			{
				if(friendTeams[i] != null)
				{
					hp += friendTeams[i].GetCurrentHP();
				}
			}
			return hp;
		}
		else {
			float hp = 0;
			for(int i = 0; i < enemyTeams.Length; ++i)
			{
				if(enemyTeams[i] != null)
				{
					hp += enemyTeams[i].GetCurrentHP();
				}
			}
			return hp;
		}
	}

	public bool IsPlayerWin()
	{
		return GetTeamCurrentHP(DataConfig.TEAM.ENEMY) <= 0;
	}

	public bool IsPlayerLoss()
	{
		return GetTeamCurrentHP(DataConfig.TEAM.MY) <= 0;
	}
	

}
