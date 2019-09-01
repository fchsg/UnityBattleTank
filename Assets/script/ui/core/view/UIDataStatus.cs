using UnityEngine;
using System.Collections;

public class UIDataStatus {

	public enum STATE
	{
		CAMPAIGN,       // 战役
		MISSION,           // 关卡详情

		BUILDING_UPGRADE,  // 建筑升级
		HERO,  					 // 军官
		TANK_REPAIR,  			// 坦克维修
		TECHNOLOGY, 			// 研发科技
		PVP, 			// PVP

	}

	private STATE _state = STATE.CAMPAIGN;
	public STATE state
	{
		set { _state = value; }
		get{ return _state; }
	}


	// mission----------------------------------------------

	public class MissionSelection
	{
		public DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.NORMAL;
	}

	private MissionSelection _missionSelection = new MissionSelection ();

	public void UpdateSelectedMission(int selectedMagicId)
	{
			_missionSelection.difficulty  = DataMission.GetDifficulty(selectedMagicId);
	}

	public int  GetMissionMagicId()
	{
		int magicId = InstancePlayer.instance.model_User.model_level.GetLastUnlockMissionMagicId (_missionSelection.difficulty);
		return magicId;
	}


}
