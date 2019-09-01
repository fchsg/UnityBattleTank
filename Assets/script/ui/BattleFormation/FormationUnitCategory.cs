using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FormationUnitCategory {
	
	public const int UNIT_SLOT_NUM = 6;

	public class Unit : IComparable<Unit>{
		public int id;
		public int num;             // 当前未分配数量
		public int formation_num;   // 阵型中使用的数量
		public string name;         
		public string small_icon; // 展示静态图片
		public string big_icon;   // 展示静态图片 

		public bool isOnDuty;   // 是否上阵

		public int Weight = 0;  // 按照unit所在slot位置排序

		public int CompareTo(Unit unit)
		{
			if (Weight > unit.Weight) {
				return -1;
			}
			else if (Weight < unit.Weight) {
				return 1;
			}
			else  {
				return 0;
			}
		}
	}

	public List<Unit> all_Units = new List<Unit> ();
	public List<Unit> gun_Units = new List<Unit> ();
	public List<Unit> cannon_Units = new List<Unit> ();
	public List<Unit> missile_Units = new List<Unit> ();
	public List<Unit> howziter_Units = new List<Unit> ();

	public FormationUnitCategory(Dictionary<int, Model_Unit> userUnits)
	{
		if (userUnits != null && userUnits.Count == 0) 
		{
		    CreateLocalTestUnits ();		
		}

		UpdateData (userUnits);
	}

	public void UpdateData(Dictionary<int, Model_Unit> userUnits)
	{
		List<Model_Unit> units = new List<Model_Unit> ();
		foreach(Model_Unit model_Unit in userUnits.Values)
		{
			units.Add(model_Unit);
		}
	
		all_Units.Clear ();
		gun_Units.Clear ();
		cannon_Units.Clear ();
		missile_Units.Clear ();
		howziter_Units.Clear ();

		foreach (Model_Unit model_Unit in units) 
		{
			DataUnit data_Unit = DataManager.instance.dataUnitsGroup.GetUnit(model_Unit.unitId);

			FormationUnitCategory.Unit unit = new FormationUnitCategory.Unit();
			unit.id = model_Unit.unitId; 
			unit.num = model_Unit.num;
			unit.name = data_Unit.name;
			unit.small_icon = UICommon.UNIT_SMALL_ICON_PATH + unit.id;

			all_Units.Add(unit);

			switch(data_Unit.bulletType)
			{
			case DataConfig.BULLET_TYPE.GUN:
				gun_Units.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.CANNON:
				cannon_Units.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.MISSILE:
				missile_Units.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.HOWITZER:
				howziter_Units.Add(unit);
				break;
			default:
				break;
			}

		}
	}

	private void UpdateUnitsOnDuty(List<FormationUnitCategory.Unit> units)
	{
		foreach (FormationUnitCategory.Unit unit in units) 
		{
			Model_Formation model_Formation = InstancePlayer.instance.model_User.model_Formation;
			int teamId = model_Formation.GetSelectTeamId();
			unit.isOnDuty = model_Formation.IsTeamContaninUnit(teamId ,unit.id);
			if(unit.isOnDuty)
			{
				int slotId = model_Formation.GetPosId(teamId ,unit.id);
				unit.Weight = UNIT_SLOT_NUM - slotId;
			}
			else
			{
				unit.Weight = -1;
			}
		}
	}

	public List<FormationUnitCategory.Unit> GetSortUnit(int index)
	{
		Model_User model_User = InstancePlayer.instance.model_User;
		UpdateData (model_User.unlockUnits);

		List<FormationUnitCategory.Unit>  units = new List<FormationUnitCategory.Unit>();
		switch (index) {
		case 0:
			units = all_Units;
			break;
		case 1:
			units = gun_Units;
			break;
		case 2:
			units = cannon_Units;
			break;
		case 3:
			units = missile_Units;
			break;
		case 4:
			units = howziter_Units;
			break;
		}

		UpdateUnitsOnDuty (units);
		units.Sort ();

		return units;
	}

	// 创建本地测试数据
	private Dictionary<int, Model_Unit> CreateLocalTestUnits()
	{
		Dictionary<int, Model_Unit> model_Units = InstancePlayer.instance.model_User.unlockUnits;
		DataUnit[] allDataUnits = DataManager.instance.dataUnitsGroup.GetAllUnits ();
		for (int i = 0; i < allDataUnits.Length; ++i) 
		{
			DataUnit dataUnit = allDataUnits[i];
			Model_Unit model_Unit = new Model_Unit();

			model_Unit.unitId = dataUnit.id;
			model_Unit.num = (int)RandomHelper.Range(50,200);

			model_Units.Add(model_Unit.unitId, model_Unit);
		}

		return model_Units;
	}

}
