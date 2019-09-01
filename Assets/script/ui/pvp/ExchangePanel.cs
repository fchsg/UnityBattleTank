using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;

public class ExchangePanel : PanelBase {

	private Transform _Container;
	private UIButton _close_Btn;
	private UILabel _namePanel;

	private Transform _info_Container;
	private UILabel _nextTime_Label;
	private UILabel _Label_21;
	private UILabel _currentRecord_Label;
	private UILabel _currentRecordValue_Label;
	private UILabel _recordValue_Label;
	private UIButton _refshBtn;
	private UILabel _refshLabel;
	private UIGrid _grid;

	//data 
	List<SellPropsItem> _sellPropsItemList = new List<SellPropsItem>();
	List<SlgPB.ShopItem> _shopItems;
	void InitUI()
	{
		_Container = transform.Find("Exchange_Container");
		_close_Btn = _Container.Find("close_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_close_Btn,OnClose);
		_namePanel = _Container.Find("panelName_Label").GetComponent<UILabel>();
		_namePanel.color = UICommon.FONT_COLOR_GREY;
		_info_Container = _Container.Find("info_Container");
		_nextTime_Label = _info_Container.Find("nextTime_Label").GetComponent<UILabel>();
		_nextTime_Label.color = UICommon.FONT_COLOR_GREY;
		_Label_21 = _info_Container.Find("nextTime_Label/Label_2").GetComponent<UILabel>();
		_Label_21.color = UICommon.FONT_COLOR_GREY;
		_currentRecord_Label = _info_Container.Find("currentRecord_Label").GetComponent<UILabel>();
		_currentRecord_Label.color = UICommon.FONT_COLOR_GREY;
		_currentRecordValue_Label = _info_Container.Find("currentRecord_Label/Label").GetComponent<UILabel>();
		_currentRecordValue_Label.color = UICommon.FONT_COLOR_GREEN;
		_refshBtn = _info_Container.Find("refresh_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_refshBtn,OnRefresh);
		_refshLabel = _info_Container.Find("refresh_Btn/Label").GetComponent<UILabel>();
		_refshLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_recordValue_Label = _info_Container.Find("recordValue_Label").GetComponent<UILabel>();
		_grid = _Container.Find("item_Container/ScrollView/Grid").GetComponent<UIGrid>();
	}
	public override void Init ()
	{
		base.Init ();
		InitUI();
		GetShop();
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);

	}
	 
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		_currentRecordValue_Label.text = InstancePlayer.instance.model_User.model_Resource.GetIntCombat().ToString();
		if(PBConnect_getShop.refreshCostType == 18)
		{
			_recordValue_Label.text = UIHelper.SetStringSixteenColor("(消耗",UICommon.SIXTEEN_GREY) +  UIHelper.SetStringSixteenColor(PBConnect_getShop.refreshCostCount.ToString(),UICommon.SIXTEEN_ORANGE)
				+  UIHelper.SetStringSixteenColor("战绩点)",UICommon.SIXTEEN_GREY);
		}
		else if(PBConnect_getShop.refreshCostType == 11)
		{
			_recordValue_Label.text = UIHelper.SetStringSixteenColor("(消耗",UICommon.SIXTEEN_GREY) +  UIHelper.SetStringSixteenColor(PBConnect_getShop.refreshCostCount.ToString(),UICommon.SIXTEEN_ORANGE)
				+  UIHelper.SetStringSixteenColor("钻石)",UICommon.SIXTEEN_GREY);
		}

	}
	void CreatePopsItem(List<SlgPB.ShopItem> shopItems)
	{
		_sellPropsItemList.Clear();
		_grid.DestoryAllChildren();

		int n = shopItems.Count;
		for(int i = 0; i < n; ++i)
		{
			SlgPB.ShopItem shopItem = shopItems[i];
			GameObject prof = (GameObject)Resources.Load(AppConfig.FOLDER_PROFAB_UI + "pvp/propsItem");
			GameObject item = NGUITools.AddChild(_grid.gameObject,prof);
			SellPropsItem sellitem = item.GetComponent<SellPropsItem>();
			sellitem.UpdateData(shopItem,i);
			_sellPropsItemList.Add(sellitem);
		}
		_grid.Reposition();
	}
	void OnRefresh()
	{
		RefreshShop();
	}
	void OnClose()
	{
		this.Delete();
	}

	//============================
	void GetShop()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_getShop.RESULT r = PBConnect_getShop.GetShop(PBConnect_buyShopItem.SHOP_TYPE.RESULT_SHOP,OnGetShop);
		switch(r)
		{
		case PBConnect_getShop.RESULT.OK:
			break;
		case PBConnect_getShop.RESULT.LACK_RESOURCE:
			UIHelper.LoadingPanelIsOpen(false);
			break;
		}
	}

	void OnGetShop(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if(success)
		{
			_shopItems = InstancePlayer.instance.model_User.model_shop.shopItems;
			CreatePopsItem(_shopItems);
		}
		else
		{
			
		}
	}
	void RefreshShop()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_getShop.RESULT r = PBConnect_getShop.RefreshShop(PBConnect_buyShopItem.SHOP_TYPE.RESULT_SHOP,OnRefreshShop);
		switch(r)
		{
		case PBConnect_getShop.RESULT.OK:
			break;
		case PBConnect_getShop.RESULT.LACK_RESOURCE:
			UIHelper.LoadingPanelIsOpen(false);
			break;
		}
	}

	void OnRefreshShop(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if(success)
		{
			_shopItems = InstancePlayer.instance.model_User.model_shop.shopItems;
			CreatePopsItem(_shopItems);
		}
		else
		{

		}
	}
}
