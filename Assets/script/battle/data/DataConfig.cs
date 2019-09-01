using UnityEngine;
using System.Collections;

public class DataConfig {

	public enum DATA_TYPE
	{
		Unit = 1,
		UnitPart = 2,
		Building = 3,
		Mission = 4,
		Battle = 5,
		DropGroup = 6,
		Food = 7,
		Oil = 8,
		Metal = 9,
		Rare = 10,
		Cash = 11,
		Exp = 12,
		Energy = 13,
		Equipment = 14,
		Item = 15,
		Honor = 16,
		Hero = 17,
		Combat = 18,

	}

	public enum BULLET_TYPE
	{
		GUN,
		CANNON,
		MISSILE,
		HOWITZER,
	};

	public enum BODY_TYPE
	{
		CAR,
		CAR_WITH_CANNON,
	};

	public enum TEAM
	{
		MY,
		ENEMY,
		FRIEND,
	};
	public static TEAM GetOppositeTeamSide(TEAM team)
	{
		return (team == TEAM.MY) ? TEAM.ENEMY : TEAM.MY;
	}

	public enum MISSION_DIFFICULTY
	{
		NORMAL,
		ELITE,
	}


	public enum EQUIPMENT
	{
		CANNON,
		AMMO,
		ENGINE,
		AIMMING,
	}


	public enum ITEM_CATEGORY
	{
		NORMAL,
		CONSUME,
	}


	public const int FORMATION_TOTAL_SLOT = 6;
	/*
	public const int FORMATION_LINE_SLOT = 3;
	public static int FORMATION_TOTAL_LINES
	{
		get { return (int)Mathf.Ceil(FORMATION_TOTAL_SLOT / FORMATION_LINE_SLOT); }
	}
	public static int CalcSlotCol(int slot)
	{
		return (int)(slot % FORMATION_LINE_SLOT);
	}
	public static int CalcSlotRow(int slot)
	{
		return (int)(slot / FORMATION_LINE_SLOT);
	}
	*/

	public enum TARGET_SELECT
	{
		UNKNOWN = -1,
		RANDOM,
		CLOSEST,
		CLOSEST_RANDOM,
		CENTER,
		CENTER_RANDOM,
	}

//	public enum TARGETS_SELECT_TYPE
//	{
//		//CONTRAPOSITION, //keeping attack opposite target until its dead
//		//CONSTANT, //random select one target, keeping attack it until its dead
//		FLOATING, //random select target per fire
//		AOE,
//		CONSTANT //random select one target, keeping attack it until its dead
//	};
	

	public enum CAMPAIGN_ARROW
	{
		UNKNOWN = -1,

		A_UPWARD,
		B_UPWARD_TURN_LEFT,
		C_UPWARD_TURN_RIGHT,

		D_UPWARD_BIG,
		E_UPWARD_TURN_LEFT_BIG,
		F_UPWARD_TURN_RIGHT_BIG,
	}

	public static string GetDataTypeName(DATA_TYPE type)
	{
		switch (type)
		{
		case DATA_TYPE.Food:
			return "食物";
		case DataConfig.DATA_TYPE.Oil:
			return "石油";
		case DataConfig.DATA_TYPE.Metal:
			return "矿产";
		case DataConfig.DATA_TYPE.Rare:
			return "稀土";
		case DataConfig.DATA_TYPE.Cash:
			return "金币";
		case DataConfig.DATA_TYPE.Exp:
			return "经验";
		case DataConfig.DATA_TYPE.Energy:
			return "体力";
		case DataConfig.DATA_TYPE.Honor:
			return "功勋";
		case DataConfig.DATA_TYPE.Combat:
			return "战绩";
		}

		return "";
	}

}
