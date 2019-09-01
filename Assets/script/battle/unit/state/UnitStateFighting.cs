using UnityEngine;
using System.Collections;

public class UnitStateFighting : IState {
	
	public enum RESULT
	{
		WORKING,
	}
	
	private RESULT _result = RESULT.WORKING;
	public RESULT result
	{
		get { return _result; }
	}
	
	private Unit unit;

	public UnitStateFighting(Unit unit)
	{
		this.unit = unit;
	}
	
	
	public void Enter()
	{
		unit.targetSelect.RefreshTarget (DataConfig.TARGET_SELECT.RANDOM);
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
		
		unit.targetSelect.Update ();
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
