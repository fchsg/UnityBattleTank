using UnityEngine;
using System.Collections;

public class InstanceTeam {
	public InstanceUnit[] units = new InstanceUnit[DataConfig.FORMATION_TOTAL_SLOT];

	/*
	public bool IsLoss()
	{
		bool loss = true;

		for (int i = 0; i < units.Length; ++i) {
			if(units[i].IsAlive())
			{
				loss = false;
				break;
			}
		}

		return loss;
	}
	*/

	public float GetInitialHP()
	{
		float teamHP = 0.0f;
		
		int unitCount = units.Length;
		for(int i = 0; i < unitCount; ++i)
		{
			InstanceUnit unit = units[i];
			if(unit != null)
			{
				teamHP += unit.complexBattleParam.hp;
			}
		}
		return teamHP;

	}

	public float GetCurrentHP()
	{
		float teamHP = 0.0f;

		int unitCount = units.Length;
		for(int i = 0; i < unitCount; ++i)
		{
			InstanceUnit unit = units[i];
			if(unit != null)
			{
				teamHP += Mathf.Max(0, unit.currentHp);
			}
		}
		return teamHP;

	}
}
