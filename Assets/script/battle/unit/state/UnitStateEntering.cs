using UnityEngine;
using System.Collections;

public class UnitStateEntering : IState {

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

	private float _delay;
	private const float DELAY_MAX = 3;
	
	private float _duration = DURATION_MAX;
	private const float DURATION_MAX = 6;

	public UnitStateEntering(Unit unit)
	{
		this.unit = unit;

		if (unit.slotIndex < 4) {
			_delay = RandomHelper.Range (0, DELAY_MAX * 0.6f);
		} else {
			_delay = RandomHelper.Range (DELAY_MAX * 0.4f, DELAY_MAX);
		}
	}
	
	
	public void Enter()
	{
		unit.aimmingControl.Resume ();
		unit.unitDriver.Resume (false);
	}
	
	public void Exit()
	{
	}
	
	public int Tick()
	{
		if(_result != RESULT.WORKING)
		{
			return (int)_result;
		}

		if (_delay > 0) {
			_delay -= TimeHelper.deltaTime;
			return (int)_result;
		}

		_duration -= TimeHelper.deltaTime;
		if (_duration <= 0) {
			_result = RESULT.DONE;
			return (int)_result;
		}
		

//		unit.targetSelect.Update ();

		Vector3 p = UnitHelper.GetOrientation (unit.body.transform);
		VectorHelper.ResizeVector (ref p, 10);
		p += unit.transform.position;
		unit.unitDriver.SetTargetPosition (p);

		unit.unitFire.Update ();

		unit.gridCorrect.HideCurrentGrid ();
		{
			unit.unitDriver.Update ();
			unit.aimmingControl.Update ();
		}
		unit.gridCorrect.ShowCurrentGrid ();
		unit.gridCorrect.Update ();

		return (int)_result;
	}
}
