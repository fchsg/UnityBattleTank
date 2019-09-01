using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

// 战斗结算界面
public class BattleSettlementPanel : PanelBase {
	
	private Transform _Settlement_Container;//胜利 
		
	private UIScrollView _Instructor_ScrollView;
	private UIGrid _Instructor_Grid;
	private UIScrollView _Drop_ScrollView;
	private UIGrid _Drop_Grid;

	private UIButton _shareBtn;
	private UIButton _replayBtn;
	private UIButton _okBtn;

	private Transform _LoserContainer;// 失败
	private UIButton _upBuilingBtn;//升级建筑 
	private UIButton _upInstructorBtn;// 提升统帅
	private UIButton _repairBtn;//修理坦克
	private UIButton _upScienceBtn;//研发科技

	private Transform _Star_Container;
	private GameObject _star_bg_1;
	private GameObject _star_bg_2;
	private GameObject _star_bg_3;
	private GameObject _star_1;
	private GameObject _star_2;
	private GameObject _star_3;
	private UISprite _winSprite;
	private UISprite _loserSprite;

	private List<GameObject> _starList = new List<GameObject>();
	private List<GameObject> _starBgList = new List<GameObject>();
	private Dictionary<int,SettlementInstructorItem> _instructItemDic = new Dictionary<int, SettlementInstructorItem>();
	private Dictionary<int,SettlementDropItem> _dropItemDic = new Dictionary<int, SettlementDropItem>();

	private  GameObject _gameTipsPanel;

	//  test
	void Awake()
	{
		//Init ();
	}
		
	public override void Init ()
	{
		base.Init ();

		_instructItemDic.Clear();

		_Settlement_Container = transform.Find("BattleSettlement/Settlement_Container");
		_Instructor_ScrollView = _Settlement_Container.Find("Instructor_ScrollView").GetComponent<UIScrollView>();
		_Instructor_Grid = _Settlement_Container.Find("Instructor_ScrollView/Instructor_Grid").GetComponent<UIGrid>();
		_Drop_ScrollView = _Settlement_Container.Find("Drop_ScrollView").GetComponent<UIScrollView>();
		_Drop_Grid = _Settlement_Container.Find("Drop_ScrollView/Drop_Grid").GetComponent<UIGrid>();

		Transform BtnContanier = transform.Find("BattleSettlement/BtnContainer");
		_shareBtn = BtnContanier.Find("shareBtn").GetComponent<UIButton>();
		EventDelegate evshare = new EventDelegate(OnShare);
		_shareBtn.onClick.Add(evshare);

		_replayBtn = BtnContanier.Find("replayBtn").GetComponent<UIButton>();
		EventDelegate evreplay = new EventDelegate(OnRePlay);
		_replayBtn.onClick.Add(evreplay);

		_okBtn = BtnContanier.Find("okBtn").GetComponent<UIButton>();
		EventDelegate evok = new EventDelegate(OnOk);
		_okBtn.onClick.Add(evok);

		_LoserContainer = transform.Find("BattleSettlement/LoserContainer");
		_Settlement_Container.gameObject.SetActive(true);
		_LoserContainer.gameObject.SetActive(false);

		_upBuilingBtn = _LoserContainer.Find("upBuilingBtn").GetComponent<UIButton>();
		EventDelegate ev1 = new EventDelegate(OnUpBuiling);
		_upBuilingBtn.onClick.Add(ev1);

		_upInstructorBtn = _LoserContainer.Find("upInstructorBtn").GetComponent<UIButton>();
		EventDelegate ev2 = new EventDelegate(OnUpInstructor);
		_upInstructorBtn.onClick.Add(ev2);

		_repairBtn = _LoserContainer.Find("repairBtn").GetComponent<UIButton>();
		EventDelegate ev3 = new EventDelegate(OnRepair);
		_repairBtn.onClick.Add(ev3);

		_upScienceBtn = _LoserContainer.Find("upScienceBtn").GetComponent<UIButton>();
		EventDelegate ev4 = new EventDelegate(OnUpScience);
		_upScienceBtn.onClick.Add(ev4);

		_Star_Container = transform.Find("BattleSettlement/Star_Container");

		_star_1 = _Star_Container.Find("Sprite/winSprite/star_bg_1/star_1").gameObject;
		_star_2 = _Star_Container.Find("Sprite/winSprite/star_bg_2/star_2").gameObject;
		_star_3 = _Star_Container.Find("Sprite/winSprite/star_bg_3/star_3").gameObject;
		_starList.Clear();
		if(_starList != null)
		{
			_starList.Add(_star_1);
			_starList.Add(_star_2);
			_starList.Add(_star_3);
		}

		_star_bg_1 = _Star_Container.Find("Sprite/winSprite/star_bg_1").gameObject;
		_star_bg_2 = _Star_Container.Find("Sprite/winSprite/star_bg_2").gameObject;
		_star_bg_3 = _Star_Container.Find("Sprite/winSprite/star_bg_3").gameObject;

		_starBgList.Clear();
		if(_starBgList != null)
		{
			_starBgList.Add(_star_bg_1);
			_starBgList.Add(_star_bg_2);
			_starBgList.Add(_star_bg_3);
		}
		_winSprite   = _Star_Container.Find("Sprite/winSprite").GetComponent<UISprite>();
		_loserSprite = _Star_Container.Find("Sprite/loserSprite").GetComponent<UISprite>();

		_gameTipsPanel = UIHelper.FindChildInObject (gameObject, "ToolTipPanel");
	}
 
	override public void Open(params System.Object[] parameters)
	{
		base.Open (parameters);
	
		bool isWin = (bool)parameters[0];
		if (isWin) 
		{
			UpdateWinUI ();
		} 
		else
		{
			UpdateLoseUI ();
		}
	}

	//胜利逻辑 
	void UpdateWinUI()
	{
		_Settlement_Container.gameObject.SetActive(true);
		_LoserContainer.gameObject.SetActive(false);
		_winSprite.gameObject.SetActive(true);
		_loserSprite.gameObject.SetActive(false);


		if(InstancePlayer.instance.pvpUser == null)
		{
			// 军官经验
			List<Model_HeroGroup.ExpChangeResult> battleHeroGotExp =  InstancePlayer.instance.battleHeroGotExp;
			if (battleHeroGotExp != null) 
			{
				foreach (Model_HeroGroup.ExpChangeResult heroExp in battleHeroGotExp) 
				{
					UpdateHerosExp(_Instructor_Grid.gameObject, heroExp);
				}
			}
			// 掉落Item
			List<PrizeItem> prizeItems = InstancePlayer.instance.battleGotPrizeItems;
			if(prizeItems != null)
			{
				foreach(PrizeItem item in prizeItems)
				{
					UpdateDropItem(_Drop_Grid.gameObject, item);
				}
			}
		}
		else
		{
			// 荣誉
			UpdateDropItem(_Drop_Grid.gameObject, null, true);
		}
		 
		// 玩家升级
		Model_User model_User = InstancePlayer.instance.model_User;
		if (model_User.honorLevelChanged > 0) 
		{
			UIController.instance.CreatePanel (UICommon.UI_PANEL_INSTRUCTOR_UPLEVEL);
		}
	}

	private void UpdateHerosExp (GameObject parentGrid, Model_HeroGroup.ExpChangeResult heroExp)
	{
		if(parentGrid != null)
		{
			GameObject btnItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "battleSettlement/InstructorItem");
			GameObject item = NGUITools.AddChild(parentGrid,btnItem);
			SettlementInstructorItem Itemclass = item.GetComponent<SettlementInstructorItem>(); 
			Itemclass.InitData(heroExp);
			_Drop_Grid.repositionNow = true;
			_Drop_Grid.Reposition();
		}
	}

	private void UpdateDropItem (GameObject parentGrid, PrizeItem data, bool isHonor = false)
	{
		if(parentGrid != null)
		{
			GameObject btnItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "battleSettlement/DropItem");
			GameObject item = NGUITools.AddChild(parentGrid,btnItem);

			SettlementDropItem btnItemclass = item.GetComponent<SettlementDropItem>();

			if (!isHonor) 
			{
				btnItemclass.InitData (data, _gameTipsPanel);
			}
			else
			{
				btnItemclass.InitHonor(_gameTipsPanel);
			}
			_Instructor_Grid.Reposition();
		}
	}


	//失败逻辑
	void UpdateLoseUI()
	{
		_Settlement_Container.gameObject.SetActive(false);
		_LoserContainer.gameObject.SetActive(true);
		_winSprite.gameObject.SetActive(false);
		_loserSprite.gameObject.SetActive(true);
	}
		
	// 底部通用逻辑

	// 成功---------------------------------------------------------

	// 确定
	private void OnOk()
	{
		SwitchMainUIScene (InstancePlayer.instance.uiDataStatus.state);
	}

	private void OnShare()
	{
		// TODO
		Trace.trace("分享",Trace.CHANNEL.UI);
	}

	private void OnRePlay()
	{
		// TODO
		Trace.trace("重播",Trace.CHANNEL.UI);
	}


	 // 失败---------------------------------------------------- 

	//升级建筑
	private void OnUpBuiling()
	{
		SwitchMainUIScene (UIDataStatus.STATE.BUILDING_UPGRADE);
	}

	// 提升统帅 
	private void OnUpInstructor()
	{
		SwitchMainUIScene (UIDataStatus.STATE.HERO);
	}

	//修理坦克
	private void OnRepair()
	{
		SwitchMainUIScene (UIDataStatus.STATE.TANK_REPAIR);
	}

	//研发科技
	private void OnUpScience()
	{
		SwitchMainUIScene (UIDataStatus.STATE.TECHNOLOGY);
	}

	private void SwitchMainUIScene(UIDataStatus.STATE uiState)
	{
		InstancePlayer.instance.uiDataStatus.state = uiState;
		SceneHelper.SwitchScene (AppConfig.SCENE_NAME_UI);
	}

}
