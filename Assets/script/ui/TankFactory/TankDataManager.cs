using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
/// <summary>
/// unit 排序数据管理
/// </summary>
public class TankDataManager  {
	public class UnitData{
		public int id;
		public bool isUnLock;
		public bool isCanUnlock;
		public bool isFormation;
		public bool isProduct;
		public int quality;
		public string iconName;
		public string iconBgName;
		public Color nameColor;
		public DataUnit unitData;
		public DataConfig.BULLET_TYPE unitType;
	}
	//		0 主战坦克 GUN,
	//		1 装甲战车 CANNON,
	//		2 特殊战车 MISSILE,
	//		3 自行火炮 HOWITZER,
	private DataUnit[] unitsGroup;
	private List<int> unlockUnits ;
	// 单位列表
	private Dictionary<int, Model_Unit> model_units ;
	// 阵型
	private Model_Formation model_Formation;
	private Model_ItemGroup model_itemGroup;
	private List<UnitData> allList = new List<UnitData>();
	private List<UnitData> gunList = new List<UnitData>();
	private List<UnitData> cannonList = new List<UnitData>();
	private List<UnitData> missileList = new List<UnitData>();
	private List<UnitData> howitzerList = new List<UnitData>();
	public TankDataManager()
	{
		UpdateData();
	}

	public void UpdateData()
	{
		unitsGroup = DataManager.instance.dataUnitsGroup.GetAllUnits();
		unlockUnits = InstancePlayer.instance.model_User.unlockUnitsId;
		model_units = InstancePlayer.instance.model_User.unlockUnits;
		model_Formation = InstancePlayer.instance.model_User.model_Formation;
		model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
		allList.Clear();
		gunList.Clear();
		cannonList.Clear();
		missileList.Clear();
		howitzerList.Clear();

		int unitsGroupCount = unitsGroup.Length;
		for(int i = 0 ;i < unitsGroupCount ; i++)
		{
			if(unitsGroup[i].buildingLevel == 0)continue;
			UnitData unit = InitUnitData(unitsGroup[i]);
			allList.Add(unit);

			switch(unitsGroup[i].bulletType)
			{
			case DataConfig.BULLET_TYPE.GUN:
				gunList.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.CANNON:
				cannonList.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.MISSILE:
				missileList.Add(unit);
				break;
			case DataConfig.BULLET_TYPE.HOWITZER:
				howitzerList.Add(unit);
				break;
			default:
				break;
			}
		}
	}


	public List<UnitData> InitUnitDataList(List<DataUnit> dataUnitList)
	{
		if(dataUnitList != null)
		{
			List<UnitData> unitDataList = new List<UnitData>();
			unitDataList.Clear();
			foreach(DataUnit unitData in dataUnitList)
			{
				UnitData unit = InitUnitData(unitData);
				if(unit != null)
				{
					unitDataList.Add(unit);
				}
			}
			return unitDataList;
		}
		return null;
	}
	/// <summary>
	/// 强化部件战车排序
	/// </summary>
	/// <returns>The sort.</returns>
	/// <param name="dataUnitList">Data unit list.</param>
	public List<UnitData> StrengthenSort(List<DataUnit> dataUnitList)
	{
		List<UnitData> strengthenSortList = new List<UnitData>();
		strengthenSortList.Clear(); 
		//上阵坦克 
		List<UnitData> formationUnit = new List<UnitData>();
		formationUnit.Clear();
		// 未上阵坦克
		List<UnitData> noFormationUnit = new List<UnitData>();
		noFormationUnit.Clear();

		if(dataUnitList != null)
		{ 
			foreach(UnitData unitdata in InitUnitDataList(dataUnitList) )
			{
				if(unitdata.isFormation)
				{
					formationUnit.Add(unitdata);
				}
				else
				{
					noFormationUnit.Add(unitdata);
				}
			} 
			//上阵的坦克品质排序
			formationUnit = QualitySorting(formationUnit,true);
			//未上阵的坦克品质排序
			noFormationUnit = QualitySorting(noFormationUnit,true);

			strengthenSortList.AddRange(formationUnit);
			strengthenSortList.AddRange(noFormationUnit);
		 
			return strengthenSortList;
		}
		return null;
	}
	public UnitData InitUnitData(DataUnit dataUnit)
	{
		if(dataUnit != null)
		{
			unlockUnits = InstancePlayer.instance.model_User.unlockUnitsId;
			model_units = InstancePlayer.instance.model_User.unlockUnits;
			model_Formation = InstancePlayer.instance.model_User.model_Formation;
			model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
			UnitData unit = new UnitData();
			unit.id = dataUnit.id;
			unit.quality = dataUnit.quality;
			unit.unitData = dataUnit;
			if(unlockUnits.Contains(dataUnit.id))
			{
				unit.isUnLock = true;
			}
			else
			{
				unit.isUnLock = false;
			}
			unit.iconName = UICommon.UNIT_SMALL_ICON_PATH + dataUnit.id;
			unit.iconBgName = UICommon.UNIT_ICON_BG + dataUnit.quality;
			switch((int)dataUnit.quality)
			{
			case 0:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_0;
				break;
			case 1:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_1;
				break;
			case 2:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_2;
				break;
			case 3:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_3;
				break;
			case 4:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_4;
				break;
			case 5:
				unit.nameColor = UICommon.UNIT_NAME_COLOR_5;
				break;
			default :
				unit.nameColor = UICommon.UNIT_NAME_COLOR_0;
				break;

			}

			Model_Unit model_unit;
			model_units.TryGetValue(dataUnit.id,out model_unit);
			if(model_unit != null && model_unit.onProduce > 0)
			{
				unit.isProduct = true;
			}
			else
			{
				unit.isProduct = false;
			}
			unit.isFormation = model_Formation.IsUnitOnDuty(dataUnit.id);
			unit.unitType = dataUnit.bulletType;
			//isCanUnlock 是否能解锁
			Item item = model_itemGroup.QueryItem(dataUnit.chipId);
			if(item.num >= dataUnit.chipCount)
			{
				unit.isCanUnlock = true;
			}
			else
			{
				unit.isCanUnlock = false;
			}
			return unit;
		}
		return null;
	}
	public List<UnitData> GetGunSortData()
	{
		UpdateData();
		return	UnitSorting(gunList);
	}

	public List<UnitData> GetCannonSortData()
	{
		UpdateData();
		return	UnitSorting(cannonList);
	}

	public List<UnitData> GetMissileSortData()
	{
		UpdateData();
		return	UnitSorting(missileList);
	}
	
	public List<UnitData> GetHowitzerSortData()
	{
		UpdateData();
		return	UnitSorting(howitzerList);
	}
	/// <summary>
	/// 战车生产排序
	/// </summary>
	/// <returns>The sorting.</returns>
	/// <param name="dataList">Data list.</param>
	List<UnitData> UnitSorting(List<UnitData> dataList)
	{
		List<UnitData> newUnit = new List<UnitData>();
		newUnit.Clear();
		//生产中的坦克列表
		List<UnitData> productUnit = new List<UnitData>();
		productUnit.Clear();
		//上阵坦克 
		List<UnitData> formationUnit = new List<UnitData>();
		formationUnit.Clear();
		// 未上阵坦克
		List<UnitData> noFormationUnit = new List<UnitData>();
		noFormationUnit.Clear();

		//可以解锁的坦克列表
		List<UnitData> canLockUnit = new List<UnitData>();
		canLockUnit.Clear();
		//不可以解锁的坦克列表
		List<UnitData> noCanLockUnit = new List<UnitData>();
		noCanLockUnit.Clear();
		if(dataList != null)
		{
			foreach(UnitData unitdata in dataList )
			{
				if(unitdata.isUnLock)
				{
					if(unitdata.isProduct)
					{
						productUnit.Add(unitdata);
					}
					else
					{
						if(unitdata.isFormation)
						{
							formationUnit.Add(unitdata);
						}
						else
						{
							noFormationUnit.Add(unitdata);
						}
					}
				}
				else
				{
					if(unitdata.isCanUnlock)
					{
						canLockUnit.Add(unitdata);
					}
					else
					{
						noCanLockUnit.Add(unitdata);
					}
				}
			} 
			//品质排序 
			productUnit = QualitySorting(productUnit,false);
			//生产中的时间排序
			productUnit.Sort(TimeSortCompare);

			//解锁之后未生产 但上阵的坦克品质排序
			formationUnit = QualitySorting(formationUnit,false);
			//解锁之后未生产 未上阵的坦克品质排序
			noFormationUnit = QualitySorting(noFormationUnit,false);
//			noFormationUnit.Sort(IDSortCompare);

			canLockUnit = QualitySorting(canLockUnit,false);
			canLockUnit.Sort(IDSortCompare);

			noCanLockUnit = QualitySorting(noCanLockUnit,true);
			newUnit.AddRange(productUnit);
			newUnit.AddRange(formationUnit);
			newUnit.AddRange(noFormationUnit);
			newUnit.AddRange(canLockUnit);
			newUnit.AddRange(noCanLockUnit);

			return newUnit;

		}
		return null;
	}
	 
	/// <summary>
	/// 按品质升序或者降序
	/// </summary>
	/// <returns>The sorting.</returns>
	/// <param name="dataList">Data list.</param>
	/// <param name="isLowHigh">If set to <c>true</c> is low high.</param>
	List<UnitData>  QualitySorting(List<UnitData> dataList,bool isLowHigh)
	{
		if(dataList != null)
		{
			if(isLowHigh)
			{
				dataList.Sort(QualitySortCompareASCE);
			}
			else
			{
				dataList.Sort(QualitySortCompareDESC);
			}
			return dataList;

		}
		return null;
	}
	/// <summary>
	/// Qualities the sort compare 升序 .
	/// </summary>
	/// <returns>The sort compare ASC.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int QualitySortCompareASCE(UnitData obj1, UnitData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality > obj2.quality)
		{
			res = 1;
		}
		else if (obj1.quality < obj2.quality)
		{
			res = -1;
		}
		return res;
	}
	/// <summary>
	/// Qualities the sort compare  降序.
	/// </summary>
	/// <returns>The sort compare DES.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int QualitySortCompareDESC(UnitData obj1, UnitData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality > obj2.quality)
		{
			res = -1;
		}
		else if (obj1.quality < obj2.quality)
		{
			res = 1;
		}
		return res;
	}
	/// <summary>
	/// 生产剩余的时间排序
	/// </summary>
	/// <returns>The sort compare.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int TimeSortCompare(UnitData obj1, UnitData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality == obj2.quality)
		{
			Model_Unit unit1;
			Model_Unit unit2;
			Dictionary<int, Model_Unit> modelUnits = InstancePlayer.instance.model_User.unlockUnits;
			modelUnits.TryGetValue(obj1.id,out unit1);
			modelUnits.TryGetValue(obj2.id,out unit2);
			if(unit1.produceLeftTime >= unit2.produceLeftTime )
			{
				res = -1;
			}
			else
			{
				res = 1;
			}

		}
		 
		return res;
	}
	/// <summary>
	/// 按照id大小
	/// </summary>
	/// <returns>The sort compare.</returns>
	/// <param name="obj1">Obj1.</param>
	/// <param name="obj2">Obj2.</param>
	private static int IDSortCompare(UnitData obj1, UnitData obj2)
	{
		int res = 0;
		if ((obj1 == null) && (obj2 == null))
		{
			return 0;
		}
		else if ((obj1 != null) && (obj2 == null))
		{
			return 1;
		}
		else if ((obj1 == null) && (obj2 != null))
		{
			return -1;
		}
		if (obj1.quality == obj2.quality)
		{
			 
			if (obj1.id > obj2.id)
			{
				res = 1;
			}
			else if (obj1.id < obj2.id)
			{
				res = -1;
			}
			
		}
		
		return res;
	}

}
