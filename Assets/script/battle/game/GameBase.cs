using UnityEngine;
using System.Collections;

public class GameBase : MonoBehaviour {

	private static GameBase _instance;
	public static GameBase instance
	{
		get { return _instance; }
	}

	private EventController _eventController = new EventController ();
	public EventController eventController
	{
		get { return _eventController; }
	}

	void Start()
	{
		Assert.assert (_instance == null);
		_instance = this;

		// 进入游戏屏幕常亮
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		DataManager.instance.InitData ();

		if (!InstancePlayer.instance.model_User.isLogin) // 登陆时进入
		{   
			UIController.instance.CreateScene (UICommon.UI_LOGIN_SCENE);
		}
		else  //  从战斗页面退出时进入
		{
			int missionMagicId = InstancePlayer.instance.uiDataStatus.GetMissionMagicId ();

			UIDataStatus.STATE page = InstancePlayer.instance.uiDataStatus.state;
			switch (page) 
			{
			case UIDataStatus.STATE.CAMPAIGN:
				UIController.instance.CreatePanel (UICommon.UI_PANEL_CAMPAIGN, missionMagicId);
				break;

			case UIDataStatus.STATE.MISSION:
				UIController.instance.CreatePanel (UICommon.UI_PANEL_CAMPAIGN, missionMagicId);
				UIController.instance.CreatePanel (UICommon.UI_PANEL_MISSION, missionMagicId);
				break;

			case UIDataStatus.STATE.HERO:
				UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
				UIController.instance.CreatePanel (UICommon.UI_PANEL_HERO);
				break;

			case UIDataStatus.STATE.BUILDING_UPGRADE: 
				UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
				break;

			case UIDataStatus.STATE.TANK_REPAIR:  
				UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
				UIController.instance.CreatePanel (UICommon.UI_PANEL_REPAIRFACTORY);
				break;

			case UIDataStatus.STATE.TECHNOLOGY: 
				UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
				break;

			case UIDataStatus.STATE.PVP:
				UIController.instance.CreateScene (UICommon.UI_MAIN_SCENE);
				UIController.instance.CreatePanel (UICommon.UI_PANEL_PVP);
				break;
			}
		}

		Model_Helper.ResumeAllTimer ();
	}

	void Update()
	{
//		msgBuildingLevelup parameter = new msgBuildingLevelup (0, 0);
//		GameEvent levelUpEvent = new GameEvent (GameEvent.EVENT.BASE_BUILDING_LEVELUP, parameter);
//		GameBase.instance.eventController.Dispatch (levelUpEvent);

	}
	
	void OnDestroy()
	{
		_instance = null;

		Model_Helper.PauseAllTimer ();
	}
	
}
