using UnityEngine;
using System.Collections;

public class UseExpItem : MonoBehaviour {

	private Transform _attribute_Container;
	private UISprite _expBg;
	private UITexture _expIcon;
	private UILabel _expName;
	private UILabel _expAttribute_Label;
	private UILabel _numberLabel;
	private UILabel _valueLabel;

	private UIButton _use_Btn;
	private UILabel _useLabel;
	private int  _itemId;
	private int _heroId;
	private DataItem _itemData;

	public void Init(int itemId,int heroId)
	{
		_itemId = itemId;
		_heroId = heroId;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdataUI();
	}
	void UpdataUI()
	{
		if(_itemId !=null)
		{
			_itemData = DataManager.instance.dataItemGroup.GetItem(_itemId);
			int itemConut = InstancePlayer.instance.model_User.model_itemGroup.GetItemCount(_itemId);
			_valueLabel.text = itemConut.ToString();
			_expName.text = _itemData.name;
			Color fontColor = new Color();
			string bgName = "";
			switch(_itemData.quality)
			{
			case 1:
				fontColor = UICommon.UNIT_NAME_COLOR_1;
				bgName = UICommon.UNIT_ICON_BG + "1";
				break;
			case 2:
				fontColor = UICommon.UNIT_NAME_COLOR_2;
				bgName = UICommon.UNIT_ICON_BG + "2";
				break;
			case 3:
				fontColor = UICommon.UNIT_NAME_COLOR_3;
				bgName = UICommon.UNIT_ICON_BG + "3";
				break;
			case 4:
				bgName = UICommon.UNIT_ICON_BG + "4";
				fontColor = UICommon.UNIT_NAME_COLOR_4;
				break;
			default:
				fontColor = UICommon.UNIT_NAME_COLOR_1;
				bgName = UICommon.UNIT_ICON_BG + "1";
				break;
			}
			_expName.color = fontColor;
			_expBg.spriteName = bgName;
			_expAttribute_Label.text = _itemData.desc;
		}

	}
	void OnUse()
	{
		UseItem_exp(_heroId,_itemId);

	}
	void Awake()
	{
		_attribute_Container = this.transform;
		_expBg = _attribute_Container.Find("skill_bg").GetComponent<UISprite>();
		_expIcon = _attribute_Container.Find("skill_bg/skillIcon").GetComponent<UITexture>();

		_expName = _attribute_Container.Find("skillName_Label").GetComponent<UILabel>();
		_expName.color = UICommon.FONT_COLOR_ORANGE;
		_expAttribute_Label = _attribute_Container.Find("skillAttribute_Label").GetComponent<UILabel>();
		_expAttribute_Label.color = UICommon.FONT_COLOR_GREY;
		_numberLabel = _attribute_Container.Find("attribute_Label_1").GetComponent<UILabel>();
		_numberLabel.color = UICommon.FONT_COLOR_GREY;
		_valueLabel = _attribute_Container.Find("attribute_Label_1/Label").GetComponent<UILabel>();
		_valueLabel.color = UICommon.FONT_COLOR_GREEN;
		_use_Btn = _attribute_Container.Find("use_Btn").GetComponent<UIButton>();
		_useLabel = _use_Btn.gameObject.transform.Find("Label").GetComponent<UILabel>();
		_useLabel.color = UICommon.FONT_COLOR_GOLDEN;
		UIHelper.AddBtnClick(_use_Btn,OnUse);
	}

	//==========================
	void UseItem_exp(int heroId,int itemId)
	{
		UIHelper.LoadingPanelIsOpen(true);
		PBConnect_useItem.USE_ITEM_RESULT r = PBConnect_useItem.UseItem (OnUseItem_exp, itemId, 1, heroId);

		switch(r)
		{
		case PBConnect_useItem.USE_ITEM_RESULT.OK:
			//				_use_Btn.isEnabled = true;
			break;
		case PBConnect_useItem.USE_ITEM_RESULT.LACK_ITEM:
			UIHelper.LoadingPanelIsOpen(false);
			UIHelper.ShowTextPromptPanel(this.gameObject,"缺少经验瓶");
			break;
		case PBConnect_useItem.USE_ITEM_RESULT.LACK_TARGET:
			UIHelper.LoadingPanelIsOpen(false);
			break;
		case PBConnect_useItem.USE_ITEM_RESULT.NOT_USEABLE:
			UIHelper.LoadingPanelIsOpen(false);
			break;
		default:
			break;
		}
	}
	void OnUseItem_exp(bool success, System.Object content)
	{
		UIHelper.LoadingPanelIsOpen(false);
		if (success) {
			Trace.trace("UseItem_exp success",Trace.CHANNEL.UI);
		} else {
			Trace.trace("UseItem_exp failure",Trace.CHANNEL.UI);
		}
	}

}
