using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class StrengthenTankItem : MonoBehaviour {
	private UILabel _tankName;
	private UILabel _tankCount;
	private UISprite _icon;
	private UISprite _iconBg;
	private UISprite _redPoint;
	private UIButton _iconBtn;
	//data 
	TankDataManager.UnitData _unitData ;
	Model_Unit  _modelUnit;
	Dictionary<int, Model_Unit> _units;
	void Awake()
	{
		_tankName = transform.Find("name_Label").GetComponent<UILabel>();
		_tankCount = transform.Find("bg_Sprite/count_Label").GetComponent<UILabel>();
		_icon = transform.Find("iconBg/TankIcon").GetComponent<UISprite>();
		_iconBtn =  transform.Find("iconBg/TankIcon").GetComponent<UIButton>();
		_iconBg = transform.Find("iconBg").GetComponent<UISprite>();
		_redPoint = transform.Find("point_Sprite").GetComponent<UISprite>();

		EventDelegate ev = new EventDelegate(OnIcon);
		_iconBtn.onClick.Add(ev);
	}
 
	void OnIcon()
	{
		NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshStrengthenTank,new Notification(_unitData));
	}
	// Update is called once per frame
	void Update () {
		UpdataUI();
	}
	void UpdataUI()
	{
		if(_unitData == null)return;
		_units = InstancePlayer.instance.model_User.unlockUnits;
		_units.TryGetValue(_unitData.id,out _modelUnit );
	
		_iconBg.spriteName = _unitData.iconBgName;
		_icon.spriteName = _unitData.iconName;
		_tankName.color = _unitData.nameColor;
		_tankName.text = _unitData.unitData.name;
		_tankCount.text = _modelUnit.num.ToString();
		UpdataPromptPoint(_unitData);

	}

	void UpdataPromptPoint(TankDataManager.UnitData unitData)
	{
		if(unitData == null)return;
 
		DataUnitPart[] unitPart = _modelUnit.GetDataParts();
		bool isshow= false;
		
		int count = unitPart.Length;
		for(int i = 0;i < count;i++)
		{
			PBConnect_upgradeUnitPart.RESULT result = PBConnect_upgradeUnitPart.CheckUpgrade(_modelUnit.unitId,i);
			isshow = result == PBConnect_upgradeUnitPart.RESULT.OK ? true:false;
			if(isshow)break;
		}
		_redPoint.gameObject.SetActive(isshow);
	}
	public void Init(TankDataManager.UnitData unitData)
	{
		_unitData = unitData;

	}
	 
}
