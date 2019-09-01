using UnityEngine;
using System.Collections;

public class UnitStateFire : IState {

	public enum RESULT
	{
		WORKING,
		DONE,
	}
	
	private RESULT _result = RESULT.WORKING;
	public RESULT result
	{
		get { return _result; }
	}
	
	private Unit unit;

	private float _shootInterval;
	private long _enterTimestamp;

	private TankSpineAttach _spineAttach;

	private CameraControl _cameraControl;

	private GameObject _fireObject = null;

	public UnitStateFire(Unit unit)
	{
		this.unit = unit;

		_shootInterval = unit.unit.dataUnit.fireInterval * 1000;
		_enterTimestamp = TimeHelper.GetCurrentTimestampScaled ();

		_spineAttach = unit.GetComponent<TankSpineAttach> ();

		_cameraControl = GameObject.FindGameObjectWithTag (AppConfig.TAB_MAIN_CAMERA).GetComponent<CameraControl> ();
	}
	
	
	public void Enter()
	{
//		_spineAttach.ChangeAnimation (SpineAttach.ANIMATION.STAND, false);
		_spineAttach.ChangeAnimation (TankSpineAttach.ANIMATION.FIRE, false);

		CreateFireEffect ();
		
	}
	
	private void CreateFireEffect()
	{
		if (_fireObject == null) {
			Vector3 pPoint;
			bool result = unit.GetFireMousePosition (out pPoint);
			if (result) {
				if(unit.unit.dataUnit.bulletType == DataConfig.BULLET_TYPE.GUN)
				{
					_fireObject = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Gun_Fire");
				}
				else
				{
					_fireObject = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT + "Tank_Fire");
				}
				
				_fireObject.transform.position = pPoint;
				_fireObject.transform.rotation = unit.launcher.transform.rotation;
			}
		}
		
	}
	
	public void Exit()
	{
		_spineAttach.ChangeAnimation (TankSpineAttach.ANIMATION.STAND, true);
	}
	
	public int Tick()
	{
		if(_result != RESULT.WORKING)
		{
			return (int)_result;
		}


		if (_spineAttach.IsAnimFinished()) {
			_result = RESULT.DONE;
			return (int)_result;
		}

		long ts = TimeHelper.GetCurrentTimestampScaled ();
		if (ts - _enterTimestamp >= _shootInterval) {
			_result = RESULT.DONE;
			return (int)_result;
		}
		
		return (int)_result;
	}

}
