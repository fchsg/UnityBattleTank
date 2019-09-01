using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSkill {

	private int _slotCount;
	private InstanceSkill[] _slots;
	
	private int _currentSlotIndex = -1;
	public int currentSlotIndex
	{
		get { return _currentSlotIndex; }
	}

	public GameSkill()
	{
		Init ();
	}

	private void Init()
	{
		List<InstanceSkill> skills = new List<InstanceSkill> ();

		foreach (InstanceUnit unit in InstancePlayer.instance.battle.myTeam.units) {
			if (unit != null && unit.heroId > 0) {
				DataHeroLeadership heroLeadership = DataManager.instance.dataHeroGroup.GetHeroLeadership (unit.heroId);
				skills.Add (new InstanceSkill (heroLeadership.skill, 1, 1));
			}
		}
		/*
		int[] heroesId = InstancePlayer.instance.model_User.model_Formation.GetSelectTeamHeroesId ();
		foreach (int heroId in heroesId) {
			DataHeroLeadership heroLeadership = DataManager.instance.dataHeroGroup.GetHeroLeadership (heroId);
			skills.Add (new InstanceSkill (heroLeadership.skill, 1, 1));
		}
		*/

		_slotCount = skills.Count;
		_slots = new InstanceSkill[_slotCount];
		_slots = skills.ToArray ();
			
	}
	public int GetSkillCount()
	{
		return _slotCount;
	}
	public bool IsSelecting()
	{
		if (_currentSlotIndex < 0) {
			return false;
		}

		if (_slots [_currentSlotIndex] == null ||
		    _slots [_currentSlotIndex].count < 0) {
			return false;
		}

		return true;
	}

	public bool IsSelecting(int index)
	{
		if (_currentSlotIndex != index) {
			return false;
		}

		return IsSelecting ();

	}

	public InstanceSkill GetCurrentSelectSkill()
	{
		Assert.assert (IsSelecting ());
		return _slots [_currentSlotIndex];
	}
	
	public void Select(int index)
	{
		_currentSlotIndex = index;
	}
	

	public void Use()
	{
		Assert.assert (IsSelecting ());
		_slots [_currentSlotIndex].count--;
	}
	
	public DataSkill GetData(int index)
	{
		return _slots [index].GetDataSkill ();
	}
	
	public int GetCount(int index)
	{
		return _slots [index].count;
	}
	


}
