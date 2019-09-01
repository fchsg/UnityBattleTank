using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
/// <summary>
/// 被动道具 panel.
/// </summary>
public class NormalPropPanel : PanelBase {
	private Transform _InitiativeProp_Container;
	private UIButton _closeBtn;
	private Transform _info_Container;
	private UILabel _itemName;
	private UILabel _number_Label;
	private UILabel _value_Label;
	private UILabel _describe_Label;
	private UISprite _itemiconbg;
	private UITexture _itemicon;
	private UIButton _iconBtn;

	private UIButton _use_Btn;
	private UILabel _useLabel;
	private UIButton _batchUse_Btn;
	private UILabel _batchUseLabel;

	private Transform _arrowSprite;
	private UIButton _drop_Btn;
	private UILabel _drop_Label;
	private Transform _mission_Container;
	private UIScrollView _ScrollView;
	private UIGrid _Grid;

	//data 
	ItemDataManager.ItemData _itemData;
	List<DataMission> _missonList = new List<DataMission>();
	void Awake()
	{
		_InitiativeProp_Container = transform.Find("NormalProp_Container");
		_closeBtn = _InitiativeProp_Container.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);

		_info_Container = _InitiativeProp_Container.Find("infoWidget");
		_itemName = _info_Container.Find("name_Label").GetComponent<UILabel>();
		_number_Label = _info_Container.Find("number_Label").GetComponent<UILabel>();
		_number_Label.color = UICommon.FONT_COLOR_GREY;
		_value_Label = _info_Container.Find("number_Label/value_Label").GetComponent<UILabel>();
		_value_Label.color = UICommon.FONT_COLOR_GREEN;
		_describe_Label = _info_Container.Find("describe_Label").GetComponent<UILabel>();
		_describe_Label.color = UICommon.FONT_COLOR_GREY;
		_itemiconbg = _info_Container.Find("itemiconbg").GetComponent<UISprite>();
		_itemicon = _info_Container.Find("itemiconbg/itemicon").GetComponent<UITexture>();

		_mission_Container = _InitiativeProp_Container.Find("mission_Container");
		_arrowSprite = _InitiativeProp_Container.Find("drop_Btn/arrow_Sprite");
		_drop_Btn = _InitiativeProp_Container.Find("drop_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_drop_Btn,OnDrop);
		_drop_Label = _InitiativeProp_Container.Find("drop_Btn/Label").GetComponent<UILabel>();
		_drop_Label.color = UICommon.FONT_COLOR_GREY;

		_ScrollView = _mission_Container.Find("ScrollView").GetComponent<UIScrollView>();
		_Grid = _mission_Container.Find("ScrollView/Grid").GetComponent<UIGrid>();
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length != 0)
		{
			_itemData = parameters[0] as ItemDataManager.ItemData;
		}
		InintGridData();
		CreateMission(_Grid,_missonList);
	}
		
	// Update is called once per frame
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		if(_itemData != null)
		{
			Model_ItemGroup model_itemGroup = InstancePlayer.instance.model_User.model_itemGroup;
			Item item = model_itemGroup.QueryItem(_itemData.id);
			_value_Label.text = item.num.ToString();
			_itemName.color = _itemData.nameColor;
			_itemName.text = _itemData.itemData.name;
			_describe_Label.text = _itemData.itemData.desc;
			_itemiconbg.spriteName = _itemData.iconBgName;
			_itemicon.SetItemTexture(_itemData.id);

		}
	}
	void InintGridData()
	{
		_missonList.Clear();
		if(_itemData != null)
		{
			if(_itemData.itemData.missionId1 != 0)
			{
				DataMission mission = DataManager.instance.dataMissionGroup.GetMission(_itemData.itemData.missionId1);
				_missonList.Add(mission);
			}
			if(_itemData.itemData.missionId2 != 0)
			{
				DataMission mission = DataManager.instance.dataMissionGroup.GetMission(_itemData.itemData.missionId2);
				_missonList.Add(mission);
			}
			if(_itemData.itemData.missionId3 != 0)
			{
				DataMission mission = DataManager.instance.dataMissionGroup.GetMission(_itemData.itemData.missionId3);
				_missonList.Add(mission);
			}



		}
	}
	void OnDrop()
	{
		_arrowSprite.Rotate(new Vector3(180.0f,0.0f,0.0f));
	}
	void OnClose()
	{
		this.Delete();
	}
	void CreateMission(UIGrid grid,List<DataMission> datalist){
		if (grid != null) {
			grid.DestoryAllChildren();
		}
		for(int i = 0; i < datalist.Count; i++)
		{
			if(grid.gameObject != null)
			{

				GameObject Item = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "warehouse/missionItem");
				GameObject item = NGUITools.AddChild(grid.gameObject,Item);
				missionItem miss = item.GetComponent<missionItem>();
				miss.Init(datalist[i]);
				item.name = "0" + i;
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
	}
}
