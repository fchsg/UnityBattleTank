using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCollisionDetector {

	public class RESULT
	{
		public Unit target;
		public float offset2;
		public Vector3 collidePoint;
	}

	private Unit _unit;
	private UnitGroup _unitGroup;


	public UnitCollisionDetector(Unit unit, UnitGroup unitGroup)
	{
		_unit = unit;
		_unitGroup = unitGroup;
	}

	public RESULT Detect_withOrientationTest(Vector3 destination)
	{
		Vector3 unitMove = destination - _unit.transform.position;
		float unitR = _unit.unit.dataUnit.GetCollisionRadius ();

		List<Unit> units = _unitGroup.allUnits;
		
		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit toUnit = units[i];
			if(_unit == toUnit)
			{
				continue;
			}

			if(!BattleConfig.DEAD_UNIT_COLLISION && toUnit.isDead)
			{
				continue;
			}

			Vector3 offset = toUnit.transform.position - destination;
			float dist2 = offset.sqrMagnitude;
			float totalR = toUnit.unit.dataUnit.GetCollisionRadius() + unitR;
			if(dist2 <= totalR * totalR)
			{
				float dot = Vector3.Dot(unitMove, offset);
				if(dot > 0)
				{
					RESULT r = new RESULT();
					r.collidePoint = offset / 2 + destination;
					r.offset2 = dist2;
					r.target = toUnit;
					return r;
				}

			}
		}

		return null;
	}

	public RESULT Detect(Vector3 destination)
	{
		float unitR = _unit.unit.dataUnit.GetCollisionRadius ();

		List<Unit> units = _unitGroup.allUnits;
		
		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit toUnit = units[i];
			if(_unit == toUnit)
			{
				continue;
			}
			
			if(!BattleConfig.DEAD_UNIT_COLLISION && toUnit.isDead)
			{
				continue;
			}
			
			Vector3 offset = toUnit.transform.position - destination;
			float dist2 = offset.sqrMagnitude;
			float totalR = toUnit.unit.dataUnit.GetCollisionRadius() + unitR;
			if(dist2 <= totalR * totalR)
			{
				RESULT r = new RESULT();
				r.collidePoint = offset / 2 + destination;
				r.offset2 = dist2;
				r.target = toUnit;
				return r;
			}
		}
		
		return null;

	}

}
