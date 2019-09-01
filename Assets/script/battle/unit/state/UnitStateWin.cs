using UnityEngine;
using System.Collections;

public class UnitStateWin : IState {

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

	private float _delaySec = 2;

	public UnitStateWin(Unit unit)
	{
		this.unit = unit;
	}
	
	public void Enter()
	{
		//		Trace.trace ("TANK " + unit.GetBaseInfo() + ", state >> idle", Trace.CHANNEL.FIGHTING);
		
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

		_delaySec -= TimeHelper.deltaTime;
		if (_delaySec >= 0) {
			Vector3 p;
			if (unit.team == DataConfig.TEAM.MY) {
				p = new Vector3(100, 0, 0);
			} else {
				p = new Vector3(-100, 0, 0);
			}
			
			p += unit.transform.position;
			unit.unitDriver.SetTargetPosition (p);
			unit.aimmingControl.TrunToTarget (p);
			
			unit.gridCorrect.HideCurrentGrid ();
			{
				unit.unitDriver.Update ();
				unit.aimmingControl.Update ();
			}
			unit.gridCorrect.ShowCurrentGrid ();
			unit.gridCorrect.Update ();
		}


		
		return (int)_result;
	}

}
