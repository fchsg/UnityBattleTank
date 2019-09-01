using UnityEngine;
using System.Collections;

public class FormationSlotUI : MonoBehaviour {

	private const string _unit_Item_Path = "profab/ui/BattleFormation/formation_unit_item";

	public Model_UnitGroup model_UnitGroup;

	public FormationDragItemUI dragItemUI;
	public BattleFormationPanel battleFormationPanel;

	public int i;
	public int j;

	public void UpdateUI(Model_UnitGroup unitGroup, BattleFormationPanel battlePanel)
	{
		model_UnitGroup = unitGroup;
		battleFormationPanel = battlePanel;
	
		// 更新阵型数据
		if (!unitGroup.isEmpty)
		{
			InitSlotItemUI();
		}
	}
	
	public void InitSlotItemUI()
	{
		GameObject prefab = Resources.Load (_unit_Item_Path) as GameObject;
		GameObject obj = NGUITools.AddChild (this.gameObject, prefab);
		obj.transform.parent = battleFormationPanel.dragDropRoot.transform;

		obj.transform.Find("formation_unit_slot").gameObject.SetActive(true);
		obj.transform.Find("formation_unit_scrollview").gameObject.SetActive(false);

		dragItemUI = obj.GetComponent<FormationDragItemUI>();
		dragItemUI.InitForSlot();
		dragItemUI.attachSlotUI = this.GetComponent<FormationSlotUI>();
		
		FormationSlotItemUI slotItem = obj.GetComponent<FormationSlotItemUI>();
		slotItem.UpdateUI(model_UnitGroup);

		FormationDragItemUI dragItem = obj.GetComponent<FormationDragItemUI>();
		dragItem.attachSlotUI = this;
		dragItem.battlePanel = battleFormationPanel;

		dragItem.unitId = slotItem.unitId;

		dragItem.CreateSpineTank ();
	}

}
