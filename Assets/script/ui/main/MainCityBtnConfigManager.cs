using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 主界面功能按钮数据初始化.
/// </summary>
public class MainCityBtnConfigManager {

	private static Dictionary<int, MainCityBtnConfig> dict;
	public static void Init()
	{
		dict = new Dictionary<int, MainCityBtnConfig> ();
		dict.Add (0, new MainCityBtnConfig(0, "任务", "main_icon_task"));
		dict.Add (1, new MainCityBtnConfig(1, "部队", "main_icon_forces"));
		dict.Add (2, new MainCityBtnConfig(2, "将领", "main_icon_general"));
		dict.Add (3, new MainCityBtnConfig(3, "邮件", "main_icon_email"));
		dict.Add (4, new MainCityBtnConfig(4, "仓库", "main_icon_wareHouse"));
		dict.Add (5, new MainCityBtnConfig(5, "设置", "main_icon_setUp"));
	}
	
	public static MainCityBtnConfig GetItem(int btnID)
	{
		if(dict == null) return null;
		return dict[btnID];
	}
}
