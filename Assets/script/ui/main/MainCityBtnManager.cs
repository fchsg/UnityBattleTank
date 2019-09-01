using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 主界面功能按钮数据管理
/// </summary>
public class MainCityBtnManager  {
	public static List<MainCityBtnData> list;
	public static void Init()
	{
		if(list == null) list = new List<MainCityBtnData>();
		list.Clear();
		list.Add (new MainCityBtnData (0));
		list.Add (new MainCityBtnData (1));
		list.Add (new MainCityBtnData (2));
		list.Add (new MainCityBtnData (3));
		list.Add (new MainCityBtnData (4));
		list.Add (new MainCityBtnData (5));
		
	}
	public static MainCityBtnData GetBtnData(int btnId)
	{
		if(list == null) return null;
		return list[btnId];
	}
}
