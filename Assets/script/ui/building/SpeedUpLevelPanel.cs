using UnityEngine;
using System.Collections;
using SlgPB;
public class SpeedUpLevelPanel : PanelBase {

	private BoxCollider _collider;
	private UIButton _closeBtn;
	private UIButton _speedBtn;
	private UILabel _cashLabel;
	private UILabel _remainingLabel;

	//data  
	Model_Building _model_building;
	Model_User _model_user;

	override public void Init ()
	{
		base.Init ();


	}


	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		_model_building  = parameters[0] as Model_Building;
		InitUI();
		_model_user = InstancePlayer.instance.model_User;
	}

	public void InitUI()
	{
		_closeBtn = transform.Find("UpRequire/bg_Sprite/BtnClose").GetComponent<UIButton>();
		_speedBtn = transform.Find("UpRequire/Btn_speed_Sprite").GetComponent<UIButton>();

		EventDelegate ev = new EventDelegate (OnHide);
		_closeBtn.onClick.Add (ev);

		EventDelegate evsp = new EventDelegate (OnSpeed);
		_speedBtn.onClick.Add (evsp);

		_cashLabel = _speedBtn.transform.Find("cash_Label").GetComponent<UILabel>();
		_remainingLabel =  transform.Find("UpRequire/remaining_time_Label/remaining_value_Label").GetComponent<UILabel>();
	}

	void Update()
	{
		if(_model_building != null)
		{
			_remainingLabel.text = UIHelper.setTimeDHMS(_model_building.buildingLevelUpTime);
			float needCash = _model_user.model_InitialConfig.GetClearBuildingCDCash (_model_building.buildingLevelUpTime);
			_cashLabel.text = needCash + "";
		}

	}

	public void OnSpeed()
	{
		OnHide();
		FinishUpgradeBuilding();
	}

	public void OnHide()
	{
		Delete();
	}

// ======================================================
	//server 


	void FinishUpgradeBuilding()
	{
		UIHelper.LoadingPanelIsOpen(true);
		UpgradeBuildingRequest request = new UpgradeBuildingRequest ();
		
		if (InstancePlayer.instance.model_User.buildings.Count > 0) {
			Model_Building building = InstancePlayer.instance.model_User.buildings [_model_building.buildingType];
			if (building != null) {
				// 秒CD使用 
				request.buildingId = building.id;
				request.buyCd = 1; //是否消耗cash忽略剩余CD时间直接完成
				
				request.api = new Model_ApiRequest ().api;	

				Model_User user = InstancePlayer.instance.model_User;
				int isUpgrade = ConnectionValidateHelper.IsEnoughCashClearBuildingCD(user, building.buildingType);
				
				if(isUpgrade == 0)
				{
					(new PBConnect_FinishUpgradeBuilding()).Send(request, OnFinishUpgradeBuilding);
				}
				else
				{
					UIHelper.BuyCashUI();
				}
				return;
			}
		}
		
	}
	void OnFinishUpgradeBuilding(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			InstancePlayer.instance.model_User.model_Queue.RemoveBuildingQueue();
			Trace.trace("秒CD  OnFinishUpgradeBuilding success",Trace.CHANNEL.UI);
		} else {
			Trace.trace(" 秒CD  OnFinishUpgradeBuilding",Trace.CHANNEL.UI);
		}
	}

 
}
