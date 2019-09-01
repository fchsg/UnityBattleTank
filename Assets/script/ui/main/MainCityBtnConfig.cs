using UnityEngine;
using System.Collections;
/// <summary>
/// 主界面功能按钮类
/// </summary>
public class MainCityBtnConfig 
{
	public int btnID;
	public string btnName; 
	public string btnIcon;
	public MainCityBtnConfig(int btnID, string btnName, string btnIcon)
	{
		this.btnID = btnID;
		this.btnName = btnName;
		this.btnIcon = btnIcon;
	}
}
