using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionPanel : PanelBase
{
	//--------------------UI
	public const string MISSION_CELL_PATH = "profab/ui/mission/MissionCell";
	public UIGrid grid_Mission;

	public MissionDetialUI missionDetialUI;

	//--------------------Data
	public List<MissionCellUI> mission_cell_list = new List<MissionCellUI>();

	public int _missionMagicId;


	// Test
	void Start ()
	{
		//Init ();
	}

	public override void Init ()
	{
		base.Init ();
		animationType = PanelBase.AnimationType.ALPHA;

		GameObject Bottom_Container = UIHelper.FindChildInObject (gameObject, "Bottom_Container");

		UIHelper.AddBtnClick (Bottom_Container, "Btn_Formation", OnFormation);
		UIHelper.AddBtnClick (Bottom_Container, "Btn_Battle", OnBattle);
		UIHelper.AddBtnClick (Bottom_Container, "Btn_Clear", OnBattleClear);

		GameObject Top_Container = UIHelper.FindChildInObject (gameObject, "Top_Sprite");
		UIHelper.AddBtnClick (Top_Container, "Btn_Closed", OnClosed);

		missionDetialUI = gameObject.GetComponent<MissionDetialUI> ();

		grid_Mission = UIHelper.FindChildInObject (gameObject, "Grid_Mission").GetComponent<UIGrid> ();
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);

		_missionMagicId = (int) parameters [0];

		UpdateMissionList ();
	}

	public void OnClosed ()
	{
		// 关闭页面更新战役数据
		CampaignPanel campaignPanel = UIController.instance.GetPanel<CampaignPanel> (UICommon.UI_PANEL_CAMPAIGN);
		if (campaignPanel != null) 
		{
			campaignPanel._campaignSpinePanel.UpdatetileUI (_missionMagicId);		
			campaignPanel.UpdateDetialUI (_missionMagicId);
		}

		base.Delete ();
	}

	public void OnFormation ()
	{
		UIController.instance.CreatePanel (UICommon.UI_PANEL_FORMATION);
	}

	// 进入战斗
	public void OnBattle ()
	{
		InstancePlayer.instance.uiDataStatus.state = UIDataStatus.STATE.MISSION;
		BattleConnection.instance.StartFight (_missionMagicId);
	}

	// 扫荡
	public void OnBattleClear ()
	{
		PBConnect_multiFight.RESULT r = BattleConnection.instance.CheckClearBattle (this.gameObject, _missionMagicId);
		if (r == PBConnect_multiFight.RESULT.OK) 
		{
			UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION_ClEAR, _missionMagicId);
		}
	}

	//  更新Mission列表
	public void UpdateMissionList ()
	{
		List<Model_Mission> model_missions;

		Model_User model_User = InstancePlayer.instance.model_User;

		Model_Level model_level = InstancePlayer.instance.model_User.model_level;

		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty(_missionMagicId);
		int stageId = DataMission.GetStageId (_missionMagicId);
		Model_Level.Campaign campaign = model_level.GetCampaign (difficulty, stageId); 
		model_missions = campaign.list;	
			
		int n = model_missions.Count;
		int currentSelectId = 0;

		GameObject cell_prefab = Resources.Load (MISSION_CELL_PATH) as GameObject;
		for (int i = 0; i < n; ++i)
		{
			GameObject cell = NGUITools.AddChild (grid_Mission.gameObject, cell_prefab);
			grid_Mission.AddChild (cell.transform);
			cell.name = UIHelper.GetItemSuffix (i);

			Model_Mission model_Mission = model_missions [i];

			MissionCellUI cellUI = cell.GetComponent<MissionCellUI> ();
			cellUI.missionDetialUI = missionDetialUI;
			cellUI.missionPanel = this;

			cellUI.isUnlock = model_Mission.actived;

			//  当前关卡选中项
			if (_missionMagicId ==model_Mission.magicId)
			{
				cellUI.isSelected = true;
			}

			// Boss
			if (i == n - 1) 
			{
				cellUI.isBoss = true;
			}

			cellUI.UpdateUI (model_Mission);
			mission_cell_list.Add (cellUI);
		}

		grid_Mission.animateSmoothly = false;
		grid_Mission.repositionNow = true;
	}


	public void OnMissionCellUIClick(GameObject go)
	{
		if (go != null) 
		{
			MissionCellUI cellUI = go.gameObject.GetComponent<MissionCellUI> ();
			if (cellUI != null) 
			{
				Model_Level model_Level = InstancePlayer.instance.model_User.model_level;
				bool isUnlock = model_Level.IsMissionUnlock (cellUI.model_Mission.magicId);
				if (isUnlock) 
				{
					_missionMagicId = cellUI.model_Mission.magicId;
					missionDetialUI.UpdateUI (cellUI.model_Mission);
					cellUI.SetAllFrameNormal ();
					cellUI.UpdateFrame (true);

					InstancePlayer.instance.uiDataStatus.UpdateSelectedMission (_missionMagicId);
				}
				else
				{
					string msg = "关卡未解锁";
					UIHelper.ShowTextPromptPanel (this.gameObject, msg);
				}
			}
		}

	}

}
