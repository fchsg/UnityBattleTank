using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGroup {

	private List<Unit> _allUnits = new List<Unit> ();
	public List<Unit> allUnits
	{
		get { return _allUnits; }
	}

	private List<Unit> _ourUnits = new List<Unit>();
	public List<Unit> ourUnits
	{
		get { return _ourUnits; }
	}
	
	private List<Unit> _myUnits = new List<Unit> ();
	public List<Unit> myUnits
	{
		get { return _myUnits; }
	}

	private List<Unit> _enemyUnits = new List<Unit> ();
	public List<Unit> enemyUnits
	{
		get { return _enemyUnits; }
	}

	private List<Unit> _friendUnits = new List<Unit> ();
	public List<Unit> friendUnits
	{
		get { return _friendUnits; }
	}

	private List<List<Unit>> _unitsWithTeam = new List<List<Unit>>();
	public List<List<Unit>> unitsWithTeam
	{
		get { return _unitsWithTeam; }
	}

	public UnitGroup()
	{
	}

	public void AddUnit(GameObject[][] tanks)
	{
		for (int i = 0; i < tanks.Length; ++i)
		{
			if(tanks[i] != null)
			{
				List<Unit> team = new List<Unit>();

				foreach (GameObject tank in tanks[i])
				{
					if(tank != null)
					{
						Unit unit = tank.GetComponent<Unit>();
						AddUnit(unit);
						team.Add(unit);
					}
				}

				if(team.Count > 0)
				{
					_unitsWithTeam.Add(team);
				}
			}
		}

	}


	public void AddUnit(Unit unit)
	{
		Assert.assert(_allUnits.IndexOf(unit) < 0);
		_allUnits.Add (unit);

		if (unit.team == DataConfig.TEAM.MY) {
			_myUnits.Add(unit);
			_ourUnits.Add(unit);
		}
		else if (unit.team == DataConfig.TEAM.ENEMY) {
			_enemyUnits.Add(unit);
		}
		else if (unit.team == DataConfig.TEAM.FRIEND) {
			_friendUnits.Add(unit);
			_ourUnits.Add(unit);
		}
		else
		{
			Assert.assert(false);
		}
	}

	public List<Unit> GetOpponents(DataConfig.TEAM team)
	{
		if (team == DataConfig.TEAM.ENEMY) {
			return _ourUnits;
		}
		else if (team == DataConfig.TEAM.MY || team == DataConfig.TEAM.FRIEND) {
			return _enemyUnits;
		}
		else
		{
			Assert.assert(false);
			return null;
		}

	}

	public List<Unit> GetPlayerDeadUnits()
	{
		List<Unit> deadUnits = new List<Unit> ();

		foreach (Unit unit in _myUnits) {
//			if(unit.isDead)
			if(unit.unit.GetDeadCount() > 0)
			{
				deadUnits.Add(unit);
			}
		}

		return deadUnits;
	}


	public float GetPlayerDeadUnitsRatio()
	{
		float dead = 0;
		float total = 0;

		foreach (Unit unit in _myUnits) {
			total += unit.unit.unitCount;
			dead += unit.unit.GetDeadCount ();
		}

		return dead / total;
	}


}
