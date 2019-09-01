using UnityEngine;
using System.Collections;

public class UnitStateDead : IState {

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
	
	public UnitStateDead(Unit unit)
	{
		this.unit = unit;
	}
	
	
	public void Enter()
	{
		unit.aimmingControl.Stop ();
		unit.unitDriver.Stop (true);

		if (!BattleConfig.DEAD_UNIT_COLLISION) {
			unit.gridCorrect.ClearSlefGrid ();
		}

		RenderHelper.ChangeWholeModelColor (unit.gameObject, Color.gray);
		unit.gameObject.GetComponent<TankSpineAttach> ().Deattach ();

		if (BattleGame.instance.mapCamera.cameraControl.IsPointInsideCamera (unit.transform.position)) {
			BattleGame.instance.mapCamera.cameraControl.Shake ();
		}

		AudioGroup.Play (unit.GetComponent<AudioGroup> ().dead, unit.gameObject);

		GameObject explode = ResourceHelper.Load (AppConfig.FOLDER_PROFAB_EFFECT_EXPLODE + "Tank_Destroy");
		explode.transform.position = unit.transform.position;
//		BattleFactory.AddUnitToLayer (explode, BattleConfig.LAYER.EFFECT);

		//
		unit.unitDriver.engine.StopEffect ();

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


		return (int)_result;
	}

}
