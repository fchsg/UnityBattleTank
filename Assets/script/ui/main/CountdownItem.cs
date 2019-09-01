using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
public class CountdownItem : MonoBehaviour {


	private UILabel _clock_Label;
	private UIButton _Clock_Btn;

	//data
	CountdownData.Countdown _countdown;
	CountdownData.CountdownType _type;

	Model_Unit _modelUnit;
	Model_RepairUnit _modelRepairUnit;
	Model_Building _modelBuilding;
	void Awake()
	{
		_clock_Label = transform.Find("Sprite/Clock_Label").GetComponent<UILabel>();
		_Clock_Btn = transform.Find("Clock_1_Btn").GetComponent<UIButton>();
		EventDelegate ev = new EventDelegate(OnClick);
		_Clock_Btn.onClick.Add(ev);
	}
	public void Init(CountdownData.Countdown data)
	{
		_countdown = data;
		_type = _countdown.dataType;
//		Trace.trace(" data " + data.dataType,Trace.CHANNEL.UI);
		switch(_type)
		{
		case CountdownData.CountdownType.BUILDINGUP:
			_modelBuilding = _countdown.data as Model_Building;
			break;
		case CountdownData.CountdownType.PRODUCTION:
			_modelUnit = _countdown.data as Model_Unit;
			break;
		case CountdownData.CountdownType.REPAIR:
			_modelRepairUnit = _countdown.data as Model_RepairUnit;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(_countdown != null)
		{
			int timer = 0;
			switch(_type)
			{
			case CountdownData.CountdownType.BUILDINGUP:
				timer = _modelBuilding.buildingLevelUpTime;
				break;
			case CountdownData.CountdownType.PRODUCTION:
				timer = _modelUnit.produceLeftTime;
				break;
			case CountdownData.CountdownType.REPAIR:
				timer = _modelRepairUnit.repairLeftTime;
				break;
			}
			_clock_Label.text = UIHelper.setTimeDHMS(timer);
			if(timer != 0)
			{
				_Clock_Btn.isEnabled = false;

			}
			else
			{
				_clock_Label.text = "完成";
				_Clock_Btn.isEnabled = true;
			}
		}
	}

	void OnClick()
	{
		Trace.trace(" OnClick ",Trace.CHANNEL.UI);
		switch(_type)
		{
		case CountdownData.CountdownType.BUILDINGUP:
			Trace.trace(" OnClick BUILDINGUP ",Trace.CHANNEL.UI);
			RequestFinishUpgradeBuilding(_modelBuilding.id);
			break;
		case CountdownData.CountdownType.PRODUCTION:
			Trace.trace(" OnClick PRODUCTION",Trace.CHANNEL.UI);
			OnProduceTimer(_modelUnit);
			break;
		case CountdownData.CountdownType.REPAIR:
			Trace.trace(" OnClick REPAIR",Trace.CHANNEL.UI);
			OnRepairTimer();
			break;
		}
	}
	void Delete()
	{
		NGUITools.Destroy(this.gameObject);
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
			Delete();
		} else {
			//	Trace.trace("OnFinishProduce failed", Trace.CHANNEL.INTEGRATION);
		}
	}


	void RequestFinishUpgradeBuilding(int id)
	{
		UIHelper.LoadingPanelIsOpen(true);
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();
		request.buildingId = id;

		request.api = new Model_ApiRequest ().api;	
		(new PBConnect_FinishUpgradeBuilding()).Send(request, OnRequestFinishUpgradeBuilding);
	}

	void OnRequestFinishUpgradeBuilding(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) 
		{
			InstancePlayer.instance.model_User.model_Queue.RemoveBuildingQueue ();
			Delete();
			//Trace.trace("OnRequestFinishUpgradeBuilding success", Trace.CHANNEL.INTEGRATION);
		} 
		else
		{
			//Trace.trace("OnRequestFinishUpgradeBuilding failed", Trace.CHANNEL.INTEGRATION);
		}
	}

	void OnRepairTimer()
	{
			SlgPB.RepairUnitRequest request = new SlgPB.RepairUnitRequest ();	
			request.api = new Model_ApiRequest ().api;
			request.buyCd = 0;
			(new PBConnect_finishRepairUnit ()).Send (request,OnFinishRepair);
	}

	void OnFinishRepair(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) 
		{
			InstancePlayer.instance.model_User.model_Queue.RemoveUnitRepairQueue();
			Delete();
			//Trace.trace("OnRequestFinishUpgradeBuilding success", Trace.CHANNEL.INTEGRATION);
		} 
		else
		{
			//Trace.trace("OnRequestFinishUpgradeBuilding failed", Trace.CHANNEL.INTEGRATION);
		}
	}
}
