using UnityEngine;
using System.Collections;
using System;

using SlgPB;

public class Model_UnitGroup : ICloneable{

	public int unitId;	//类型ID
	public int num;		//数量
	public int posId;	//位置编号，1-6
	public int teamId;	//所属编组id，1-3
	public int heroId;

	public bool isEmpty; //是否是空位

	public int maxNum //上阵上限
	{
		get{  return CalcUnitCapacity(); }
	}

	//TODO
	public bool isUnlock = true; //是否解锁

	public Model_UnitGroup()
	{
		Empty ();
	}

	public Model_UnitGroup(int i, int j)
	{
		teamId = i + 1;
		posId = j + 1;	
			
		Empty ();
	}

	public void Parse(SlgPB.UnitGroup unitGroup)
	{
		unitId = unitGroup.unitId;
		num = unitGroup.num;		
		heroId = unitGroup.heroId;

		if (unitId > 0) 
		{
			isEmpty = false;
		}
	}

	public object Clone()
	{
		Model_UnitGroup clone = new Model_UnitGroup ();
		clone.unitId = unitId;	
		clone.num 	 = num;		
		clone.posId  = posId;	
		clone.teamId = teamId;	
		clone.isEmpty = isEmpty; 

		clone.heroId = heroId;

		return clone;
	}

	public void Empty()
	{
		unitId  = 0;
		num 	= 0;		
		isEmpty = true;
	}

	// 上阵
	public int Add(Model_UnitGroup unitGroup)
	{
		int needMax = maxNum - num;
		int realNum = Mathf.Min (needMax, unitGroup.num);

		num += realNum;
		unitId  = unitGroup.unitId;	
		isEmpty = false;

		return realNum;
	}

	// 上阵替换
	public int AddExchage(Model_UnitGroup unitGroup)
	{
		int realNum = Mathf.Min (maxNum, unitGroup.num);

		num = realNum;
		unitId  = unitGroup.unitId;	
		isEmpty = false;
		
		return realNum;
	}
	
	public void Remove(Model_UnitGroup unitGroup)
	{
		num -= unitGroup.num;

		if (num <= 0)
		{
			Empty();
		}
	}

	public void Exchange(Model_UnitGroup unitGroup)
	{
		if (this.isEmpty) 
		{
			num     = unitGroup.num;
			unitId  = unitGroup.unitId;	
			isEmpty = false;

			UpdateUnitNum (this);

			unitGroup.Empty();
		} 
		else if (unitGroup.isEmpty) 
		{
			unitGroup.num     = num;
			unitGroup.unitId  = unitId;	
			unitGroup.isEmpty = false;

			UpdateUnitNum (unitGroup);

			Empty();
		} 
		else
		{
			MathHelper.Swap (ref unitId, ref unitGroup.unitId);
			MathHelper.Swap (ref num, ref unitGroup.num);

			UpdateUnitNum (this);
			UpdateUnitNum (unitGroup);
		}
	}

	public SlgPB.UnitGroup ConvertSlgPBUnitGroup()
	{
		SlgPB.UnitGroup SlgPB_UnitGroup = new SlgPB.UnitGroup ();

		SlgPB_UnitGroup.teamId  = teamId;
		SlgPB_UnitGroup.posId 	  = posId;
		SlgPB_UnitGroup.unitId    = unitId;
		SlgPB_UnitGroup.num      = num;
		SlgPB_UnitGroup.heroId   = heroId;

		return SlgPB_UnitGroup;
	}
		
	// 补兵
	public int Replenish()
	{
		int realNum = 0;
		if (!isEmpty) 
		{
			Model_User model_User = InstancePlayer.instance.model_User;

			Model_Unit model_Unit;
			model_User.unlockUnits.TryGetValue (unitId, out model_Unit);
			if (model_Unit != null) 
			{
				int hadNum = model_Unit.num;
				int needMax = maxNum - num;
				realNum = Mathf.Min (needMax, hadNum);

				num += realNum;
			}
		}

		return realNum;
	}

	// 战斗力
	public int CalcPower()
	{
		int power = 0;

		if (!isEmpty) {
			power = Model_Unit.CalcUnitPower (unitId, num, heroId);
		} 

		return power;
	}


// 军官 -----------------------------------------

	public void ExchangeHero(Model_UnitGroup unitGroup)
	{
		MathHelper.Swap (ref heroId, ref unitGroup.heroId);


		UpdateUnitNum (this);
		UpdateUnitNum (unitGroup);
	}
		
	public void AddHero(int id)
	{
		heroId =id;
	}

	public void RemoveHero()
	{
		heroId = 0;
		UpdateUnitNum (this);
	}

	public bool HasHero()
	{
		if (heroId > 0)
			return true;
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


	// =================================================
	// capacity

	public int CalcUnitCapacity()
	{
		int capacity = InstancePlayer.instance.model_User.CalcPlayerUnitCapacity ();

		if (heroId > 0) {
			int heroLeadership = CalcHeroLeadership (heroId, unitId);
			capacity += heroLeadership;
		}

		return capacity;

	}

	public int CalcHeroLeadership(int heroId, int unitId)
	{
		SlgPB.Hero hero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);

		DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, hero.exp, hero.stage);
		return dataHero.leadership;

	}

	private void UpdateUnitNum(Model_UnitGroup unitGroup)
	{
		if (unitGroup.num > unitGroup.maxNum)
		{
			ChangeUserUnitNum (unitGroup.unitId, unitGroup.num - unitGroup.maxNum);
			unitGroup.num = unitGroup.maxNum;
		}
	}

}
