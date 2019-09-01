using UnityEngine;
using System.Collections;

public class CampaignSpinePanel : MonoBehaviour {

	public CampaignMapControl mapControl = null;

	public CampaignPanel _campaignPanel;

	private MouseStatus _mouseStatus = new MouseStatus ();
	public MouseStatus mouseStatus
	{
		get { return _mouseStatus; }
	}

	private CampaignCamera _campaignCamera;

	public void Init (CampaignPanel campaignPanel) 
	{
		GameObject camera = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA);
		_campaignCamera = camera.GetComponent<CampaignCamera> ();
		_campaignPanel = campaignPanel;
	}

	public void UpdatePageUI(int missionMagicId)
	{
		FreeMapControl ();

		DataConfig.MISSION_DIFFICULTY difficulty = DataMission.GetDifficulty (missionMagicId);
		int stageId = DataMission.GetStageId (missionMagicId);
		int missionId = DataMission.GetMissionId (missionMagicId);

		// TODO 强制战役背景资源 ID 为1
//		stageId = 1;

		DataMissionGroup.DataCampaign campaign = DataManager.instance.dataMissionGroup.GetCampaign (difficulty, stageId);
		mapControl = new CampaignMapControl (campaign, missionId - 1);

		int tileIndex = missionId - 1;
		if (tileIndex >= 0) 
		{
			mapControl.SelectTile(tileIndex);
		}
	}

	public void UpdatetileUI(int missionMagicId)
	{
		int missionId = DataMission.GetMissionId (missionMagicId);

		int tileIndex = missionId - 1;
		if (tileIndex >= 0) 
		{
			mapControl.SelectTile(tileIndex);
		}
	}

	void Update ()
	{
		if (mapControl != null) 
		{
			mapControl.Update ();

			_mouseStatus.Update ();

			if (_mouseStatus.GetMouseJustDown (MouseStatus.KEY.LEFT) &&
				!UICamera.Raycast (Input.mousePosition)) 
			{
				Vector3 worldPoint;
				Vector3 mousePosition = _mouseStatus.GetMouseJustDownPos();
				bool raycast = _campaignCamera.ProjectScreenPointToPlane(out worldPoint, mousePosition);
				if(raycast)
				{
					int tileIndex = mapControl.DetectTileIndex(worldPoint);
					if(tileIndex >= 0)
					{
						if (_campaignPanel != null) 
						{
							_campaignPanel.OnSelectMission (tileIndex + 1);
						}
					}
				}
			}
		}

	}

	void OnDestroy() 
	{
		FreeMapControl ();
	}

	private void FreeMapControl()
	{
		if (mapControl != null) 
		{
			mapControl.Free();
			mapControl = null;
		}
	}

}


