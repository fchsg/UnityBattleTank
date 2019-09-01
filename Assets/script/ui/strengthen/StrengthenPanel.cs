using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class StrengthenPanel : PanelBase {
	private Transform _StrengthenContainer;
	private UIButton _closeBtn;

	private Transform _describeContainer;
	private UILabel _tankName_Label;
	private UILabel _systemName_Label;

	private Transform _attributeContainer;
	private UILabel _describeUpLevel;
	private UILabel _attribute_1_Label;
	private UILabel _1_Curruent_Label;
	private UILabel _1_willBe_Label;
	private UILabel _attribute_2_Label;
	private UILabel _2_Curruent_Label;
	private UILabel _2_willBe_Label;
	private UILabel _attribute_3_Label;
	private UILabel _3_Curruent_Label;
	private UILabel _3_willBe_Label;
	private UILabel _attribute_4_Label;
	private UILabel _4_Curruent_Label;
	private UILabel _4_willBe_Label;
	private UILabel _attribute_5_Label;
	private UILabel _5_Curruent_Label;
	private UILabel _5_willBe_Label;
	List<UILabel> _attributeList = new List<UILabel>();
	List<UILabel> _curruentList = new List<UILabel>();
	List<UILabel> _willBeList = new List<UILabel>();
	private Transform _resContainer;
	private UILabel _consumptionRes_Label;
	private UISprite _FoodSprite;
	private UILabel _1_Label;
	private UISprite _oilSprite;
	private UILabel _2_Label;
	private UISprite _matalSprite;
	private UILabel _3_Label;
	private UISprite _rareSprite;
	private UILabel _4_Label;

	private Transform _iconContainer;
	private UISprite _icon_bg_1;
	private UITexture _1_Sprite;
	private UILabel _1_icon_Label;
	private UISprite _icon_bg_2;
	private UITexture _2_Sprite;
	private UILabel _2_icon_Label;
	private UISprite _icon_bg_3;
	private UITexture _3_Sprite;
	private UILabel _3_icon_Label;
	List<UISprite> _iconBgList = new List<UISprite>();
	List<UITexture> _iconItemList = new List<UITexture>();
	List<UILabel> _iconLabelList = new List<UILabel>();
	private UIButton _StrengthenTenBtn;
	private UIButton _StrengthenBtn;
	private UILabel _StrengthenTenBtnLabel;
	private UILabel _StrengthenBtnLabel;

	private Transform _showContainer;
//	private UISprite _tank_bg;
	private UITexture _tank_icon;
	private UIButton _HpSystem_Btn;
	private UIButton _ApSystem_Btn;
	private UIButton _DrSystem_Btn;
	private UIButton _DpSystem_Btn;
	private UISprite _point_Sprite_1;
	private UISprite _point_Sprite_2;
	private UISprite _point_Sprite_3;
	private UISprite _point_Sprite_4;
	List<UISprite> _point_SpriteList = new List<UISprite>();
	private UILabel _system_Label_1;
	private UILabel _system_Label_2;
	private UILabel _system_Label_3;
	private UILabel _system_Label_4;
	private Transform _tankContainer;
	private UIScrollView _scrollview;
	private UIGrid _grid;
	public enum SystemType
	{
		HP = 0,//血量
		AP = 1,//攻击
		DP = 2,//防御
		DR = 3,//命中率
	}

	//data  
	List<TankDataManager.UnitData> _unitDataList ;
	Dictionary<int,StrengthenTankItem> _strengthenTankItemDic = new Dictionary<int, StrengthenTankItem>();
	TankDataManager.UnitData _currentUnitData;
	Dictionary<int,Model_Unit> _units;
	Model_Unit _modelUnit;
	DataUnitPart[] _unitPart;
	DataUnitPart _currentUnitPart;
	SystemType _partType;

	void Awake()
	{
		_StrengthenContainer = transform.Find("StrengthenContainer");
		_closeBtn = _StrengthenContainer.Find("showContainer/closeBtn").GetComponent<UIButton>();
		EventDelegate evcloes = new EventDelegate(Onclose);
		_closeBtn.onClick.Add(evcloes);
		_describeContainer = _StrengthenContainer.Find("describeContainer");
		_tankName_Label = _describeContainer.Find("tankName_Label").GetComponent<UILabel>();
		_systemName_Label = _describeContainer.Find("systemName_Label").GetComponent<UILabel>();
		_systemName_Label.color = UICommon.FONT_COLOR_GREY;
		_attributeContainer = _describeContainer.Find("attributeContainer");
		_describeUpLevel = _attributeContainer.Find("Sprite/attribute_Label").GetComponent<UILabel>();
		_describeUpLevel.color = UICommon.FONT_COLOR_GREY;

		_attribute_1_Label = _attributeContainer.Find("attribute_1_Label").GetComponent<UILabel>();
		_attribute_1_Label.color = UICommon.FONT_COLOR_GREY;
		_1_Curruent_Label = _attributeContainer.Find("attribute_1_Label/Curruent_Label").GetComponent<UILabel>();
		_1_Curruent_Label.color = UICommon.FONT_COLOR_GREY;
		_1_willBe_Label = _attributeContainer.Find("attribute_1_Label/willBe_Label").GetComponent<UILabel>();
		_1_willBe_Label.color = UICommon.FONT_COLOR_GREY;
		_attribute_2_Label = _attributeContainer.Find("attribute_2_Label").GetComponent<UILabel>();

		_2_Curruent_Label = _attributeContainer.Find("attribute_2_Label/Curruent_Label").GetComponent<UILabel>();
		_2_willBe_Label = _attributeContainer.Find("attribute_2_Label/willBe_Label").GetComponent<UILabel>();
		_attribute_3_Label = _attributeContainer.Find("attribute_3_Label").GetComponent<UILabel>();
		_3_Curruent_Label = _attributeContainer.Find("attribute_3_Label/Curruent_Label").GetComponent<UILabel>();
		_3_willBe_Label = _attributeContainer.Find("attribute_3_Label/willBe_Label").GetComponent<UILabel>();
		_attribute_4_Label = _attributeContainer.Find("attribute_4_Label").GetComponent<UILabel>();
		_4_Curruent_Label = _attributeContainer.Find("attribute_4_Label/Curruent_Label").GetComponent<UILabel>();
		_4_willBe_Label = _attributeContainer.Find("attribute_4_Label/willBe_Label").GetComponent<UILabel>();
		_attribute_5_Label = _attributeContainer.Find("attribute_5_Label").GetComponent<UILabel>();
		_5_Curruent_Label = _attributeContainer.Find("attribute_5_Label/Curruent_Label").GetComponent<UILabel>();
		_5_willBe_Label = _attributeContainer.Find("attribute_5_Label/willBe_Label").GetComponent<UILabel>();

		_attributeList.Clear();
		_curruentList.Clear();
		_willBeList.Clear();
		_attributeList.Add(_attribute_2_Label);
		_attributeList.Add(_attribute_3_Label);
		_attributeList.Add(_attribute_4_Label);
		_attributeList.Add(_attribute_5_Label);
		_curruentList.Add(_2_Curruent_Label);
		_curruentList.Add(_3_Curruent_Label);
		_curruentList.Add(_4_Curruent_Label);
		_curruentList.Add(_5_Curruent_Label);
		_willBeList.Add(_2_willBe_Label);
		_willBeList.Add(_3_willBe_Label);
		_willBeList.Add(_4_willBe_Label);
		_willBeList.Add(_5_willBe_Label);
		foreach(UILabel label in _curruentList)
		{
			label.color = UICommon.FONT_COLOR_ORANGE;
		}
		foreach(UILabel label in _willBeList)
		{
			label.color = UICommon.FONT_COLOR_GREEN;
		}
		foreach(UILabel label in _attributeList)
		{
			label.color = UICommon.FONT_COLOR_GREY;
			label.gameObject.SetActive(false);
		}
		 
		_resContainer = _describeContainer.Find("resContainer");
		_consumptionRes_Label = _resContainer.Find("resbg/consumptionRes_Label").GetComponent<UILabel>();
		_consumptionRes_Label.color = UICommon.FONT_COLOR_GREY;
		_FoodSprite = _resContainer.Find("FoodSprite").GetComponent<UISprite>();
		_1_Label = _resContainer.Find("FoodSprite/Label").GetComponent<UILabel>();
		_oilSprite = _resContainer.Find("oilSprite").GetComponent<UISprite>();
		_2_Label = _resContainer.Find("oilSprite/Label").GetComponent<UILabel>();
		_matalSprite = _resContainer.Find("matalSprite").GetComponent<UISprite>();
		_3_Label = _resContainer.Find("matalSprite/Label").GetComponent<UILabel>();
		_rareSprite = _resContainer.Find("rareSprite").GetComponent<UISprite>();
		_4_Label = _resContainer.Find("rareSprite/Label").GetComponent<UILabel>();
		_1_Label.color = UICommon.FONT_COLOR_GREY;
		_2_Label.color = UICommon.FONT_COLOR_GREY;
		_3_Label.color = UICommon.FONT_COLOR_GREY;
		_4_Label.color = UICommon.FONT_COLOR_GREY;
		_iconContainer = _describeContainer.Find("iconContainer");
		_icon_bg_1 = _iconContainer.Find("icon_bg_1").GetComponent<UISprite>();
		_1_Sprite = _iconContainer.Find("icon_bg_1/Sprite").GetComponent<UITexture>();
		_1_icon_Label = _iconContainer.Find("icon_bg_1/Label").GetComponent<UILabel>();
		_icon_bg_2 = _iconContainer.Find("icon_bg_2").GetComponent<UISprite>();
		_2_Sprite = _iconContainer.Find("icon_bg_2/Sprite").GetComponent<UITexture>();
		_2_icon_Label = _iconContainer.Find("icon_bg_2/Label").GetComponent<UILabel>();
		_icon_bg_3 = _iconContainer.Find("icon_bg_3").GetComponent<UISprite>();
		_3_Sprite = _iconContainer.Find("icon_bg_3/Sprite").GetComponent<UITexture>();
		_3_icon_Label = _iconContainer.Find("icon_bg_3/Label").GetComponent<UILabel>();
		_iconBgList.Clear();
		_iconItemList.Clear();
		_iconLabelList.Clear();
		_iconBgList.Add(_icon_bg_1);
		_iconBgList.Add(_icon_bg_2);
		_iconBgList.Add(_icon_bg_3);
		_iconItemList.Add(_1_Sprite);
		_iconItemList.Add(_2_Sprite);
		_iconItemList.Add(_3_Sprite);
		_iconLabelList.Add(_1_icon_Label);
		_iconLabelList.Add(_2_icon_Label);
		_iconLabelList.Add(_3_icon_Label);
		_StrengthenTenBtn = _describeContainer.Find("StrengthenTenBtn").GetComponent<UIButton>();
		EventDelegate evTenbtn = new EventDelegate(OnTen);
		_StrengthenTenBtn.onClick.Add(evTenbtn);
		_StrengthenBtn = _describeContainer.Find("StrengthenBtn").GetComponent<UIButton>();
		EventDelegate evOnebtn = new EventDelegate(OnOne);
		_StrengthenBtn.onClick.Add(evOnebtn);
		_StrengthenTenBtnLabel = _StrengthenTenBtn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_StrengthenTenBtnLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_StrengthenBtnLabel = _StrengthenBtn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_StrengthenBtnLabel.color = UICommon.FONT_COLOR_GOLDEN;
	
		_tankContainer = _StrengthenContainer.Find("tankContainer");
		_scrollview = _tankContainer.Find("ScrollView").GetComponent<UIScrollView>();
		_grid = _tankContainer.Find("ScrollView/Grid").GetComponent<UIGrid>();

		_showContainer = _StrengthenContainer.Find("showContainer");
//		_tank_bg = _showContainer.FindChild("tank_bg").GetComponent<UISprite>();
		_tank_icon = _showContainer.Find("tank_icon").GetComponent<UITexture>();
		_HpSystem_Btn = _showContainer.Find("HpSystem_Btn").GetComponent<UIButton>();
		EventDelegate evhp = new EventDelegate(OnHP);
		_HpSystem_Btn.onClick.Add(evhp);

		_ApSystem_Btn = _showContainer.Find("ApSystem_Btn").GetComponent<UIButton>();
		EventDelegate evap = new EventDelegate(OnAP);
		_ApSystem_Btn.onClick.Add(evap);

		_DrSystem_Btn = _showContainer.Find("DrSystem_Btn").GetComponent<UIButton>();
		EventDelegate evdr = new EventDelegate(OnDR);
		_DrSystem_Btn.onClick.Add(evdr);

		_DpSystem_Btn = _showContainer.Find("DpSystem_Btn").GetComponent<UIButton>();
		EventDelegate evdp = new EventDelegate(OnDP);
		_DpSystem_Btn.onClick.Add(evdp);
		_point_Sprite_1 = _showContainer.Find("HpSystem_Btn/point_Sprite").GetComponent<UISprite>();
		_point_Sprite_2 = _showContainer.Find("ApSystem_Btn/point_Sprite").GetComponent<UISprite>();
		_point_Sprite_3 = _showContainer.Find("DpSystem_Btn/point_Sprite").GetComponent<UISprite>();
		_point_Sprite_4 = _showContainer.Find("DrSystem_Btn/point_Sprite").GetComponent<UISprite>();
		_point_SpriteList.Clear();
		_point_SpriteList.Add(_point_Sprite_1);
		_point_SpriteList.Add(_point_Sprite_2);
		_point_SpriteList.Add(_point_Sprite_3);
		_point_SpriteList.Add(_point_Sprite_4);


		_system_Label_1 = _HpSystem_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_system_Label_1.color = UICommon.FONT_COLOR_GREY;
		_system_Label_2 = _ApSystem_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_system_Label_2.color = UICommon.FONT_COLOR_GREY;
		_system_Label_3 = _DrSystem_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_system_Label_3.color = UICommon.FONT_COLOR_GREY;
		_system_Label_4 = _DpSystem_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_system_Label_4.color = UICommon.FONT_COLOR_GREY;
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		NotificationCenter.instance.AddEventListener(Notification_Type.RefreshStrengthenTank,OnUpdateTank);
		NotificationCenter.instance.AddEventListener(Notification_Type.RequestStrengthenPart,OnStrengthenPart);
		InitData();
	}
	void InitData()
	{
		_strengthenTankItemDic.Clear();
		List<int> unlockUnits = InstancePlayer.instance.model_User.unlockUnitsId;
		List<DataUnit> dataUnitList = new List<DataUnit>();
		dataUnitList.Clear();
		foreach(int id in unlockUnits)
		{
			DataUnit dataunit = DataManager.instance.dataUnitsGroup.GetUnit(id);
			dataUnitList.Add(dataunit);
		}
		TankDataManager tankManagetr = new TankDataManager();
		_unitDataList = tankManagetr.StrengthenSort(dataUnitList);

		if(_unitDataList.Count > 0)
		{
			_currentUnitData = _unitDataList[0];
			UpdataTankInfo(_currentUnitData);
			_units = InstancePlayer.instance.model_User.unlockUnits;
			_units.TryGetValue(_currentUnitData.id,out _modelUnit);

			_unitPart = _modelUnit.GetDataParts();
			_currentUnitPart = _unitPart[0];
			_partType = SystemType.HP;
			UpdataUnitPart(_currentUnitPart,_partType);
		}

	}
	 
	void OnUpdateTank(Notification notification)
	{
		TankDataManager.UnitData unit = notification._data as TankDataManager.UnitData;
		_currentUnitData = unit;
		UpdataTankInfo(_currentUnitData);
		OnHP();
	}
	void OnStrengthenPart(Notification notification)
	{
		int times = (int)notification._data ;
		if(times == 1)
		{
			OnOne();
		}
		else
		{
			OnTen();
		}
	}
	void Update () {

		_currentUnitData = _currentUnitData;
		_partType = _partType;
		if(_unitDataList != null)
		{
			SetWrapContent(_grid,_unitDataList,_strengthenTankItemDic,OnUpdateItem);
		}
		if(_currentUnitData != null)
		{
			UpdataTankInfo(_currentUnitData);
			_units = InstancePlayer.instance.model_User.unlockUnits;
			_units.TryGetValue(_currentUnitData.id,out _modelUnit);

			_unitPart = _modelUnit.GetDataParts();
			_currentUnitPart = _unitPart[(int)_partType];
			UpdataPromptPoint(_currentUnitData);
		}
		if(_currentUnitPart != null)
		{
			UpdataUnitPart(_currentUnitPart,_partType);
		}
	}
	void UpdataTankInfo(TankDataManager.UnitData unitData)
	{
		if(unitData != null)
		{
			_currentUnitData = unitData;
			_tank_icon.SetUnitBigTexture(_currentUnitData.id);
//			_tank_bg.spriteName = _currentUnitData.iconBgName;
			_tankName_Label.color = _currentUnitData.nameColor;
			_tankName_Label.text = _currentUnitData.unitData.name;
		}
	}
	void UpdataPromptPoint(TankDataManager.UnitData unitData)
	{
		if(unitData != null)
		{
			_units = InstancePlayer.instance.model_User.unlockUnits;
			_units.TryGetValue(_currentUnitData.id,out _modelUnit);

			_unitPart = _modelUnit.GetDataParts();
			int count = _unitPart.Length;
			for(int i = 0;i < count;i++)
			{
				PBConnect_upgradeUnitPart.RESULT result = PBConnect_upgradeUnitPart.CheckUpgrade(_modelUnit.unitId,i);
				if(result == PBConnect_upgradeUnitPart.RESULT.OK)
				{ 
					_point_SpriteList[i].gameObject.SetActive(true);
				}
				else
				{
					_point_SpriteList[i].gameObject.SetActive(false);
				}
			}
		}
	}
	void UpdataUnitPart(DataUnitPart unitPart,SystemType type)
	{
		List<float> batterdata = new List<float>();
		List<float> willbatterdata = new List<float>();
		List<string> attrstr = new List<string>();
		batterdata.Clear();
		attrstr.Clear();
		willbatterdata.Clear();
		bool isMaxLevel = false;
		int maxLevelPart = DataManager.instance.dataUnitPartGroup.GetPartMaxLevel(unitPart.id);
		int partLevel = 1;
		if(unitPart.level < maxLevelPart)
		{
			isMaxLevel = false;
			partLevel = unitPart.level + 1;
			_1_Curruent_Label.text = unitPart.level.ToString();
			_1_willBe_Label.gameObject.SetActive(true);
			_1_willBe_Label.text = (unitPart.level + 1).ToString() ;
		}
		else
		{
			isMaxLevel = true;
			partLevel = unitPart.level ;
			_1_Curruent_Label.text = "已满级";
			_1_willBe_Label.gameObject.SetActive(false);
		}
		DataUnitPart nextLevelPart = DataManager.instance.dataUnitPartGroup.GetPart(unitPart.id,partLevel);

		string systemName = "";
		switch(type)
		{
		case SystemType.HP:
			systemName = "车体系统";
		
			batterdata.Add(unitPart.battleParam.hp);
			willbatterdata.Add(nextLevelPart.battleParam.hp);
			attrstr.Add("血量");
			break;
		case SystemType.AP:
			systemName = "武器系统";
			batterdata.Add(unitPart.battleParam.damage);
			willbatterdata.Add(nextLevelPart.battleParam.damage);
			attrstr.Add("攻击");
			break;
		case SystemType.DR:
			systemName = "动力系统";
			batterdata.Add(unitPart.battleParam.hitRate);
			willbatterdata.Add(nextLevelPart.battleParam.hitRate);
			attrstr.Add("命中率");
			break;
		case SystemType.DP:
			systemName = "护甲系统";
			batterdata.Add(unitPart.battleParam.ammo);
			willbatterdata.Add(nextLevelPart.battleParam.ammo);
			attrstr.Add("防御");
			break;
		}

		if(unitPart.battleParam.dodgeRate != 0.0f)
		{
			batterdata.Add(unitPart.battleParam.dodgeRate);
			willbatterdata.Add(nextLevelPart.battleParam.dodgeRate);
			attrstr.Add("闪避率");
		}
		if(unitPart.battleParam.doubleDamageRate != 0.0f)
		{
			batterdata.Add(unitPart.battleParam.doubleDamageRate);
			willbatterdata.Add(nextLevelPart.battleParam.doubleDamageRate);
			attrstr.Add("暴击率");
		}
		int count = attrstr.Count;
		for(int i = 0;i < count; i++)
		{
			_attributeList[i].gameObject.SetActive(true);
			_attributeList[i].text = attrstr[i];
			_curruentList[i].text = batterdata[i].ToString();
			if(isMaxLevel)
			{
				_willBeList[i].gameObject.SetActive(false);
			}
			else
			{
				_willBeList[i].gameObject.SetActive(true);
			}
			_willBeList[i].text = willbatterdata[i].ToString();
		}
		_systemName_Label.text = systemName + "(LV." +unitPart.level.ToString() + ")";
		 
		_1_Label.text = unitPart.cost.costFood.ToString();
		_2_Label.text = unitPart.cost.costOil.ToString();
		_3_Label.text = unitPart.cost.costMetal.ToString();
		_4_Label.text = unitPart.cost.costRare.ToString();
		_iconBgList[2].gameObject.SetActive(false);
		if(unitPart.itemCost1 != null && unitPart.itemCost2 != null)
		{
//			_iconBgList.Clear();
//			_iconItemList.Clear();
//			_iconLabelList.Clear();
			_iconItemList[0].SetItemTexture(unitPart.itemCost1.id);
			_iconLabelList[0].text = GetItemCount(unitPart.itemCost1);
			_iconItemList[1].SetItemTexture(unitPart.itemCost2.id);
			_iconLabelList[1].text = GetItemCount(unitPart.itemCost2);
		}
		else if(unitPart.itemCost1 != null && unitPart.itemCost2 == null)
		{
			_iconItemList[0].SetItemTexture(unitPart.itemCost1.id);
			_iconLabelList[0].text = GetItemCount(unitPart.itemCost1);
			_iconBgList[1].gameObject.SetActive(false);
		}
		else if(unitPart.itemCost2 != null && unitPart.itemCost1 == null)
		{
			_iconItemList[0].SetItemTexture(unitPart.itemCost2.id);
			_iconLabelList[0].text = GetItemCount(unitPart.itemCost2);
			_iconBgList[1].gameObject.SetActive(false);
		}
		else
		{
			_iconBgList[0].gameObject.SetActive(false);
			_iconBgList[1].gameObject.SetActive(false);
			_iconBgList[2].gameObject.SetActive(false);
		}
		 
	}
	string GetItemCount(DataUnit.ItemCost item)
	{
		Model_ItemGroup itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
		int haveCount = itemGroup.GetItemCount(item.id);
		int needCount = item.count;
		string str = "";
		if(haveCount > needCount)
		{
			str = UIHelper.SetStringSixteenColor(haveCount + "/" +needCount,UICommon.SIXTEEN_GREEN);
		}
		else
		{
			str = UIHelper.SetStringSixteenColor(haveCount.ToString(),UICommon.SIXTEEN_RED) + UIHelper.SetStringSixteenColor("/" +needCount,UICommon.SIXTEEN_GREEN);;
		}
		return str;
	}

	void OnUpdateItem(GameObject go, int index, int realIndex)
	{
		int index_ = 0;
		int indexList = Mathf.Abs(realIndex);
		StrengthenTankItem tankItem1 = go.transform.GetComponent<StrengthenTankItem>();
		tankItem1.Init(_unitDataList[indexList]);
		if(!_strengthenTankItemDic.ContainsKey(_unitDataList[indexList].id))
		{
			_strengthenTankItemDic.Add(_unitDataList[indexList].id,tankItem1);
		}

	}
	void SetWrapContent(UIGrid grid,List<TankDataManager.UnitData> dataList,Dictionary<int,StrengthenTankItem> dataDic ,UIWrapContent.OnInitializeItem OnUpdateItemMain)
	{
		int tankCont = dataList.Count;
		int gridCont = grid.GetChildList().Count;

		if(tankCont <= 7)
		{
			if(grid.gameObject.GetComponent<UIWrapContent>())
			{
				NGUITools.Destroy(grid.gameObject.GetComponent<UIWrapContent>());
			}
			if(gridCont != tankCont)
			{
				StartCoroutine(CreateUnit(grid,dataList,dataDic));

			}
			_scrollview.enabled = false;
		}
		else
		{
			_scrollview.enabled = true;
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
			if(gridCont != 8)
			{
				if (grid != null) {
					grid.DestoryAllChildren();
				}
				CreateWrapUnit(grid,8);
				if(wrap != null)
				{
					//绑定方法
					wrap.itemSize = (int)grid.cellWidth;
					wrap.minIndex = 0;
					wrap.maxIndex = (tankCont - 1);
					wrap.onInitializeItem = OnUpdateItemMain;
					wrap.enabled = true;
					wrap.SortAlphabetically();
				}
			} 
		}
	}


	IEnumerator CreateUnit(UIGrid grid, List<TankDataManager.UnitData> unitData,Dictionary<int,StrengthenTankItem> dataDic){
		yield return new WaitForSeconds (0.01f);	
		if (grid != null) {
			grid.DestoryAllChildren();
		}

		int tankCount = unitData.Count;
		 
		foreach(TankDataManager.UnitData unit in unitData)
		{
			if(grid.gameObject != null)
			{
				int index_ = 0;
				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "strengthen/StrengthenTankItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
//				item.name = "0" + i;
				StrengthenTankItem tankItem1 = item.transform.GetComponent<StrengthenTankItem>();
				tankItem1.Init(unit);
				if(!dataDic.ContainsKey(unit.id))
				{
					dataDic.Add(unit.id,tankItem1);
				}
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}

	void CreateWrapUnit(UIGrid grid,int num){
		if (grid != null) {
			grid.DestoryAllChildren();
		}
		for(int i = 0; i < num; i++)
		{
			if(grid.gameObject != null)
			{

				GameObject tankItem = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "strengthen/StrengthenTankItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,tankItem);
//				item.name = "0" + i;
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}
	void TenOrOne(int times)
	{
		_partType = _partType;
		_currentUnitPart = _currentUnitPart;
		_units.TryGetValue(_currentUnitData.id,out _modelUnit);
		PBConnect_upgradeUnitPart.RESULT result = PBConnect_upgradeUnitPart.CheckUpgrade(_modelUnit.unitId,(int)_partType);
		if(result == PBConnect_upgradeUnitPart.RESULT.OK)
		{
			if(times == 1)
			{
				UpgradeOneUnitPart();
			}
			else
			{
				UpgradeTenUnitPart();
			}

		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.LACK_ITEM1)
		{
			string str = "材料不足";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.LACK_ITEM2)
		{
			string str = "材料不足";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.LACK_MAIN_LEVEL)
		{
			string str = "车体系统等级不足，请升级！";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.LACK_PLAYER_LEVEL)
		{
			string str = "部件等级超过玩家等级！";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.MAX_LEVEL)
		{
			string str = "部件达到最大级别！";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.LACK_RESOURCE)
		{
			int[] resArr = Model_Helper.GetPlayerNeedBuyRes(_currentUnitPart.cost.costFood,_currentUnitPart.cost.costOil,_currentUnitPart.cost.costMetal,_currentUnitPart.cost.costRare);
			UIHelper.BuyResourcesUI(resArr,ResourcesBuyType.StrengThenPartType,times);
		}
		else if(result == PBConnect_upgradeUnitPart.RESULT.UNIT_LOCK)
		{
			string str = "UNIT_LOCK！";
			UIHelper.ShowTextPromptPanel(this.gameObject,str);
		}
	}

	void OnTen()
	{
		TenOrOne(10);
	}

	void OnOne()
	{
		TenOrOne(1);
	}
	void OnHP()
	{
		_currentUnitData = _currentUnitData;
		_units = InstancePlayer.instance.model_User.unlockUnits;
		_units.TryGetValue(_currentUnitData.id,out _modelUnit);

		_unitPart = _modelUnit.GetDataParts();
		_currentUnitPart = _unitPart[0];
		_partType = SystemType.HP;
		UpdataUnitPart(_currentUnitPart,_partType);
	}
	void OnAP()
	{
		_currentUnitData = _currentUnitData;
		_units = InstancePlayer.instance.model_User.unlockUnits;
		_units.TryGetValue(_currentUnitData.id,out _modelUnit);

		_unitPart = _modelUnit.GetDataParts();
		_currentUnitPart = _unitPart[1];
		_partType = SystemType.AP;
		UpdataUnitPart(_currentUnitPart,_partType);
	}

	void OnDP()
	{
		_currentUnitData = _currentUnitData;
		_units = InstancePlayer.instance.model_User.unlockUnits;
		_units.TryGetValue(_currentUnitData.id,out _modelUnit);

		_unitPart = _modelUnit.GetDataParts();
		_currentUnitPart = _unitPart[2];
		_partType = SystemType.DP;
		UpdataUnitPart(_currentUnitPart,_partType);
	}
	void OnDR()
	{
		_currentUnitData = _currentUnitData;
		_units = InstancePlayer.instance.model_User.unlockUnits;
		_units.TryGetValue(_currentUnitData.id,out _modelUnit);

		_unitPart = _modelUnit.GetDataParts();
		_currentUnitPart = _unitPart[3];
		_partType = SystemType.DR;
		UpdataUnitPart(_currentUnitPart,_partType);
	}
	void OnDestroy()
	{
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RefreshStrengthenTank);
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RequestStrengthenPart);

	}
	void Onclose()
	{
		this.Delete();
	}

	// ===========================


	void UpgradeOneUnitPart()
	{
		UIHelper.LoadingPanelIsOpen(true);
		_currentUnitData = _currentUnitData;
		_partType = _partType;
		UpgradeUnitPartRequest request = new UpgradeUnitPartRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = _currentUnitData.id;
		request.partIndex = (int)_partType;

		(new PBConnect_upgradeUnitPart ()).Send (request, OnUpgradeUnitPart);

	}
	void OnUpgradeUnitPart(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {		 
			Trace.trace ("OnUpgradeUnitPart succ", Trace.CHANNEL.UI);
		} else {
			Trace.trace ("OnUpgradeUnitPart failure", Trace.CHANNEL.UI);
		}
	}
	void UpgradeTenUnitPart()
	{
		UIHelper.LoadingPanelIsOpen(true);
		_currentUnitData = _currentUnitData;
		_partType = _partType;
		int times = _modelUnit.GetPartLevelMax((int)_partType) - _modelUnit.GetPartLevel((int)_partType);
		PBConnect_upgradeUnitPart.UpdateMultyTimes(OnUpgradeTenUnitPart,times,_currentUnitData.id,(int)_partType);
	}
	void OnUpgradeTenUnitPart(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {		 
			Trace.trace ("OnUpgrade Ten UnitPart succ", Trace.CHANNEL.UI);
		} else {
			Trace.trace ("OnUpgrade Ten UnitPart failure", Trace.CHANNEL.UI);
		}
	}



}



///*
//                   _ooOoo_
//                  o8888888o
//                  88" . "88
//                  (| -_- |)
//                  O\  =  /O
//               ____/`---'\____
//             .'  \\|     |//  `.
//            /  \\|||  :  |||//  \
//           /  _||||| -:- |||||-  \
//           |   | \\\  -  /// |   |
//           | \_|  ''\---/''  |   |
//           \  .-\__  `-`  ___/-. /
//         ___`. .'  /--.--\  `. . __
//      ."" '<  `.___\_<|>_/___.'  >'"".
//     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//     \  \ `-.   \_ __\ /__ _/   .-` /  /
//======`-.____`-.___\_____/___.-`____.-'======
//                   `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//         佛祖保佑       永无BUG
//*/

