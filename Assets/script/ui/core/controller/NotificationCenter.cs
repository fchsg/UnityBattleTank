using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Notification_Type
{
	RefreshGold = 0,//刷新金币
	RefreshCash = 1,//  刷新cash 
	RefreshBuilding = 2,//刷新建筑信息
	RefreshFunctionBtnItem= 3,//刷新建筑功能按钮

	RefreshFood = 4,// 刷新资源  
	RefreshOil = 5,
	RefreshMetal = 6,
	RefreshRare = 7,

	RequestBuilingUpLevel = 8,//请求建筑升级 
	RequestBuilingBugQueue = 9,//请求建筑购买队列升级

	RequestTrainTank = 10,//请求生产坦克
	RequestRepairTank = 11,//请求维修坦克

	RefreshProductTank = 12,//请求刷新生产界面坦克
	RefreshStrengthenTank = 13,//刷新强化界面坦克
	RequestStrengthenPart = 14,//请求强化升级部件
	RefreshPropItem = 15,//刷新道具item
	RefreshMailItem = 16,//刷新邮件 
	RefreshMailInfo = 17,//刷新邮件 info
	SHOWABILITYPANL = 18,//显示隐藏能力图
}

public delegate void OnNotificationDelegate(Notification notification);

public class NotificationCenter
{
	private static NotificationCenter _instance = null;
	public static NotificationCenter instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new NotificationCenter();
			}
			return _instance;
		}
	}

	private Dictionary<int, OnNotificationDelegate> _eventListerners = new Dictionary<int, OnNotificationDelegate>();
	
	public void AddEventListener(Notification_Type type, OnNotificationDelegate listener)
	{
		if (!_eventListerners.ContainsKey((int)type))
		{
			OnNotificationDelegate deleg = null;
			_eventListerners[(int)type] = deleg;
		}
		_eventListerners[(int)type] += listener;
	}

	public void RemoveEventListener(Notification_Type type, OnNotificationDelegate listener)
	{
		if (!_eventListerners.ContainsKey((int)type))
		{
			return;
		}
		_eventListerners[(int)type] -= listener;
	}

	public void RemoveEventListener(Notification_Type type)
	{
		if (_eventListerners.ContainsKey((int)type))
		{
			_eventListerners.Remove((int)type);
		}
	}
	
	public void DispatchEvent(Notification_Type type, Notification note)
	{
		if (_eventListerners.ContainsKey((int)type))
		{
			_eventListerners[(int)type](note);
		}
	}

	public void DispatchEvent(Notification_Type type)
	{
		DispatchEvent(type, null);
	}

	public bool HasEventListener(Notification_Type type)
	{
		return _eventListerners.ContainsKey((int)type);
	}
}

