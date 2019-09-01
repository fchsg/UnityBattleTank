using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 待维修的坦克
/// </summary>
public class TankRepairItem : MonoBehaviour {
 	
	private UILabel _tankName;
	private UISprite _icon;
	private UISprite _iconBg;
	private UIButton _tankIcon;
	private Transform _recoverContainer;
	private UIButton _addBtn;
	private UIButton _delBtn;
	private UISlider _tankSlider;
	private UILabel _addTankNumLabel;
	private UILabel _Damage_3_ValueLabel;
	private UILabel _Damage_2_ValueLabel;
	private UILabel _Damage_1_ValueLabel;
 	
	private Transform _speed_Container;
	private UISlider _Timer_Colored_Slider;
	private UISprite _thumbIcon;
//	private UILabel _Timel_Label;

	private float _canRepairUnitCount;
	private int _CurrentTankNum;

	private bool isAll = false;
	//data 
	Model_Unit _model_Unit;
	Dictionary<int,Model_Unit> _units;
	DataUnit _dataUnit ;

	Model_RepairUnit _model_RepairUnit;
	List<SlgPB.Unit> _repairUnitsList ;
	SlgPB.Unit _repairUnit ;

	Dictionary<int,int> _resCountDic = new Dictionary<int,int>();
	TankDataManager.UnitData  _unitData = new TankDataManager.UnitData();
	void Awake()
	{
		_tankName = transform.Find("TankName_Label").GetComponent<UILabel>();
		_tankIcon = transform.Find("icon_bg/TankIcon").GetComponent<UIButton>();
		_icon = transform.Find("icon_bg/TankIcon").GetComponent<UISprite>();
		_iconBg = transform.Find("icon_bg").GetComponent<UISprite>();
		_recoverContainer = transform.Find("RecoverContainer");
		_addBtn = _recoverContainer.Find("AddBtn").GetComponent<UIButton>();
		_delBtn = _recoverContainer.Find("DelBtn").GetComponent<UIButton>();
		_tankSlider = _recoverContainer.Find("TankItem_Slider").GetComponent<UISlider>();
		_addTankNumLabel = _recoverContainer.Find("AddTankNumLabel").GetComponent<UILabel>();
		_Damage_3_ValueLabel = transform.Find("Damage_3_Label/Damage_3_ValueLabel").GetComponent<UILabel>();
		_Damage_2_ValueLabel = transform.Find("Damage_2_Label/Damage_2_ValueLabel").GetComponent<UILabel>();
		_Damage_1_ValueLabel = transform.Find("Damage_1_Label/Damage_1_ValueLabel").GetComponent<UILabel>();

		_speed_Container = transform.Find("speed_Container");
		_Timer_Colored_Slider = _speed_Container.Find("Timer_Colored_Slider").GetComponent<UISlider>();
//		_Timel_Label = _speed_Container.FindChild("Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_thumbIcon = _speed_Container.Find("Timer_Colored_Slider/Thumb").GetComponent<UISprite>();
//		_speed_Container.gameObject.SetActive(false);
//		_recoverContainer.gameObject.SetActive(false);

		EventDelegate evicon = new EventDelegate(onClickIcon);
		_tankIcon.onClick.Add(evicon);
		EventDelegate evadd = new EventDelegate(OnAdd);
		_addBtn.onClick.Add(evadd);
		EventDelegate evdel = new EventDelegate(OnDel);
		_delBtn.onClick.Add(evdel);
		SetRepairAllTank();
	}
	void Start () {

	}
	public void Init(Model_Unit model_Unit)
	{
		if(model_Unit != null)
		{
			_model_Unit = model_Unit;
			Trace.trace("model_Unit" + model_Unit.unitId,Trace.CHANNEL.UI);
		}
	}

	void Update () {
		UpdateUI();
	}
	void UpdateUI()
	{
		if(_model_Unit != null)
		{
			_units = InstancePlayer.instance.model_User.unlockUnits;
			_units.TryGetValue(_model_Unit.unitId,out _model_Unit);
			_model_RepairUnit = InstancePlayer.instance.model_User.model_RepairUnit;
			_repairUnitsList = _model_RepairUnit.GetRepairUnits();

			_dataUnit = DataManager.instance.dataUnitsGroup.GetUnit(_model_Unit.unitId);
			TankDataManager tankmanager = new TankDataManager();
			_unitData = tankmanager.InitUnitData(_dataUnit);

			_canRepairUnitCount = (float)_model_Unit.onDamaged;
			_tankName.color = _unitData.nameColor;
			_iconBg.spriteName = _unitData.iconBgName;
			_icon.spriteName = _unitData.iconName;
			_thumbIcon.spriteName = _unitData.iconName;
			_tankName.text = _dataUnit.name;
	
			if (_tankSlider != null && _addTankNumLabel != null)
			{
				_addTankNumLabel.text = Mathf.RoundToInt(_tankSlider.value * _canRepairUnitCount).ToString();
				_CurrentTankNum = Mathf.RoundToInt(_tankSlider.value * _canRepairUnitCount);
			}

			_Damage_3_ValueLabel.text = _model_Unit.onDamaged.ToString();
			_Damage_2_ValueLabel.text = _model_Unit.num.ToString();
			_Damage_1_ValueLabel.text = _model_Unit.onProduce.ToString();

			bool isRepairIng = false;
			foreach(SlgPB.Unit unit in _repairUnitsList)
			{
				if(unit.unitId == _model_Unit.unitId)
				{
					if(_model_RepairUnit.repairLeftTime > 0)
					{
						isRepairIng = true;
					}
					break;
				}
			}
			if(isRepairIng)
			{
				if(_model_RepairUnit != null && _model_RepairUnit.repairLeftTime > 0)
				{
					_speed_Container.gameObject.SetActive(true);
					_recoverContainer.gameObject.SetActive(false);
					_Timer_Colored_Slider.value = (float)_model_RepairUnit.repairLeftTime / _model_RepairUnit.repairTotalTimeSec;
				}
			}
			else
			{
				_speed_Container.gameObject.SetActive(false);
				_recoverContainer.gameObject.SetActive(true);
			}
			ResData();
			if((_model_Unit.onDamaged  == 0) && (_model_Unit.onRepair == 0))
			{
				NGUITools.Destroy(this.gameObject);
				NotificationCenter.instance.DispatchEvent(Notification_Type.RequestRepairTank,new Notification(_model_Unit));
			}
		}
	}

	public int GetTankRepairCount()
	{
		if(_CurrentTankNum != null)
		{
			return _CurrentTankNum;
		}
		return 0;

	}
	public int GetCurrentRepairTankNum()
	{
		if(_CurrentTankNum != null)
		{
			return _CurrentTankNum;
		}
		return 0;
	}
	//
	public Model_Unit GetCurrentModelUnit()
	{
		if(_model_Unit != null)
		{
			return _model_Unit;
		}
		return null;
	}

	void ResData()
	{
		if(_resCountDic != null)
		{
			_resCountDic.Clear();
			int current = _CurrentTankNum;
			_resCountDic.Add(1,(Mathf.CeilToInt(_dataUnit.cost.costFood * current / 2.0f)));
			_resCountDic.Add(2,(Mathf.CeilToInt(_dataUnit.cost.costOil  * current / 2.0f)));
			_resCountDic.Add(3,(Mathf.CeilToInt(_dataUnit.cost.costMetal * current/ 2.0f)));
			_resCountDic.Add(4,(Mathf.CeilToInt(_dataUnit.cost.costRare * current / 2.0f)));
			_resCountDic.Add(5,(Mathf.CeilToInt(_dataUnit.cost.costTime  * current/ 2.0f)));
			_resCountDic.Add(6,(Mathf.CeilToInt(_dataUnit.costCdCash * current / 2.0f) ));

		}
	}

	public void SetRepairAllTank()
	{
		if(isAll == false)
		{
			_tankSlider.value = 1.0f;
			isAll = true;
		}
		else
		{
			_tankSlider.value = 0.0f;
			isAll = false;
		}

	}

	public Dictionary<int,int> GetResCountDic()
	{
		if(_resCountDic != null)
		{
			return _resCountDic;
		}
		return null;
	}
	public void OnAdd()
	{
		if (_tankSlider != null && _addTankNumLabel != null)
		{
			_tankSlider.value = _tankSlider.value + (1/_canRepairUnitCount);
			_addTankNumLabel.text = Mathf.RoundToInt(_tankSlider.value * _canRepairUnitCount).ToString();
		}
	}
	void onClickIcon()
	{
		TankInfoPanel.PanelType _type = TankInfoPanel.PanelType.TANKREPAIR;
		UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKINFO,_dataUnit,_type);
	}
	public void OnDel()
	{
		if (_tankSlider != null && _addTankNumLabel != null)
		{
			_tankSlider.value = _tankSlider.value - (1/_canRepairUnitCount);
			_addTankNumLabel.text = Mathf.RoundToInt(_tankSlider.value * _canRepairUnitCount).ToString();
		}
	}

}
