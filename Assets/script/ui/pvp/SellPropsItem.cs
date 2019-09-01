using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;


public class SellPropsItem : MonoBehaviour {

	private Transform _transform;
	private UILabel _name;
	private UISprite _iconBg;
	private UITexture _icon;
	private UILabel _number;
	private UIButton _bug_Btn;
	private UILabel _CashLabel;
	private UILabel _Label_ed;
	 //data 
	SlgPB.ShopItem _ShopItem;
	int _index = 0;
	void Awake()
	{
		_transform = this.transform;
		_name = _transform.Find("name").GetComponent<UILabel>();
		_iconBg = _transform.Find("iconBg").GetComponent<UISprite>();
		_icon = _transform.Find("iconBg/icon").GetComponent<UITexture>();
		_number = _transform.Find("number").GetComponent<UILabel>();
		_number.color = UICommon.FONT_COLOR_GREY;
		_bug_Btn = _transform.Find("bug_Btn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_bug_Btn,OnClickBuy);
		_CashLabel = _transform.Find("bug_Btn/Label").GetComponent<UILabel>();
		_Label_ed = _transform.Find("bug_Btn/Label_ed").GetComponent<UILabel>();
		_CashLabel.color = UICommon.FONT_COLOR_ORANGE;
	}

	public void UpdateData(SlgPB.ShopItem shopItem,int index)
	{
		if(shopItem != null)
		{
			_ShopItem = shopItem;
			_index = index;
		}
	}
	void Update () {
		UpdateUI ();
	}
	void UpdateUI () {
		if(_index == null)return;
		List<SlgPB.ShopItem> shopItems = InstancePlayer.instance.model_User.model_shop.shopItems;
		_ShopItem = shopItems[_index];
//		MultipleItem multipItem = UIHelper.GetMultipleItem(_ShopItem.itemID,_ShopItem.num,_ShopItem.itemType,1,_icon);
		 
		SlgPB.PrizeItem prizeItem = new SlgPB.PrizeItem();
		prizeItem.itemId = _ShopItem.itemID;
		prizeItem.type = _ShopItem.itemType;
		prizeItem.num = _ShopItem.num;
		UIDropItem dropitem = UIHelper.GetUIDropItemByPrizeItem(prizeItem);
		_icon.SetTexturePath(dropitem.icon);
		int quality = DataManager.instance.dataItemGroup.GetItem(_ShopItem.itemID).quality;

		switch(quality - 1)
		{
		case 0:
			_name.color = UICommon.UNIT_NAME_COLOR_0;
			break;
		case 1:
			_name.color = UICommon.UNIT_NAME_COLOR_1;
			break;
		case 2:
			_name.color = UICommon.UNIT_NAME_COLOR_2;
			break;
		case 3:
			_name.color = UICommon.UNIT_NAME_COLOR_3;
			break;
		case 4:
			_name.color = UICommon.UNIT_NAME_COLOR_4;
			break;
		case 5:
			_name.color = UICommon.UNIT_NAME_COLOR_5;
			break;
		default :
			_name.color = UICommon.UNIT_NAME_COLOR_0;
			break;

		}
		 
		_name.text = DataManager.instance.dataItemGroup.GetItem(_ShopItem.itemID).name;
		_iconBg.spriteName = "pvp_itemBg_" + quality;
		_number.text = _ShopItem.num.ToString();
		if(_ShopItem.isSold == 0)
		{
			_CashLabel.gameObject.SetActive(true);
			_Label_ed.gameObject.SetActive(false);
			_CashLabel.text = _ShopItem.price.ToString();
			_bug_Btn.isEnabled = true;
		}
		else
		{
			_CashLabel.gameObject.SetActive(false);
			_Label_ed.gameObject.SetActive(true);
			_bug_Btn.isEnabled = false;
		}
	}
	void OnClickBuy()
	{
		Buy();
	}
	//======================= 
	void Buy()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_buyShopItem.RESULT r = PBConnect_buyShopItem.Buy(_index,OnBuy);
		switch(r)
		{
		case PBConnect_buyShopItem.RESULT.SOLD:
			break;
		case PBConnect_buyShopItem.RESULT.OK:
			break;
		}
	}
	void OnBuy(bool success, System.Object content)
	{
		if(success)
		{
			GetShop();
		}
		else
		{
		}
	}

	void GetShop()
	{
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
 
		}
		else
		{

		}
	}
}
