using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOffine : MonoBehaviour {

	public static bool START_OF_OFFLINE = false;

	void Awake()
	{
		DataManager.instance.InitData ();
	}

	// Use this for initialization
	void Start() {

		if (!GameOffine.START_OF_OFFLINE) 
		{
			DataManager.instance.InitData ();

			Model_User model_User = InstancePlayer.instance.model_User;

			// 创建mission 数据
			DataMission[]	allMissions = DataManager.instance.dataMissionGroup.GetAllMissions();
			List<SlgPB.Mission> list = new List<SlgPB.Mission> ();
			foreach (DataMission dataMission in allMissions) 
			{
				SlgPB.Mission mission = new SlgPB.Mission ();
				mission.missonId =dataMission.magicId;
				mission.remainFightNum = 50;
				mission.star = 7;
				list.Add (mission);
			}
			model_User.model_level.Init (list);

			//  创建user数据
			DataPersistent.Init (0xcdcdcd);
		}

		START_OF_OFFLINE = true;

		int magicId = InstancePlayer.instance.missionMagicId;

		UIDataStatus.STATE page = InstancePlayer.instance.uiDataStatus.state;
		switch (page) 
		{
		case UIDataStatus.STATE.CAMPAIGN:
			UIController.instance.CreatePanel (UICommon.UI_PANEL_CAMPAIGN, magicId);
			break;
		case UIDataStatus.STATE.MISSION:
			UIController.instance.CreatePanel (UICommon.UI_PANEL_CAMPAIGN, magicId);
			UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION, magicId);
			break;
		}

	}

	void Update () {
	
	}

	void OnDestroy()
	{

	}
}
