using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraAutoFollow {

	private BattleGame _game;
	private MapCamera _mapCamera;

	private Vector3 _targetCameraCenter = new Vector3();
	public Vector3 targetCameraCenter
	{
		get { return _targetCameraCenter; }
	}

	private Vector3 _cameraCenter = new Vector3 ();
	public Vector3 cameraCenter
	{
		get { return _cameraCenter; }
	}

	public CameraAutoFollow(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;
	}

	public void Update () {
		BattleGameHelper.UnitLayout layout = BattleGameHelper.CalcMyAliveTanksLayout ();
		if (layout != null) {
			UpdateCamera(layout);
		}

		//
		Vector3 v = _targetCameraCenter - _cameraCenter;
		v *= 0.5f;
		_cameraCenter += v;

		_mapCamera.cameraControl.FollowPoint (_cameraCenter);

	}

	private void UpdateCamera(BattleGameHelper.UnitLayout layout)
	{
		float x = layout.max.x + MapGrid.GRID_SIZE * 2;
		float z = _game.mapGrid.GetMapHeight () / 2;
		float y = 0;

		Vector3 targetCenter = new Vector3 (x, y, z);
		targetCenter.x = Mathf.Max (targetCenter.x, _targetCameraCenter.x);

		_targetCameraCenter = targetCenter;
	}




}
