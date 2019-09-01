using UnityEngine;
using System.Collections;

public class Model_InitialConfig {

	private DataInitialConfig _dataInitialConfig;

	public Model_InitialConfig()
	{
		_dataInitialConfig = DataManager.instance.dataInitialConfigGroup.GetDataInitialConfig();
	}

	public int GetClearUnitCDCash(float leftTime)
	{
		int needTime = Mathf.CeilToInt (leftTime * _dataInitialConfig.unitCashFactor);
		return needTime;
	}

	public int GetClearBuildingCDCash(float leftTime)
	{
		int needTime = Mathf.CeilToInt (leftTime * _dataInitialConfig.buildingCashFactor);
		return needTime;
	}

	public int GetClearSkillCDCash(float leftTime)
	{
		int needTime = Mathf.CeilToInt (leftTime * _dataInitialConfig.skillCashFactor);
		return needTime;
	}

}
