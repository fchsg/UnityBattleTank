using UnityEngine;
using System.Collections;

public class FormationSlotItemUI : MonoBehaviour {
	
	//-----------------------Data
	public int unitId;

	public int fightCnt;   //已上阵数
	public int maxCnt;	 //最高可上阵数

	public int unitInitialDepth;    // unit sprite 初始depth

	
	//-----------------------UI

	public GameObject slotItemUI;
	
	public UILabel name_Label;

	public GameObject number_Container;
	public UILabel up_Label;
	public UILabel amount_Label;

	
	void Awake()
	{
		slotItemUI = transform.Find ("formation_unit_slot").gameObject;

		name_Label = slotItemUI.transform.Find ("name_Label").GetComponent<UILabel>();
		number_Container = slotItemUI.transform.Find ("number_Container").gameObject;
		up_Label = number_Container.transform.Find ("up_Label").GetComponent<UILabel>();
		amount_Label = number_Container.transform.Find("amount_Label").GetComponent<UILabel>();
	}
	
	public void UpdateUI(Model_UnitGroup unitGroup)
	{
		unitId = unitGroup.unitId;
		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);

		up_Label.text = unitGroup.num + ""; // 上阵数量
		amount_Label.text = unitGroup.maxNum + "";  // 最大上阵数量 

		if (dataUnit != null) 
		{		
			name_Label.text = dataUnit.name;
		}

		this.GetComponent<FormationDragItemUI> ().unitId = unitId;
	}

	public void UpdateUI(int unitId)
	{
		DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
		name_Label.text = dataUnit.name;
	}

	public void ResetUI()
	{
		SetLabelVisible (true);
		FormationDragItemUI dragItem = transform.GetComponent<FormationDragItemUI> ();
		dragItem.SetSortingOrder (1);
	}

	public void SetLabelVisible(bool isVisible)
	{
		number_Container.SetActive (isVisible);
		name_Label.gameObject.SetActive (isVisible);
	}
	
	public void IncreaseDepth()
	{
		FormationDragItemUI dragItem = transform.GetComponent<FormationDragItemUI> ();
		dragItem.SetSortingOrder (2);
	}

}
