using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnitTargetSelect {

	public class RESULT : IComparable<RESULT>
	{
		public Unit target;
		public float offset;

		public float weight;
		public int CompareTo(RESULT other)
		{
			if (weight < other.weight) {
				return -1;
			} else if (weight > other.weight) {
				return 1;
			} else {
				return 0;
			}
		}
	}

	private Unit _unit;
	private UnitGroup _unitGroup;


	private UnitTargetSelect.RESULT _currentTarget;
	public UnitTargetSelect.RESULT currentTarget
	{
		get { return _currentTarget; }
	}


	private long _refreshTimestamp = 0;
	public const float REFRESH_INTERVAL = 1;


	public UnitTargetSelect(Unit unit, UnitGroup unitGroup)
	{
		_unitGroup = unitGroup;
		_unit = unit;
	}

	public bool IsTargetAvailable()
	{
		if (_currentTarget != null &&
		    !_currentTarget.target.isDead) {
			return true;
		} else {
			return false;
		}
	}
	

	public void Update()
	{
		RemoveDeadTarget ();

		RefreshTarget();
//		bool targetInside = IsTargetInShootRange (_currentTarget);
//		if (!targetInside) {
//			RefreshTarget();
//		}

		RunningToTarget ();
		AimmingToTarget ();
	}

	private void RemoveDeadTarget()
	{
		if(_currentTarget != null)
		{
			if(_currentTarget.target.isDead)
			{
				_currentTarget = null;
			}
		}

	}

	public void RefreshTarget(DataConfig.TARGET_SELECT forceSelectType = DataConfig.TARGET_SELECT.UNKNOWN)
	{
		if (_currentTarget == null) {
			_refreshTimestamp = 0;
		}

		long ct = TimeHelper.GetCurrentTimestampScaled ();
		long dt = ct - _refreshTimestamp;
		if (ct < REFRESH_INTERVAL * 1000) {
			return;
		}
		_refreshTimestamp = ct;


		UnitTargetSelect.RESULT r = Select (forceSelectType);
		if (r != null) {
			SwitchTarget(r);
		}

	}

	private void SwitchTarget(RESULT r)
	{
		if (_unit.unitFire.isAttacking) {
			return;
		}

		if(_currentTarget == null)
		{
			_currentTarget = r;
		}
		else if(IsTargetInShootRange(r) && !IsTargetInShootRange(_currentTarget))
		{
			_currentTarget = r;
		}
		else if(IsTargetInCloseRange(r))
		{
			if(IsTargetInCloseRange(_currentTarget))
			{
				float n = (r.target.transform.position - _unit.transform.position).magnitude;
				float o = (_currentTarget.target.transform.position - _unit.transform.position).magnitude;
				if(n < o)
				{
					_currentTarget = r;
				}
			}
			else
			{
				_currentTarget = r;
			}
		}
		else
		{
			/*
				Vector3 v1 = _currentTarget.target.transform.position - _unit.transform.position;
				Vector3 v2 = r.target.transform.position - _unit.transform.position;
				if(v2.magnitude < v1.magnitude * 0.7f)
				{
					_currentTarget = r;
				}
				*/
		}
	}
	

	private void RunningToTarget()
	{
		if (_currentTarget != null) {
			Vector3 v = _currentTarget.target.transform.position - _unit.transform.position;
			float dist = v.magnitude;

			if (dist > _unit.unit.dataUnit.fightRange) {
				_unit.unitDriver.SetTargetPosition (_currentTarget.target.transform.position);
			} else {
				_unit.unitDriver.ClearTargetPosition ();
			}
		} else {
			_unit.unitDriver.ClearTargetPosition();
		}

	}

	private void AimmingToTarget()
	{
		if (_currentTarget != null) {
			_unit.aimmingControl.TrunToTarget (_currentTarget.target.transform.position);
		} else {
			_unit.aimmingControl.ClearTarget();
		}

	}

	public bool IsTargetInShootRange(RESULT target)
	{
		if (target == null) {
			return false;
		}

		bool inside = IsTargetInShootRange (target.target);
		return inside;


	}

	public bool IsTargetInCloseRange(RESULT target)
	{
		if (target == null) {
			return false;
		}

		bool inside = IsTargetInCloseRange (target.target);
		return inside;

		
	}

	public bool IsTargetInShootRange(Unit target)
	{
		
		if (target.isDead) {
			return false;
		}

		return IsTargetInShootRange(target.transform.position);

		
	}
	
	public bool IsTargetInCloseRange(Unit target)
	{
		
		if (target.isDead) {
			return false;
		}
		
		return IsTargetInCloseRange(target.transform.position);

	}
	
	public bool IsTargetInShootRange(Vector3 position)
	{
		Vector3 offset = _unit.transform.position - position;
		float dist = offset.magnitude;
		float shootRange = _unit.unit.dataUnit.shootRange;
		return shootRange >= dist;
		
		
	}
	
	public bool IsTargetInCloseRange(Vector3 position)
	{
		Vector3 offset = _unit.transform.position - position;
		float dist = offset.magnitude;
		float shootRange = _unit.unit.dataUnit.closeRange;
		return shootRange >= dist;
		
		
	}


	public void BeHurt(UnitFire.ShootParam shootParam)
	{
		if (shootParam.attacker != null) {
			RESULT r = new RESULT ();
			r.target = shootParam.attacker;
			r.offset = (shootParam.attacker.transform.position - _unit.transform.position).magnitude;
			SwitchTarget (r);
		}
	}

	// ====================================
	// select strategy


	public RESULT Select(DataConfig.TARGET_SELECT forceSelectType = DataConfig.TARGET_SELECT.UNKNOWN)
	{
		if (forceSelectType == DataConfig.TARGET_SELECT.UNKNOWN) {
			forceSelectType = _unit.unit.dataUnit.targetSelect;
		}

		switch (forceSelectType) {
		case DataConfig.TARGET_SELECT.CENTER:
			return Select_center();
			break;
			
		case DataConfig.TARGET_SELECT.CENTER_RANDOM:
			return Select_centerRandom();
			break;
			
		case DataConfig.TARGET_SELECT.CLOSEST:
			return Select_closest();
			break;
			
		case DataConfig.TARGET_SELECT.CLOSEST_RANDOM:
			return Select_closetRandom();
			break;
			
		case DataConfig.TARGET_SELECT.RANDOM:
			return Select_random();
			break;
			
		default:
			return Select_random();
			break;
		}

	}

	private RESULT Select_center()
	{
		List<Unit> units = _unitGroup.GetOpponents (_unit.team);

		int unitCount = 0;
		Vector3 unitCenter = new Vector3 ();

		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit enemyUnit = units[i];
			if(enemyUnit.isDead)
			{
				continue;
			}

			++unitCount;
			unitCenter += enemyUnit.transform.position;

		}

		if(unitCount > 0)
		{
			unitCenter /= unitCount;

			Unit target = null;
			float minDist = int.MaxValue;

			for (int i = 0; i < n; ++i) {
				Unit enemyUnit = units[i];
				if(enemyUnit.isDead)
				{
					continue;
				}

				Vector3 offset = enemyUnit.transform.position - unitCenter;
				float dist = offset.magnitude;
				if(dist < minDist)
				{
					target = enemyUnit;
					minDist = dist;
				}

			}

			if (target != null) {
				RESULT r = new RESULT ();
				r.target = target;
				r.offset = minDist;
				return r;
			} else {
				return null;
			}

		}
		else
		{
			return null;
		}

	}
	
	private RESULT Select_centerRandom()
	{
		List<Unit> units = _unitGroup.GetOpponents (_unit.team);

		int unitCount = 0;
		Vector3 unitCenter = new Vector3 ();
		
		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit enemyUnit = units[i];
			if(enemyUnit.isDead)
			{
				continue;
			}
			
			++unitCount;
			unitCenter += enemyUnit.transform.position;
			
		}
		
		if(unitCount > 0)
		{
			unitCenter /= unitCount;
			
			List<RESULT> candidates = new List<RESULT> ();

			for (int i = 0; i < n; ++i) {
				Unit enemyUnit = units[i];
				if(enemyUnit.isDead)
				{
					continue;
				}
				
				Vector3 offset = enemyUnit.transform.position - unitCenter;
				float dist = offset.magnitude;

				RESULT r = new RESULT ();
				r.target = enemyUnit;
				r.offset = dist;
				r.weight = RandomHelper.Range(0, dist);
				candidates.Add(r);

			}
			
			if (candidates.Count > 0) {
				candidates.Sort ();
				return candidates [0];
			} else {
				return null;
			}

		}
		else
		{
			return null;
		}
		
	}
	
	private RESULT Select_closetRandom()
	{
		List<Unit> units = _unitGroup.GetOpponents (_unit.team);

		List<RESULT> candidates = new List<RESULT> ();
		
		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit enemyUnit = units[i];
			if(enemyUnit.isDead)
			{
				continue;
			}
			
			Vector3 offset = enemyUnit.transform.position - _unit.transform.position;
			float dist = offset.magnitude;

			RESULT r = new RESULT ();
			r.target = enemyUnit;
			r.offset = dist;
			r.weight = RandomHelper.Range(0, dist);
			candidates.Add(r);

		}

		if (candidates.Count > 0) {
			candidates.Sort ();
			return candidates [0];
		} else {
			return null;
		}

	}

	private RESULT Select_closest()
	{
		List<Unit> units = _unitGroup.GetOpponents (_unit.team);

		Unit target = null;
		float minDist = int.MaxValue;

		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit enemyUnit = units[i];
			if(enemyUnit.isDead)
			{
				continue;
			}

			Vector3 offset = enemyUnit.transform.position - _unit.transform.position;
			float dist = offset.magnitude;
			if(dist < minDist)
			{
				target = enemyUnit;
				minDist = dist;
			}

		}

		if (target != null) {
			RESULT r = new RESULT ();
			r.target = target;
			r.offset = minDist;
			return r;
		} else {
			return null;
		}

	}

	private RESULT Select_random()
	{
		List<Unit> units = _unitGroup.GetOpponents (_unit.team);

		List<Unit> alive_units = new List<Unit> ();
		int n = units.Count;
		for (int i = 0; i < n; ++i) {
			Unit enemyUnit = units [i];
			if(enemyUnit.isDead)
			{
				continue;
			}

			alive_units.Add(enemyUnit);
		}

		if (alive_units.Count > 0) {
			int index = (int)RandomHelper.Range(0, alive_units.Count);

			RESULT r = new RESULT ();
			r.target = alive_units [index];
			r.offset = (r.target.transform.position - _unit.transform.position).magnitude;
			return r;
		} else {
			return null;
		}

	}
}
