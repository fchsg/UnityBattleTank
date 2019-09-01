using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CampaignMapControl {

	private DataMissionGroup.DataCampaign _dataCampaign;
	private Model_Level.Campaign _campaign;
	private int _selectTileIndex = -1;

	private SkeletonAnimation _skeletonAnim;
	public SkeletonAnimation skeletonAnim
	{
		get { return _skeletonAnim; }
	}


	private List<GameObject> _skeletonAnimArrows = new List<GameObject> ();
	private GameObject _skeletonStar = null;

	private GameObject _boundingBox = null;
	private LineRenderer _boundingBoxLineRender = null;


	private GameObject _tankIconRoot;
	private GameObject[] _tankIcons = new GameObject[DataConfig.FORMATION_TOTAL_SLOT];

	public const float TANK_SIZE_SCALE = 3f;


	private const bool SHOW_BOUNDING = AppConfig.DEBUGGING && true;

	private const bool USE_TEST_MAP = AppConfig.DEBUGGING && true;


	public CampaignMapControl(DataMissionGroup.DataCampaign dataCampaign, int startTileIndex = 0)
	{
		_dataCampaign = dataCampaign;
		_campaign = InstancePlayer.instance.model_User.model_level.GetCampaign (dataCampaign.difficulty, dataCampaign.stageId);

		LoadSpine ();
		UpdateTileState ();

		SelectTile (startTileIndex);

	}

	public void Free()
	{
		_dataCampaign = null;
		_campaign = null;
		_selectTileIndex = -1;

		if (_skeletonAnim != null) {
			MonoBehaviour.DestroyImmediate(_skeletonAnim.gameObject);
			_skeletonAnim = null;
		}

		if (_boundingBox != null) {
			MonoBehaviour.DestroyImmediate(_boundingBox.gameObject);
			_boundingBox = null;

			_boundingBoxLineRender = null;
		}

		FreeArrow ();

		if (_tankIconRoot != null) {
			MonoBehaviour.DestroyImmediate (_tankIconRoot);
			_tankIconRoot = null;
		}

		FreeStar ();

	}

	private void FreeArrow()
	{
		foreach (GameObject anim in _skeletonAnimArrows) {
			MonoBehaviour.DestroyImmediate(anim);
		}
		_skeletonAnimArrows.Clear ();
	}

	private void FreeStar()
	{
		if (_skeletonStar != null) {
			MonoBehaviour.DestroyImmediate(_skeletonStar);
			_skeletonStar = null;
		}
	}

	private void LoadSpine()
	{
		string spineName = AppConfig.FOLDER_PROFAB_CAMPAIGN + _dataCampaign.stageId;
		if (USE_TEST_MAP) {
			spineName = AppConfig.FOLDER_PROFAB_CAMPAIGN + "1";
		}

		GameObject map = ResourceHelper.Load (spineName);

		_skeletonAnim = map.GetComponent<SkeletonAnimation> ();

	}

	public void UpdateTileState()
	{
		foreach (Spine.Slot slot in _skeletonAnim.skeleton.slots) {
			string boneName = slot.Bone.Data.Name;
			if(IsTileName(boneName))
			{
				bool active = IsTileActive(boneName);
				if(active)
				{
					slot.R = 1;
					slot.G = 1;
					slot.B = 1;
				}
				else
				{
					slot.R = 0.5f;
					slot.G = 0.5f;
					slot.B = 0.5f;
//					slot.R = 0.3f;
//					slot.G = 0.59f;
//					slot.B = 0.11f;
				}
			}
		}

	}

	private Vector3 CreateTankIcon()
	{
		if (_tankIconRoot == null) {
			_tankIconRoot = new GameObject ("TankIconRoot");
		}

		_tankIconRoot.transform.position = new Vector3 ();
		Vector3 tileCenter = GetTileCenter (_selectTileIndex);

		float minX = int.MaxValue;
		float minY = int.MaxValue;
		float maxX = int.MinValue;
		float maxY = int.MinValue;

		DataMission mission = _dataCampaign.missions [_selectTileIndex];
		InstanceBattle battle = new InstanceBattle ();
		battle.ImportFromLevel (mission);
		InstanceTeam team = battle.enemyTeams [0];
		for (int i = 0; i < team.units.Length; ++i) {
			int unitId = team.units [i].unitId;
			if (unitId > 0) {
				DataUnit unit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
				
//				GameObject tank = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_TANK + "tankIconPrimitive");
//				tank.transform.parent = _tankIconRoot.transform;
				
				Vector3 pos = tileCenter;
				pos.x += ((i % 3) - 1) * 27 * TANK_SIZE_SCALE;
				pos.y += ((i / 3) - 1) * 18 * TANK_SIZE_SCALE;
				pos.z = -1;
//				tank.GetComponent<CampaignUnitIcon> ().Init (pos, unit, -60, -60);

				GameObject tank = TankIconSpineAttach.Create (unit, pos, TANK_SIZE_SCALE, -60, -60);
				tank.transform.parent = _tankIconRoot.transform;

				_tankIcons [i] = tank;

				minX = Math.Min (minX, pos.x);
				minY = Math.Min (minY, pos.y);
				maxX = Math.Max (maxX, pos.x);
				maxY = Math.Max (maxY, pos.y);

			}
		}

		Vector3 translate = FixIconPosition (minX, minY, maxX, maxY);
		return translate;

	}

	private Vector3 FixIconPosition(float minX, float minY, float maxX, float maxY)
	{
//		GameObject camera = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA);
//		CampaignCamera campaignCamera = camera.GetComponent<CampaignCamera>();
//		Vector3 corners = campaignCamera.camera.GetWorldCorners ();

		float EDGE = TANK_SIZE_SCALE * 20;
		float X0 = EDGE;
		float X1 = AppConfig.DESIGN_WIDTH - EDGE;
		float Y0 = EDGE;
		float Y1 = AppConfig.DESIGN_HEIGHT - EDGE;

		float fixX = 0;
		float fixY = 0;

		if (minX < X0) {
			fixX = X0 - minX;
		} else if (maxX > X1) {
			fixX = X1 - maxX;
		}

		if (minY < Y0) {
			fixY = Y0 - minY;
		} else if (maxY > Y1) {
			fixY = Y1 - maxY;
		}

		Vector3 translate = new Vector3 (fixX, fixY, 0);
		_tankIconRoot.transform.Translate (translate);
		return translate;
	}

	private void FreeTankIcon()
	{
		for(int i = 0; i < _tankIcons.Length; ++i)
		{
			GameObject tank = _tankIcons[i];
			_tankIcons[i] = null;
			
			if(tank != null)
			{
				MonoBehaviour.DestroyImmediate(tank);
			}
		}

	}
	
	private bool IsTileName(string s)
	{
		if (s.Length != 1) {
			return false;
		}

		char c = s [0];
		return c >= 'a' && c <= 'z';
	}

	private bool IsTileActive(string tileName)
	{
		int index = GetTileIndex (tileName);

		if (_campaign == null) {
			return index == 0;
		} else {
			if (index >= _campaign.list.Count) {
				return false;
			} else {
				Model_Mission mission = _campaign.list[index];
				return (mission.actived);
			}

		}
	}
	
	public void SelectTile(int index)
	{
		if (index >= 0) {

//			Trace.trace("select tile " + index, Trace.CHANNEL.UI);

			if (index != _selectTileIndex) {
				FreeTankIcon();
				_selectTileIndex = index;

				Vector3 translate = CreateTankIcon();
				PlayTilePopupAnim();
				ShowAttackArrow();
				ShowStar (translate);

			}
		}

	}

	private void PlayTilePopupAnim()
	{
		string name = GetTileName(_selectTileIndex);
		_skeletonAnim.state.SetAnimation (0, name, false);
	}

	public void ShowAttackArrow()
	{
		FreeArrow ();

		DataMission dataMission = _dataCampaign.missions [_selectTileIndex];
		for (int i = 0; i < DataMission.ARROW_COUNT_MAX; ++i) {
			DataConfig.CAMPAIGN_ARROW arrow = dataMission.arrows[i];
			float arrowRotation = dataMission.arrowsRotation[i];
//			arrowRotation = 180;

			if(arrow != DataConfig.CAMPAIGN_ARROW.UNKNOWN)
			{
				string spineName = GetArrowFileName(arrow);
				GameObject spineArrow = ResourceHelper.Load (spineName);
				_skeletonAnimArrows.Add(spineArrow);

				Spine.Slot boundingSlot = GetBoundingSlot(_selectTileIndex);
				Vector3 center = SpineHelper.CalcTileArrowOffset(boundingSlot, _skeletonAnim.transform, arrowRotation);
				spineArrow.transform.position = center + new Vector3(0, 0, -1);
				spineArrow.transform.Rotate(0, 0, arrowRotation);
			}

		}

	}


	public void ShowStar(Vector3 translate)
	{
		FreeStar ();


		DataMission dataMission = _dataCampaign.missions [_selectTileIndex];
		Model_Mission modelMission = InstancePlayer.instance.model_User.model_level.GetMission (dataMission.magicId);

		string animName = "no_star";
		if (modelMission.starCount == 1) {
			animName = "01";
		}
		if (modelMission.starCount == 2) {
			animName = "02";
		}
		if (modelMission.starCount == 3) {
			animName = "03";
		}


		Vector3 tileCenter = GetTileCenter (_selectTileIndex);
		tileCenter.y += 70;
		tileCenter.z = -1;
		tileCenter += translate;

		string spineName = AppConfig.FOLDER_PROFAB_CAMPAIGN + "Star";
		_skeletonStar = ResourceHelper.Load (spineName);
		_skeletonStar.transform.position = tileCenter;

		SkeletonAnimation anim = _skeletonStar.GetComponentInChildren<SkeletonAnimation> ();
		anim.state.SetAnimation(0, animName, false);

	}

	private string GetArrowFileName(DataConfig.CAMPAIGN_ARROW arrow)
	{
		string spineName = AppConfig.FOLDER_PROFAB_CAMPAIGN;

		switch (arrow) {
		case DataConfig.CAMPAIGN_ARROW.A_UPWARD:
			spineName += "ArrowIn";
			break;
		case DataConfig.CAMPAIGN_ARROW.B_UPWARD_TURN_LEFT:
			spineName += "ArrowLeft";
			break;
		case DataConfig.CAMPAIGN_ARROW.C_UPWARD_TURN_RIGHT:
			spineName += "ArrowRight";
			break;
		case DataConfig.CAMPAIGN_ARROW.D_UPWARD_BIG:
			spineName += "ArrowIn2";
			break;
		case DataConfig.CAMPAIGN_ARROW.E_UPWARD_TURN_LEFT_BIG:
			spineName += "ArrowLeft2";
			break;
		case DataConfig.CAMPAIGN_ARROW.F_UPWARD_TURN_RIGHT_BIG:
			spineName += "ArrowRight2";
			break;
		}

		return spineName;
	}

	public void Update()
	{
		if (SHOW_BOUNDING) {
			ShowBoundingBox();
		}
		
	}

	private void ShowBoundingBox()
	{
		if (_boundingBox == null) {
			_boundingBox = ResourceHelper.Load(AppConfig.FOLDER_PROFAB_CAMPAIGN + "CampaignBoundingBox");
			_boundingBoxLineRender = _boundingBox.GetComponent<LineRenderer>();
		}
		
		if (_selectTileIndex < 0) {
			_boundingBoxLineRender.SetVertexCount(0);
			return;
		}
		
		Spine.Slot boundingSlot = GetBoundingSlot(_selectTileIndex);
		SpineHelper.ShowBounding (boundingSlot, _skeletonAnim.transform, _boundingBoxLineRender);

		/*
		Spine.BoundingBoxAttachment bounding = slot.Attachment as Spine.BoundingBoxAttachment;
		float[] vertices = bounding.Vertices;
		
		int n = vertices.Length / 2;
		_boundingBoxLineRender.SetVertexCount (n);
		for(int i = 0; i < n; ++i)
		{
			float localX = vertices[i * 2 + 0];
			float localY = vertices[i * 2 + 1];
			float worldX;
			float worldY;
			bone.localToWorld(localX, localY, out worldX, out worldY);

			Vector3 p = new Vector3(worldX, worldY, 0);
			p = _skeletonAnim.transform.TransformPoint(p);

			_boundingBoxLineRender.SetPosition(i, new Vector3(p.x, p.y, -1));
		}
		*/

	}
	

	// ==========================================================================
	// helper

	private static string GetTileName(int index)
	{
		System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
		byte asc = (byte)('a' + index);
		string name = asciiEncoding.GetString (new byte[]{asc});
		return name;
	}
	
	private static int GetTileIndex(string name)
	{
		char c = name [0];
		int index = c - 'a';
		return index;
	}
	

	public Vector3 GetTileCenter(int index)
	{
		if (index >= 0) {
			string name = GetTileName(index);
			Spine.Bone bone = _skeletonAnim.Skeleton.FindBone(name);

			Vector3 center = SpineHelper.GetBoneCenter(_skeletonAnim.transform, bone);
			return center;
		}
		
		return Vector3.zero;
	}
	
	public Vector3 GetSelectTileCenter()
	{
		return GetTileCenter(_selectTileIndex);
	}

	public Spine.Slot GetBoundingSlot(int index)
	{
		string name = GetTileName (index);
		Spine.Slot boundingSlot = _skeletonAnim.Skeleton.FindSlot ("_" + name);
		return boundingSlot;
	}
	
	// ==========================================================================
	// collision
	
	public int DetectTileIndex(Vector3 worldPoint)
	{
		foreach (Spine.Slot slot in _skeletonAnim.skeleton.slots) {
			string boneName = slot.Bone.Data.Name;
			if(!IsTileName(boneName))
			{
				continue;
			}

			bool inside = false;

			Spine.Slot boundingSlot = _skeletonAnim.Skeleton.FindSlot ("_" + boneName);
			if (boundingSlot != null) {
				inside = SpineHelper.IsPointInsideBounding(worldPoint, boundingSlot, _skeletonAnim.transform);
			}

			/*
			Spine.BoundingBoxAttachment bounding = boundingSlot.Attachment as Spine.BoundingBoxAttachment;
			float[] vertices = (float[])bounding.Vertices.Clone();
			
			int n = vertices.Length / 2;
			for(int i = 0; i < n; ++i)
			{
				float localX = vertices[i * 2 + 0];
				float localY = vertices[i * 2 + 1];
				float worldX;
				float worldY;
				bone.localToWorld(localX, localY, out worldX, out worldY);
				
				Vector3 p = new Vector3(worldX, worldY, 0);
				p = _skeletonAnim.transform.TransformPoint(p);
				
				vertices[i * 2 + 0] = p.x;
				vertices[i * 2 + 1] = p.y;
			}

			bool inside = GeometryHelper.IsPointInsidePolygon(mx, my, vertices);
			*/

			if(inside)
			{
				int index = GetTileIndex(boneName);
				return index;
			}
		}

		return -1;
	}
	

}
