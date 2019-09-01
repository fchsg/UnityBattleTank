using UnityEngine;
using System.Collections;

public class FormationScrollViewItemUI : MonoBehaviour {

	//-----------------------Data
	public int unitId;
	public int remainCnt;  //剩余未上阵的战车数量

	public int amount;

	//-----------------------UI
	
	public const string bg_normal_path = "cell_normal";
	public const string bg_select_path = "cell_select";
	
	
	public UILabel name_Label;
	public UILabel count_Label;
	public UISprite unit_Sprite; 
	
	public UISprite battle_Sprite;
	public UISprite bg_Sprite;
	
	public GameObject scrollViewItemUI;

	public Model_Formation model_Formation;

	void Awake()
	{
		scrollViewItemUI = transform.Find ("formation_unit_scrollview").gameObject;

		name_Label = scrollViewItemUI.transform.Find ("name_Label").GetComponent<UILabel>();
		count_Label = scrollViewItemUI.transform.Find ("count_Label").GetComponent<UILabel>();
		unit_Sprite = scrollViewItemUI.transform.Find ("unit_Sprite").GetComponent<UISprite>();
		
		battle_Sprite = scrollViewItemUI.transform.Find ("battle_Sprite").GetComponent<UISprite>();
		bg_Sprite = scrollViewItemUI.transform.Find ("bg_Sprite").GetComponent<UISprite>();

		model_Formation = InstancePlayer.instance.model_User.model_Formation;

		UIHelper.AddBtnClick (GetComponent<UIButton> (), UnitDetialPanelCB);
	}
	
	public void UpdateUI(FormationUnitCategory.Unit unit)
	{
		unitId = unit.id;
		remainCnt = unit.num;

		name_Label.text = unit.name;
		count_Label.text = unit.num + "";
		unit_Sprite.spriteName = unit.small_icon;

		int selectTeamId = model_Formation.GetSelectTeamId ();
		bool bOnDuty = model_Formation.IsTeamContaninUnit (selectTeamId, unitId);

		OnDuty (bOnDuty);

		this.GetComponent<FormationDragItemUI> ().unitId = unitId;
	}
	
	// 是否上阵
	public void OnDuty(bool b)
	{
		if (b) 
		{
			battle_Sprite.gameObject.SetActive(true);
			bg_Sprite.spriteName = bg_select_path;
		}
		else 
		{
			battle_Sprite.gameObject.SetActive(false);
			bg_Sprite.spriteName = bg_normal_path;
		}
	}

	public void UnitDetialPanelCB()
	{
		FormationDragItemUI dragUI = GetComponent<FormationDragItemUI> ();
		if (dragUI.state == FormationDragItemUI.STATE.SCROLLVIEW) 
		{
			TankInfoPanel.PanelType type = TankInfoPanel.PanelType.TANKFACTORY;
			DataUnit dataUnit = DataManager.instance.dataUnitsGroup.GetUnit (unitId);
			UIController.instance.CreatePanel (UICommon.UI_PANEL_TANKINFO,dataUnit,type);
		}
	}

}
