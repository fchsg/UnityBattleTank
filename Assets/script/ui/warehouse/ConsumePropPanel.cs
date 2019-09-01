using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SlgPB;
/// <summary>
/// 主动道具 panel.
/// </summary>
public class ConsumePropPanel : PanelBase {
	private Transform _PassiveProp_Container;
	private UIButton _closeBtn;
	private Transform _info_Container;
	private UILabel _itemName;
	private UILabel _number_Label;
	private UILabel _value_Label;
	private UILabel _describe_Label;
	private UISprite _itemiconbg;
	private UITexture _itemicon;
	 
	private UIButton _AddBtn;
	private UIButton _DelBtn;
	private UISlider _TankItem_Slider;
	private UILabel _AddTankNumLabel;

	private UIButton _okBtn;
	private UIButton _cancelBtn;
	private UILabel _okLabel;
	private UILabel _cancelLabel;
	//data 
	ItemDataManager.ItemData _itemData;
	private int _currentItemCount = 0;
	private float ITEMCOUNT = 0.0f;
	void Awake()
	{
		_PassiveProp_Container = transform.Find("ConsumeProp_Container");
		_closeBtn = _PassiveProp_Container.Find("CloseBtn").GetComponent<UIButton>();
		UIHelper.AddBtnClick(_closeBtn,OnClose);

		_info_Container = _PassiveProp_Container.Find("infoWidget");
		_itemName = _info_Container.Find("name_Label").GetComponent<UILabel>();
		_number_Label = _info_Container.Find("number_Label").GetComponent<UILabel>();
		_number_Label.color = UICommon.FONT_COLOR_GREY;
		_value_Label = _info_Container.Find("number_Label/value_Label").GetComponent<UILabel>();
		_value_Label.color = UICommon.FONT_COLOR_GREEN;
		_describe_Label = _info_Container.Find("describe_Label").GetComponent<UILabel>();
		_describe_Label.color = UICommon.FONT_COLOR_GREY;
		_itemiconbg = _info_Container.Find("itemiconbg").GetComponent<UISprite>();
		_itemicon = _info_Container.Find("itemiconbg/itemicon").GetComponent<UITexture>();
		 
		_AddBtn = _PassiveProp_Container.Find("trainContainer/AddBtn").GetComponent<UIButton>();
		_DelBtn = _PassiveProp_Container.Find("trainContainer/DelBtn").GetComponent<UIButton>();
		_TankItem_Slider = _PassiveProp_Container.Find("trainContainer/TankItem_Slider").GetComponent<UISlider>();     
		_AddTankNumLabel = _PassiveProp_Container.Find("trainContainer/AddTankNumLabel").GetComponent<UILabel>();         
		_AddTankNumLabel.color = UICommon.FONT_COLOR_ORANGE;
		UIHelper.AddBtnClick(_AddBtn,OnAdd);
		UIHelper.AddBtnClick(_DelBtn,OnDel);

		_okBtn = _PassiveProp_Container.Find("ok_Btn").GetComponent<UIButton>();
		_cancelBtn = _PassiveProp_Container.Find("cancel_Btn").GetComponent<UIButton>();
		_okLabel = _PassiveProp_Container.Find("ok_Btn/Label").GetComponent<UILabel>();
		_cancelLabel = _PassiveProp_Container.Find("cancel_Btn/Label").GetComponent<UILabel>();
		_okLabel.color = UICommon.FONT_COLOR_GOLDEN;
		_cancelLabel.color = UICommon.FONT_COLOR_GOLDEN;
		UIHelper.AddBtnClick(_okBtn,OnOK);
		UIHelper.AddBtnClick(_cancelBtn,OnCancel);
	}

	public override void Open (params object[] parameters)
	{
		base.Open (parameters);
		if(parameters != null && parameters.Length != 0)
		{
			_itemData = parameters[0] as ItemDataManager.ItemData;
		}
	}

	public void Init(ItemDataManager.ItemData itemData)
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

			ITEMCOUNT = (float)item.num;
			_currentItemCount = Mathf.RoundToInt(_TankItem_Slider.value * ITEMCOUNT);
			if (_TankItem_Slider != null && _AddTankNumLabel != null)
				_AddTankNumLabel.text = _currentItemCount.ToString();
		}
	}
	void OnCancel()
	{
		OnClose();
	}

	void OnOK()
	{
		_currentItemCount = Mathf.RoundToInt(_TankItem_Slider.value * ITEMCOUNT);

		BatchUseItem(_itemData.id,_currentItemCount);
		OnClose();
	}

	void OnAdd()
	{
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value + 0.01f;
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * ITEMCOUNT).ToString();
		}
	}

	void OnDel()
	{
		if (_TankItem_Slider != null && _AddTankNumLabel != null)
		{
			_TankItem_Slider.value = _TankItem_Slider.value - 0.01f;
			_AddTankNumLabel.text = Mathf.RoundToInt(_TankItem_Slider.value * ITEMCOUNT).ToString();
		}
	}
	void OnClose()
	{
		this.Delete();
	}
	//=======================
	void BatchUseItem(int id,int num)
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_useItem.USE_ITEM_RESULT r = PBConnect_useItem.UseItem (OnUseItem, id, num, 0);

		if (r != PBConnect_useItem.USE_ITEM_RESULT.OK)
		{


		}
	}
	void OnUseItem(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("BatchUseItem success",Trace.CHANNEL.UI);
			NotificationCenter.instance.DispatchEvent(Notification_Type.RefreshPropItem,new Notification(""));

		} else {
			Trace.trace("BatchUseItem failure",Trace.CHANNEL.UI);

		}
	}
}
