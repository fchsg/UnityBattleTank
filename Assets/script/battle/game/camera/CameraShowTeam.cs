using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraShowTeam {

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

	public enum STATE
	{
		MOVE,
		STOP,
		OVER,
	}
	private STATE _state = STATE.STOP;
	public STATE state
	{
		get { return _state; }
	}

	private float _stopTimeLeft = STOP_DURATION;
	private const float STOP_DURATION = 1;

	private List<Vector3> _targetsPosition;
	private int _targetIndex = 0;


	public CameraShowTeam(MapCamera mapCamera)
	{
		_game = BattleGame.instance;
		_mapCamera = mapCamera;
		
	}

	public void Update () {

		if (_targetsPosition == null) {
			Init ();
		}

		switch (state) {
		case STATE.MOVE:
			UpdateMove();
			break;
		case STATE.STOP:
			UpdateStop();
			break;
		case STATE.OVER:
			break;
		}

		_mapCamera.cameraControl.FollowPoint (_cameraCenter);

	}

	private void Init()
	{
		_targetsPosition = new List<Vector3> ();
		
		List<List<Unit>> unitsWithTeam = _game.unitGroup.unitsWithTeam;
		foreach (List<Unit> units in unitsWithTeam) {
			Vector3 c = GetUnitsCenter(units);
			_targetsPosition.Add(c);
		}
		_targetsPosition.Add (_targetsPosition [0]);
		_targetsPosition.RemoveAt (0);
		
		//
		_targetIndex = 0;
		_targetCameraCenter = _targetsPosition [_targetIndex];
		_cameraCenter = _targetCameraCenter;
		_state = STATE.STOP;
	}
	
	private Vector3 GetUnitsCenter(List<Unit> units)
	{
		Vector3 c = new Vector3 ();
		foreach (Unit unit in units) {
			c += unit.transform.position;
		}
		c /= units.Count;

		c.z = _game.mapGrid.GetMapHeight () / 2;
		return c;
	}
	

	private void UpdateMove()
	{
		Vector3 v = _targetCameraCenter - _cameraCenter;
		if (v.magnitude <= 1f) {
			_state = STATE.STOP;
		} else {
			v *= 2f * TimeHelper.deltaTime;
			_cameraCenter += v;
		}

	}

	private void UpdateStop()
	{
		_stopTimeLeft -= TimeHelper.deltaTime;
		if(_stopTimeLeft < 0)
		{
			_stopTimeLeft = STOP_DURATION;
			
			_targetIndex++;
			if(_targetIndex >= _targetsPosition.Count)
			{
				_state = STATE.OVER;
			}
			else
			{
				_targetCameraCenter = _targetsPosition [_targetIndex];
				_state = STATE.MOVE;
			}
		}

	}
	

}
