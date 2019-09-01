using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleFormationPanel : PanelBase {

	public enum STATE
	{ 
		UNIT,   // 阵型
		HERO  // 军官
	}

//--------------------UI

	public const int BOTTOM_TAB_COUNT = 5;
	public const int UNIT_SHOW_MAX = 7;

	public const int TEAM_COUNT = 3;
	public const int SLOT_COUNT = 6;

	public UIButton _close_btn;
	public UIButton _battle_btn;
	public UIButton _replenish_btn;

	public UILabel  _attack_Label;
	
	public List<UIScrollView> unit_scView_list = new List<UIScrollView> (BOTTOM_TAB_COUNT);
	public List<UIGrid> unit_grid_list = new List<UIGrid>(BOTTOM_TAB_COUNT);
	
	public bool[] _isInit = new bool[5];
	public int selectUnitIndex = 0;  // 当前选中unit类型列表

	public UIDragDropRoot dragDropRoot;
	public UIPanel UIDragDropRootPanel;

	public GameObject[] formation_team = new GameObject[TEAM_COUNT];
	public UIButton[] formation_select_btn = new UIButton[TEAM_COUNT];

	public STATE _state;

	public bool last_top_layer = false;

	
//--------------------data

	private const string title_bg_normal = "title_normal";
	private const string title_bg_select = "title_select";
	private const string team_number_normal = "team_{0}";
	private const string team_number_select = "team_{0}_select";

	private Color font_tab_normal = new Color (208.0f / 255.0f, 206.0f / 255.0f, 191.0f / 255.0f);
	private Color font_tab_select = new Color (241.0f / 255.0f, 242.0f / 255.0f, 243.0f / 255.0f);
	
	private const string UNIT_ITEM_PATH = "profab/ui/BattleFormation/formation_unit_item";

	public FormationUnitCategory formationUnitCategory;
	public Model_Formation model_Formation;
	public FormationSlotUIManager slotUIManager;
	public FormationUIController formationUIController;

	public List<GameObject> particleList = new List<GameObject> ();// 记录粒子GameObject
	 
	public UIHeroPanel _heroPanel;

	
	void Start()
	{
		// test
		//InstancePlayer.instance.model_User.model_Formation.CreateLocalData(); // 传入添加阵型测试数据
		//Init ();
	}

	public override void Init ()
	{
		base.Init ();

		animationType = AnimationType.ALPHA;

		dragDropRoot = UIHelper.FindChildInObject (gameObject, "UIDragDropRoot").GetComponent<UIDragDropRoot>();
	    UIDragDropRootPanel = dragDropRoot.GetComponent<UIPanel>();

		_close_btn = UIHelper.FindChildInObject (gameObject, "CloseBtn").GetComponent<UIButton> ();
		_battle_btn = UIHelper.FindChildInObject (gameObject, "BattleBtn").GetComponent<UIButton> ();

		UpdateBtnState (_battle_btn.gameObject);

		_attack_Label = UIHelper.FindChildInObject (gameObject, "EnergyLabel").transform.Find ("Label").GetComponent<UILabel> ();
		_replenish_btn = UIHelper.FindChildInObject (gameObject, "supplybtn").GetComponent<UIButton> ();

		formationUIController = gameObject.GetComponent<FormationUIController> ();


		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			formation_team[i] = UIHelper.FindChildInObject(gameObject, "Formation_Team_" + i);
		}

		CreateScrollView ();

		AddBtnClick ();

		// 当前玩家拥有Unit分组
		formationUnitCategory = new FormationUnitCategory (InstancePlayer.instance.model_User.unlockUnits);

		// 阵型数据 
		model_Formation = InstancePlayer.instance.model_User.model_Formation;

		// 初始化军官
		_heroPanel = this.GetComponent<UIHeroPanel>();
		_heroPanel.Init (this);

		// slot UI Manager
		slotUIManager = this.GetComponent<FormationSlotUIManager> ();

		// 初始化 slot 数据
		InitSlotUI ();

		// 默认显示Team 1 

		int teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId ();
		if (teamId <= 0) {
			teamId = 0;
		} else {
			teamId = teamId - 1;
		}
		UpdateTeamSlotUI (teamId);
		UpdateBottomBtnUI ();

		// 更新底部 scrollView列表
		UpdateUnitList (0);

		//  默认显示unit
		UpdateState (STATE.UNIT);
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
	}

	public void CreateScrollView()
	{
		GameObject scViewContainer = UIHelper.FindChildInObject (gameObject, "TankFormationContainer");

		for (int i = 0; i < BOTTOM_TAB_COUNT; ++i) 
		{
			unit_scView_list.Add(UIHelper.FindChildInObject (scViewContainer, ("tank_scView_" + i)).GetComponent<UIScrollView>());
			unit_grid_list.Add(UIHelper.FindChildInObject (unit_scView_list[i].gameObject, "ui_grid").GetComponent<UIGrid>());
		
			_isInit[i] = false;
		}
	}
	
	public void AddBtnClick()
	{
		Transform tabs = UIHelper.FindChildInObject (gameObject, "bottom_btn_title").transform;
		for (int i = 0; i < BOTTOM_TAB_COUNT; ++i) 
		{
			UIButton btn = tabs.Find("tab_btn_" + i).GetComponent<UIButton>();
			UIHelper.AddBtnClick(btn, OnBottomBtnClick);
		}

		Transform tank_container = UIHelper.FindChildInObject (gameObject, "team_btn_container").transform;
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			formation_select_btn[i] = tank_container.Find("team_btn_" + i).GetComponent<UIButton>();
			UIHelper.AddBtnClick(formation_select_btn[i], OnSelectTeamClick);
		}

		// 军官
		UIButton btn_hero = tabs.Find("tab_hero").GetComponent<UIButton>();
		UIHelper.AddBtnClick(btn_hero, OnHeroBtnClick);

		// 关闭
		UIHelper.AddBtnClick (_close_btn, OnClosed);

		// 保存
		UIHelper.AddBtnClick (_battle_btn, OnSave);

		// 一键补兵
		UIHelper.AddBtnClick (_replenish_btn, OnReplenish);
	}

	//----------------------- team 选择列表
	
	// Team 切换按钮回调
	public void OnSelectTeamClick(GameObject sender)
	{
		int clickId = 0;
		if (sender.name.Equals ("team_btn_0")) {
			clickId = 0;
		} else if (sender.name.Equals ("team_btn_1")) {
			clickId = 1;
		} else if (sender.name.Equals ("team_btn_2")) {
			clickId = 2;
		}

		UpdateTeamSelect (clickId);

		if (_state == STATE.UNIT) 
		{
			UpdateCurrectUnitList();
			DestroyAllParticle ();
		}
		else if (_state == STATE.HERO)
		{
			_heroPanel.UpdateScrollView ();
		}
	}

	private void UpdateTeamSelect(int i)
	{
		int teamId = i + 1;
		model_Formation.SetSelectTeamId (teamId);
		UpdateTeamSlotUI (i);
	}

	// 更新布阵slot UI
	public void UpdateTeamSlotUI(int teamId)
	{
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			UIButton btn = formation_select_btn [i];
			UISprite sprite = btn.transform.Find("label").GetComponent<UISprite>();

			if(i == teamId) 
			{
				formation_team[i].SetActive(true);

				btn.normalSprite = title_bg_select;
				string path = string.Format(team_number_select, i + 1);
				sprite.spriteName = path;

			}
			else
			{
				formation_team[i].SetActive(false);

				formation_select_btn [i].normalSprite = title_bg_normal;
				string path = string.Format(team_number_normal, i + 1);
				sprite.spriteName = path;
			}
		}

		// 处理UIDragDropRoot下的上阵Item 
		slotUIManager.SetSlotUIEanbled(teamId);
	}

	public void ResetCurrentTeamSlotItemUI()
	{
		int teamId = model_Formation.GetSelectTeamId ();
		if (teamId > 0) 
		{
			slotUIManager.ResetTeamSlotItemUI(teamId);
		}
	}

	//----------------------- 初始化Slot
	
	public void InitSlotUI()
	{
		for (int i = 0; i < TEAM_COUNT; ++i)
		{
			Transform slots = UIHelper.FindChildInObject (gameObject, "Formation_Team_" + i).transform;
			for (int j = 0; j < SLOT_COUNT; ++j) 
			{
				Model_UnitGroup model_UnitGroup = model_Formation.GetUnitGroup(i + 1, j + 1);

				Transform slotTransUnit =  slots.Find("tank_slot_" + j);
				FormationSlotUI slotUIUnit = slotTransUnit.GetComponent<FormationSlotUI>(); 
				slotUIUnit.i = i;
				slotUIUnit.j = j;
				slotUIManager.SetUnitSlotUI(i, j, slotUIUnit);
				slotUIUnit.UpdateUI(model_UnitGroup, this);

				Transform slotTransHero =  slots.Find("hero_slot_" + j);
				UIHeroSlot slotUIHero = slotTransHero.GetComponent<UIHeroSlot>(); 
				slotUIHero.i = i;
				slotUIHero.j = j;
				slotUIManager.SetHeroSlotUI(i, j, slotUIHero);
				slotUIHero.UpdateUI(model_UnitGroup, this._heroPanel);
			}
		}
	}

	//----------------------- 更新底部Unit战斗队列


	// Unit 列表切换按钮回调
	public void OnBottomBtnClick(GameObject sender)
	{
		if (sender.name.Equals ("tab_btn_0")) {
			selectUnitIndex = 0;
		}
		else if (sender.name.Equals ("tab_btn_1")) {
			selectUnitIndex = 1;
		}
		else if (sender.name.Equals ("tab_btn_2")) {
			selectUnitIndex = 2;
		}
		else if (sender.name.Equals ("tab_btn_3")) {
			selectUnitIndex = 3;
		}
		else if (sender.name.Equals ("tab_btn_4")) {
			selectUnitIndex = 4;
		}

		UpdateState (STATE.UNIT);

		UpdateUnitList(selectUnitIndex);
		UpdateBottomBtnUI ();
	}

	// 更新底部按钮UI显示状态
	public void UpdateBottomBtnUI()
	{
		Transform tabs = UIHelper.FindChildInObject (gameObject, "bottom_btn_title").transform;
		for (int i = 0; i < BOTTOM_TAB_COUNT; ++i) 
		{
			UIButton btn = tabs.Find("tab_btn_" + i).GetComponent<UIButton>();
			UILabel label = btn.transform.Find("Label").GetComponent<UILabel>();
		
			if(i == selectUnitIndex)
			{
				btn.normalSprite = title_bg_select;
				label.color = font_tab_select;
			}
			else
			{
				btn.normalSprite = title_bg_normal;
				label.color = font_tab_normal;
			}
		}
	}

	// 取消Unit 队列选中状态
	public void ResetBottomBtnUI()
	{
		Transform tabs = UIHelper.FindChildInObject (gameObject, "bottom_btn_title").transform;
		for (int i = 0; i < BOTTOM_TAB_COUNT; ++i) 
		{
			UIButton btn = tabs.Find("tab_btn_" + i).GetComponent<UIButton>();
			UILabel label = btn.transform.Find("Label").GetComponent<UILabel>();


			btn.normalSprite = title_bg_normal;
			label.color = font_tab_normal;
		}
	}
	
	public void UpdateCurrectUnitList()
	{
		if (_state == STATE.UNIT)
		{
			UpdateUnitList (selectUnitIndex);
		}
	}
	
	public void UpdateUnitList(int index)
	{
		// 获取排序后的UI列表
		List<FormationUnitCategory.Unit> units = formationUnitCategory.GetSortUnit (index);

		for (int i = 0; i < BOTTOM_TAB_COUNT; ++i) 
		{
			GameObject sv = unit_scView_list[i].gameObject;
			sv.SetActive(false);
			if(i == index)
			{
				sv.SetActive(true);
			}
		}

		if (!_isInit [index]) 
		{
			_isInit [index] = true;

			int id = 0;

			foreach (FormationUnitCategory.Unit unit in units)
			{
				GameObject prefab = Resources.Load (UNIT_ITEM_PATH) as GameObject;
				GameObject obj = NGUITools.AddChild (unit_grid_list [index].gameObject, prefab);
				unit_grid_list [index].AddChild (obj.transform);

				obj.name = UIHelper.GetItemSuffix(++id);

				UIDragScrollView dragScrollView = obj.GetComponent<UIDragScrollView>();
				if(dragScrollView == null)
				{
					dragScrollView = obj.AddComponent<UIDragScrollView>();
					dragScrollView.scrollView = unit_scView_list[index];
				}

				FormationScrollViewItemUI scViewItemUI = obj.GetComponent<FormationScrollViewItemUI> ();
				scViewItemUI.UpdateUI (unit);

				FormationSlotItemUI slotItemUI = obj.GetComponent<FormationSlotItemUI> ();
				slotItemUI.UpdateUI (unit.id);

				FormationDragItemUI itemDrag = obj.GetComponent<FormationDragItemUI>();
				itemDrag.battlePanel = this;

				obj.transform.Find("formation_unit_slot").gameObject.SetActive(false);
				obj.transform.Find("formation_unit_scrollview").gameObject.SetActive(true);
			}

			// scrollView cell 排序
			unit_grid_list [index].animateSmoothly = false;
			unit_grid_list [index].repositionNow = true;

			// 小于单行最多显示 停止滑动
			if(units.Count < UNIT_SHOW_MAX)
			{
				unit_scView_list[index].enabled = false;
			}
			else
			{
				unit_scView_list[index].enabled = true;
			}
		}
		else
		{
			int id = 0;
			foreach (FormationUnitCategory.Unit unit in units)
			{
				string cellPath = UIHelper.GetItemSuffix(++id);
				GameObject cell = unit_grid_list[index].transform.Find(cellPath).gameObject;

				FormationScrollViewItemUI scViewItemUI = cell.GetComponent<FormationScrollViewItemUI> ();
				scViewItemUI.UpdateUI (unit);
				
				FormationSlotItemUI slotItemUI = cell.GetComponent<FormationSlotItemUI> ();

				if(unit.isOnDuty)
				{
					int teamId = model_Formation.GetSelectTeamId();
					Model_UnitGroup unitGroup = model_Formation.GetUnitGroupByUnitId(teamId, unit.id);
					if(unitGroup != null)
					{
						slotItemUI.UpdateUI (unitGroup);
					}
				}
				else
				{
					slotItemUI.UpdateUI (unit.id);
				}
			}
		}
	}

	// 更新战斗力
	public void UpdateAttackPowerUI()
	{
		if (_attack_Label != null && model_Formation != null)
		{
			int teamAP = model_Formation.CalcPower ();
			_attack_Label.text = teamAP + "";
		}
	}

	// 一键补兵
	public void OnReplenish()
	{	
		int teamId = model_Formation.GetSelectTeamId ();
		Model_Formation.RESULT result = model_Formation.TeamReplenish (teamId);

		if (result != Model_Formation.RESULT.SUCCESS) 
		{
			string msg = "没有可战斗Unit";
			UIHelper.ShowTextPromptPanel (this.gameObject, msg);
		}
		else 
		{
			slotUIManager.UpdateUnitSlotUI (teamId);

			if (_state == STATE.UNIT)
			{
				UpdateCurrectUnitList ();
			}
		}
	}


	// 战斗或保存
	public void OnSave()
	{
		UIDataStatus pageStatus = InstancePlayer.instance.uiDataStatus;
		BattleConnection.instance.SetFormation ();
	}

	public void UpdateBtnState(GameObject obj)
	{
		GameObject save = obj.transform.Find ("save").gameObject;
		GameObject battle = obj.transform.Find ("battle").gameObject;
		save.SetActive (true);
		battle.SetActive (false);
	}
	
	public void OnClosed()
	{
		Delete ();
	}

	// 切换阵型时 删除粒子实例
	public void DestroyAllParticle()
	{
		for (int i = particleList.Count - 1; i >= 0; --i) 
		{
			GameObject go = particleList [i];
			if (go != null)
			{
				DestroyImmediate (go);
			}
		}
	}

// 军官------------------------------------------------------------

	// 军官按钮点击
	public void OnHeroBtnClick()
	{
		UpdateState (STATE.HERO);
		_heroPanel.UpdateScrollView ();
	}

	public void UpdateState(STATE s)
	{
		_state = s;

		GameObject scrollview_bg_hero = UIHelper.FindChildInObject (gameObject, "scrollView_bg_hero");
		GameObject scrollview_bg_unit = UIHelper.FindChildInObject (gameObject, "scrollView_bg_unit");

		switch (_state) 
		{
		case STATE.UNIT:
			{
				_heroPanel._scrollView.gameObject.SetActive(false);
				scrollview_bg_hero.SetActive (false);
				scrollview_bg_unit.SetActive (true);
			}
			break;
		case STATE.HERO:
			{
				// 阵型scView
				int scViewCount = unit_scView_list.Count;
				for (int i = 0; i < scViewCount; ++i) 
				{
					unit_scView_list [i].gameObject.SetActive(false);		
				}

				_heroPanel._scrollView.gameObject.SetActive (true);
				ResetBottomBtnUI ();

				scrollview_bg_hero.SetActive (true);
				scrollview_bg_unit.SetActive (false);
			}

			break;
		}
	}

	void Update () 
	{
		UpdateAttackPowerUI ();

		UpateLayer ();
	}


	//  层级刷新
	private void UpateLayer()
	{
		bool isTop = UIPanelManager.instance.IsTopPanel(this);

		if (isTop != last_top_layer) 
		{
			int sortingOrder = 0;
			if (isTop) {
				sortingOrder = 1;
			} else {
				sortingOrder = 0;
			}

			if (UIDragDropRootPanel != null) 
			{
				RenderHelper.SetUIPanelSortingOrder (UIDragDropRootPanel, sortingOrder);
			}

			int teamId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamId ();
			slotUIManager.SetUnitLayer (teamId, sortingOrder);

			last_top_layer = isTop;
		}
	}

}
