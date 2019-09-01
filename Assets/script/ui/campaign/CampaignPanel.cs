using UnityEngine;
using System.Collections;

public class CampaignPanel : PanelBase {

	//------------------ UI

	public UIButton closed;
	public UIButton left_arrow;
	public UIButton right_arrow;

	public UIButton mission_detial;
	public UILabel    mission_name_label;
	public UILabel 	   all_star_label;
	public UILabel    energy_label;
	public UILabel    recommend_power_label;
	public UILabel    left_count_label;


	public CampaignSpinePanel _campaignSpinePanel;

	//------------------ Data
	public int _missionMagicId;

	//test
	void Awake()
	{
		//Init ();
	}

	public override void Init ()
	{
		base.Init ();

		animationType = PanelBase.AnimationType.ALPHA;

		closed = UIHelper.FindChildInObject(gameObject, "Btn_Closed").GetComponent<UIButton>();
		mission_detial = UIHelper.FindChildInObject(gameObject, "Btn_Detial").GetComponent<UIButton>();

		left_arrow = UIHelper.FindChildInObject(gameObject, "Btn_Left").GetComponent<UIButton>();
		right_arrow = UIHelper.FindChildInObject(gameObject, "Btn_Right").GetComponent<UIButton>();

		UIHelper.AddBtnClick (closed, OnClosedClick);
		UIHelper.AddBtnClick (left_arrow, OnLeftArrowClick);
		UIHelper.AddBtnClick (right_arrow, OnRightArrowClick);
		UIHelper.AddBtnClick (mission_detial, OnMissionDetialClick);
		UIHelper.AddBtnClick (gameObject, "Btn_Battle", OnBattle);

		UIHelper.AddBtnClick(gameObject , "Top_Container/normal_btn", OnNormalClick);
		UIHelper.AddBtnClick(gameObject , "Top_Container/elite_btn", OnEliteClick);

		mission_name_label =  UIHelper.FindChildInObject(gameObject, "Bottom_Container/Label").GetComponent<UILabel>();
		all_star_label = UIHelper.FindChildInObject (gameObject, "Top_Container/Left_Sprite/Label").GetComponent<UILabel>();
		energy_label = UIHelper.FindChildInObject (gameObject, "Top_Container/right_Sprite/Label").GetComponent<UILabel> ();
		recommend_power_label =  UIHelper.FindChildInObject (gameObject, "Detial_Panel/Energy_Label").GetComponent<UILabel> (); 
		left_count_label =  UIHelper.FindChildInObject (gameObject, "Detial_Panel/Count_Label").GetComponent<UILabel> ();               

		_campaignSpinePanel = gameObject.AddComponent<CampaignSpinePanel> ();
		_campaignSpinePanel.Init (this);
	}
		
	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		int magicId =  (int) parameters [0];

		// 初始化数据
		_missionMagicId = magicId;

		// 更新UI
		UpdatePanelUI ();
		UpdateArrowUI ();
	}

	public void UpdatePanelUI()
	{
		UpdateDetialUI (_missionMagicId);
		_campaignSpinePanel.UpdatePageUI (_missionMagicId);

		InstancePlayer.instance.uiDataStatus.UpdateSelectedMission (_missionMagicId);
	}

	public void OnClosedClick()
	{
		UISceneManager.instance.SetActive (true);
		base.Delete ();
	}
		
	public void OnMissionDetialClick()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION, _missionMagicId);
	}

	// 更新panel UI
	public void UpdateDetialUI(int magicId)
	{
		Model_Level model_level = InstancePlayer.instance.model_User.model_level;
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (magicId);

		int allStarCount =model_level.GetAllMissionsStar (difficulty);

		mission_name_label.text = dataMission.name;
		all_star_label.text = allStarCount + "";
		recommend_power_label.text = dataMission.evaluate + ""; 

		Model_Mission model_Mission = model_level.GetMission (_missionMagicId);
		if (model_Mission != null) 
		{
			left_count_label.text = model_Mission.remainFightNum + "";          
		}
	}

	// 直接战斗
	public void OnBattle()
	{
		InstancePlayer.instance.uiDataStatus.state = UIDataStatus.STATE.CAMPAIGN;
		BattleConnection.instance.StartFight (_missionMagicId);
	}

	// 左选择
	public void OnLeftArrowClick()
	{
		int stageId = DataMission.GetStageId (_missionMagicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);
		--stageId;

		if (stageId > 0) 
		{
			Model_Level.Campaign campaign = InstancePlayer.instance.model_User.model_level.GetCampaign (difficulty, stageId);
			Model_Mission model_Mission = campaign.list [0];
			_missionMagicId = model_Mission.magicId;

			UpdatePanelUI ();
			UpdateArrowUI ();
		}
	}

	// 右选择
	public void OnRightArrowClick()
	{
		Model_Level model_level = InstancePlayer.instance.model_User.model_level;
		int stageId = DataMission.GetStageId (_missionMagicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);

		++stageId;
		Model_Level.Chapters chapter = InstancePlayer.instance.model_User.model_level.GetChapters (difficulty);
		if (stageId <= chapter.list.Count) 
		{
			// 判定是否解锁
			bool isUnlock = false;  
			if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
				isUnlock= model_level.IsNextNormalCampaignUnlock (stageId);
			} else {
				isUnlock = model_level.IsNextEliteCampaignUnlock (stageId);
			}

			if (isUnlock) 
			{
				Model_Level.Campaign campaign = InstancePlayer.instance.model_User.model_level.GetCampaign (difficulty, stageId);
				Model_Mission model_Mission = campaign.list [0];
				_missionMagicId = model_Mission.magicId;

				UpdatePanelUI ();
				UpdateArrowUI ();
			}
			else
			{
				if (difficulty == DataConfig.MISSION_DIFFICULTY.NORMAL) {
					string msg = "下一章节普通关卡未解锁";
					UIHelper.ShowTextPromptPanel (this.gameObject, msg);

				} else {
					string msg = "下一章节精英关卡未解锁";
					UIHelper.ShowTextPromptPanel (this.gameObject, msg);
				}
			}
		}
	}

	// 更新按钮是否显示
	public void UpdateArrowUI()
	{
		int stageId = DataMission.GetStageId (_missionMagicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);
		Model_Level.Chapters chapter = InstancePlayer.instance.model_User.model_level.GetChapters (difficulty);

		if (stageId <= 1) 
		{
			left_arrow.gameObject.SetActive (false);
		}
		else
		{
			left_arrow.gameObject.SetActive (true);
		}

		if (stageId >= chapter.list.Count)
		{
			right_arrow.gameObject.SetActive (false);
		}
		else
		{
			right_arrow.gameObject.SetActive (true);
		}
	}

	// 普通
	public void OnNormalClick()
	{
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);

		if (difficulty != DataConfig.MISSION_DIFFICULTY.NORMAL) 
		{
			Model_Level model_Level = InstancePlayer.instance.model_User.model_level;
			_missionMagicId = model_Level.GetLastUnlockMissionMagicId (DataConfig.MISSION_DIFFICULTY.NORMAL);
			UpdatePanelUI ();

			UpdateArrowUI ();
		}
		else
		{
			string msg = "当前关卡为普通关卡";
			UIHelper.ShowTextPromptPanel (this.gameObject, msg);
		}
	}

	// 精英
	public void OnEliteClick()
	{
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);

		if (difficulty != DataConfig.MISSION_DIFFICULTY.ELITE) 
		{
			Model_Level model_Level = InstancePlayer.instance.model_User.model_level;

			int magicId = model_Level.GetLastUnlockMissionMagicId (DataConfig.MISSION_DIFFICULTY.ELITE);
			if (magicId != 0) {

				_missionMagicId = magicId;

				UpdatePanelUI ();
				UpdateArrowUI ();
			} 
			else
			{
				string msg = "精英战役未解锁";
				UIHelper.ShowTextPromptPanel (this.gameObject, msg);
			}
		}
		else
		{
			string msg = "当前关卡为精英关卡";
			UIHelper.ShowTextPromptPanel (this.gameObject, msg);
		}
	}

	// 选择关卡
	public void OnSelectMission(int missionId)
	{
		int stageId = DataMission.GetStageId (_missionMagicId);
		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (_missionMagicId);
		DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (difficulty, stageId, missionId);

		Model_Level model_Level = InstancePlayer.instance.model_User.model_level;
		bool isUnlock = model_Level.IsMissionUnlock (dataMission.magicId);
		if (isUnlock) 
		{
			_missionMagicId = dataMission.magicId;
			UpdateDetialUI (_missionMagicId);
			_campaignSpinePanel.UpdatetileUI (_missionMagicId);	

			InstancePlayer.instance.uiDataStatus.UpdateSelectedMission (_missionMagicId);
		}
		else
		{
			string msg = "关卡未解锁";
			UIHelper.ShowTextPromptPanel (this.gameObject, msg);
		}
	}

	void Update()
	{
		// 更新体力
		int energy = InstancePlayer.instance.model_User.model_Energy.energy;
		if(energy_label != null)
		{
			energy_label.text = energy + "";
		}
	}



}
