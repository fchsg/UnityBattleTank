using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraAutoScale {

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

	private float _targetCameraScale = 1;
	private float _cameraScale = 1;
	public float cameraScale
	{
		get { return _cameraScale; }
	}

	public CameraAutoScale(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;
	}
	
	public void Update () {
		BattleGameHelper.UnitLayout layout = BattleGameHelper.CalcAllAliveTanksLayout ();
		if (layout != null) {
			UpdateCamera(layout);
		}

		//
		Vector3 v = _targetCameraCenter - _cameraCenter;
		v *= 0.5f;
		_cameraCenter += v;

		_mapCamera.cameraControl.FollowPoint (_cameraCenter);

		//
		float d = _targetCameraScale - _cameraScale;
		d *= 0.5f;
		_cameraScale += d;

		float expectHeight2 = _mapCamera.cameraControl.sizeControl.expectScreenHeight2 * _cameraScale;
		_mapCamera.cameraControl.camera.orthographicSize = expectHeight2;

	}
	
	private void UpdateCamera(BattleGameHelper.UnitLayout layout)
	{

		Vector3 targetCenter = (layout.min + layout.max) / 2;
		_targetCameraCenter = targetCenter;

		float EDGE2 = MapGrid.GRID_SIZE;
		float width = layout.max.x - layout.min.x + EDGE2;

		float expectWidth2 = _mapCamera.cameraControl.sizeControl.expectScreenWidth2;
		float scale = width / 2 / expectWidth2;
		_targetCameraScale = Mathf.Max (1, scale);
	}
	


}
