using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 月签到奖励界面
/// </summary>
public class SignInPanel : PanelBase {
	private Transform _panelContainer;
	private UIButton _closeBtn;
	private UILabel _panelName;
	private Transform _Checkbox_Container;

	private UIGrid _30Grid;
	private UIGrid _4Grid;
	int _days;
	void Awake()
	{
		_panelContainer = transform.Find("SignInContainer");
		_closeBtn = _panelContainer.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_panelName = _panelContainer.Find("panelName_Label").GetComponent<UILabel>();
		_panelName.color = UICommon.FONT_COLOR_GREY;
		_30Grid = _panelContainer.Find("30_Container/Grid").GetComponent<UIGrid>();
		_4Grid  = _panelContainer.Find("reward_Container/Grid").GetComponent<UIGrid>();
		DateTime dtNow = DateTime.Now;
		_days = DateTime.DaysInMonth(dtNow.Year ,dtNow.Month);
		CreateItem(_30Grid,_days,SignInItem.SignInItemType.SIGININ);
		CreateItem(_4Grid,4,SignInItem.SignInItemType.CUMULATIVE);
	}
	public override void Init ()
	{
		base.Init ();
//		InitUI();


	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
	}
	void Update () {
	
	}

	void OnClose()
	{
		this.Delete();
	}

	void CreateItem(UIGrid grid,int num,SignInItem.SignInItemType type){
		grid.DestoryAllChildren();
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{
				GameObject profab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "SignIn/SignInItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,profab);
				SignInItem classItem = item.GetComponent<SignInItem>();
				classItem.UpdateData(i,type);
				item.name = "0" + i;		 
			}
		}
		grid.Reposition();
	}

}
