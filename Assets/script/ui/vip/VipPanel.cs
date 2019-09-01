using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipPanel : PanelBase {
	private Transform _parent;
	private UIButton _closeBtn;

	private Transform _vipExp_Container;
	private Transform _Vipbtn_Container;
	private Transform _info_Container;

	private UISlider _Timer_Colored_Slider;
	private UILabel _expValueLabel;
	private UILabel _curren_Label;
	private UILabel _will_Label;
	private UILabel _needVipExpLabel;

	private UIGrid _btnGrid;
	List<UIToggle> _toggleList = new List<UIToggle>();
	void Awake()
	{
		_parent = transform.Find("VipContainer");
		_closeBtn = _parent.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);
		_vipExp_Container = _parent.Find("vipExp_Container");
		_Vipbtn_Container = _parent.Find("Vipbtn_Container");
		_info_Container = _parent.Find("info_Container");

		_Timer_Colored_Slider = _vipExp_Container.Find("Timer_Colored_Slider").GetComponent<UISlider>();
		_expValueLabel = _vipExp_Container.Find("Timer_Colored_Slider/Label").GetComponent<UILabel>();
		_expValueLabel.color = UICommon.FONT_COLOR_ORANGE;
		_curren_Label= _vipExp_Container.Find("curren_Label").GetComponent<UILabel>();
		_curren_Label.color = UICommon.FONT_COLOR_GREY;
		_will_Label= _vipExp_Container.Find("will_Label").GetComponent<UILabel>();
		_will_Label.color = UICommon.FONT_COLOR_GREY;
		_needVipExpLabel= _vipExp_Container.Find("Label").GetComponent<UILabel>();
		_needVipExpLabel.color = UICommon.FONT_COLOR_GREY;

		_btnGrid = _Vipbtn_Container.Find("Grid").GetComponent<UIGrid>();


	}
	void Start()
	{
		CreateItem(_btnGrid,10);

	}
	void Update () {
	
	}

	void CreateItem(UIGrid grid,int num){
		grid.DestoryAllChildren();
		_toggleList.Clear();
		for(int i = 1; i <= num; i++)
		{
			if(grid.gameObject != null)
			{
				GameObject profab = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "vip/vipBtn_1");
				GameObject item = NGUITools.AddChild(grid.gameObject,profab);
				UIToggle toggle = item.GetComponent<UIToggle>();
				UILabel label_1 = item.transform.Find("Label").GetComponent<UILabel>();
				label_1.text = "VIP" + i;
				UILabel label_2 = item.transform.Find("vip_Sprite/Label").GetComponent<UILabel>();
				label_2.text = i.ToString();
				_toggleList.Add(toggle);
				item.name = "vipBtn_" + i;		 
			}
		}
		grid.Reposition();
	}

	void AddClick()
	{
		EventDelegate.Add(_toggleList[0].onChange,Onchange_1);
		EventDelegate.Add(_toggleList[1].onChange,Onchange_2);
//		EventDelegate.Add(_toggleList[2].onChange,Onchange_3);
//		EventDelegate.Add(_toggleList[3].onChange,Onchange_4);
//		EventDelegate.Add(_toggleList[4].onChange,Onchange_5);
//		EventDelegate.Add(_toggleList[5].onChange,Onchange_6);
//		EventDelegate.Add(_toggleList[6].onChange,Onchange_7);
//		EventDelegate.Add(_toggleList[7].onChange,Onchange_8);
//		EventDelegate.Add(_toggleList[8].onChange,Onchange_9);
//		EventDelegate.Add(_toggleList[9].onChange,Onchange_10);
	}
	void OnClose()
	{
		this.Delete();
	}
	void Onchange_1()
	{
		OnToggleChange();
	}
	void Onchange_2()
	{
		OnToggleChange();
	}

	void OnToggleChange()
	{
		UIToggle toggle = UIToggle.current;
//		GameObject toggleGame = toggle.gameObject;
//		GameObject label = toggleGame.;//.FindChild("Label");
//		GameObject vip_Sprite = (GameObject)toggle.transform.gameObject.("vip_Sprite");
		bool check = toggle.value;
		if(check)
		{
			
		}
		else
		{
			
		}
	}

}
