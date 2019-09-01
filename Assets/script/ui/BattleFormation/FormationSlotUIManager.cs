using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationSlotUIManager : MonoBehaviour {

	private const int TEAM_COUNT = 3;    
	private const int POSITION_COUNT = 6;

	public Dictionary<int, Dictionary<int, FormationSlotUI>> _unitSlotMap;
	public Dictionary<int, Dictionary<int, UIHeroSlot>> _heroSlotMap;

	
    void Awake()
	{
		_unitSlotMap = new Dictionary<int, Dictionary<int, FormationSlotUI>> (TEAM_COUNT);
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			_unitSlotMap[i] = new Dictionary<int, FormationSlotUI>(POSITION_COUNT);
		}

		_heroSlotMap = new Dictionary<int, Dictionary<int, UIHeroSlot>> (TEAM_COUNT);
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			_heroSlotMap[i] = new Dictionary<int, UIHeroSlot>(POSITION_COUNT);
		}
	}
	
	public void SetUnitSlotUI(int teamId, int slotId, FormationSlotUI slotUI)
	{
		_unitSlotMap [teamId].Add (slotId, slotUI);
	}

	public void SetHeroSlotUI(int teamId, int slotId, UIHeroSlot slotUI)
	{
		_heroSlotMap [teamId].Add (slotId, slotUI);
	}


	public FormationSlotUI GetFormationSlotUI(int i, int j)
	{
		return _unitSlotMap [i] [j];
	}


	public void SetSlotUIEanbled(int teamId)
	{
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			if(teamId == i)
			{
				SetUnitSlotEnabled(_unitSlotMap[i], true);
				SetHeroSlotEnabled (_heroSlotMap [i], true);
			}
			else
			{
				SetUnitSlotEnabled(_unitSlotMap[i], false);
				SetHeroSlotEnabled (_heroSlotMap [i], false);
			}
		}
	}

	private void SetUnitSlotEnabled(Dictionary<int, FormationSlotUI> team, bool active)
	{
		foreach (FormationSlotUI slotUI in team.Values) 
		{
			if(slotUI.dragItemUI != null)
			{
				slotUI.dragItemUI.gameObject.SetActive(active);

				FormationSlotItemUI slotItemUI = slotUI.dragItemUI.GetComponent<FormationSlotItemUI>();
				slotItemUI.ResetUI();
			}
		}
	}

	public void SetUnitLayer(int teamId, int sortingLayer)
	{
		teamId = teamId - 1;
		if (teamId >= 0 && teamId <3) 
		{
			Dictionary<int, FormationSlotUI> team = _unitSlotMap [teamId];
			if (team != null) 
			{
				foreach (FormationSlotUI slotUI in team.Values) 
				{
					if(slotUI.dragItemUI != null)
					{
						UIDragDropItemFormation dragItem= slotUI.dragItemUI;
						dragItem.SetSortingOrder (sortingLayer);
					}
				}
			}
		}

	}

	private void SetHeroSlotEnabled(Dictionary<int, UIHeroSlot> team, bool active)
	{
		foreach (UIHeroSlot slotUI in team.Values) 
		{
			if(slotUI.dragItemUI != null)
			{
				slotUI.dragItemUI.gameObject.SetActive(active);
			}
		}
	}

	public void ResetTeamSlotItemUI(int teamId)
	{
		foreach (FormationSlotUI slotUI in _unitSlotMap [teamId - 1].Values) 
		{
			if(slotUI != null && slotUI.dragItemUI != null)
			{
				FormationSlotItemUI slotItemUI = slotUI.dragItemUI.GetComponent<FormationSlotItemUI>();
				if(slotItemUI != null)
				{
					slotItemUI.ResetUI();
				}
			}
		}
	}

	public void UpdateUnitSlotUI (int teamId)
	{
		Model_UnitGroup[] team = InstancePlayer.instance.model_User.model_Formation.GetUnitTeam (teamId);
		if (team != null) 
		{
			int index = 0;
			foreach (FormationSlotUI slotUI in _unitSlotMap [teamId - 1].Values) 
			{
				if(slotUI != null && slotUI.dragItemUI != null)
				{
					FormationSlotItemUI slotItemUI = slotUI.dragItemUI.GetComponent<FormationSlotItemUI>();

					if(team != null && team[index] != null)
					{
						slotItemUI.UpdateUI(team[index]);
					}
				}
				++index;
			}
		}
	}

	// 设置 hero 或 Unit 上阵 Item 是否可以拖动
	public void SetHeroOrUnitDragable(bool isHero)
	{
		for (int i = 0; i < TEAM_COUNT; ++i) 
		{
			for (int j = 0; j < POSITION_COUNT; ++j) 
			{
				FormationSlotUI unitSlotUI= _unitSlotMap [i] [j];
				FormationDragItemUI unitDragUI= unitSlotUI.dragItemUI;
				if (unitDragUI != null) 
				{
					unitDragUI.GetComponent<BoxCollider> ().enabled = !isHero;
				}
					
				UIHeroSlot heroSlotUI = _heroSlotMap [i] [j];
				UIHeroDragItem heroDragUI = heroSlotUI.dragItemUI;
				if (heroDragUI != null) 
				{
					heroDragUI.GetComponent<BoxCollider> ().enabled = isHero;
				}
			}
		}
	}


	// 更新上阵Unit数量
	public void UpdateBattleUnitNum(Model_UnitGroup unit_Group)
	{
		int i = unit_Group.teamId - 1;
		int j = unit_Group.posId - 1;


		FormationSlotUI formation	= GetFormationSlotUI (i, j);
		FormationDragItemUI dragItem = formation.dragItemUI;
		if (dragItem != null) 
		{
			FormationSlotItemUI slotItem = dragItem.GetComponent<FormationSlotItemUI> ();
			slotItem.UpdateUI (unit_Group);
		}
	}


}
