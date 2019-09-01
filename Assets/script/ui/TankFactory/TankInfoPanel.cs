using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Tank info panel.
/// </summary>
public class TankInfoPanel : PanelBase {

	public enum PanelType {
		TANKPRODUCT = 0,//生产坦克
		TANKFACTORY = 1,//兵工厂
		TANKREPAIR = 2,//维修界面

	}
	private UIButton _RecyclingBtn;


	private UITexture _TankIcon;
	private UIButton _close;

	private UILabel _TankNameLabel;    
	private UILabel _CurrentTankValueLabel;

	private UILabel _describeValueLabel;

	private UILabel _FightingValueLabel;
	private UILabel _APValueLabel;     
	private UILabel _HPValueLabel;     
	private UILabel _DPValueLabel;     
	private UILabel _HitRateValueLabel;
	private UILabel _SpeedValueLabel;  
	private UILabel _LengthValueLabel; 
	private UILabel _DoubleDamageRateValueLabel;

	private UIButton _productBtn;
	private UIButton _strongBtn;

	private Transform _RecyclingContainer; 

	private RecyclingTankPanel _RecyclingTankPanel;
	//data 
	DataUnit _dataUnit;
	Model_Unit _model_Unit;
	Dictionary<int, Model_Unit> units = new Dictionary<int, Model_Unit>();
	TankInfoPanel.PanelType _panelType;
	TankDataManager.UnitData  _unitData;
	override public void Init()
	{
		base.Init ();
	}

	override public void Open(params System.Object[] parameters)
	{
		base.Open (parameters);
		if (parameters != null && parameters.Length > 0) {
			_dataUnit = parameters[0] as DataUnit;
			_panelType = (PanelType)parameters[1];
			TankDataManager tankmanager = new TankDataManager();
			_unitData = tankmanager.InitUnitData(_dataUnit);
		}

	}
	void Update()
	{
		UpdateUI();
	}

	public void UpdateUI(){

		if(_dataUnit != null)
		{
			units = InstancePlayer.instance.model_User.unlockUnits;
			if(units != null && units.ContainsKey(_dataUnit.id))
			{
				_model_Unit = units[_dataUnit.id];
			}

			_TankNameLabel.color = _unitData.nameColor;
//			_iconBg.spriteName = _unitData.iconBgName;
			_TankIcon.SetUnitBigTexture(_dataUnit.id);
			_TankNameLabel.text = _dataUnit.name; 
	
			_FightingValueLabel.text = _dataUnit.battleParam.hp.ToString(); 
			_APValueLabel.text = _dataUnit.battleParam.damage.ToString() ;     
			_HPValueLabel.text = _dataUnit.battleParam.hp.ToString();    
			_DPValueLabel.text = _dataUnit.battleParam.ammo.ToString();    
			_HitRateValueLabel.text = _dataUnit.battleParam.hitRate.ToString(); 
			_SpeedValueLabel.text = _dataUnit.speed.ToString(); 
			_LengthValueLabel.text = _dataUnit.length.ToString(); 
			_DoubleDamageRateValueLabel.text = _dataUnit.battleParam.doubleDamageRate.ToString() ; 
			 
			if(_model_Unit != null)
			{
				_CurrentTankValueLabel.text = _model_Unit.num.ToString();
			}
			else
			{
				_CurrentTankValueLabel.text = "0";
			}
			if(_unitData.isUnLock)
			{
				_strongBtn.gameObject.SetActive(true);
				_RecyclingBtn.gameObject.SetActive(true);
				_productBtn.gameObject.SetActive(true);
			}
			else
			{
				_strongBtn.gameObject.SetActive(false);
				_RecyclingBtn.gameObject.SetActive(false);
				_productBtn.gameObject.SetActive(false);

			}
		}

	}



	public void OnRecyclingNum()
	{
		if(_model_Unit != null)
		{
			if(_model_Unit.num > 0)
			{
				//申请分解坦克
				_RecyclingTankPanel.Init(_model_Unit);
				_RecyclingContainer.gameObject.SetActive(true);

			}
			else
			{
				Trace.trace("分解坦克数量大于拥有坦克数量 ",Trace.CHANNEL.UI);
			}
		}
	}

	void OnProduct()
	{
		if(_panelType != null)
		{
			switch(_panelType)
			{
			case PanelType.TANKFACTORY:
				
				if(_unitData.isUnLock)
				{
					OnDestroyPanel();
					UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKPRODUCT,_unitData);
				}
				else
				{	
					string str = "当前坦克未解锁";
					UIHelper.ShowTextPromptPanel(this.gameObject,str);
				}

				break;
			case PanelType.TANKPRODUCT:
				OnDestroyPanel();
				break;
			case PanelType.TANKREPAIR:
				OnDestroyPanel();
				UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKPRODUCT,_unitData);
				break;
			default :
				break;
			}
		}
		 

	}

	void OnStrong()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_STRENGTHEN);
	}
	void OnDestroyPanel()
	{
		Delete();
	}
	void OnDestroy()
	{
	
	}
	void Awake()
	{
		_RecyclingBtn = transform.Find("TankInfoContainer/TankInfo/RecyclingBtn").GetComponent<UIButton>();
		
		
		_TankIcon = transform.Find("TankInfoContainer/TankInfo/TankIcon").GetComponent<UITexture>();
		_close = transform.Find("TankInfoContainer/btnClose").GetComponent<UIButton>();
		
		_RecyclingContainer = transform.Find("TankInfoContainer/RecyclingContainer");
		
		_TankNameLabel = transform.Find("TankInfoContainer/TankInfo/TankNameLabel").GetComponent<UILabel>();
		_CurrentTankValueLabel = transform.Find("TankInfoContainer/TankInfo/CurrentTankLabel/CurrentTankValueLabel").GetComponent<UILabel>();
		_describeValueLabel = transform.Find("TankInfoContainer/Fight_Container/describeLabel/describeValueLabel").GetComponent<UILabel>();

		_FightingValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_1_Label/FightingValueLabel").GetComponent<UILabel>();
		_APValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_2_Label/FightingValueLabel").GetComponent<UILabel>();   
		_HPValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_3_Label/FightingValueLabel").GetComponent<UILabel>();
		_DPValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_4_Label/FightingValueLabel").GetComponent<UILabel>();
		_HitRateValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_5_Label/FightingValueLabel").GetComponent<UILabel>();
		_SpeedValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_6_Label/FightingValueLabel").GetComponent<UILabel>();
		_LengthValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_7_Label/FightingValueLabel").GetComponent<UILabel>();
		_DoubleDamageRateValueLabel = transform.Find("TankInfoContainer/Fight_Container/Fighting_8_Label/FightingValueLabel").GetComponent<UILabel>();
		
		
		EventDelegate evClose = new EventDelegate (OnDestroyPanel);
		_close.onClick.Add (evClose);
		
		EventDelegate evRecyclingNum = new EventDelegate (OnRecyclingNum);
		_RecyclingBtn.onClick.Add (evRecyclingNum);
		
		_RecyclingTankPanel = _RecyclingContainer.GetComponent<RecyclingTankPanel>();
		_RecyclingContainer.gameObject.SetActive(false);

		_productBtn = transform.Find("TankInfoContainer/Fight_Container/productBtn").GetComponent<UIButton>();
		_strongBtn = transform.Find("TankInfoContainer/Fight_Container/strongBtn").GetComponent<UIButton>();
		EventDelegate evpro = new EventDelegate(OnProduct);
		_productBtn.onClick.Add(evpro);
		EventDelegate evstron = new EventDelegate(OnStrong);
		_strongBtn.onClick.Add(evstron);
		
	}

	//==================================================================
	//	向服务器请求分解坦克 
}
