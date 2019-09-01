using UnityEngine;
using System;
using System.Collections.Generic;

public class TimerManager
{
	public static float time;
	private static Dictionary<string, TimerItem> dictList = new Dictionary<string, TimerItem>();
	private static Dictionary<string, TimerItem> CoolingList = new Dictionary<string, TimerItem>();
	
	/// <summary>
	/// 注册计时
	/// </summary>
	/// <param name="timerKey">Timer key.</param>
	/// <param name="totalNum">Total number.</param>
	/// <param name="delayTime">Delay time.</param>
	/// <param name="callback">Callback.</param>
	/// <param name="endCallback">End callback.</param>
	/// <param name="isCountCown">是否倒计时</param>
	public static void Register(string timerKey, int totalNum, float delayTime, Action<int> callback, Action<string> endCallback,bool isCountCown)
	{
		TimerItem timerItem = null;
		if(!dictList.ContainsKey(timerKey))
		{
			GameObject objectItem = new GameObject ();
			objectItem.name = timerKey;

			timerItem = objectItem.AddComponent<TimerItem> ();
			dictList.Add(timerKey, timerItem);
		}else
		{
			timerItem = dictList[timerKey];
		}

		if(timerItem != null)
		{
			timerItem.Run(timerKey,totalNum, delayTime, callback, endCallback,isCountCown);
		}
	}
	/// <summary>
	/// Registers the cooling time.
	/// </summary>
	/// <param name="timerKey">Timer key.</param>
	/// <param name="delayTime">Delay time.</param>
	/// <param name="callback">Callback.</param>
	public static void RegisterCoolingTime(string timerKey, float delayTime, Action<string> callback)
	{
		if(!CoolingList.ContainsKey(timerKey))
		{
			TimerItem timerItem = null;
			if(!CoolingList.ContainsKey(timerKey)){
				GameObject objectItem = new GameObject();
				objectItem.name = timerKey;

				timerItem = objectItem.AddComponent<TimerItem>();
				timerItem.Init(timerKey,TimerManager.time, delayTime, callback);
				CoolingList.Add(timerKey, timerItem);
			}else
			{
				timerItem = CoolingList[timerKey];
			}
		}
	}
	/// <summary>
	/// 冷却计时
	/// </summary>
	public static void RunCooling(){	
		// 设置时间值
		TimerManager.time = Time.time;
			
		// 锁定
		foreach(TimerItem timerItem in CoolingList.Values)
		{
			if(timerItem != null) timerItem.RunCoolingTime(TimerManager.time);
		}
	}

	/// <summary>
	/// 取消注册计时
	/// </summary>
	/// <param name="timerKey">Timer key.</param>
	public static void UnRegister(string timerKey)
	{
		if(!dictList.ContainsKey(timerKey)) return;

		TimerItem timerItem = dictList [timerKey];
		if(timerItem != null)
		{
			timerItem.Stop ();
			GameObject.Destroy(timerItem.gameObject);
		}

	}
	/// <summary>
	/// 取消冷却计时
	/// </summary>
	/// <param name="timerKey">Timer key.</param>
	public static void UnCoolingRegister(string objectItem)
	{
		if(!CoolingList.ContainsKey(objectItem)) return;
		
		TimerItem timerItem = CoolingList [objectItem];
		if(timerItem != null)
		{
			GameObject.Destroy(timerItem.gameObject);
		}
	}


}


