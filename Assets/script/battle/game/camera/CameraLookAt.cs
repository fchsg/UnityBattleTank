using UnityEngine;
using System.Collections;

public class CameraLookAt {

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

	private float _speed = 1;

	private bool _isArrive = false;
	public bool isArrive
	{
		get { return _isArrive; }
	}

	
	public CameraLookAt(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;
		
	}
	
	public void Update () {

		Vector3 v = _targetCameraCenter - _cameraCenter;
		if (v.magnitude <= 1f) {
			_isArrive = true;
		} else {
			v *= _speed * TimeHelper.deltaTime;
			_cameraCenter += v;
		}

		_mapCamera.cameraControl.FollowPoint (_cameraCenter);
		
	}

	public void Init(Vector3 current, Vector3 target, float speed)
	{
		_cameraCenter = current;
		_targetCameraCenter = target;
		_speed = speed;
	}
	
	public void Init(Vector3 target, float speed)
	{
		_targetCameraCenter = target;
		_speed = speed;
	}

	
}
