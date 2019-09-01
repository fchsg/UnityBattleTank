using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class TankProductionPanel : PanelBase {
	private UIButton _closeBtn;
	private Transform _TankInfo;
	private UIButton _OnceTrain;
	private UIButton _Train;
	private UIButton _AddBtn;
	private UIButton _DelBtn;
	private UISprite _iconBg;
	private UISprite _icon;
	private UIButton _iconBtn;
	private UILabel _TrainTimeLabel;

	private UILabel _OnceTrainCoin;
	private UISprite _FoodSprite;
	private UILabel _FoodValueLabel;
	private UISprite _OilSprite;
	private UILabel _OilValueLabel;
	private UISprite _MetalSprite;
	private UILabel _MetalValueLabel;
	private UISprite _RareSprite;
	private UILabel _RareValueLabel;

	private UILabel _AllTankValue_Label;
	private UISlider _TankItem_Slider;
	private UILabel _AddTankNumLabel;
	private UILabel _tankName;

	private Transform _trainContainer;
	/// <summary>
	///训练进行中显示
	/// </summary>
//	private Transform _speed_Container;
//	private UIButton _speedBtn;
//	private UILabel _speedCoinLabel;
	private UISlider _Timer_Colored_Slider;
	private UILabel _timerLabel;
	private UILabel _unlockLabel;

	private Transform _currentInventoryContainer;
	/// <summary>
	/// 当前库存
	/// </summary>
	private UILabel _currentInventoryLabel;
	/// <summary>
	/// 当前战损
	/// </summary>
	private UILabel _currentDamageLabel;
	/// <summary>
	/// 正在维修
	/// </summary>
	private UILabel _currentMaintenanceLabel;
	private UIButton _gotoBtn;


	private bool _isUnlock = false; //是否解锁
	private bool _isCanProduction = true;//生产
	private int _lackFood = 0;
	private int _lackOil = 0;
	private int _lackMetal = 0;
	private int _lackRare = 0;

	private int _currentTankCount = 0;
	private int[] _resArr = new int[4];

	//data
	TankDataManager.UnitData _unitData;
	Dictionary<int,Model_Unit> _model_Units;
	Model_Unit _model_unit;
	DataUnit _dataUnit;
	Model_User _model_User;

	Model_RepairUnit _model_RepairUnit;
	List<SlgPB.Unit> _RepairUnits = new List<SlgPB.Unit>();
	SlgPB.Unit _RepairUnit;
	List<int> _unlockUnits;

	void Start(){

		NotificationCenter.instance.AddEventListener( Notification_Type.RequestTrainTank,ReqestTrainTank);
	}
		
	public void ReqestTrainTank(Notification notification)
	{
		Model_Unit model_Unit = notification._data as Model_Unit;
		if(model_Unit != null && model_Unit.unitId == _dataUnit.id )
		{
			AddUnit();
		}
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null)
		{
			_unitData = parameters[0] as TankDataManager.UnitData;
			_dataUnit = _unitData.unitData;
		}

	}
	// Update is called once per frame
	void Update () {
		if(_dataUnit != null)
		{
			UpdateUI();
			UpdateRes();
			UpdateCurrentTankState();
		}
	}


	public void UpdateUI()
	{
		_model_User = InstancePlayer.instance.model_User;
		_model_Units = InstancePlayer.instance.model_User.unlockUnits;
		_unlockUnits = InstancePlayer.instance.model_User.unlockUnitsId;
		_dataUnit = DataManager.instance.dataUnitsGroup.GetUnit(_dataUnit.id);
		_model_Units.TryGetValue(_dataUnit.id, out _model_unit);
		if(_TankItem_Slider.value <= 0.01f)
		{
			_TankItem_Slider.value = 0.01f;
		}
		_currentTankCount = Mathf.RoundToInt(_TankItem_Slider.value * 100f);

		_tankName.color = _unitData.nameColor;
		_tankName.text = _dataUnit.name;
		_iconBg.spriteName = _unitData.iconBgName;
		_icon.spriteName = _unitData.iconName;

		int cdCashCount = Mathf.CeilToInt(_currentTankCount * _dataUnit.costCdCash);
		_OnceTrainCoin.text =  cdCashCount + "";

		if (_TankItem_Slider != null && _AddTankNumLabel != null)
			_AddTankNumLabel.text = _currentTankCount.ToString();

	}
	public void UpdateRes()
	{
		if(_dataUnit != null)
		{
			int sliderValue = _currentTankCount;

			int food = _model_User.model_Resource.GetIntFood();
			if(Mathf.RoundToInt(_dataUnit.cost.costFood) * sliderValue  > food)
			{
				_lackFood = Mathf.RoundToInt(_dataUnit.cost.costFood) * sliderValue - food;
				_FoodValueLabel.text = UIHelper.SetStringColor((Mathf.RoundToInt(_dataUnit.cost.costFood) * sliderValue ).ToString());
			}
			else
			{
				_lackFood = 0;
				_FoodValueLabel.text = (Mathf.RoundToInt(_dataUnit.cost.costFood) * sliderValue ).ToString();
			}

			int oil = _model_User.model_Resource.GetIntOil();
			if(Mathf.RoundToInt(_dataUnit.cost.costOil) * sliderValue  > oil)
			{
				_lackOil = Mathf.RoundToInt(_dataUnit.cost.costOil) * sliderValue - oil;
				_OilValueLabel.text = UIHelper.SetStringColor((Mathf.RoundToInt(_dataUnit.cost.costOil) * sliderValue ).ToString());
			}
			else
			{
				_lackOil = 0;
				_OilValueLabel.text = (Mathf.RoundToInt(_dataUnit.cost.costOil)* sliderValue ).ToString();
			}

			int metal = _model_User.model_Resource.GetIntMetal();
			if(Mathf.RoundToInt(_dataUnit.cost.costMetal) * sliderValue  > metal)
			{
				_lackMetal = Mathf.RoundToInt(_dataUnit.cost.costMetal) * sliderValue - metal;
				_MetalValueLabel.text = UIHelper.SetStringColor( (Mathf.RoundToInt(_dataUnit.cost.costMetal) * sliderValue ).ToString());
			}
			else
			{
				_lackMetal = 0;
				_MetalValueLabel.text = (Mathf.RoundToInt(_dataUnit.cost.costMetal)* sliderValue ).ToString();
			}

			int Rare = _model_User.model_Resource.GetIntRare();
			if(Mathf.RoundToInt(_dataUnit.cost.costRare) * sliderValue  > Rare)
			{
				_lackRare = Mathf.RoundToInt(_dataUnit.cost.costRare) * sliderValue - Rare;
				_RareValueLabel.text = UIHelper.SetStringColor((Mathf.RoundToInt(_dataUnit.cost.costRare) * sliderValue ).ToString() );
			}
			else
			{
				_lackRare = 0;
				_RareValueLabel.text = (Mathf.RoundToInt(_dataUnit.cost.costRare)* sliderValue ).ToString();
			}
			if(_lackFood + _lackOil + _lackMetal + _lackRare > 0)
			{
				_isCanProduction = false;
			}
			else
			{
				_isCanProduction = true;
			}

			if(_dataUnit.cost.costFood == 0.0f)
			{
				_FoodSprite.gameObject.SetActive(false);
			}
			else
			{
				_FoodSprite.gameObject.SetActive(true);
			}
			if(_dataUnit.cost.costOil == 0.0f)
			{
				_OilSprite.gameObject.SetActive(false);
			}
			else
			{
				_OilSprite.gameObject.SetActive(true);
			}
			if(_dataUnit.cost.costMetal == 0.0f)
			{
				_MetalSprite.gameObject.SetActive(false);
			}
			else
			{
				_MetalSprite.gameObject.SetActive(true);
			}
			if(_dataUnit.cost.costRare == 0.0f)
			{
				_RareSprite.gameObject.SetActive(false);
			}
			else
			{
				_RareSprite.gameObject.SetActive(true);
			}
			_TrainTimeLabel.text = UIHelper.setTimeDHMS((int)_dataUnit.cost.costTime * sliderValue);
			LackResArr();
		}
	}
	/// <summary>
	/// Updates the state of the current tank.
	/// </summary>
	void UpdateCurrentTankState()
	{
		if(_dataUnit != null)
		{
			_model_RepairUnit = InstancePlayer.instance.model_User.model_RepairUnit;

			_RepairUnits = _model_RepairUnit.GetRepairUnits();
			foreach(SlgPB.Unit unit in _RepairUnits)
			{
				if(_dataUnit.id == unit.unitId)
				{
					_RepairUnit = unit;
					break;
				}
				else
				{
					_RepairUnit = null;
				}
			}
		}

		if(_model_unit != null)
		{
			_currentInventoryLabel.text = _model_unit.num.ToString();
			_currentDamageLabel.text = _model_unit.onDamaged.ToString();
			if(_RepairUnit != null)
			{
				_currentMaintenanceLabel.text = _RepairUnit.onRepair.ToString();
			}
			else
			{
				_currentMaintenanceLabel.text = "0";
			}
		}
		else
		{
			_currentInventoryLabel.text = "0";
			_currentDamageLabel.text = "0";
			_currentMaintenanceLabel.text = "0";
		}

	}

	private void LackResArr()
	{
		if(_resArr != null)
		{
			_resArr[0] = _lackFood;
			_resArr[1] = _lackOil;
			_resArr[2] = _lackMetal;
			_resArr[3] = _lackRare;
		}
	}
	//详情
	void OnGoToDetail()
	{
		TankInfoPanel.PanelType type = TankInfoPanel.PanelType.TANKFACTORY;
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKINFO,_dataUnit,type);
	}

	public void OnAdd()
	{
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value + 0.01f;
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * 100f).ToString();
		}
	}

	public void OnDel()
	{
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value - 0.01f;
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * 100f).ToString();
		}
	}

	// 立即训练 
	public void OnOnceTrain(){
		if(_dataUnit != null)
		{
			int cdCashCount = Mathf.CeilToInt(_currentTankCount * _dataUnit.costCdCash);
			int isProduction = ConnectionValidateHelper.IsEnoughCashBuyResources(cdCashCount);
			if(isProduction == 0)
			{
				SpeedAddUnit();
			}
			else
			{
				UIHelper.BuyCashUI();
			}
		}


	}
	// 训练 
	public void OnTrain(){

		if(!_isCanProduction)
		{
			UIHelper.BuyResourcesUI(_resArr,ResourcesBuyType.TrainTankType,_model_unit);
		}
		else
		{
			AddUnit();
		}
	}
	 

	void OnDestroy()
	{
		NotificationCenter.instance.RemoveEventListener( Notification_Type.RequestTrainTank);
	}

	void OnClose()
	{
		if(this.gameObject != null)
		{
			this.Delete();
		}
	}
	void Awake()
	{
		_closeBtn = transform.Find("ProductionContainer/CloseBtn").GetComponent<UIButton>();
		EventDelegate evclose = new EventDelegate(OnClose);
		_closeBtn.onClick.Add(evclose);
		_TankInfo = transform.Find("ProductionContainer/TankInfo");
		_iconBg = _TankInfo.Find("tankbg").GetComponent<UISprite>();
		_icon = _iconBg.gameObject.transform.Find("TankIcon").GetComponent<UISprite>();
		_iconBtn = _iconBg.gameObject.transform.Find("TankIcon").GetComponent<UIButton>(); 
		EventDelegate evicon = new EventDelegate(OnGoToDetail);
		_iconBtn.onClick.Add(evicon);
		_OnceTrain = _TankInfo.Find("OnceTrainBtn").GetComponent<UIButton>();
		_Train = _TankInfo.Find("trainContainer/trainBtn").GetComponent<UIButton>();
		_AddBtn = _TankInfo.Find("trainContainer/AddBtn").GetComponent<UIButton>();
		_DelBtn = _TankInfo.Find("trainContainer/DelBtn").GetComponent<UIButton>();
		_TrainTimeLabel = _TankInfo.Find("trainContainer/trainBtn/TrainValueLabel").GetComponent<UILabel>();

		_OnceTrainCoin = _TankInfo.Find("OnceTrainBtn/TrainCoinLabel").GetComponent<UILabel>();      
		_FoodSprite = _TankInfo.Find("Res_Container/FoodLabel").GetComponent<UISprite>();
		_FoodValueLabel = _TankInfo.Find("Res_Container/FoodLabel/FoodValueLabel").GetComponent<UILabel>();    
		_OilSprite = _TankInfo.Find("Res_Container/OilLabel").GetComponent<UISprite>();          
		_OilValueLabel = _TankInfo.Find("Res_Container/OilLabel/OilValueLabel").GetComponent<UILabel>();
		_MetalSprite = _TankInfo.Find("Res_Container/MetalLabel").GetComponent<UISprite>();
		_MetalValueLabel = _TankInfo.Find("Res_Container/MetalLabel/MetalValueLabel").GetComponent<UILabel>();
		_RareSprite = _TankInfo.Find("Res_Container/RareLabel").GetComponent<UISprite>();
		_RareValueLabel = _TankInfo.Find("Res_Container/RareLabel/RareValueLabel").GetComponent<UILabel>();

		_TankItem_Slider = _TankInfo.Find("trainContainer/TankItem_Slider").GetComponent<UISlider>();     
		_AddTankNumLabel = _TankInfo.Find("trainContainer/TankNumLabel/AddTankNumLabel").GetComponent<UILabel>();         
		_AllTankValue_Label = _TankInfo.Find("trainContainer/TankAllNumLabel/AllTankValue_Label").GetComponent<UILabel>();  
		_trainContainer = _TankInfo.Find("trainContainer");
//		_speed_Container = _TankInfo.FindChild("speed_Container");
//
//		_speedBtn = _TankInfo.FindChild("speed_Container/speedBtn").GetComponent<UIButton>();
//
//		_speedCoinLabel = _TankInfo.FindChild("speed_Container/speedBtn/speedCoinLabel").GetComponent<UILabel>();
		_Timer_Colored_Slider = _TankInfo.Find("speed_Container/Timer_Colored_Slider").GetComponent<UISlider>();
		_timerLabel = _TankInfo.Find("speed_Container/Timer_Colored_Slider/Label").GetComponent<UILabel>();

		_tankName = _TankInfo.Find("currentContainer/TankNameLabel").GetComponent<UILabel>();

		_currentInventoryContainer = _TankInfo.Find("currentContainer");
		_currentInventoryLabel = _currentInventoryContainer.Find("currentInventoryLabel/currentValueLabel").GetComponent<UILabel>();
		_currentDamageLabel = _currentInventoryContainer.Find("currentDamageLabel/currentValueLabel").GetComponent<UILabel>();
		_currentMaintenanceLabel = _currentInventoryContainer.Find("currentMaintenanceLabel/currentValueLabel").GetComponent<UILabel>();
		//		_gotoBtn = _currentInventoryContainer.FindChild("goToBTn").GetComponent<UIButton>();
		//
		//		EventDelegate evgo = new EventDelegate(OnGoTO);
		//		_gotoBtn.onClick.Add(evgo);

		EventDelegate evontr = new EventDelegate (OnOnceTrain);
		_OnceTrain.onClick.Add (evontr);
		EventDelegate evtrain = new EventDelegate (OnTrain);
		_Train.onClick.Add (evtrain);
		EventDelegate evAdd = new EventDelegate (OnAdd);
		_AddBtn.onClick.Add (evAdd);

		EventDelegate evDel = new EventDelegate (OnDel);
		_DelBtn.onClick.Add (evDel);
//		EventDelegate evSpeedBtn = new EventDelegate (OnSpeed);
//		_speedBtn.onClick.Add (evSpeedBtn);
		_TankItem_Slider.value = 0.01f;

	}


	//=========================================================================
	//	向服务器请求训练坦克
	void AddUnit()
	{
		UIHelper.LoadingPanelIsOpen(true);
		ProcessUnitRequest request = new ProcessUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = _dataUnit.id;
		request.num = _currentTankCount;
		//request.buyCd = 0;
		(new PBConnect_addUnit ()).Send (request, OnAddUnit);
		Model_User user = InstancePlayer.instance.model_User;
		user.model_Queue.AddUnitProduceQueue();
	}
	void OnAddUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			OnClose();
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshProductTank,new Notification(""));
			Trace.trace("OnAddUnit  success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnAddUnit failed",Trace.CHANNEL.UI);
		}
	}

	/// <summary>
	///  立即训练
	/// </summary>
	void SpeedAddUnit()
	{
		UIHelper.LoadingPanelIsOpen(true);
		ProcessUnitRequest request = new ProcessUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = _dataUnit.id;
		request.num = _currentTankCount;
		Trace.trace("_currentTankCount  success" + _currentTankCount,Trace.CHANNEL.UI);
		request.buyCd = 1;
		(new PBConnect_addUnit ()).Send (request, OnSpeedAddUnit);

		Assert.assert(InstancePlayer.instance != null, "InstancePlayer.instance is NULL!!!!");
		Model_User user = InstancePlayer.instance.model_User;
//		user.model_Queue.AddUnitProduceQueue();
	}
	void OnSpeedAddUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("SpeedAddUnit  success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("SpeedAddUnit failed",Trace.CHANNEL.UI);
		}
	}

}
