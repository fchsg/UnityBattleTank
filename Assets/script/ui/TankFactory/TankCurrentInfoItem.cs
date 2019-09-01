using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class TankCurrentInfoItem : MonoBehaviour {

	private UILabel _TankNameLabel ;

	private UISprite _iconBg;
	private UITexture _icon;
	private UIButton _iconBtn;
	private UISprite _format_Sprite;
	private UISprite _canResearch_Sprite;

	private Transform _lock_Container;
	private UILabel _needPaperValue_Label;
	private UILabel _currentValue_Label;

	private UISprite _redPoint_Sprite;
	private UIButton _researchBtn;
	private UISprite _researchRedPoint;
	private UIButton _toObtainBtn;
	private UIButton _harvestBtn;

	private Transform _unlock_Container;
	private UILabel _currentInventoryLabel;
	private UILabel _repairIngLabel;
	private UILabel _currentDamageLabel;
	private UIButton _productBtn;
	private UIButton _repairBtn;
	private UISprite _product_redPoint;
	private UISprite _repair_redPoint;
	private Transform _speed_Container;
	private UISlider _Timer_Colored_Slider;
	private UILabel _timeLabel;
	private UIButton _speedBtn;
	private UILabel _speedCoinLabel;

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
	
	public void Init(TankDataManager.UnitData unitData)
	{
		if(unitData != null)
		{
			_unitData = unitData;
		}


	}

	public void UpdateData(TankDataManager.UnitData unitData )
	{
		if(unitData != null)
		{
			_unitData = unitData;
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI();
		UpdateCurrentTankState();
	}
	void UpdateUI()
	{
		if(_unitData != null)
		{
			_model_User = InstancePlayer.instance.model_User;
			_model_Units = InstancePlayer.instance.model_User.unlockUnits;
			_dataUnit = _unitData.unitData;

			_TankNameLabel.text = _unitData.unitData.name.ToString();
			_TankNameLabel.color = _unitData.nameColor;
//			Trace.trace("_unitData.nameColor " +  _unitData.nameColor ,Trace.CHANNEL.UI);
			_iconBg.spriteName = _unitData.iconBgName;
			_icon.SetUnitSmallTexture(_unitData.id);
			if(_unitData.isUnLock)
			{
				_canResearch_Sprite.gameObject.SetActive(false);	
			}
			else
			{
				_canResearch_Sprite.gameObject.SetActive(_unitData.isCanUnlock);
			}

			_format_Sprite.gameObject.SetActive(_unitData.isFormation);
			 
			if(_unitData.isUnLock)
			{
				_model_Units.TryGetValue(_dataUnit.id, out _model_unit);
			}
			_lock_Container.gameObject.SetActive(!_unitData.isUnLock);
			_unlock_Container.gameObject.SetActive(_unitData.isUnLock); 

			if(_model_unit != null && _model_unit.onProduce > 0)
			{
				if(_model_unit.produceLeftTime == 0)
				{
					_productBtn.gameObject.SetActive(false);
					_repairBtn.gameObject.SetActive(false);
					_harvestBtn.gameObject.SetActive(true);
					_speed_Container.gameObject.SetActive(false);		
				}
				else
				{
					_productBtn.gameObject.SetActive(false);
					_repairBtn.gameObject.SetActive(false);
					_harvestBtn.gameObject.SetActive(false);
					_speed_Container.gameObject.SetActive(true);		
				}

			}
			else
			{
				_productBtn.gameObject.SetActive(true);
				_repairBtn.gameObject.SetActive(true);
				_harvestBtn.gameObject.SetActive(false);
				_speed_Container.gameObject.SetActive(false);		
			}
//
//			bool isSpeedShow ;
//			if(_model_unit != null && _model_unit.produceLeftTime > 0)
//			{
//				isSpeedShow = true;
//			}
//			else
//			{
//				isSpeedShow = false;
//			}
//			_speed_Container.gameObject.SetActive(isSpeedShow);			 
//			_productBtn.gameObject.SetActive(!isSpeedShow);
//			_repairBtn.gameObject.SetActive(!isSpeedShow);

			//坦克研发红点
			_researchRedPoint.gameObject.SetActive(_unitData.isCanUnlock);
			_researchBtn.isEnabled = _unitData.isCanUnlock;
//			所需图纸个数
			_needPaperValue_Label.text = _unitData.unitData.chipCount.ToString();
			Model_ItemGroup model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
			Item item = model_itemGroup.QueryItem(_unitData.unitData.chipId);
//			拥有的个数 
			_currentValue_Label.text = item.num.ToString();
			// 生产的小红点
			ConnectionValidateHelper.CostCheck costCheck = ConnectionValidateHelper.IsEnoughCost(_dataUnit.cost);
			if(costCheck == ConnectionValidateHelper.CostCheck.OK)
			{
				_product_redPoint.gameObject.SetActive(true);
			}
			else
			{
				_product_redPoint.gameObject.SetActive(false);
			}

			// Clear CD Need Cash
			if (_model_unit != null) {
				_Timer_Colored_Slider.value = (_model_unit.produceLeftTime) / (float)_model_unit.produceTotalTime;
				_timeLabel.text = UIHelper.setTimeDHMS(_model_unit.produceLeftTime);
				float needCash = _model_User.model_InitialConfig.GetClearUnitCDCash (_model_unit.produceLeftTime);
				_speedCoinLabel.text = (int)needCash + "";
			}
			 
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
			if(_unitData.isUnLock  == true)
			{
				_currentInventoryLabel.text = _model_unit.num.ToString();
				_currentDamageLabel.text = _model_unit.onDamaged.ToString();
				if(_RepairUnit != null)
				{
					_repairIngLabel.text = _RepairUnit.onRepair.ToString();
				}
				else
				{
					_repairIngLabel.text = "0";
				}
			}
		}
		else
		{
			_currentInventoryLabel.text = "0";
			_repairIngLabel.text = "0";
			_currentDamageLabel.text = "0";
		}
		
	}
	void OnHarvest()
	{
		Trace.trace("OnHarvest ",Trace.CHANNEL.UI);
		OnProduceTimer(_model_unit);
	}
	//详情
	void OnGoToDetail()
	{
		TankInfoPanel.PanelType type = TankInfoPanel.PanelType.TANKFACTORY;
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKINFO,_dataUnit,type);
	}
	void OnToObtain()
	{
		ItemDataManager itemManger = new ItemDataManager();
		ItemDataManager.ItemData itemData = itemManger.getItemDataByID(_dataUnit.chipId);
		UIController.instance.CreatePanel(UICommon.UI_PANEL_NORMALPROP,itemData);
	}
	void OnResearch()
	{
		UnlockUnit();
	}
	void OnProduct()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKPRODUCT,_unitData);
	}
	void OnRepair()
	{
		UIController.instance.DeletePanel(UICommon.UI_PANEL_TANKFACTORY);
		UIController.instance.CreatePanel(UICommon.UI_PANEL_REPAIRFACTORY);
	}
	void OnSpeed()
	{
		int requst = ConnectionValidateHelper.IsEnoughCashClearBuildUnitCD(_model_User,_model_unit.unitId);
		if(requst == 0)
		{
			if(_model_unit != null)
			{

				FinishAddUnit();
			}
		}
		else
		{
			UIHelper.BuyCashUI();
		}
	}
	void OpenResearSuccPanel()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_RESEARCHSUCC,_unitData);
	}
	void Awake()
	{
		_TankNameLabel = transform.Find("TankNameLabel").GetComponent<UILabel>();
		_iconBg = transform.Find("tankbg").GetComponent<UISprite>();
		_icon = _iconBg.gameObject.transform.Find("TankIcon").GetComponent<UITexture>();
		_iconBtn = _iconBg.gameObject.transform.Find("TankIcon").GetComponent<UIButton>(); 
		_format_Sprite = _iconBg.gameObject.transform.Find("format_Sprite").GetComponent<UISprite>();
		_canResearch_Sprite = _iconBg.gameObject.transform.Find("canResearch_Sprite").GetComponent<UISprite>();

		EventDelegate evicon = new EventDelegate(OnGoToDetail);
		_iconBtn.onClick.Add(evicon);
		_lock_Container = transform.Find("lock_Container");
		_needPaperValue_Label = _lock_Container.Find("needPaper_Label/needPaperValue_Label").GetComponent<UILabel>();
		_currentValue_Label = _lock_Container.Find("current_Label/currentValue_Label").GetComponent<UILabel>();
		
		_redPoint_Sprite = _lock_Container.Find("researchBtn/redPoint_Sprite").GetComponent<UISprite>();
		_researchBtn = _lock_Container.Find("researchBtn").GetComponent<UIButton>();
		_toObtainBtn = _lock_Container.Find("toObtainBtn").GetComponent<UIButton>();
		_researchRedPoint = _researchBtn.gameObject.transform.Find("redPoint_Sprite").GetComponent<UISprite>();
		EventDelegate evresear = new EventDelegate(OnResearch);
		_researchBtn.onClick.Add(evresear);
		EventDelegate evtoba = new EventDelegate(OnToObtain);
		_toObtainBtn.onClick.Add(evtoba);
		
		
		_unlock_Container = transform.Find("unlock_Container");
		_speed_Container = _unlock_Container.Find("speed_Container");
		_currentInventoryLabel = _unlock_Container.Find("currentInventoryLabel/currentValueLabel").GetComponent<UILabel>();
		_repairIngLabel = _unlock_Container.Find("currentMaintenanceLabel/currentValueLabel").GetComponent<UILabel>();
		_currentDamageLabel = _unlock_Container.Find("currentDamageLabel/currentValueLabel").GetComponent<UILabel>();
		_productBtn = _unlock_Container.Find("productBtn").GetComponent<UIButton>();
		_repairBtn = _unlock_Container.Find("repairBtn").GetComponent<UIButton>();
		_product_redPoint = _unlock_Container.Find("productBtn/redPoint_Sprite").GetComponent<UISprite>();
		_repair_redPoint = _unlock_Container.Find("repairBtn/redPoint_Sprite").GetComponent<UISprite>();
		_repair_redPoint.gameObject.SetActive(false);

		EventDelegate evpro = new EventDelegate(OnProduct);
		_productBtn.onClick.Add(evpro);
		EventDelegate evrep = new EventDelegate(OnRepair);
		_repairBtn.onClick.Add(evrep);

		_harvestBtn = _unlock_Container.Find("harvestBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_harvestBtn,OnHarvest);
		_Timer_Colored_Slider = _unlock_Container.Find("speed_Container/Timer_Colored_Slider").GetComponent<UISlider>();
		_timeLabel = _unlock_Container.Find("speed_Container/Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_speedBtn = _unlock_Container.Find("speed_Container/speedBtn").GetComponent<UIButton>();
		EventDelegate evspe = new EventDelegate(OnSpeed);
		_speedBtn.onClick.Add(evspe);
		_speedCoinLabel = _unlock_Container.Find("speed_Container/speedBtn/speedCoinLabel").GetComponent<UILabel>();
	}
	//===============================
	/// <summary>
	/// 加速
	/// </summary>
	void FinishAddUnit() 
	{
		UIHelper.LoadingPanelIsOpen(true);
		ProcessUnitRequest request = new ProcessUnitRequest ();

		request.api = new Model_ApiRequest ().api;
		request.unitId = _model_unit.unitId;
		request.num = _model_unit.onProduce;
		request.buyCd = 1;

		(new PBConnect_finishAddUnit ()).Send (request, OnFinishAddUnit);
	}
	void OnFinishAddUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {		
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshProductTank,new Notification(""));
			Trace.trace("OnFinishAddUnit success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnFinishAddUnit failure",Trace.CHANNEL.UI);
		}
	}

	void UnlockUnit()
	{
		UIHelper.LoadingPanelIsOpen(true);
		ComposeUnitRequest request = new ComposeUnitRequest ();
		request.api = new Model_ApiRequest ().api;
		request.unitId = DataManager.instance.dataUnitsGroup.GetUnitUnlockWithItem (_dataUnit.chipId).id;

		(new PBConnect_composeUnit ()).Send (request, OnUnlockUnit);
	}
	void OnUnlockUnit(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			
			OpenResearSuccPanel();
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshProductTank,new Notification(""));
			Trace.trace("OnUnlockUnit success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("OnUnlockUnit fail",Trace.CHANNEL.UI);
		}
	}

	void OnProduceTimer(Model_Unit modelUnit)
	{
		UIHelper.LoadingPanelIsOpen(true);
		SlgPB.ProcessUnitRequest request = new SlgPB.ProcessUnitRequest ();	
		request.api = new Model_ApiRequest ().api;
		request.unitId = modelUnit.unitId; 
		request.num = modelUnit.onProduce;
		(new PBConnect_FinishUnit ()).Send (request, OnFinishProduce);

	}
	void OnFinishProduce(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {

			InstancePlayer.instance.model_User.model_Queue.RemoveUnitProduceQueue();

		} else {
			//	Trace.trace("OnFinishProduce failed", Trace.CHANNEL.INTEGRATION);
		}
	}

}
