using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 主界面功能按钮UI逻辑管理
/// </summary>

public class FunctionBtnManager : MonoBehaviour {
	private UIScrollView _FunctionBtnScrollView;
	private UIGrid _FunctionBtnGrid;



	//data 
	List<MainCityBtnData> _BtnDataList;
	Dictionary<int,MainCityBtnItem> _BtnItemDict = new Dictionary<int, MainCityBtnItem>();
	void Awake()
	{
		_FunctionBtnScrollView = transform.Find("FunctionBtnScrollView").GetComponent<UIScrollView>();
		_FunctionBtnGrid = transform.Find("FunctionBtnScrollView/FunctionBtnGrid").GetComponent<UIGrid>();


		MainCityBtnConfigManager.Init();
		MainCityBtnManager.Init();
		_BtnDataList = MainCityBtnManager.list;
		InitBtnUI();
	}
 
	void Update () {
		if(_FunctionBtnGrid != null)
		{
			int childCount = _FunctionBtnGrid.GetChildList().Count;
			if(childCount == 6)
			{
				_FunctionBtnScrollView.enabled = false;
				_FunctionBtnGrid.Reposition();
			}
			else
			{
				_FunctionBtnScrollView.enabled = true;
			}
		}
	}

	void InitBtnUI()
	{
		if(_BtnDataList != null)
		{
			if(_BtnItemDict != null)
			{
				_BtnItemDict.Clear();
			}
			_FunctionBtnGrid.DestoryAllChildren();
			foreach(MainCityBtnData btnData in _BtnDataList)
			{
				CreateBtnItem(_FunctionBtnGrid.gameObject,btnData);
			}
		}

	}

	public GameObject CreateBtnItem (GameObject parentGrid,MainCityBtnData btnData)
	{
		if(parentGrid != null)
		{
			GameObject btnItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "main/FunctionBtnItem");
			GameObject item = NGUITools.AddChild(parentGrid,btnItem);
			MainCityBtnItem btnItemclass = item.GetComponent<MainCityBtnItem>(); 
			btnItemclass.InitData(btnData);
//			_BtnItemDict.Add(btnData.btnID,btnItemclass);
			return item;
		}
		return null;	
	}
}
