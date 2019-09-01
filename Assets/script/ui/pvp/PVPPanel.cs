using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class PVPPanel : PanelBase {
	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;

	private Transform _info_Container;
	private UIButton _rank_Btn;
	private UIButton _report_Btn;
	private UIButton _exchange_Btn;
	private UIButton _enemy_Btn;

	private UISprite _headIcon;
	private UILabel _playLevel_Label;
	private UILabel _currentRank_Lable;
	private UILabel _fight_Lable;
	private UILabel _rankReward_Label;
	private UILabel _rankRewardtimes_Label;
	private UILabel _record;
	private UILabel _recordValue;
	private UILabel _diamond;
	private UILabel _diamondValue;

	private Transform _play_Container;
	private UILabel _times_Label;
	private UILabel _timesValue_Label;
	private UILabel _cd_Label;
	private UILabel _cdValue_Label;

	private UILabel _clear;
	private UILabel _refreshLabel;
	private UILabel _refreshtimes_Label;
	private UIButton _clear_btn;
	private UIButton _refresh_btn;

	private UIGrid _Grid;
	private UIScrollView _scrollview;
	//list 
	List<UILabel> _grayList = new List<UILabel>(); 
	List<UILabel> _greenList = new List<UILabel>(); 
	List<UILabel> _orangeList = new List<UILabel>();

	//data
	List<PlayItem> _playItemList = new List<PlayItem>();
	Model_PvpUser _model_pvpUser;
	SlgPB.PVPUser _selfPvpUser = null;
	List<SlgPB.PVPUser> _pvpUsers = new List<SlgPB.PVPUser> ();
	Dictionary<int,PlayItem> _playItemDic = new Dictionary<int,PlayItem>();
	void Awake()
	{
		_grayList.Clear();
		_greenList.Clear();
		_orangeList.Clear();
		_Container = transform.Find("PVPContainer");
		_close_Btn = _Container.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_namePanel = _Container.Find("panelName_Label").GetComponent<UILabel>();
		_grayList.Add(_namePanel);

		_info_Container = _Container.Find("info_Container");
		_rank_Btn = _info_Container.Find("Btn_1/btn").GetComponent<UIButton>();
		_report_Btn = _info_Container.Find("Btn_2/btn").GetComponent<UIButton>();
		_exchange_Btn = _info_Container.Find("Btn_3/btn").GetComponent<UIButton>();
		_enemy_Btn = _info_Container.Find("Btn_4/btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_rank_Btn,OnRank);
		UIHelper.AddBtnClick(_report_Btn,OnReport);
		UIHelper.AddBtnClick(_exchange_Btn,OnExchage);
		UIHelper.AddBtnClick(_enemy_Btn,OnEnemy);

		_headIcon = _info_Container.Find("headIcon_Bg/headIcon").GetComponent<UISprite>();
		_playLevel_Label = _info_Container.Find("headIcon_Bg/playLevel_Label").GetComponent<UILabel>();
		_grayList.Add(_playLevel_Label);
		_currentRank_Lable = _info_Container.Find("rank_bg/Label").GetComponent<UILabel>();
		_fight_Lable = _info_Container.Find("fight_bg/Label").GetComponent<UILabel>();
		_rankReward_Label = _info_Container.Find("rankReward_Label").GetComponent<UILabel>();
		_grayList.Add(_rankReward_Label);
		_rankRewardtimes_Label = _info_Container.Find("rankReward_Label/Label").GetComponent<UILabel>();
		_greenList.Add(_rankRewardtimes_Label);
		_record = _info_Container.Find("Label_1").GetComponent<UILabel>();
		_grayList.Add(_record);
		_recordValue = _info_Container.Find("Label_1/Label").GetComponent<UILabel>();
		_orangeList.Add(_recordValue);
		_diamond = _info_Container.Find("Label_2").GetComponent<UILabel>();
		_grayList.Add(_diamond);
		_diamondValue = _info_Container.Find("Label_2/Label").GetComponent<UILabel>();
		_orangeList.Add(_diamondValue);


		_play_Container = _Container.Find("play_Container");
		_times_Label = _play_Container.Find("times_Label").GetComponent<UILabel>();
		_grayList.Add(_times_Label);
		_timesValue_Label = _play_Container.Find("times_Label/Label").GetComponent<UILabel>();
		_cd_Label = _play_Container.Find("cd_Label").GetComponent<UILabel>();
		_grayList.Add(_cd_Label);
		_cdValue_Label = _play_Container.Find("cd_Label/Label").GetComponent<UILabel>();
		_greenList.Add(_cdValue_Label);

		_clear = _Container.Find("clear_btn/Label").GetComponent<UILabel>();
		_clear.color = UICommon.FONT_COLOR_GOLDEN;
		_refreshLabel = _Container.Find("refresh_btn/refresh_Label").GetComponent<UILabel>();
		_refreshLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_refreshtimes_Label = _Container.Find("refresh_btn/times_Label").GetComponent<UILabel>();
		_orangeList.Add(_refreshtimes_Label);
		_clear_btn = _Container.Find("clear_btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_clear_btn,OnClear);
		_refresh_btn = _Container.Find("refresh_btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_refresh_btn,OnRefresh);

		_Grid = _play_Container.Find("ScrollView/Grid").GetComponent<UIGrid>();
		_scrollview = _play_Container.Find("ScrollView").GetComponent<UIScrollView>();
		foreach(UILabel label in _grayList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
		foreach(UILabel label in _greenList)
		{
			label.color = UICommon.FONT_COLOR_GREEN;
		}
		foreach(UILabel label in _orangeList)
		{
			label.color = UICommon.FONT_COLOR_ORANGE;
		}


	}
	public override void Init ()
	{
		base.Init ();

	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		GetLadder();
//		GetLadderRank();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		_model_pvpUser = InstancePlayer.instance.model_User.model_pvpUser;
		_selfPvpUser = _model_pvpUser.selfPvpUser;
		_pvpUsers = _model_pvpUser.pvpUsers;
		if(_selfPvpUser != null)
		{
//			_playLevel_Label.text = ;
			_currentRank_Lable.text = _selfPvpUser.rank.ToString();
			_fight_Lable.text = InstancePlayer.instance.model_User.model_Formation.CalcPower().ToString();

			_timesValue_Label.text = UIHelper.SetStringSixteenColor(_model_pvpUser.remainChallengeTimes.ToString() + "/",UICommon.SIXTEEN_GREEN) + UIHelper.SetStringSixteenColor( "10",UICommon.SIXTEEN_ORANGE) ;
			if(_model_pvpUser.refreshCost == 0)
			{
				_refreshtimes_Label.text = "(免费)";
			}
			else
			{
				_refreshtimes_Label.text = "(" + _model_pvpUser.refreshCost + "钻石)";
			}
			DataLadder ladder = DataManager.instance.dataLadderGroup.GetLadder(_selfPvpUser.rank);
			if(ladder != null)
			{
				_recordValue.text = ladder.combat.ToString();
				_diamondValue.text = ladder.cash.ToString();
			}
			else
			{
				_recordValue.text = "0";
				_diamondValue.text = "0";
			}
		}
	}
	void InitPVPItem()
	{
		_playItemDic.Clear();
		SetWrapContent(_Grid,_scrollview,_pvpUsers,OnUpdateItemMain);
	}
	void SetWrapContent(UIGrid grid,UIScrollView scrollview,List<SlgPB.PVPUser> dataList,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int dataCount = dataList.Count;
		if(dataCount <= 5 )
		{
			scrollview.enabled = false;
		}
		else
		{
			scrollview.enabled = true;
		}
		Trace.trace("dataCount  " + dataCount,Trace.CHANNEL.UI);
		UIWrapContent wrap = null;
		if(grid.gameObject.GetComponent<UIWrapContent>())
		{
			wrap = grid.gameObject.GetComponent<UIWrapContent>();
		}
		else
		{
			grid.gameObject.AddComponent<UIWrapContent>();
			wrap = grid.gameObject.GetComponent<UIWrapContent>();

		}
		if(wrap != null)
		{
			//绑定方法
			wrap.itemSize = (int)grid.cellWidth;
			wrap.minIndex = -(dataCount - 5);
			wrap.maxIndex = dataCount + (-(dataCount - 5)) -1;

//			wrap.onInitializeItem = OnUpdateItemMain;
			wrap.enabled = true;
//			wrap.SortAlphabetically();
		}
	}
	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_pvpUsers,_playItemDic);
	}
	void OnUpateItem(GameObject go, int index, int realIndex,List<SlgPB.PVPUser> dataList,Dictionary<int,PlayItem> dataDic)
	{
		int dataCount = dataList.Count;
		int indexList = Mathf.Abs(realIndex);
		PlayItem Item1 = go.GetComponent<PlayItem>();
		int index_ = indexList ;

		if(index_ > (dataCount - 1) )
		{
			Item1.gameObject.SetActive(false);

			return;
		}
		else
		{
			Item1.gameObject.SetActive(true);
			Item1.Init(dataList[index_]);
			if(!dataDic.ContainsKey(dataList[index_].userId))
			{
				dataDic.Add(dataList[index_].userId,Item1);
			}
		}
	}
	void CreateItem(List<SlgPB.PVPUser> _pvpUsers)
	{
		_playItemList.Clear();
		_Grid.DestoryAllChildren();
		int dataCount = _pvpUsers.Count;
		if(dataCount > 5)
		{
			_Grid.pivot = UIWidget.Pivot.Right;
			_scrollview.enabled = true;
		}
		else
		{
			_Grid.pivot = UIWidget.Pivot.Center;
			_scrollview.enabled = false;
		}
		for(int i= 0; i< dataCount; i++)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "pvp/playItem");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,prefab);
			PlayItem playitem = item.GetComponent<PlayItem>();
			item.name = "0" + i;
			playitem.Init(_pvpUsers[i]);
			_playItemList.Add(playitem);
		}
		_Grid.Reposition();
	}
	//清CD
	void OnClear()
	{
		
	}
	//刷新玩家
	void OnRefresh()
	{
		RefreshLadder();
	}

	void OnRank()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_PVPRANK);
	}
	void OnReport()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_PVPREPORT);
	}
	void OnExchage()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_PVPEXCHANGE);
	}
	void OnEnemy()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_FORMATION);

	}
	void OnClose()
	{
		this.Delete();
	}

	//=============================
	void GetLadder()
	{
		UIHelper.LoadingPanelIsOpen(true);
		CommonRequest request = new CommonRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_getPvpLadder ()).Send (request, OnGetLadder);
	}
	void OnGetLadder(bool success, System.Object content)
	{
		if (success) {
			_model_pvpUser = InstancePlayer.instance.model_User.model_pvpUser;
			_selfPvpUser = _model_pvpUser.selfPvpUser;
			_pvpUsers = _model_pvpUser.pvpUsers;
			CreateItem(_pvpUsers);
			UIHelper.LoadingPanelIsOpen(false);
			Trace.trace("OnGetLadder success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnGetLadder failure",Trace.CHANNEL.UI);
		}
	}


	void RefreshLadder()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_refreshLadder.RESULT r = PBConnect_refreshLadder.RefreshLadder (OnRefreshLadder);
		if (r == PBConnect_refreshLadder.RESULT.OK) {
			

		}
		else if(r == PBConnect_refreshLadder.RESULT.NEED_CASH)
		{
			UIHelper.LoadingPanelIsOpen(false);
		}

	}
	void OnRefreshLadder(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			_model_pvpUser = InstancePlayer.instance.model_User.model_pvpUser;
			_pvpUsers = _model_pvpUser.pvpUsers;
			CreateItem(_pvpUsers);
			Trace.trace("OnRefreshLadder success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnRefreshLadder failure",Trace.CHANNEL.UI);
		}
	}
}
