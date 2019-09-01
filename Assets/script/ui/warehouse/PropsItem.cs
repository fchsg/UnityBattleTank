using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
/// <summary>
/// 道具 item.
/// </summary>
public class PropsItem : MonoBehaviour {

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
	//data 
	ItemDataManager.ItemData _itemData;

	void Awake()
	{
		_info_Container = this.transform;
		_itemName = _info_Container.Find("name_Label").GetComponent<UILabel>();
		_number_Label = _info_Container.Find("number_Label").GetComponent<UILabel>();
		_number_Label.color = UICommon.FONT_COLOR_GREY;
		_value_Label = _info_Container.Find("number_Label/value_Label").GetComponent<UILabel>();
		_value_Label.color = UICommon.FONT_COLOR_GREEN;
		_describe_Label = _info_Container.Find("describe_Label").GetComponent<UILabel>();
		_describe_Label.color = UICommon.FONT_COLOR_GREY;
		_itemiconbg = transform.Find("itemiconbg").GetComponent<UISprite>();
		_itemicon = transform.Find("itemiconbg/itemicon").GetComponent<UITexture>();
		_iconBtn = transform.Find("itemiconbg/itemicon").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_iconBtn,OnItemInfo);
		_use_Btn = transform.Find("use_Btn").GetComponent<UIButton>();
		_batchUse_Btn = transform.Find("batchUse_Btn").GetComponent<UIButton>();
		_useLabel = _use_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_batchUseLabel = _batchUse_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_useLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_batchUseLabel.color = UICommon.FONT_COLOR_GOLDEN;
		UIHelper.AddBtnClick(_use_Btn,OnUse);
		UIHelper.AddBtnClick(_batchUse_Btn,OnBatchUse);
	}


	public void Init(ItemDataManager.ItemData itemData)
	{
		_itemData = itemData;

	}
	public void UpdateData(ItemDataManager.ItemData itemData)
	{
		_itemData = itemData;
	}

	// Use this for initialization
	void Start () {
		
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
			if(_itemData.type == DataConfig.ITEM_CATEGORY.CONSUME)
			{
				_use_Btn.gameObject.SetActive(true);
				_batchUse_Btn.gameObject.SetActive(true);
			}
			else
			{
				_use_Btn.gameObject.SetActive(false);
				_batchUse_Btn.gameObject.SetActive(false);
			}
		}
	}
	void OnUse()
	{
		UseItem();
	}
	void OnBatchUse()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_CONSUMEPROP,_itemData);

	}
	void OnItemInfo()
	{
		UIController.instance.CreatePanel(UICommon.UI_PANEL_NORMALPROP,_itemData);
	}

	//==============================

	void UseItem()
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_useItem.USE_ITEM_RESULT r = PBConnect_useItem.UseItem (OnUseItem, _itemData.id, 1, 0);
		if (r != PBConnect_useItem.USE_ITEM_RESULT.OK)
		{
			Trace.trace("UseItem success",Trace.CHANNEL.UI);
 	 
		}
	}
	void OnUseItem(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("UseItem success",Trace.CHANNEL.UI);
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshPropItem,new Notification(""));
			 
		} else {
			Trace.trace("UseItem failure",Trace.CHANNEL.UI);

		}
	}

}
