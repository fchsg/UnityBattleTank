using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
// 建筑升级

public class BuildingUpLevel : PanelBase 
{
	
	// UI
	
	private UIButton _btnClose;	// 关闭按钮
	private UIButton _btnUpLevel;	// 升级按钮
	private UIButton _btnOnceUpLevel;	// 立即升级
	private UIButton _btnBugQueue;	// 购买队列
	private UISprite _btnOnceUpLevelSprite;

	private UILabel _pay_bug_queue_CoinValue ;// 购买队列消费金币值
	private UILabel _payTimeValue ;// 升级花费时间
	private UILabel _payCoinValue ;// 升级花费金币
	private UILabel _atOnceUpLevel_label;
	private UILabel _UpLevel_label;
	private UILabel _bug_queue_label;

	private UISprite _OnceUpLevelSprite;
	private UISprite _BugQueueSprite;
	
	private Transform _buildingInfo;
	private UILabel _buildingName_left_Label;// 当前建筑名称
	private UILabel _currentLevel;	// 当前等级

	private UILabel _netxLevelvalue;	// 下一等级
	private UILabel _propertyInfo_1_Label;	// 功能文字描述
	private UILabel _propertyValue_1_Label;	// 功能文字描述值
	private UILabel _propertyInfo_2_Label;
	private UILabel _propertyValue_2_Label;

	
	private Transform _buildingRequest;
	private UILabel _buildingName_1_right;	// 升级所需建筑名字
	private UILabel _buildingName_1_value;	// 升级所需建筑等级
	private UILabel _buildingName_2_right;
	private UILabel _buildingName_2_value;
	private UILabel _queue_right;
	private UILabel _queue_Name_3_value;

	private UIButton _Btn_goto_building_1;
	private UIButton _Btn_goto_building_2;
//	private UIButton _Btn_goto_building_3;
	
	private UILabel _resName_1;	// 升级所需资源名字
	private UILabel _res_1_value;	// 升级所需资源量
	
	private UILabel _resName_2;
	private UILabel _res_2_value;
	private UILabel _resName_3;
	private UILabel _res_3_value;
	private UILabel _resName_4; 
	private UILabel _res_4_value;

	private UIButton _Btn_getRes_1;
	private UIButton _Btn_getRes_2;
	private UIButton _Btn_getRes_3;
	private UIButton _Btn_getRes_4;

	private UISprite _queueSprite_1;
	private UISprite _builingSprite_1;
	private UISprite _builingSprite_2;
	private UISprite _resSprite_1;
	private UISprite _resSprite_2;
	private UISprite _resSprite_3;
	private UISprite _resSprite_4;

	private Transform _Unlocking_Container;
	private UIScrollView _Unlocking_ScrollView;
	private UIGrid _Unlocking_Grid;

	// data	
	Model_Building _model_Building;
	DataBuilding _data_Building;
	DataBuilding _data_Next_Building;

	private bool _isBuilding1 = true;
	private bool _isBuilding2 = true;
	private bool _isRes1 = true;
	private bool _isRes2 = true;
	private bool _isRes3 = true;
	private bool _isRes4 = true;
	private int[] _resArr = new int[4];

	override public void Init()
	{
		base.Init ();
		InitUI();
		NotificationCenter.instance.AddEventListener(Notification_Type.RequestBuilingUpLevel,RequestUpgradeBuilding);
		NotificationCenter.instance.AddEventListener(Notification_Type.RequestBuilingBugQueue,RequestBugQueueUpgrade);

		// 设置页面打开动画类型 默认为缩放
		// this.animationType = PanelBase.AnimationType.SCALE;
	}

	void InitUI()
	{
		_Unlocking_Container = transform.Find("building_bg/buildingRequest/Unlocking_Container");
		_Unlocking_ScrollView = _Unlocking_Container.Find("Unlocking_Scroll View").GetComponent<UIScrollView>();
		_Unlocking_Grid = _Unlocking_Container.Find("Unlocking_Scroll View/Unlocking_Grid").GetComponent<UIGrid>();
		_buildingInfo = transform.Find("building_bg/buildingInfo");
		if (_buildingInfo != null) 
		{
			_buildingName_left_Label = _buildingInfo.Find("buildingName_left_Label").GetComponent<UILabel>();
			_currentLevel = _buildingInfo.Find("currentLevel_Label/currentLevel").GetComponent<UILabel>();
			
			_netxLevelvalue = _buildingInfo.Find("next_Level_Label/netxLevelvalue").GetComponent<UILabel>();
			_propertyInfo_1_Label = _buildingInfo.Find("propertyInfo_1_Label").GetComponent<UILabel>();
			_propertyValue_1_Label = _buildingInfo.Find("propertyInfo_1_Label/propertyValue_1_Label").GetComponent<UILabel>();
			_propertyInfo_2_Label = _buildingInfo.Find("propertyInfo_2_Label").GetComponent<UILabel>();
			_propertyValue_2_Label = _buildingInfo.Find("propertyInfo_2_Label/propertyValue_2_Label").GetComponent<UILabel>();
			
		}
		_buildingRequest = transform.Find("building_bg/buildingRequest");
		if (_buildingRequest != null) 
		{
			_buildingName_1_right = _buildingRequest.Find("buildingName_1_right").GetComponent<UILabel>();
			_buildingName_1_value = _buildingRequest.Find("buildingName_1_right/buildingName_1_value").GetComponent<UILabel>();
			_buildingName_2_right = _buildingRequest.Find("buildingName_2_right").GetComponent<UILabel>();
			_buildingName_2_value = _buildingRequest.Find("buildingName_2_right/buildingName_2_value").GetComponent<UILabel>();
			_Btn_goto_building_1 = _buildingName_1_right.transform.Find("Btn_goto_building_1").GetComponent<UIButton>();
			EventDelegate evenBtngoto1 = new EventDelegate(OnGotoBuilding_1);
			_Btn_goto_building_1.onClick.Add(evenBtngoto1);
			
			_Btn_goto_building_2 = _buildingName_2_right.transform.Find("Btn_goto_building_2").GetComponent<UIButton>();
			EventDelegate evenBtngoto2 = new EventDelegate(OnGotoBuilding_2);
			_Btn_goto_building_2.onClick.Add(evenBtngoto2);

			_queue_right = _buildingRequest.Find("queue_right").GetComponent<UILabel>();
			_queue_Name_3_value = _buildingRequest.Find("queue_right/queue_Name_3_value").GetComponent<UILabel>();
			
			_resName_1 = _buildingRequest.Find("resName_1").GetComponent<UILabel>();
			_res_1_value = _buildingRequest.Find("resName_1/res_1_value").GetComponent<UILabel>();
			_resName_2 = _buildingRequest.Find("resName_2").GetComponent<UILabel>();
			_res_2_value = _buildingRequest.Find("resName_2/res_2_value").GetComponent<UILabel>();
			_resName_3 = _buildingRequest.Find("resName_3").GetComponent<UILabel>();
			_res_3_value = _buildingRequest.Find("resName_3/res_3_value").GetComponent<UILabel>();
			_resName_4 = _buildingRequest.Find("resName_4").GetComponent<UILabel>();
			_res_4_value = _buildingRequest.Find("resName_4/res_4_value").GetComponent<UILabel>();
			_queueSprite_1 = _buildingRequest.Find("queue_right/symbol_Sprite").GetComponent<UISprite>();
			_builingSprite_1 = _buildingRequest.Find("buildingName_1_right/symbol_Sprite").GetComponent<UISprite>();
			_builingSprite_2 = _buildingRequest.Find("buildingName_2_right/symbol_Sprite").GetComponent<UISprite>();
			_resSprite_1 = _buildingRequest.Find("resName_1/symbol_Sprite").GetComponent<UISprite>();
			_resSprite_2 = _buildingRequest.Find("resName_2/symbol_Sprite").GetComponent<UISprite>();
			_resSprite_3 = _buildingRequest.Find("resName_3/symbol_Sprite").GetComponent<UISprite>();
			_resSprite_4 = _buildingRequest.Find("resName_4/symbol_Sprite").GetComponent<UISprite>();			
			_Btn_getRes_1 = _resName_1.transform.Find("Btn_getRes").GetComponent<UIButton>();
			EventDelegate eveRes_1 = new EventDelegate(OngetRes_1);
			_Btn_getRes_1.onClick.Add(eveRes_1);			
			_Btn_getRes_2 = _resName_2.transform.Find("Btn_getRes").GetComponent<UIButton>();
			EventDelegate eveRes_2 = new EventDelegate(OngetRes_2);
			_Btn_getRes_2.onClick.Add(eveRes_2);			
			_Btn_getRes_3 = _resName_3.transform.Find("Btn_getRes").GetComponent<UIButton>();
			EventDelegate eveRes_3 = new EventDelegate(OngetRes_3);
			_Btn_getRes_3.onClick.Add(eveRes_3);			
			_Btn_getRes_4 = _resName_4.transform.Find("Btn_getRes").GetComponent<UIButton>();
			EventDelegate eveRes_4 = new EventDelegate(OngetRes_4);
			_Btn_getRes_4.onClick.Add(eveRes_4);
		}		
		_propertyInfo_1_Label.gameObject.SetActive(false);
		_propertyValue_1_Label.gameObject.SetActive(false);
		_propertyInfo_2_Label.gameObject.SetActive(false);
		_propertyValue_2_Label.gameObject.SetActive(false);		
		_buildingName_1_right.gameObject.SetActive(false);
		_buildingName_2_right.gameObject.SetActive(false);
		//		_buildingName_3_right.gameObject.SetActive(false);		
		_resName_1.gameObject.SetActive(false);
		_resName_2.gameObject.SetActive(false);
		_resName_3.gameObject.SetActive(false);
		_resName_4.gameObject.SetActive(false);		
		_Btn_goto_building_1.gameObject.SetActive(false);
		_Btn_goto_building_2.gameObject.SetActive(false);
		//		_Btn_goto_building_3.gameObject.SetActive(false);
		_Btn_getRes_1.gameObject.SetActive(false);
		_Btn_getRes_2.gameObject.SetActive(false);
		_Btn_getRes_3.gameObject.SetActive(false);
		_Btn_getRes_4.gameObject.SetActive(false);		
		// 立即升级
		_btnOnceUpLevel = transform.Find ("building_bg/buildingRequest/Btn_atOnce_upLevel").GetComponent<UIButton> (); 
		_btnOnceUpLevelSprite = _btnOnceUpLevel.gameObject.transform.GetComponent<UISprite>();
		// 正常升级
		_btnUpLevel = transform.Find ("building_bg/buildingRequest/Btn_upLevel").GetComponent<UIButton> ();
		
		// 购买队列
		_btnBugQueue = transform.Find ("building_bg/buildingRequest/Btn_bug_queue").GetComponent<UIButton> ();
		_pay_bug_queue_CoinValue = _btnBugQueue.transform.Find("pay_bug_queue_CoinValue").GetComponent<UILabel>();// 购买队列消费金币值
		_payTimeValue  = _btnUpLevel.transform.Find("payTimeValue").GetComponent<UILabel>();// 升级花费时间
		_payCoinValue  = _btnOnceUpLevel.transform.Find("payCoinValue").GetComponent<UILabel>();// 升级花费金币
		
		_atOnceUpLevel_label = _btnOnceUpLevel.transform.Find("atOnceUpLevel_label").GetComponent<UILabel>();
		_UpLevel_label = _btnUpLevel.transform.Find("UpLevel_label").GetComponent<UILabel>();
		_bug_queue_label = _btnBugQueue.transform.Find("bug_queue_label").GetComponent<UILabel>();
		
		
		_OnceUpLevelSprite = _btnOnceUpLevel.transform.Find("Sprite").GetComponent<UISprite>();
		_BugQueueSprite = _btnBugQueue.transform.Find("Sprite").GetComponent<UISprite>();
		// add button listener
		if (_btnOnceUpLevel != null) 
		{
			EventDelegate evenBtnOnceUp = new EventDelegate(OnOnceUpLevel);
			_btnOnceUpLevel.onClick.Add(evenBtnOnceUp);
		}
		
		if (_btnBugQueue != null) 
		{
			EventDelegate evenBtnQ = new EventDelegate(OnBugQueue);
			_btnBugQueue.onClick.Add(evenBtnQ);
		}
		
		if (_btnUpLevel != null) 
		{
			EventDelegate evenBtnUp = new EventDelegate(OnUpLevel);
			_btnUpLevel.onClick.Add(evenBtnUp);
		}
		
		_btnClose = transform.Find("building_bg/BtnClose").GetComponent<UIButton>();
		
		if (_btnClose != null) 
		{
			EventDelegate evenBtnClose = new EventDelegate(PlayPanelAniDeletePanel);
			_btnClose.onClick.Add(evenBtnClose);
		}

	}



	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		_model_Building = parameters [0] as Model_Building;
		InitData();
	}

	void Update()
	{
		_model_Building = InstancePlayer.instance.model_User.buildings[_model_Building.buildingType];
		_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level);
		BuilingData();
		ResData();
		QueueData();
		UpdateUI();
		UpdateButtonStatus ();

	}

	void InitData()
	{
		_model_Building = InstancePlayer.instance.model_User.buildings[_model_Building.buildingType];
		_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level);
		InitProductUI ();
		UnlockBuildingUIData();
	}
	void UnlockBuildingUIData()
	{
		if(_model_Building.buildingType == Model_Building.Building_Type.ControlCenter)
		{
			_Unlocking_Container.gameObject.SetActive(true);
			List<Model_Building.Building_Type> unLockedBuildingsType = new List<Model_Building.Building_Type>(); //  _model_Building.unLockedBuildingsType;
			unLockedBuildingsType.Clear();
			foreach(KeyValuePair<Model_Building.Building_Type,Model_Building> kvp in InstancePlayer.instance.model_User.buildings)
			{
				if(kvp.Value.isUnlockedNextLevel)
				{
					unLockedBuildingsType.Add(kvp.Key);
				}
			}
			CreateUnlockBuildingItem(unLockedBuildingsType);
		}
		else
		{
			_Unlocking_Container.gameObject.SetActive(false);
		}
	}
	 

	void CreateUnlockBuildingItem(List<Model_Building.Building_Type> unLockedBuildingsType )
	{
		_Unlocking_Grid.DestoryAllChildren();
		if(unLockedBuildingsType.Count <=5)
		{
			_Unlocking_ScrollView.enabled = false;
		}
		else
		{
			_Unlocking_ScrollView.enabled = true;
		}
		foreach(Model_Building.Building_Type type in unLockedBuildingsType)
		{
			GameObject prefab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "building/UnlockingBuilingItem");
			GameObject item = NGUITools.AddChild(_Unlocking_Grid.gameObject,prefab);
			UILabel label = item.transform.Find("BuilingName_Label").GetComponent<UILabel>();
			UISprite building = item.transform.Find("BuilingIcon_Sprite/builingIcon").GetComponent<UISprite>();
			DataBuilding data_Building = DataManager.instance.dataBuildingGroup.GetBuilding ((int)type, 1);
			label.text = data_Building.name.ToString();
		}
		_Unlocking_Grid.Reposition();
	}

	public void UpdateButtonStatus()
	{
		// 升级队列
		if (InstancePlayer.instance.model_User.model_Queue.IsHadBuildingQueue()) 
		{
			_btnBugQueue.gameObject.SetActive(false);	// 购买队列
			_btnUpLevel.gameObject.SetActive(true);	// 升级按钮
		}
		else
		{
			_btnBugQueue.gameObject.SetActive(true);	// 购买队列
			_btnUpLevel.gameObject.SetActive(false);	// 升级按钮

		}
		// 正常升级
		if(_model_Building.isUpgrading){
			_btnUpLevel.isEnabled = false;
		}
		else
		{
			_btnUpLevel.isEnabled = true;
		}

		if(_isBuilding1 && _isBuilding2)
		{
			_btnUpLevel.isEnabled = true;	// 升级按钮
			_btnOnceUpLevel.isEnabled = true;	// 立即升级
			_btnBugQueue.isEnabled = true;	// 购买队列
		}
		else
		{
			_btnUpLevel.isEnabled = false;	// 升级按钮
			_btnOnceUpLevel.isEnabled = false;	// 立即升级
			_btnBugQueue.isEnabled = false;	// 购买队列

			_OnceUpLevelSprite.color = new Color(0.0f,178.0f,178.0f);
			_BugQueueSprite.color = new Color(0.0f,178.0f,178.0f);
			_pay_bug_queue_CoinValue.color = new Color(118.0f,118.0f,118.0f);
			_payTimeValue.color = new Color(118.0f,118.0f,118.0f);
			_payCoinValue.color = new Color(118.0f,118.0f,118.0f);
			_atOnceUpLevel_label.color = new Color(118.0f,118.0f,118.0f);
			_UpLevel_label.color = new Color(118.0f,118.0f,118.0f);
			_bug_queue_label.color = new Color(118.0f,118.0f,118.0f);

		}

	}
	
	public void UpdateUI()
	{
		if(_model_Building.level >=1)
		{
			_data_Next_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level + 1);
		}

		_data_Building = DataManager.instance.dataBuildingGroup.GetBuilding (_model_Building.id, _model_Building.level);

		//当前等级 
		_currentLevel.text = "" + _model_Building.level;
		// 当前建筑名称
		_buildingName_left_Label.text = _data_Building.name;
		// 下一等级
		_netxLevelvalue.text = _data_Next_Building.level + "";

		_payCoinValue.text = ((int)_data_Building.upgradeCash) + "";
		_pay_bug_queue_CoinValue.text = InstancePlayer.instance.model_User.model_Queue.GetTempBuildingQueueCash(_model_Building.id,_model_Building.level) + "";
		//当前等级升级所需要的时间
		if(_model_Building != null)
		{
			_payTimeValue.text = UIHelper.setTimeDHMS((int)_data_Building.cost.costTime);	
		}

	}


	public void BuilingData()
	{
		if(_data_Building != null)
		{
			int budingIdCount = _data_Building.buildingID.Length;
			int budinglevelCount = _data_Building.buildingLevel.Length;

			for(int i = 0 ;i < budingIdCount ; i++)
			{
				int id = _data_Building.buildingID[i];
				if(id == 0)	return;
				Model_Building currenBuiling = InstancePlayer.instance.model_User.buildings[(Model_Building.Building_Type)id];
				DataBuilding currenBuilingData = DataManager.instance.dataBuildingGroup.GetBuilding(currenBuiling.id, currenBuiling.level);
				int currenLevel = currenBuiling.level;
				int requireLevel =  _data_Building.buildingLevel[i];
				string builingName = currenBuilingData.name;
				if(i == 0)
				{
					_buildingName_1_right.gameObject.SetActive(true);
					_buildingName_1_right.text = builingName;	// 升级所需建筑名字
					if(currenLevel >= requireLevel)
					{
						_isBuilding1 = true;
						_buildingName_1_value.text =  requireLevel + "级" ;	// 升级所需建筑等级
						_builingSprite_1.spriteName = "correctSymbol";
					}
					else
					{
						_isBuilding1 = false;
						_buildingName_1_value.text = UIHelper.SetStringColor( requireLevel + "级");	// 升级所需建筑等级
						_Btn_goto_building_1.gameObject.SetActive(true);
						_builingSprite_1.spriteName = "errorSymbol";
					}
				}
				else if(i == 1)
				{
					_buildingName_2_right.gameObject.SetActive(true);
					_buildingName_2_right.text = builingName;	// 升级所需建筑名字
					if(currenLevel >= requireLevel)
					{
						_isBuilding2 = true;
						_buildingName_2_value.text =  requireLevel + "级" ;	// 升级所需建筑等级
						_builingSprite_2.spriteName = "correctSymbol";
					}
					else
					{
						_isBuilding2 = false;
						_buildingName_2_value.text = UIHelper.SetStringColor( requireLevel + "级") ;	// 升级所需建筑等级
						_Btn_goto_building_2.gameObject.SetActive(true);
						_builingSprite_2.spriteName = "errorSymbol";
					}
				}
			}

		}
	}

	public void ResData()
	{
		if(_data_Building != null)
		{
			if(_data_Building.cost.costFood != 0.0f)
			{
				_resName_1.gameObject.SetActive(true);
				int currenNum = InstancePlayer.instance.model_User.model_Resource.GetIntFood(); 
				int requireNum = (int)_data_Building.cost.costFood;
				if(currenNum >= requireNum)
				{
					_isRes1 = true;
					_res_1_value.text = currenNum + " / " + requireNum;
					_resSprite_1.spriteName = "correctSymbol";
				}
				else
				{
					_isRes1 = false;
					_res_1_value.text = UIHelper.SetStringColor(currenNum.ToString() ) +  " / " + requireNum ;
					_Btn_getRes_1.gameObject.SetActive(true);
					_resSprite_1.spriteName = "errorSymbol";
				}
	
			}
			if(_data_Building.cost.costOil != 0.0f)
			{
				_resName_2.gameObject.SetActive(true);
				int currenNum = InstancePlayer.instance.model_User.model_Resource.GetIntOil(); 
				int requireNum = (int)_data_Building.cost.costOil;
				if(currenNum >= requireNum)
				{
					_isRes2 = true;
					_res_2_value.text = currenNum + " / " + requireNum;
					_resSprite_2.spriteName = "correctSymbol";
				}
				else
				{
					_isRes2 = false;
					_res_2_value.text = UIHelper.SetStringColor(currenNum.ToString() ) +  " / " + requireNum ;
					_Btn_getRes_2.gameObject.SetActive(true);
					_resSprite_2.spriteName = "errorSymbol";
				}
			}

			if(_data_Building.cost.costMetal != 0.0f)
			{
				_resName_3.gameObject.SetActive(true);
				int currenNum = InstancePlayer.instance.model_User.model_Resource.GetIntMetal(); 
				int requireNum = (int)_data_Building.cost.costMetal;
				if(currenNum >= requireNum)
				{
					_isRes3 = true;
					_res_3_value.text = currenNum + " / " + requireNum;
					_resSprite_3.spriteName = "correctSymbol";
				}
				else
				{
					_isRes3 = false;
					_res_3_value.text = UIHelper.SetStringColor(currenNum.ToString() ) +  " / " + requireNum  ;
					_Btn_getRes_3.gameObject.SetActive(true);
					_resSprite_3.spriteName = "errorSymbol";
				}
			}

			if(_data_Building.cost.costRare != 0.0f)
			{
				_resName_4.gameObject.SetActive(true);
				int currenNum = InstancePlayer.instance.model_User.model_Resource.GetIntRare(); 
				int requireNum = (int)_data_Building.cost.costRare;
				if(currenNum >= requireNum)
				{
					_isRes4 = true;
					_res_4_value.text = currenNum + " / " + requireNum;
					_resSprite_4.spriteName = "correctSymbol";
				}
				else
				{
					_isRes4 = false;
					_res_4_value.text = UIHelper.SetStringColor(currenNum.ToString() ) +  " / " + requireNum ;
					_Btn_getRes_4.gameObject.SetActive(true);
					_resSprite_4.spriteName = "errorSymbol";
				}	
			}
		}
	}

	private void QueueData()
	{
		int allQueueNum = InstancePlayer.instance.model_User.model_Queue.buildingQueueMaxNum;
		int currueQueueNum = InstancePlayer.instance.model_User.model_Queue.buildingQueueUsedNum;

		if(currueQueueNum >= allQueueNum)
		{
			_queue_Name_3_value.text = UIHelper.SetStringColor(currueQueueNum + " / " + allQueueNum );
			_queueSprite_1.spriteName = "errorSymbol";
		}
		else
		{
			_queue_Name_3_value.text = currueQueueNum + " / " + allQueueNum;
			_queueSprite_1.spriteName = "correctSymbol";
		}
		
	}

	private void SetResPosition(int count)
	{

		float num = 12f;
		_resName_1.transform.localPosition = new Vector3(_resName_1.transform.localPosition.x,_resName_1.transform.localPosition.y + _buildingName_1_right.height * count + num,0.0f);
		_resName_2.transform.localPosition = new Vector3(_resName_2.transform.localPosition.x,_resName_2.transform.localPosition.y + _buildingName_1_right.height * count + num,0.0f);
		_resName_3.transform.localPosition = new Vector3(_resName_3.transform.localPosition.x,_resName_3.transform.localPosition.y + _buildingName_1_right.height * count + num,0.0f);
		_resName_4.transform.localPosition = new Vector3(_resName_4.transform.localPosition.x,_resName_4.transform.localPosition.y + _buildingName_1_right.height * count + num,0.0f);
	}

	private void GoTOBuiling()
	{
		// 同一页面 设置播放完动画 回调
		SetOnFinishCallBack(ClosedFinish);

		OnDeletePanel();
	}


	// 等待播放完动画后 打开新页面
	private void ClosedFinish()
	{
		int id = _data_Building.buildingID[_idBuiling];
		if(id == 0)	return;
		Model_Building currenBuiling = InstancePlayer.instance.model_User.buildings[(Model_Building.Building_Type)id];
		if(!currenBuiling.isUpgrading)
		{
			UIController.instance.CreatePanel(UICommon.UI_PANEL_BUILDINGUPLEVEL,currenBuiling);
		}
	}
	private void LackResArr()
	{
		if(_resArr != null)
		{
			_resArr[0] = Model_Helper.GetNeedBuyFoodCount((int)_data_Building.cost.costFood);
			_resArr[1] = Model_Helper.GetNeedBuyOilCount((int)_data_Building.cost.costOil);
			_resArr[2] = Model_Helper.GetNeedBuyMetalCount((int)_data_Building.cost.costMetal);
			_resArr[3] = Model_Helper.GetNeedBuyRareCount((int)_data_Building.cost.costRare);
		}
	}

	public void RequestUpgradeBuilding(Notification notification)
	{
		UpgradeBuilding();
	}
	
	public void RequestBugQueueUpgrade(Notification notification)
	{
		BugQueueUpgrade();
	}
	//购买升级队列
	public void OnBugQueue()
	{
		if(_isRes1 && _isRes2 && _isRes3 && _isRes4)
		{
			BugQueueUpgrade();
		}
		else
		{
			LackResArr();
			UIController.instance.CreatePanel(UICommon.UI_TIPS_BUYRES, _resArr,ResourcesBuyType.BugQueueUpLevelType);
		}
	}
	
	//立即升级
	public void OnOnceUpLevel()
	{
		OnceFinishUpgradeBuilding();

	}
	
	//升级
	public void OnUpLevel()
	{
		if(_isRes1 && _isRes2 && _isRes3 && _isRes4)
		{
			UpgradeBuilding();
		}
		else
		{
			LackResArr();
			UIController.instance.CreatePanel(UICommon.UI_TIPS_BUYRES, _resArr,ResourcesBuyType.UpLevelType);
		}
	}

	private int _idBuiling = 0;
	public void OnGotoBuilding_1()
	{
		GoTOBuiling();
		_idBuiling = 0;
	}

	public void OnGotoBuilding_2()
	{
		GoTOBuiling();
		_idBuiling = 1;
	}


	public void OngetRes_1()
	{

	}

	public void OngetRes_2()
	{

	}
	public void OngetRes_3()
	{

	}
	public void OngetRes_4()
	{
	
	}

	public void PlayPanelAniDeletePanel()
	{
//		UIHelper.PlayPanelAnimation (this, false, UICommon.Panel_Anim_Style.Scale,OnDeletePanel);
		OnDeletePanel();
	}
	public void OnDeletePanel()
	{
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RequestBuilingUpLevel);
		NotificationCenter.instance.RemoveEventListener(Notification_Type.RequestBuilingBugQueue);
		Delete ();
	}
	public void InitProductUI()
	{
		
		DataProduct dataProduct = new DataProduct();
		DataProduct nextDataProduct = new DataProduct();
		
		if(_model_Building.buildingType == Model_Building.Building_Type.FoodFactory )
		{
			dataProduct = DataManager.instance.dataProductFoodGroup.GetProduct(_model_Building.level);
			nextDataProduct = DataManager.instance.dataProductFoodGroup.GetProduct(_model_Building.level + 1);
			ShowSpeedNum(dataProduct,nextDataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.OilFactory )
		{
			dataProduct = DataManager.instance.dataProductOilGroup.GetProduct(_model_Building.level);
			nextDataProduct = DataManager.instance.dataProductOilGroup.GetProduct(_model_Building.level + 1);
			ShowSpeedNum(dataProduct,nextDataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.MetalFactory )
		{
			dataProduct = DataManager.instance.dataProductMetalGroup.GetProduct(_model_Building.level);
			nextDataProduct = DataManager.instance.dataProductMetalGroup.GetProduct(_model_Building.level + 1);
			ShowSpeedNum(dataProduct,nextDataProduct);
		}
		else if( _model_Building.buildingType == Model_Building.Building_Type.RareFactory )
		{
			dataProduct = DataManager.instance.dataProductRareGroup.GetProduct(_model_Building.level);
			nextDataProduct = DataManager.instance.dataProductRareGroup.GetProduct(_model_Building.level + 1);
			ShowSpeedNum(dataProduct,nextDataProduct);
		}

		int budingIdCount = _data_Building.buildingID.Length;
		int budinglevelCount = _data_Building.buildingLevel.Length;
		if(budingIdCount != 2)
		{
			SetResPosition(2 - budingIdCount);
		}
		
	}

	
	public void ShowSpeedNum(DataProduct dataProduct,DataProduct nextDataProduct)
	{
		_propertyInfo_1_Label.gameObject.SetActive(true);
		_propertyValue_1_Label.gameObject.SetActive(true);
		_propertyInfo_2_Label.gameObject.SetActive(true);
		_propertyValue_2_Label.gameObject.SetActive(true);

		if(dataProduct != null && nextDataProduct != null)
		{
			int addSpeed = (int)(nextDataProduct.produceSpeed * 3600) -  (int)(dataProduct.produceSpeed * 3600);
			int addCapaity = (int)nextDataProduct.capacity - (int)dataProduct.capacity;
			_propertyValue_1_Label.text = dataProduct.produceSpeed * 3600 +"[00FF00]" + " + " + addSpeed + "[-]";
			_propertyValue_2_Label.text = dataProduct.capacity + "[00FF00]" + " + " + addCapaity + "[-]";
		}

		if(nextDataProduct == null)
		{
			_propertyValue_1_Label.text = dataProduct.produceSpeed * 3600 + "";
			_propertyValue_2_Label.text = dataProduct.capacity + "";
		}
	}

	// ===============================================================


	//升级
	public  void UpgradeBuilding()
	{
		UIHelper.LoadingPanelIsOpen(true);
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();
		Model_Building building =  _model_Building;
		if (building != null) {
			
			request.buildingId = building.id;
			request.api = new Model_ApiRequest ().api;	
			(new PBConnect_upgradeBuilding()).Send(request, OnUpgradeBuilding);
			return;
		}
	}
	void OnUpgradeBuilding(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) 
		{
			InstancePlayer.instance.model_User.model_Queue.AddBuildingQueue ();
			if(this)
			{
				PlayPanelAniDeletePanel();
				UpdateButtonStatus();			
			}
			Trace.trace("OnUpgradeBuilding success ",Trace.CHANNEL.UI);
		} 
		else 
		{
			Trace.trace("OnUpgradeBuilding fail",Trace.CHANNEL.UI);
		}
	}
	//立即升级

	public void OnceFinishUpgradeBuilding()
	{
		UIHelper.LoadingPanelIsOpen(true);
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();
		if (InstancePlayer.instance.model_User.buildings.Count > 0) {
			Model_Building building =  _model_Building;
			if (building != null) {
				// 立即升级 
				request.buildingId = building.id;
				request.byCash = 1;//是否只消耗cash来立即升级，1：是
				request.api = new Model_ApiRequest ().api;	
				(new PBConnect_upgradeBuilding()).Send(request, OnOnceFinishUpgradeBuilding);
				return;
			}
		}	
	}
	void OnOnceFinishUpgradeBuilding(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("OnFinishUpgradeBuilding success",Trace.CHANNEL.UI);
			if(this)
			{
				PlayPanelAniDeletePanel();
				UpdateButtonStatus();			
			}
		} else 
		{
			Trace.trace("OnFinishUpgradeBuilding fail",Trace.CHANNEL.UI);
		}
	}
	//	购买队列升级
	public void BugQueueUpgrade()
	{
		UIHelper.LoadingPanelIsOpen(true);
		UpgradeBuildingRequest request = new UpgradeBuildingRequest();
		if(InstancePlayer.instance.model_User.buildings.Count > 0)
		{
			Model_Building builing = _model_Building;
			if(builing != null)
			{
				// 立即升级 
				request.buildingId = builing.id;
				request.buyQueue = 1;//是否消耗cash忽视建筑升级队列限制，1：是；0：否
				request.api = new Model_ApiRequest().api;
				(new PBConnect_upgradeBuilding()).Send(request,OnBugQueueUpgrade);
				return;
			}
		}
	}

	void OnBugQueueUpgrade(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			InstancePlayer.instance.model_User.model_Queue.AddBuildingQueue ();
			if(this)
			{
				PlayPanelAniDeletePanel();
				UpdateButtonStatus();			
			}
		}
		else 
		{
			Trace.trace("BugQueueUpgrade fail",Trace.CHANNEL.UI);
		}
	}
	


}
