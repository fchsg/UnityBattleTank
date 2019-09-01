using UnityEngine;
using System.Collections;

public class FormationUIController : MonoBehaviour {

	private int _commanderLevel = 60; // 指挥官等级
	public int commanderLevel
	{
		set { _commanderLevel = value; }
		get { return _commanderLevel; }
	}

	// 判定当前当前slot是否解锁
	public bool IsUnlockSlot(int slotId)
	{
		return true;
	}

	//  判定当前当前team是否解锁
	public bool IsUnlockTeam(int teamId)
	{
		return true;
	}
		
	 // 上阵战车基础数量
	public int GetBattleUnitBasicCount()
	{
		int level = InstancePlayer.instance.model_User.honorLevel;
		DataLeader leader = DataManager.instance.dataLeaderGroup.GetLeader (level);
		return leader.Leadership;
	}

	//  获取当前slot 所需指挥官等级 slotId(1-6)
	public int GetNeedLevelUnlcokSlot(int slotId)   
	{
		switch (slotId) 
		{
		case 1:
			return 0;
			break;
		case 2:
			return 10;
			break;
		case 3:
			return 15;
			break;
		case 4:
			return 20;
			break;
		case 5:
			return 25;
			break;
		case 6:
			return 30;
			break;
		}
		return 0;
	}

	// 最大战队编组数量
	public int GetBatteTeamMax()
	{
		if (_commanderLevel < 30) 
		{
			return 1;
		} 
		else if (_commanderLevel < 60) 
		{
			return 2;
		}
		else 
		{
			return 3;
		}
	}

	// 获取战队编组所需指挥官等级 teamId(1-3)
	public int GetNeedLevelUnlockTeam(int teamId)
	{
		switch (teamId) 
		{
		case 1:
			return 0;
			break;
		case 2:
			return 30;
			break;
		case 3:
			return 60;
			break;
		}
		return 0;
	}

	
}
