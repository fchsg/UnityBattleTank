using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountdownData{
	public enum CountdownType
	{
		BUILDINGUP = 0,//建筑升级
		PRODUCTION = 1,//生产坦克
		REPAIR = 2,//修理坦克
	}
	public class Countdown
	{
		public CountdownType dataType;
		public System.Object data;
		public string iconName;
	}


	private List<Countdown> countdownList = new List<Countdown>();

	public CountdownData()
	{
		Init();
	}

	public void Init()
	{
		countdownList.Clear();


		//生产
		Dictionary<int, Model_Unit> units = InstancePlayer.instance.model_User.unlockUnits;

		foreach(KeyValuePair<int,Model_Unit> kvp in units)
		{
			if(kvp.Value.onProduce > 0)
			{
				Countdown countdown = new Countdown();
				countdown.dataType = CountdownType.PRODUCTION;
				countdown.data = kvp.Value;
				countdown.iconName = "";
				countdownList.Add(countdown);
			}
		}

		// 维修
		Model_RepairUnit repair = InstancePlayer.instance.model_User.model_RepairUnit;

		if(repair.repairLeftTime >= 0 && repair.repairTotalTimeSec > 0)
		{
			Countdown repaircountdown = new Countdown();
			repaircountdown.dataType = CountdownType.REPAIR;
			repaircountdown.data = repair;
			repaircountdown.iconName = "";
			countdownList.Add(repaircountdown);
		}
		//建筑升级
		Dictionary<Model_Building.Building_Type, Model_Building> buildings = InstancePlayer.instance.model_User.buildings;
		foreach(KeyValuePair<Model_Building.Building_Type,Model_Building> kvp in buildings)
		{
			if(kvp.Value.isUpgrading)
			{
				Countdown countdown = new Countdown();
				countdown.dataType = CountdownType.BUILDINGUP;
				countdown.data = kvp.Value;
				countdown.iconName = "";
				countdownList.Add(countdown);
			}
		}
	}

	public List<Countdown> GetCountdownData()
	{
		Init();
		return countdownList;
	}

}
