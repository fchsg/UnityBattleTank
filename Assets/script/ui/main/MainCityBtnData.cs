using UnityEngine;
using System.Collections;
/// <summary>
/// 主界面功能按钮数据
/// </summary>
public class MainCityBtnData 
{
	public int btnID;
	public MainCityBtnConfig btnConfig;
	
	public MainCityBtnData(int btnID)
	{
		this.btnID = btnID;
		
		this.btnConfig = MainCityBtnConfigManager.GetItem (this.btnID);
	}
}
