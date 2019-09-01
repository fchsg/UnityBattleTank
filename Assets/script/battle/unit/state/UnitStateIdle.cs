using UnityEngine;
using System.Collections;

public class UnitStateIdle : IState {

	public enum RESULT
	{
		WORKING,
//		FIND_ENEMY,
	}
	
	private RESULT _result = RESULT.WORKING;
	public RESULT result
	{
		get { return _result; }
	}

	private Unit unit;

	private UnitTargetSelect.RESULT _targetSelect;
	public UnitTargetSelect.RESULT targetSelect
	{
		get { return _targetSelect; }
	}
	
	public UnitStateIdle(Unit unit)
	{
		this.unit = unit;
	}

	public void Enter()
	{
//		Trace.trace ("TANK " + unit.GetBaseInfo() + ", state >> idle", Trace.CHANNEL.FIGHTING);

		unit.aimmingControl.Stop ();
		unit.unitDriver.Stop (true);

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


//		_targetSelect = unit.targetSelect.Select ();
//		if (_targetSelect != null) {
//			_result = RESULT.FIND_ENEMY;
//			return (int)_result;
//		}

		unit.gridCorrect.Update ();


		return (int)_result;
	}

}
