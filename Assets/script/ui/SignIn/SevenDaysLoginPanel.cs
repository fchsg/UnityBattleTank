using UnityEngine;
using System.Collections;
/// <summary>
/// 七日登录奖励界面.
/// </summary>
public class SevenDaysLoginPanel : PanelBase {
	 
	private Transform _panelContainer;
	private UIButton _closeBtn;
	private UILabel _panelName;
	private UIScrollView _ScrollView;
	private UIGrid _Grid;

	void Awake()
	{
		_panelContainer = transform.Find("SevenDaysLoginContainer");
		_closeBtn = _panelContainer.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_panelName = _panelContainer.Find("panelName_Label").GetComponent<UILabel>();
		_panelName.color = UICommon.FONT_COLOR_GREY;
		_ScrollView = _panelContainer.Find("ScrollView").GetComponent<UIScrollView>();
		_Grid = _panelContainer.Find("ScrollView/Grid").GetComponent<UIGrid>();
		CreateItem(_Grid,7);
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
	void CreateItem(UIGrid grid,int num){
		grid.DestoryAllChildren();
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{
				GameObject profab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "SignIn/DaysItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,profab);
				DaysItem classItem = item.GetComponent<DaysItem>();
				classItem.UpdateData(i);
				item.name = "0" + i;		 
			}
		}
		grid.Reposition();
	}
}
