using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;


public class ReportPanel : PanelBase {
	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;
	private UIGrid _Grid;
	private UIScrollView _scrollview;

	//data 
	List<ReportItem> _reportItemList = new List<ReportItem>();
	Model_PvpUser _model_pvpUser;
	List<SlgPB.FightLog> _pvpLogs;
	void Awake()
	{
		_Container = transform.Find("ReportContainer");
		_close_Btn = _Container.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_namePanel = _Container.Find("panelName_Label").GetComponent<UILabel>();
		_namePanel.color = UICommon.FONT_COLOR_GREY;
		_Grid = _Container.Find("infoContainer/ScrollView/Grid").GetComponent<UIGrid>();
		_scrollview = _Container.Find("infoContainer/ScrollView").GetComponent<UIScrollView>();
	}
	public override void Init ()
	{
		base.Init ();
		GetLog();

	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
	}
 
	// Update is called once per frame
	void Update () {

	}
	void CreateItem(List<SlgPB.FightLog> pvpLogs)
	{
		_reportItemList.Clear();
		_Grid.DestoryAllChildren();
		int dataCount = pvpLogs.Count;

		for(int i= 0; i< dataCount; i++)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "pvp/reportItem");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,prefab);
			ReportItem reportitem = item.GetComponent<ReportItem>();
			item.name = "0" + i;
			reportitem.Init(pvpLogs[i]);
			_reportItemList.Add(reportitem);
		}
		_Grid.Reposition();
	}
	void OnClose()
	{
		this.Delete();
	}

	//=========================
	void GetLog()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_getPvpLog getPvplog = new PBConnect_getPvpLog();
		getPvplog.GetLog(OnGetLog);
	}
	void OnGetLog(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			_model_pvpUser = InstancePlayer.instance.model_User.model_pvpUser;
			_pvpLogs = _model_pvpUser.pvpLogs;
			CreateItem(_pvpLogs);
			Trace.trace("OnGetLog success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnGetLog failed",Trace.CHANNEL.UI);
		}
	}
}
