using UnityEngine;
using System.Collections;

public class CameraSmoothMove {

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

	private Vector3 _moveSpeed;
	private float _moveTimeLeft;

	public CameraSmoothMove(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;

	}

	/*
	public void Init(float duration)
	{
		float x = _game.mapGrid.GetMapWidth () / 2;
		float z = _game.mapGrid.GetMapHeight () / 2;
		float y = 0;
		
		_cameraCenter = new Vector3(0, 0, z);
		
		Vector3 targetCenter = new Vector3 (x, y, z);
		_targetCameraCenter = targetCenter;

		_moveTimeLeft = duration;
		_moveSpeed = (_targetCameraCenter - _cameraCenter) / duration;
	}
	
	public void Init(Vector3 center, float duration)
	{
		float x = _game.mapGrid.GetMapWidth () / 2;
		float z = _game.mapGrid.GetMapHeight () / 2;
		float y = 0;
		
		_cameraCenter = center;
		
		Vector3 targetCenter = new Vector3 (x, y, z);
		_targetCameraCenter = targetCenter;
		
		_moveTimeLeft = duration;
		_moveSpeed = (_targetCameraCenter - _cameraCenter) / duration;
	}
	*/
	
	public void Init(Vector3 center, Vector3 target, float duration)
	{
		_cameraCenter = center;
		_targetCameraCenter = target;
		
		_moveTimeLeft = duration;
		_moveSpeed = (_targetCameraCenter - _cameraCenter) / duration;
	}
	
	public void Update () {
		if (_moveTimeLeft > 0) {
			Vector3 v = _moveSpeed * TimeHelper.deltaTime;
			_cameraCenter += v;
		}
		_moveTimeLeft -= TimeHelper.deltaTime;

		_mapCamera.cameraControl.FollowPoint (_cameraCenter);
		
	}
	
}
