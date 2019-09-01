using UnityEngine;
using System.Collections;

public class MissionCellUI : MonoBehaviour {
	
// data-----------------------------
	public bool isUnlock = false;  //  关卡解锁
	public bool isBoss = false;     //   是否是关卡Boss
	public bool isSelected = false;  //   是否值当前选中项

	public Model_Mission model_Mission;

// UI --------------------------------
	public MissionPanel missionPanel;
	public MissionDetialUI missionDetialUI;


	public UILabel name_Label;
	public UISprite mission_Sprite;
	public UISprite boss_Sprite;

	public UISprite[] stars_Sprite = new UISprite[3];     // 星级图标

	public UISprite frames_Normal;   //  选中框
	public UISprite frames_Selected;  //  选中框

	public UIButton button;

    void Awake()
	{
		mission_Sprite = UIHelper.FindChildInObject (gameObject, "Mission_Sprite").GetComponent<UISprite> ();
		name_Label = UIHelper.FindChildInObject (gameObject, "Name_Label").GetComponent<UILabel> ();
		boss_Sprite = UIHelper.FindChildInObject (gameObject, "Boss_Sprite").GetComponent<UISprite> ();

		// 选中框
		frames_Normal = UIHelper.FindChildInObject (gameObject, "Frame_0").GetComponent<UISprite> ();
		frames_Selected = UIHelper.FindChildInObject (gameObject, "Frame_1").GetComponent<UISprite> ();

		 // 星级
		for (int i = 0; i < 3; ++i) 
		{
			stars_Sprite[i] = UIHelper.FindChildInObject (gameObject, "Star_" + i).GetComponent<UISprite> ();
			stars_Sprite [i].SetColorGrey (true);
		}

		UpdateFrame (false);
		UpdateBoss (false);

		button = gameObject.GetComponent<UIButton> ();
		UIHelper.AddBtnClick (button, OnSelectMission);
	}
		
	public void UpdateUI(Model_Mission model_Mission)
	{
		if (model_Mission != null) 
		{
			this.model_Mission = model_Mission;

			mission_Sprite.spriteName = UICommon.MISSION_ICON_PATH + model_Mission.magicId;
			DataMission dataMission = DataManager.instance.dataMissionGroup.GetMission (model_Mission.magicId);
			name_Label.text = dataMission.name;
		}

		// 更新当前选中
		if (isSelected) 
		{
			UpdateFrame (true);
			missionDetialUI.UpdateUI (model_Mission);

			InstancePlayer.instance.uiDataStatus.UpdateSelectedMission (model_Mission.magicId);
		}

		// 更新是否解锁
		UpdateUnlock(isUnlock);
		UpdateBoss (isBoss);

		// 更新星级
		UpdateStar(model_Mission.starCount);
	}

	//  按钮点击
	public void OnSelectMission (GameObject go)
	{
		missionPanel.OnMissionCellUIClick (go);
	}

	//  选中框
	public void UpdateFrame(bool isSelected)
	{
		frames_Normal.gameObject.SetActive (!isSelected);
		frames_Selected.gameObject.SetActive (isSelected);
	}

	// 更新是否解锁
	public void UpdateUnlock(bool isUnlock)
	{
		name_Label.SetColorGrey (!isUnlock);  // 名称
		boss_Sprite.SetColorGrey (!isUnlock); // BOSS
		mission_Sprite.SetColorGrey (!isUnlock); // mission Icon
	}

	//  设置所有选中框Normal
	public void SetAllFrameNormal()
	{
		if(missionPanel != null)
		{
			foreach(MissionCellUI cellUI in missionPanel.mission_cell_list)
			{
				cellUI.UpdateFrame (false);
			}
		}
	}

	public void UpdateBoss(bool isVisible)
	{
		boss_Sprite.gameObject.SetActive (isVisible);
	}


	public void UpdateStar(int starCount)
	{
		for (int i = 0; i < starCount; ++i) 
		{
			stars_Sprite [i].SetColorGrey(false);
		}
	}

}
