using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SlgPB;

public class Model_Building {


	public enum Building_Type
	{
		ControlCenter = 1, 	//指挥所
		WeaponsFactory = 2,	//军工厂
		ScienceCenter = 3,	//科技大厦
		FoodFactory = 4,	//粮田
		OilFactory = 5, 	//油井
		MetalFactory = 6,   //矿井
		RareFactory = 7		//稀土
	}


	public int id; //建筑id
	public int level; //建筑等级
	public bool isUnlockedNextLevel; //下一个等级是否已经解锁，0：未解锁；1：已解锁，可升级
	public bool isUpgrading;		  //是否正在升级过程中，0：不是；1：是
	public int endToUpgrade;	  //距离升级结束剩余时间（秒）
	public Model_Production model_Production; //基本资源产出的数据，如果没有则表示无资源产出

	public bool isUnlocked = false; // 建筑是否解锁
	public Building_Type buildingType; // 建筑类型
	public List<Building_Type> unLockedBuildingsType = new List<Building_Type> (); // 主建筑下 其他建筑的的解锁
	
	private long _nextLevelUpTime; // 下次建筑升级完成时间
	
	public int buildingLevelUpTime // 升级倒计时
	{
		get
		{
			int leftTime = Mathf.CeilToInt(TimeHelper.GetLeftSecondsToEndTimestamp(_nextLevelUpTime));
			return leftTime;
		}
	}
		
	public Model_Building()
	{
	}

	public void Parse(Building building, bool isLogin = false)
	{
		id = building.id;
		isUnlockedNextLevel = (building.unlockedNextLevel == 0) ? false : true; 
		isUpgrading = (building.upgrading == 0) ? false : true;
		endToUpgrade = building.endToUpgrade;

		if (!isUnlocked)
		{
			isUnlocked = true;
		}
		level = building.level;

		if (isUpgrading) {
			// 登录时正在升级建筑 增加建筑队列数
			if (isLogin) {
				InstancePlayer.instance.model_User.model_Queue.AddBuildingQueue ();
			}

			_nextLevelUpTime = TimeHelper.GetCurrentRealTimestamp() + this.endToUpgrade * 1000;
		} 
		else 
		{
			_nextLevelUpTime = 0;
		}

		if (building.production != null) 
		{
			if(model_Production == null)
			{
				model_Production = new Model_Production();
			}
			model_Production.Parse (building.production, level);
		} 
		else 
		{
			if(model_Production != null)
			{
				model_Production.StopTimer();
				model_Production = null;
			}
		}

		UpdateUnlockedBuildings ();
	}

	//根据指挥所等级,创建可解锁的建筑id 用于UI展示
	private void UpdateUnlockedBuildings()
	{
		if (buildingType == Building_Type.ControlCenter) 
		{
			unLockedBuildingsType.Clear();
			foreach(Building_Type type in Enum.GetValues(typeof(Building_Type)))
			{
				if(type != Building_Type.ControlCenter)
				{
					int nType = (int)type;
					int openLevel = DataManager.instance.dataBuildingGroup.GetBuilding(nType, 1).openLevel;
					if(level >= openLevel)
					{
						unLockedBuildingsType.Add(type);
					}
				}
			}
		}
	}
	
	//建筑下一等级解锁
	public void UnlockBuildingNextLevel()
	{
		isUnlockedNextLevel = true;
	}

}
