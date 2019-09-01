using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGameHelper {

	public class UnitLayout {
		
		public Vector3 min;
		public Vector3 max;
	}
	
	public static UnitLayout CalcMyAliveTanksLayout()
	{
		UnitLayout layout = null;
		
		BattleGame game = BattleGame.instance;
		List<Unit> tanks = game.unitGroup.myUnits;
		
		for(int i = 0; i < tanks.Count; ++i)
		{
			Unit unit = tanks[i];
			if(unit.isDead)
			{
				continue;
			}
			
			float length = unit.unit.dataUnit.length;
			
			if(layout == null)
			{
				layout = new UnitLayout();
				layout.min = unit.transform.position - new Vector3(length, length, length);
				layout.max = unit.transform.position + new Vector3(length, length, length);
			}
			else
			{
				layout.min.x = Mathf.Min(layout.min.x, unit.transform.position.x);
				layout.min.y = Mathf.Min(layout.min.y, unit.transform.position.y);
				layout.min.z = Mathf.Min(layout.min.z, unit.transform.position.z);
				
				layout.max.x = Mathf.Max(layout.max.x, unit.transform.position.x);
				layout.max.y = Mathf.Max(layout.max.y, unit.transform.position.y);
				layout.max.z = Mathf.Max(layout.max.z, unit.transform.position.z);
			}
			
		}
		
		return layout;
		
	}
	
	public static UnitLayout CalcAllAliveTanksLayout()
	{
		UnitLayout layout = null;
		
		BattleGame game = BattleGame.instance;
		List<Unit> tanks = game.unitGroup.allUnits;
		
		for(int i = 0; i < tanks.Count; ++i)
		{
			Unit unit = tanks[i];
			if(unit.isDead)
			{
				continue;
			}
			
			float length = unit.unit.dataUnit.length;
			
			if(layout == null)
			{
				layout = new UnitLayout();
				layout.min = unit.transform.position - new Vector3(length, length, length);
				layout.max = unit.transform.position + new Vector3(length, length, length);
			}
			else
			{
				layout.min.x = Mathf.Min(layout.min.x, unit.transform.position.x);
				layout.min.y = Mathf.Min(layout.min.y, unit.transform.position.y);
				layout.min.z = Mathf.Min(layout.min.z, unit.transform.position.z);
				
				layout.max.x = Mathf.Max(layout.max.x, unit.transform.position.x);
				layout.max.y = Mathf.Max(layout.max.y, unit.transform.position.y);
				layout.max.z = Mathf.Max(layout.max.z, unit.transform.position.z);
			}
			
		}
		
		return layout;
		
	}


	public static float GetPlayerTeamAmountHP()
	{
		return InstancePlayer.instance.battle.GetTeamTotalHP(DataConfig.TEAM.MY);
	}
	
	public static float GetPlayerTeamCurrentHP()
	{
		return InstancePlayer.instance.battle.GetTeamCurrentHP(DataConfig.TEAM.MY);
	}

	public static float GetEnemyTeamAmountHP()
	{
		return InstancePlayer.instance.battle.GetTeamTotalHP(DataConfig.TEAM.ENEMY);
	}
	
	public static float GetEnemyTeamCurrentHP()
	{
		return InstancePlayer.instance.battle.GetTeamCurrentHP(DataConfig.TEAM.ENEMY);
	}


	public static void PreloadAssets()
	{
		Object profab  = Resources.Load(AppConfig.FOLDER_PROFAB_EFFECT_EXPLODE + "Tank_Destroy");
	}


}
