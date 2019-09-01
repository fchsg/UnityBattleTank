using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model_RepairUnit {

	private List<SlgPB.Unit> repairUnits = new List<SlgPB.Unit> (); // 维修中units

	public int  repairTotalTimeSec; //Unit维修需要总时间（秒)
	public int repairLeftTime  	    //距离修理结束时间（秒）
	{
		get
		{
			int leftTime = Mathf.CeilToInt(TimeHelper.GetLeftSecondsToEndTimestamp(nextRepairEndTimestamp));
			return leftTime;
		}
	}

	private long nextRepairEndTimestamp;
	private int repairEndTimeSec;   //距离Unit维修结束时间（秒）
	private List<SlgPB.Unit> repairUnitsCopy = new List<SlgPB.Unit> ();

	public Model_RepairUnit()
	{
	}


	public void Parse(List<SlgPB.Unit> units, int repairEndTime, int repairTotalTime, bool isLogin = false)
	{
		repairEndTimeSec = repairEndTime;
		repairTotalTimeSec = repairTotalTime;

		repairUnits.Clear ();
		foreach(SlgPB.Unit unit in units)
		{
			if(unit.onRepair > 0)
			{
				repairUnits.Add(unit);
			}
		}

		if (repairEndTimeSec > 0)
		{
			if(isLogin)
			{
				InstancePlayer.instance.model_User.model_Queue.AddUnitRepairQueue();
			}

			nextRepairEndTimestamp = TimeHelper.GetCurrentRealTimestamp() + repairEndTimeSec * 1000;
		} 
		else
		{
			nextRepairEndTimestamp = 0;
		}
	}

	public List<SlgPB.Unit> GetRepairUnits()
	{
		return repairUnits;
	}

	private void CopyRepairUnits()
	{
		repairUnitsCopy.Clear ();
		foreach (SlgPB.Unit unit in repairUnits) 
		{
			repairUnitsCopy.Add(unit);
		}
	}

}
