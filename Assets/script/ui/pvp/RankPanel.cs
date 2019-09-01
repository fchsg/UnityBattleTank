using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class RankPanel : PanelBase {

	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;
	private UILabel _1_Label;
	private UILabel _2_Label;
	private UILabel _3_Label;
	private UILabel _4_Label;
	private UILabel _5_Label;

	private UIGrid _Grid;
	private UIScrollView _scrollview;
	List<UILabel> _grayList = new List<UILabel>();
	 
	//data
	List<RankItem> _playItemList = new List<RankItem>();
	Model_PvpUser _model_pvpUser;
 
	List<SlgPB.PVPUser> _pvpRankUsers = new List<SlgPB.PVPUser> ();
	Dictionary<int,RankItem> _playItemDic = new Dictionary<int,RankItem>();
	void Awake()
	{
		_grayList.Clear();
		_Container = transform.Find("Rank_Container");
		_close_Btn = _Container.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_namePanel = _Container.Find("panelName_Label").GetComponent<UILabel>();
		_grayList.Add(_namePanel);
		_1_Label = _Container.Find("1_Label").GetComponent<UILabel>();
		_2_Label = _Container.Find("2_Label").GetComponent<UILabel>();
		_3_Label = _Container.Find("3_Label").GetComponent<UILabel>();
		_4_Label = _Container.Find("4_Label").GetComponent<UILabel>();
		_5_Label = _Container.Find("5_Label").GetComponent<UILabel>();
		_grayList.Add(_1_Label);
		_grayList.Add(_2_Label);
		_grayList.Add(_3_Label);
		_grayList.Add(_4_Label);
		_grayList.Add(_5_Label);
		_Grid = _Container.Find("ScrollView/Grid").GetComponent<UIGrid>();
		_scrollview = _Container.Find("ScrollView").GetComponent<UIScrollView>();
		foreach(UILabel label in _grayList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
		}
	}
	public override void Init ()
	{
		base.Init ();
		GetLadderRank();
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void InitPVPItem()
	{
		_playItemDic.Clear();
		SetWrapContent(_Grid,_scrollview,_pvpRankUsers,OnUpdateItemMain);
	}
	void SetWrapContent(UIGrid grid,UIScrollView scrollview,List<SlgPB.PVPUser> dataList,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int dataCount = dataList.Count;
		if(dataCount <= 7 )
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
			wrap.itemSize = (int)grid.cellHeight;
			wrap.minIndex = -(dataCount -1);
			wrap.maxIndex = 0;

 			wrap.onInitializeItem = OnUpdateItemMain;
			wrap.enabled = true;
			wrap.SortAlphabetically();
		}
	}
	void OnUpdateItemMain(GameObject go, int index, int realIndex)
	{
		OnUpateItem(go,index,realIndex,_pvpRankUsers,_playItemDic);
	}
	void OnUpateItem(GameObject go, int index, int realIndex,List<SlgPB.PVPUser> dataList,Dictionary<int,RankItem> dataDic)
	{
		int dataCount = dataList.Count;
		int indexList = Mathf.Abs(realIndex);
		RankItem Item1 = go.GetComponent<RankItem>();
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
//		_playItemList.Clear();
		_Grid.DestoryAllChildren();
//		int dataCount = _pvpUsers.Count;
		 
		for(int i= 0; i< 8; i++)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "pvp/rankItem");
			GameObject item = NGUITools.AddChild(_Grid.gameObject,prefab);
			RankItem playitem = item.GetComponent<RankItem>();
			item.name = "0" + i;
//			playitem.Init(_pvpUsers[i]);
//			_playItemList.Add(playitem);
		}
		_Grid.Reposition();
	}

	void OnClose()
	{
		this.Delete();
	}


	// ====================== 
	void GetLadderRank()
	{
		UIHelper.LoadingPanelIsOpen(true);
		CommonRequest request = new CommonRequest ();
		request.api = new Model_ApiRequest ().api;

		(new PBConnect_getPvpLadderRank ()).Send (request, OnGetLadderRank);

	}
	void OnGetLadderRank(bool success, System.Object content)
	{
		if (success) {
			_model_pvpUser = InstancePlayer.instance.model_User.model_pvpUser;
			_pvpRankUsers = _model_pvpUser.pvpRankUsers;
			CreateItem(_pvpRankUsers);
			InitPVPItem();
			UIHelper.LoadingPanelIsOpen(false);
			Trace.trace("OnGetLadderRank success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnGetLadderRank failure",Trace.CHANNEL.UI);
		}
	}
}
