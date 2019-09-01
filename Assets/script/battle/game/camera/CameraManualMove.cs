using UnityEngine;
using System.Collections;

public class CameraManualMove {

	
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
		set { _cameraCenter = value; }
		get { return _cameraCenter; }
	}
	
	private Vector3 _moveSpeed;

	private float _moveTimeLeft;

	public CameraManualMove(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;
		
	}

	public void UpdateMovePos(Vector3 targetPos)
	{
		float expectWidth = _mapCamera.cameraControl.sizeControl.expectScreenWidth2 / 2.0f;
		Vector3 tmpCameraCenter = _cameraCenter - new Vector3(targetPos.x / expectWidth, 0, 0);
		_cameraCenter = _mapCamera.cameraControl.LimitCameraCenter(tmpCameraCenter);
		_mapCamera.cameraControl.FollowPoint (_cameraCenter);
	}

	public void UpdateEasingPos(float dragTime, Vector3 dragDistance)
	{
		_moveTimeLeft = dragTime;
		float expectWidth = _mapCamera.cameraControl.sizeControl.expectScreenWidth2 / 2.0f;
		_moveSpeed = dragDistance / expectWidth / _moveTimeLeft;
		
		if(dragTime <= 0.1f)
		{
			_moveTimeLeft *= 1.0f;
			_moveSpeed *= 1.0f;
		}
		else if(dragTime <= 0.2f)
		{
			_moveTimeLeft *= 0.9f;
			_moveSpeed *= 0.9f;
		}
		else if(dragTime <= 0.3f)
		{
			_moveTimeLeft *= 0.8f;
			_moveSpeed *= 0.8f;
		}
		else 
		{
			_moveTimeLeft *= 0.7f;
			_moveSpeed *= 0.7f;
		}
	}

	public void Update () {
		if (_moveTimeLeft > 0) 
		{
			Vector3 v = _moveSpeed * TimeHelper.unscaledDeltaTime;
			_cameraCenter += v;
			_cameraCenter = _mapCamera.cameraControl.LimitCameraCenter(_cameraCenter);
		}
		_moveTimeLeft -= TimeHelper.unscaledDeltaTime;
		
		_mapCamera.cameraControl.FollowPoint (_cameraCenter);
		
	}

	public void ResetMove()
	{
		_moveSpeed = Vector3.zero;
		_moveTimeLeft = 0.0f;
	}


}
