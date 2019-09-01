using UnityEngine;
using System.Collections;

public class GameTestCampaign : MonoBehaviour {

	public CampaignMapControl mapControl = null;

	private MouseStatus _mouseStatus = new MouseStatus ();
	public MouseStatus mouseStatus
	{
		get { return _mouseStatus; }
	}

	private CampaignCamera _campaignCamera;


	// Use this for initialization
	void Start () {
		DataManager.instance.InitData ();

		DataConfig.MISSION_DIFFICULTY difficulty = DataConfig.MISSION_DIFFICULTY.NORMAL;
		int stageId = 1;
		DataMissionGroup.DataCampaign campaign = DataManager.instance.dataMissionGroup.GetCampaign (difficulty, stageId);
		mapControl = new CampaignMapControl (campaign, 0);

		GameObject camera = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA);
		_campaignCamera = camera.GetComponent<CampaignCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		mapControl.Update ();
//		return;

		_mouseStatus.Update ();

		if (_mouseStatus.GetMouseJustDown (MouseStatus.KEY.LEFT) &&
		    !UICamera.Raycast (Input.mousePosition)) {
			Vector3 worldPoint;
			Vector3 mousePosition = _mouseStatus.GetMouseJustDownPos();
			bool raycast = _campaignCamera.ProjectScreenPointToPlane(out worldPoint, mousePosition);
			if(raycast)
			{
				int tileIndex = mapControl.DetectTileIndex(worldPoint);
				if(tileIndex >= 0)
				{
					mapControl.SelectTile(tileIndex);
				}
			}
		}
	}

	void OnDestroy() {
		if (mapControl != null) {
			mapControl.Free();
			mapControl = null;
		}
	}

}
