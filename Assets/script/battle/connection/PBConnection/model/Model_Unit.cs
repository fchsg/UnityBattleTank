using UnityEngine;
using System.Collections;

public class Model_Unit {

	public int unitId;//类型ID
	public int num;//可分配给战斗队伍的数量
	public int onDamaged;//待维修的数量
	public int onProduce;//建造中的数量
	public int onRepair;//修理中的数量

	public int produceTotalTime;//当前建造总时间（秒）
	public int produceLeftTime  //距离建造结束时间（秒)
	{
		get
		{
			int leftTime = Mathf.CeilToInt(TimeHelper.GetLeftSecondsToEndTimestamp(nextProduceEndTimestamp));
			return leftTime;
		}
	}

	private long nextProduceEndTimestamp;
	private int produceEndTimeSec;

	public int[] partLevels = new int[]{1, 1, 1, 1};


	public Model_Unit()
	{
	}

	public void Parse(SlgPB.Unit unit)
	{
		unitId = unit.unitId;
		num = unit.num;
		onProduce = unit.onProduce;
		produceEndTimeSec = unit.produceEndTime;
		produceTotalTime = unit.produceTotalTime;
		onRepair = unit.onRepair;

		onDamaged = unit.onDamaged;

		if (onProduce > 0) 
		{
			nextProduceEndTimestamp = TimeHelper.GetCurrentRealTimestamp() + produceEndTimeSec * 1000;
		} 
		else 
		{
			nextProduceEndTimestamp = 0;
		}

		partLevels = unit.unitPartLevel.ToArray ();
		Assert.assert (partLevels.Length > 0);

	}

	public DataUnitPart[] GetDataParts()
	{
		DataUnitPart[] parts = new DataUnitPart[4];

		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
		DataUnitPartGroup partGroup = DataManager.instance.dataUnitPartGroup;

		for (int i = 0; i < parts.Length; ++i) {
			int partId = dataUnit.partsId[i];
			int partLevel = partLevels[i];
			parts[i] = partGroup.GetPart (partId, partLevel);
		}

		return parts;
	}

	// ========================================================================
	// helper


	public enum UPGRADE_PART_RESULT
	{
		OK,
		NEED_RESOURCE,
		NEED_ITEM,
		NEED_MAIN_LEVEL,
		NEED_USER_LEVEL,
		MAX_LEVEL,
	}
	public UPGRADE_PART_RESULT CanUpgradePart(int index)
	{
		DataUnitPart mainDataPart = GetDataParts()[0];
		DataUnitPart dataPart = GetDataParts()[index];
		Trace.trace("GetPartLevelMax(index) " + GetPartLevelMax(index),Trace.CHANNEL.UI);
		if(dataPart.level + 1 > GetPartLevelMax(index))
		{
			return UPGRADE_PART_RESULT.MAX_LEVEL;
		}
		if (dataPart.mainLevel > mainDataPart.level) 
		{
			return UPGRADE_PART_RESULT.NEED_MAIN_LEVEL;
		}
		if( index != 0)
		{
			if (mainDataPart.level < dataPart.level + 1 ) 
			{
				return UPGRADE_PART_RESULT.NEED_MAIN_LEVEL;
			}
		}
		else
		{
			int userLevel =  InstancePlayer.instance.model_User.honorLevel; 
			if(mainDataPart.level + 1 > userLevel)
			{
				return UPGRADE_PART_RESULT.NEED_USER_LEVEL;
			}

		}


		ConnectionValidateHelper.CostCheck r = ConnectionValidateHelper.IsEnoughCost (dataPart.cost);
		if(r != ConnectionValidateHelper.CostCheck.OK)
		{
			return UPGRADE_PART_RESULT.NEED_RESOURCE;
		}

		return UPGRADE_PART_RESULT.OK;

	}

	public int GetPartLevelMax(int index)
	{
		DataUnitPart dataPart = GetDataParts()[index];
		int maxLevel = DataManager.instance.dataUnitPartGroup.GetPartMaxLevel (dataPart.id);
		return maxLevel;
	}

	public int GetPartLevel(int index)
	{
		DataUnitPart dataPart = GetDataParts()[index];
		return dataPart.level;
	}



	public static int CalcUnitPower(int unitId, int unitCount, int heroId)
	{
		Model_Unit modelUnit = InstancePlayer.instance.model_User.unlockUnits [unitId];
		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
		DataUnitPart[] dataUnitParts = modelUnit.GetDataParts ();

		int pHero = 0;
		if (heroId > 0) 
		{
			SlgPB.Hero modelHero = InstancePlayer.instance.model_User.model_heroGroup.GetHero (heroId);	
			DataHero dataHero = DataManager.instance.dataHeroGroup.GetHero (heroId, modelHero.exp, modelHero.stage);
			pHero = dataHero.basicParam.CalcPower ();
		}

		int pUnit = dataUnit.battleParam.CalcPower ();
		int pTotal = pHero + pUnit * unitCount;
		foreach (DataUnitPart dataUnitPart in dataUnitParts) {
			if (dataUnitPart != null) {
				int pPart = dataUnitPart.battleParam.CalcPower ();
				pTotal += pPart;
			}
		}

		return pTotal;

	}




}
